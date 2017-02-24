﻿// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Identity
// File             : IPasswordHasher.cs
// Created          : 2016-07-02  4:28 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Provides an abstraction for hashing passwords.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        ///     Returns a hashed representation of the supplied <paramref name="password" /> for the specified user.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>A hashed representation of the supplied <paramref name="password" /> for the specified user.</returns>
        string HashPassword(string password);

        /// <summary>
        ///     Returns a <see cref="PasswordVerificationResult" /> indicating the result of a password hash comparison.
        /// </summary>
        /// <param name="hashedPassword">The hash value for a user's stored password.</param>
        /// <param name="providedPassword">The password supplied for comparison.</param>
        /// <returns>A <see cref="PasswordVerificationResult" /> indicating the result of a password hash comparison.</returns>
        /// <remarks>Implementations of this method should be time consistent.</remarks>
        PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword);
    }
}