// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.Logging
// File             : ICreateFileParameters.cs
// Created          : 2016-09-28  5:00 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Credit.Kolibre.Foundation.Logging.Internal
{
    /// <summary>
    ///     Interface that provides parameters for create file function.
    /// </summary>
    internal interface ICreateFileParameters
    {
        /// <summary>
        ///     Gets or sets the delay in milliseconds to wait before attempting to write to the file again.
        /// </summary>
        int ConcurrentWriteAttemptDelay { get; }

        /// <summary>
        ///     Gets or sets the number of times the write is appended on the file before NLog
        ///     discards the log message.
        /// </summary>
        int ConcurrentWriteAttempts { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether concurrent writes to the log file by multiple processes on the same host.
        /// </summary>
        /// <remarks>
        ///     This makes multi-process logging possible. NLog uses a special technique
        ///     that lets it keep the files open for writing.
        /// </remarks>
        bool ConcurrentWrites { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether to create directories if they do not exist.
        /// </summary>
        /// <remarks>
        ///     Setting this to false may improve performance a bit, but you'll receive an error
        ///     when attempting to write to a directory that's not present.
        /// </remarks>
        bool CreateDirs { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether to enable log file(s) to be deleted.
        /// </summary>
        bool EnableFileDelete { get; }

        /// <summary>
        ///     Gets or sets the log file buffer size in bytes.
        /// </summary>
        int BufferSize { get; }

        /// <summary>
        ///     Gets or set a value indicating whether a managed file stream is forced, instead of used the native implementation.
        /// </summary>
        bool ForceManaged { get; }

#if !SILVERLIGHT
        /// <summary>
        ///     Gets or sets the file attributes (Windows only).
        /// </summary>
        Win32FileAttributes FileAttributes { get; }
#endif

        /// <summary>
        ///     Should we capture the last write time of a file?
        /// </summary>
        bool CaptureLastWriteTime { get; }
    }
}