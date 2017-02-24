// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Identity
// File             : IdentityRoleClaim.cs
// Created          : 2016-07-02  11:10 AM
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
    ///     Represents a claim that is granted to all users within a role.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key of the role associated with this claim.</typeparam>
    public class IdentityRoleClaim<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        ///     Gets or sets the identifier for this role claim.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        ///     Gets or sets the of the primary key of the role associated with this claim.
        /// </summary>
        public virtual TKey RoleId { get; set; }

        /// <summary>
        ///     Gets or sets the claim type for this claim.
        /// </summary>
        public virtual string ClaimType { get; set; }

        /// <summary>
        ///     Gets or sets the claim value for this claim.
        /// </summary>
        public virtual string ClaimValue { get; set; }

        public virtual void InitializeFromClaim(Claim other)
        {
            ClaimType = other?.Type;
            ClaimValue = other?.Value;
        }

        public virtual Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }
    }
}