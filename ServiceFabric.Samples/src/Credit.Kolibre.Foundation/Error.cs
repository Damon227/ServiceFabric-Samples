// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : Error.cs
// Created          : 2016-07-14  10:00 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Credit.Kolibre.Foundation
{
    /// <summary>
    ///     Encapsulates an error from the subsystem.
    /// </summary>
    public class Error
    {
        /// <summary>
        ///     Gets or sets the code for this error.
        /// </summary>
        /// <value>
        ///     The code for this error.
        /// </value>
        public int Code { get; set; }

        /// <summary>
        ///     Gets or sets the message for this error.
        /// </summary>
        /// <value>
        ///     The message for this error.
        /// </value>
        public string Message { get; set; }
    }
}