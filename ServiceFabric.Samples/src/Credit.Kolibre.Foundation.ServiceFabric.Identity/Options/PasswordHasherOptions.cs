﻿// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Identity
// File             : PasswordHasherOptions.cs
// Created          : 2016-07-01  4:37 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Security.Cryptography;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity.Options
{
    /// <summary>
    ///     Specifies options for password hashing.
    /// </summary>
    public class PasswordHasherOptions
    {
        private static readonly RandomNumberGenerator s_defaultRng = RandomNumberGenerator.Create(); // secure PRNG

        /// <summary>
        ///     Gets or sets the compatibility mode used when hashing passwords.
        /// </summary>
        /// <value>
        ///     The compatibility mode used when hashing passwords.
        /// </value>
        /// <remarks>
        ///     The default compatibility mode is 'ASP.NET Identity version 3'.
        /// </remarks>
        public PasswordHasherCompatibilityMode CompatibilityMode { get; set; } = PasswordHasherCompatibilityMode.IdentityV3;

        /// <summary>
        ///     Gets or sets the number of iterations used when hashing passwords using PBKDF2.
        /// </summary>
        /// <value>
        ///     The number of iterations used when hashing passwords using PBKDF2.
        /// </value>
        /// <remarks>
        ///     This value is only used when the compatibility mode is set to 'V3'.
        ///     The value must be a positive integer. The default value is 10,000.
        /// </remarks>
        public int IterationCount { get; set; } = 10000;

        // for unit testing
        internal RandomNumberGenerator Rng { get; set; } = s_defaultRng;
    }
}