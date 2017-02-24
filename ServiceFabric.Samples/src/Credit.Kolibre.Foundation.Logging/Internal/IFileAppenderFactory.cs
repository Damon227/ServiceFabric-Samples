// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.Logging
// File             : IFileAppenderFactory.cs
// Created          : 2016-09-28  4:07 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Credit.Kolibre.Foundation.Logging.Internal
{
    /// <summary>
    ///     Interface implemented by all factories capable of creating file appenders.
    /// </summary>
    internal interface IFileAppenderFactory
    {
        /// <summary>
        ///     Opens the appender for given file name and parameters.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="parameters">Creation parameters.</param>
        /// <returns>Instance of <see cref="BaseFileAppender" /> which can be used to write to the file.</returns>
        BaseFileAppender Open(string fileName, ICreateFileParameters parameters);
    }
}