// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.Logging
// File             : IFileCompressor.cs
// Created          : 2016-11-17  11:55 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Credit.Kolibre.Foundation.Logging.Internal
{
    /// <summary>
    ///     <see cref="FileLogger" /> may be configured to compress archived files in a custom way
    ///     by setting
    ///     <see>
    ///         <cref>FileLogger.FileCompressor</cref>
    ///     </see>
    ///     before logging your first event.
    /// </summary>
    public interface IFileCompressor
    {
        /// <summary>
        ///     Create compressed archive file with <see cref="archiveFileName" /> by log file with <see cref="fileName" />.
        /// </summary>
        /// <param name="fileName">Absolute path to the log file to compress.</param>
        /// <param name="archiveFileName">Absolute path to the compressed archive file to create.</param>
        void CompressFile(string fileName, string archiveFileName);
    }
}