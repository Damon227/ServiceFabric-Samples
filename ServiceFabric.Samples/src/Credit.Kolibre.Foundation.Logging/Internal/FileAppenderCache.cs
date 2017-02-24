// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.Logging
// File             : FileAppenderCache.cs
// Created          : 2016-10-11  3:03 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.IO;
using System.Threading;

namespace Credit.Kolibre.Foundation.Logging.Internal
{
    /// <summary>
    ///     Maintains a collection of file appenders usually associated with file targets.
    /// </summary>
    internal sealed class FileAppenderCache
    {
        /// <summary>
        ///     An "empty" instance of the <see cref="FileAppenderCache" /> class with zero size and empty list of appenders.
        /// </summary>
        public static readonly FileAppenderCache Empty = new FileAppenderCache();

        private readonly BaseFileAppender[] _appenders;

        /// <summary>
        ///     Initializes a new "empty" instance of the <see cref="FileAppenderCache" /> class with zero size and empty
        ///     list of appenders.
        /// </summary>
        private FileAppenderCache() : this(0, null, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileAppenderCache" /> class.
        /// </summary>
        /// <remarks>
        ///     The size of the list should be positive. No validations are performed during initialisation as it is an
        ///     intenal class.
        /// </remarks>
        /// <param name="size">Total number of appenders allowed in list.</param>
        /// <param name="appenderFactory">Factory used to create each appender.</param>
        /// <param name="createFileParams">Parameters used for creating a file.</param>
        public FileAppenderCache(int size, IFileAppenderFactory appenderFactory, ICreateFileParameters createFileParams)
        {
            Size = size;
            Factory = appenderFactory;
            CreateFileParameters = createFileParams;

            _appenders = new BaseFileAppender[Size];

#if !SILVERLIGHT && !__IOS__ && !__ANDROID__
            _externalFileArchivingWatcher.OnChange += ExternalFileArchivingWatcher_OnChange;
#endif
        }

        /// <summary>
        ///     Gets the parameters which will be used for creating a file.
        /// </summary>
        public ICreateFileParameters CreateFileParameters { get; }

        /// <summary>
        ///     Gets the file appender factory used by all the appenders in this list.
        /// </summary>
        public IFileAppenderFactory Factory { get; }

        /// <summary>
        ///     Gets the number of appenders which the list can hold.
        /// </summary>
        public int Size { get; }

        /// <summary>
        ///     It allocates the first slot in the list when the file name does not already in the list and clean up any
        ///     unused slots.
        /// </summary>
        /// <param name="fileName">File name associated with a single appender.</param>
        /// <returns>The allocated appender.</returns>
        /// <exception cref="NullReferenceException">
        ///     Thrown when <see cref="M:AllocateAppender" /> is called on an <c>Empty</c><see cref="FileAppenderCache" /> instance.
        /// </exception>
        public BaseFileAppender AllocateAppender(string fileName)
        {
            //
            // BaseFileAppender.Write is the most expensive operation here
            // so the in-memory data structure doesn't have to be 
            // very sophisticated. It's a table-based LRU, where we move 
            // the used element to become the first one.
            // The number of items is usually very limited so the 
            // performance should be equivalent to the one of the hashtable.
            //

            BaseFileAppender appenderToWrite = null;
            int freeSpot = _appenders.Length - 1;

            for (int i = 0; i < _appenders.Length; ++i)
            {
                // Use empty slot in recent appender list, if there is one.
                if (_appenders[i] == null)
                {
                    freeSpot = i;
                    break;
                }

                if (_appenders[i].FileName == fileName)
                {
                    // found it, move it to the first place on the list
                    // (MRU)

                    // file open has a chance of failure
                    // if it fails in the constructor, we won't modify any data structures
                    BaseFileAppender app = _appenders[i];
                    for (int j = i; j > 0; --j)
                    {
                        _appenders[j] = _appenders[j - 1];
                    }

                    _appenders[0] = app;
                    appenderToWrite = app;
                    break;
                }
            }

            if (appenderToWrite == null)
            {
                BaseFileAppender newAppender = Factory.Open(fileName, CreateFileParameters);

                if (_appenders[freeSpot] != null)
                {
                    CloseAppender(_appenders[freeSpot]);
                    _appenders[freeSpot] = null;
                }

                for (int j = freeSpot; j > 0; --j)
                {
                    _appenders[j] = _appenders[j - 1];
                }

                _appenders[0] = newAppender;
                appenderToWrite = newAppender;

#if !SILVERLIGHT && !__IOS__ && !__ANDROID__
                if (!string.IsNullOrEmpty(_archiveFilePatternToWatch))
                {
                    string directoryPath = Path.GetDirectoryName(_archiveFilePatternToWatch);
                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);

                    _externalFileArchivingWatcher.Watch(_archiveFilePatternToWatch);
                }
#endif
            }

            return appenderToWrite;
        }

        /// <summary>
        ///     Close all the allocated appenders.
        /// </summary>
        public void CloseAppenders()
        {
            if (_appenders != null)
            {
                for (int i = 0; i < _appenders.Length; ++i)
                {
                    if (_appenders[i] == null)
                    {
                        break;
                    }

                    CloseAppender(_appenders[i]);
                    _appenders[i] = null;
                }
            }
        }

        /// <summary>
        ///     Close the allocated appenders initialised before the supplied time.
        /// </summary>
        /// <param name="expireTime">The time which prior the appenders considered expired</param>
        public void CloseAppenders(DateTime expireTime)
        {
            for (int i = 0; i < _appenders.Length; ++i)
            {
                if (_appenders[i] == null)
                {
                    break;
                }

                if (_appenders[i].OpenTime < expireTime)
                {
                    for (int j = i; j < _appenders.Length; ++j)
                    {
                        if (_appenders[j] == null)
                        {
                            break;
                        }

                        CloseAppender(_appenders[j]);
                        _appenders[j] = null;
                    }

                    break;
                }
            }
        }

        /// <summary>
        ///     Fluch all the allocated appenders.
        /// </summary>
        public void FlushAppenders()
        {
            foreach (BaseFileAppender appender in _appenders)
            {
                if (appender == null)
                {
                    break;
                }

                appender.Flush();
            }
        }

#if !SILVERLIGHT
        public Mutex GetArchiveMutex(string fileName)
        {
            BaseFileAppender appender = GetAppender(fileName);
            return appender == null ? null : appender.ArchiveMutex;
        }
#endif

        public DateTime? GetFileCreationTimeUtc(string filePath, bool fallback)
        {
            BaseFileAppender appender = GetAppender(filePath);
            DateTime? result = null;
            if (appender != null)
                result = appender.GetFileCreationTimeUtc();
            if (result == null && fallback)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    throw new NotImplementedException();
                    //return fileInfo.GetCreationTimeUtc();
                }
            }

            return result;
        }

        public DateTime? GetFileLastWriteTimeUtc(string filePath, bool fallback)
        {
            BaseFileAppender appender = GetAppender(filePath);
            DateTime? result = null;
            if (appender != null)
                result = appender.GetFileLastWriteTimeUtc();
            if (result == null && fallback)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    throw new NotImplementedException();
                    // return fileInfo.GetLastWriteTimeUtc();
                }
            }

            return result;
        }

        public long? GetFileLength(string filePath, bool fallback)
        {
            BaseFileAppender appender = GetAppender(filePath);
            long? result = null;
            if (appender != null)
                result = appender.GetFileLength();
            if (result == null && fallback)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    return fileInfo.Length;
                }
            }

            return result;
        }

        /// <summary>
        ///     Closes the specified appender and removes it from the list.
        /// </summary>
        /// <param name="filePath">File name of the appender to be closed.</param>
        public void InvalidateAppender(string filePath)
        {
            for (int i = 0; i < _appenders.Length; ++i)
            {
                if (_appenders[i] == null)
                {
                    break;
                }

                if (_appenders[i].FileName == filePath)
                {
                    CloseAppender(_appenders[i]);
                    for (int j = i; j < _appenders.Length - 1; ++j)
                    {
                        _appenders[j] = _appenders[j + 1];
                    }

                    _appenders[_appenders.Length - 1] = null;
                    break;
                }
            }
        }

        private void CloseAppender(BaseFileAppender appender)
        {
            appender.Close();

#if !SILVERLIGHT && !__IOS__ && !__ANDROID__
            _externalFileArchivingWatcher.StopWatching();
#endif
        }

        private BaseFileAppender GetAppender(string fileName)
        {
            foreach (BaseFileAppender appender in _appenders)
            {
                if (appender == null)
                    break;

                if (appender.FileName == fileName)
                    return appender;
            }

            return null;
        }

#if !SILVERLIGHT && !__IOS__ && !__ANDROID__
        private string _archiveFilePatternToWatch;
        private readonly MultiFileWatcher _externalFileArchivingWatcher = new MultiFileWatcher(NotifyFilters.FileName);
        private bool _logFileWasArchived;
#endif

#if !SILVERLIGHT && !__IOS__ && !__ANDROID__
        private void ExternalFileArchivingWatcher_OnChange(object sender, FileSystemEventArgs e)
        {
            if ((e.ChangeType & WatcherChangeTypes.Created) == WatcherChangeTypes.Created)
                _logFileWasArchived = true;
        }

        /// <summary>
        ///     The archive file path pattern that is used to detect when archiving occurs.
        /// </summary>
        public string ArchiveFilePatternToWatch
        {
            get { return _archiveFilePatternToWatch; }
            set
            {
                if (_archiveFilePatternToWatch != value)
                {
                    _archiveFilePatternToWatch = value;

                    _logFileWasArchived = false;
                    _externalFileArchivingWatcher.StopWatching();
                }
            }
        }

        /// <summary>
        ///     Invalidates appenders for all files that were archived.
        /// </summary>
        public void InvalidateAppendersForInvalidFiles()
        {
            if (_logFileWasArchived)
            {
                CloseAppenders();
                _logFileWasArchived = false;
            }
        }
#endif
    }
}