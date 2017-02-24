// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Identity
// File             : PasswordHasherCompatibilityMode.cs
// Created          : 2016-07-01  4:35 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Specifies the format used for hashing passwords.
    /// </summary>
    public enum PasswordHasherCompatibilityMode
    {
        /// <summary>
        ///     Indicates hashing passwords in a way that is compatible with ASP.NET Identity versions 1 and 2.
        /// </summary>
        IdentityV2,

        /// <summary>
        ///     Indicates hashing passwords in a way that is compatible with ASP.NET Identity version 3.
        /// </summary>
        IdentityV3
    }
}