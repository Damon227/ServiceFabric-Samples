// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Identity
// File             : IdentityUserRole.cs
// Created          : 2016-07-01  7:46 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Represents the link between a user and a role.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key used for users and roles.</typeparam>
    public class IdentityUserRole<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        ///     Gets or sets the primary key of the user that is linked to a role.
        /// </summary>
        public virtual TKey UserId { get; set; }

        /// <summary>
        ///     Gets or sets the primary key of the role that is linked to the user.
        /// </summary>
        public virtual TKey RoleId { get; set; }
    }
}