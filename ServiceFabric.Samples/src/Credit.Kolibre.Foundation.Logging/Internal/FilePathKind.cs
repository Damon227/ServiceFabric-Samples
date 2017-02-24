// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.Logging
// File             : FilePathKind.cs
// Created          : 2016-09-28  3:54 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Credit.Kolibre.Foundation.Logging.Internal
{
    /// <summary>
    ///     Type of filepath
    /// </summary>
    public enum FilePathKind : byte
    {
        /// <summary>
        ///     Detect of relative or absolute
        /// </summary>
        Unknown,

        /// <summary>
        ///     Relative path
        /// </summary>
        Relative,

        /// <summary>
        ///     Absolute path
        /// </summary>
        /// <remarks>Best for performance</remarks>
        Absolute
    }
}