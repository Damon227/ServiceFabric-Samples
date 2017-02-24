// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Identity
// File             : LockoutOptions.cs
// Created          : 2016-07-01  8:12 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity.Options
{
    /// <summary>
    ///     Options for configuring user lockout.
    /// </summary>
    public class LockoutOptions
    {
        /// <value>
        ///     True if a newly created user can be locked out, otherwise false.
        /// </value>
        /// <remarks>
        ///     Defaults to true.
        /// </remarks>
        public bool AllowedForNewUsers { get; set; } = true;

        /// <summary>
        ///     Gets or sets the number of unsuccessfully access attempts allowed before a user is locked out,
        ///     assuming lock out is enabled.
        /// </summary>
        /// <value>
        ///     The number of unsuccessfully access attempts allowed before a user is locked out, if lockout is enabled.
        /// </value>
        /// <remarks>Defaults to 5 unsuccessfully attempts before an account is locked out.</remarks>
        public int MaxFailedAccessAttempts { get; set; } = 5;

        /// <summary>
        ///     Gets or sets the <see cref="TimeSpan" /> a user is locked out for when a lockout occurs.
        /// </summary>
        /// <value>The <see cref="TimeSpan" /> a user is locked out for when a lockout occurs.</value>
        /// <remarks>Defaults to 5 minutes.</remarks>
        public TimeSpan DefaultLockoutTimeSpan { get; set; } = TimeSpan.FromHours(1);
    }
}