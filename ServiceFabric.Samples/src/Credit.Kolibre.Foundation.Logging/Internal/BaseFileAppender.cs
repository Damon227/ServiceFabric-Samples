// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.Logging
// File             : BaseFileAppender.cs
// Created          : 2016-10-12  16:11
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Microsoft.Win32.SafeHandles;

// ReSharper disable InconsistentNaming

namespace Credit.Kolibre.Foundation.Logging.Internal
{
    /// <summary>
    ///     Base class for optimized file appenders.
    /// </summary>
    [SecuritySafeCritical]
    internal abstract class BaseFileAppender : IDisposable
    {
        private readonly Random _random = new Random();

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseFileAppender" /> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="createParameters">The create parameters.</param>
        protected BaseFileAppender(string fileName, ICreateFileParameters createParameters)
        {
            CreateFileParameters = createParameters;
            FileName = fileName;
            OpenTime = DateTime.UtcNow; // to be consistent with timeToKill in FileTarget.AutoClosingTimerCallback
            LastWriteTime = DateTime.MinValue;
            CaptureLastWriteTime = createParameters.CaptureLastWriteTime;
#if !SILVERLIGHT
            ArchiveMutex = CreateArchiveMutex();
#endif
        }

        protected bool CaptureLastWriteTime { get; }

        /// <summary>
        ///     Gets the path of the file, including file extension.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; }

        /// <summary>
        ///     Gets the file creation time.
        /// </summary>
        /// <value>The file creation time. DateTime value must be of UTC kind.</value>
        public DateTime CreationTime { get; private set; }

        /// <summary>
        ///     Gets the open time of the file.
        /// </summary>
        /// <value>The open time. DateTime value must be of UTC kind.</value>
        public DateTime OpenTime { get; private set; }

        /// <summary>
        ///     Gets the last write time.
        /// </summary>
        /// <value>The time the file was last written to. DateTime value must be of UTC kind.</value>
        public DateTime LastWriteTime { get; private set; }

        /// <summary>
        ///     Gets the file creation parameters.
        /// </summary>
        /// <value>The file creation parameters.</value>
        public ICreateFileParameters CreateFileParameters { get; }

#if !SILVERLIGHT
        /// <summary>
        ///     Gets the mutually-exclusive lock for archiving files.
        /// </summary>
        /// <value>The mutex for archiving.</value>
        public Mutex ArchiveMutex { get; private set; }
#endif

        #region IDisposable Members

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        ///     Closes this instance.
        /// </summary>
        public abstract void Close();

        /// <summary>
        ///     Flushes this instance.
        /// </summary>
        public abstract void Flush();

        public abstract DateTime? GetFileCreationTimeUtc();
        public abstract DateTime? GetFileLastWriteTimeUtc();
        public abstract long? GetFileLength();

        /// <summary>
        ///     Writes the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public abstract void Write(byte[] bytes);

        /// <summary>
        ///     Creates the file stream.
        /// </summary>
        /// <param name="allowFileSharedWriting">If set to <c>true</c> sets the file stream to allow shared writing.</param>
        /// <returns>A <see cref="FileStream" /> object which can be used to write to the file.</returns>
        protected FileStream CreateFileStream(bool allowFileSharedWriting)
        {
            int currentDelay = CreateFileParameters.ConcurrentWriteAttemptDelay;

            for (int i = 0; i < CreateFileParameters.ConcurrentWriteAttempts; ++i)
            {
                try
                {
                    try
                    {
                        return TryCreateFileStream(allowFileSharedWriting);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        //we don't check the directory on beforehand, as that will really slow down writing.
                        if (!CreateFileParameters.CreateDirs)
                        {
                            throw;
                        }
                        string directoryName = Path.GetDirectoryName(FileName);

                        Directory.CreateDirectory(directoryName);
                        return TryCreateFileStream(allowFileSharedWriting);
                    }
                }
                catch (IOException)
                {
                    if (!CreateFileParameters.ConcurrentWrites || i + 1 == CreateFileParameters.ConcurrentWriteAttempts)
                    {
                        throw; // rethrow
                    }

                    int actualDelay = _random.Next(currentDelay);
                    currentDelay *= 2;
                    Thread.Sleep(actualDelay);
                }
            }

            throw new InvalidOperationException("Should not be reached.");
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Close();
            }
        }

        /// <summary>
        ///     Updates the last write time of the file.
        /// </summary>
        protected void FileTouched()
        {
            if (CaptureLastWriteTime)
            {
                FileTouched(DateTime.UtcNow);
            }
        }

        /// <summary>
        ///     Updates the last write time of the file to the specified date.
        /// </summary>
        /// <param name="dateTime">Date and time when the last write occurred in UTC.</param>
        protected void FileTouched(DateTime dateTime)
        {
            LastWriteTime = dateTime;
        }

        private FileStream TryCreateFileStream(bool allowFileSharedWriting)
        {
            UpdateCreationTime();

#if !SILVERLIGHT && !MONO && !__IOS__ && !__ANDROID__
            try
            {
                if (!CreateFileParameters.ForceManaged && (Environment.OSVersion.Platform == PlatformID.Win32Windows || Environment.OSVersion.Platform == PlatformID.Win32NT))
                {
                    return WindowsCreateFile(FileName, allowFileSharedWriting);
                }
            }
            catch (SecurityException)
            {
            }
#endif

            FileShare fileShare = allowFileSharedWriting ? FileShare.ReadWrite : FileShare.Read;
            if (CreateFileParameters.EnableFileDelete && Environment.OSVersion.Platform != PlatformID.Win32Windows)
            {
                fileShare |= FileShare.Delete;
            }

            return new FileStream(
                FileName,
                FileMode.Append,
                FileAccess.Write,
                fileShare,
                CreateFileParameters.BufferSize);
        }

        private void UpdateCreationTime()
        {
            if (File.Exists(FileName))
            {
#if !SILVERLIGHT
                CreationTime = File.GetCreationTimeUtc(FileName);
#else
                CreationTime = File.GetCreationTime(this.FileName);
#endif
            }
            else
            {
                File.Create(FileName).Dispose();

#if !SILVERLIGHT
                CreationTime = DateTime.UtcNow;
                // Set the file's creation time to avoid being thwarted by Windows' Tunneling capabilities (https://support.microsoft.com/en-us/kb/172190).
                File.SetCreationTimeUtc(FileName, CreationTime);
#else
                CreationTime = File.GetCreationTime(this.FileName);
#endif
            }
        }

#if !SILVERLIGHT && !MONO && !__IOS__ && !__ANDROID__
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Objects are disposed elsewhere")]
        private FileStream WindowsCreateFile(string fileName, bool allowFileSharedWriting)
        {
            int fileShare = Win32FileNativeMethods.FILE_SHARE_READ;

            if (allowFileSharedWriting)
            {
                fileShare |= Win32FileNativeMethods.FILE_SHARE_WRITE;
            }

            if (CreateFileParameters.EnableFileDelete && Environment.OSVersion.Platform != PlatformID.Win32Windows)
            {
                fileShare |= Win32FileNativeMethods.FILE_SHARE_DELETE;
            }

            SafeFileHandle handle = null;
            FileStream fileStream = null;

            try
            {
                handle = Win32FileNativeMethods.CreateFile(
                    fileName,
                    Win32FileNativeMethods.FileAccess.GenericWrite,
                    fileShare,
                    IntPtr.Zero,
                    Win32FileNativeMethods.CreationDisposition.OpenAlways,
                    CreateFileParameters.FileAttributes,
                    IntPtr.Zero);

                if (handle.IsInvalid)
                {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }

                fileStream = new FileStream(handle, FileAccess.Write, CreateFileParameters.BufferSize);
                fileStream.Seek(0, SeekOrigin.End);
                return fileStream;
            }
            catch
            {
                fileStream?.Dispose();

                if ((handle != null) && !handle.IsClosed)
                    handle.Close();

                throw;
            }
        }
#endif

#if !SILVERLIGHT
        /// <summary>
        ///     Creates a mutually-exclusive lock for archiving files.
        /// </summary>
        /// <returns>A <see cref="Mutex" /> object which can be used for controlling the archiving of files.</returns>
        protected virtual Mutex CreateArchiveMutex()
        {
            return new Mutex();
        }

        /// <summary>
        ///     Creates a mutex for archiving that is sharable by more than one process.
        /// </summary>
        /// <returns>A <see cref="Mutex" /> object which can be used for controlling the archiving of files.</returns>
        protected Mutex CreateSharableArchiveMutex()
        {
            return CreateSharableMutex("FileArchiveLock");
        }

        /// <summary>
        ///     Creates a mutex that is sharable by more than one process.
        /// </summary>
        /// <param name="mutexNamePrefix">The prefix to use for the name of the mutex.</param>
        /// <returns>A <see cref="Mutex" /> object which is sharable by multiple processes.</returns>
        protected Mutex CreateSharableMutex(string mutexNamePrefix)
        {
            // Creates a mutex sharable by more than one process
            MutexSecurity mutexSecurity = new MutexSecurity();
            SecurityIdentifier everyoneSid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            mutexSecurity.AddAccessRule(new MutexAccessRule(everyoneSid, MutexRights.FullControl, AccessControlType.Allow));

            // The constructor will either create new mutex or open
            // an existing one, in a thread-safe manner
            bool createdNew;
            return new Mutex(false, GetMutexName(mutexNamePrefix), out createdNew, mutexSecurity);
        }

        private string GetMutexName(string mutexNamePrefix)
        {
            const string mutexNameFormatString = @"Global\NLog-File{0}-{1}";
            const int maxMutexNameLength = 260;

            string canonicalName = Path.GetFullPath(FileName).ToLowerInvariant();

            // Mutex names must not contain a backslash, it's the namespace separator,
            // but all other are OK
            canonicalName = canonicalName.Replace('\\', '/');
            string mutexName = string.Format(mutexNameFormatString, mutexNamePrefix, canonicalName);

            // A mutex name must not exceed MAX_PATH (260) characters
            if (mutexName.Length <= maxMutexNameLength)
            {
                return mutexName;
            }

            // The unusual case of the path being too long; let's hash the canonical name,
            // so it can be safely shortened and still remain unique
            string hash;
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(canonicalName));
                hash = Convert.ToBase64String(bytes);
            }

            // The hash makes the name unique, but also add the end of the path,
            // so the end of the name tells us which file it is (for debugging)
            mutexName = string.Format(mutexNameFormatString, mutexNamePrefix, hash);
            int cutOffIndex = canonicalName.Length - (maxMutexNameLength - mutexName.Length);
            return mutexName + canonicalName.Substring(cutOffIndex);
        }
#endif
    }
}