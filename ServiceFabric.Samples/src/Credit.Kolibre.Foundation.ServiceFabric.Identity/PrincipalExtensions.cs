// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Identity
// File             : PrincipalExtensions.cs
// Created          : 2016-07-02  2:37 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Security.Claims;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Claims related extensions for <see cref="ClaimsPrincipal" />.
    /// </summary>
    public static class PrincipalExtensions
    {
        /// <summary>
        ///     Returns the value for the first claim of the specified type otherwise null the claim is not present.
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal" /> instance this method extends.</param>
        /// <param name="claimType">The claim type whose first value should be returned.</param>
        /// <returns>The value of the first instance of the specified claim type, or null if the claim is not present.</returns>
        public static string FindFirstValue(this ClaimsPrincipal principal, string claimType)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            Claim claim = principal.FindFirst(claimType);
            return claim != null ? claim.Value : null;
        }
    }
}