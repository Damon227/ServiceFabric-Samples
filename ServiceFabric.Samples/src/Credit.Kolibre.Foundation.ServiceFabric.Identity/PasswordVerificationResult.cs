// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Identity
// File             : PasswordVerificationResult.cs
// Created          : 2016-07-01  8:12 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Specifies the results for password verification.
    /// </summary>
    public enum PasswordVerificationResult
    {
        /// <summary>
        ///     Indicates password verification unsuccessfully.
        /// </summary>
        Failed = 0,

        /// <summary>
        ///     Indicates password verification was successful.
        /// </summary>
        Success = 1,

        /// <summary>
        ///     Indicates password verification was successful however the password was encoded using a deprecated algorithm
        ///     and should be rehashed and updated.
        /// </summary>
        SuccessRehashNeeded = 2
    }
}