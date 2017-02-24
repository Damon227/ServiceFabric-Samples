// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Identity
// File             : IdentityUser.cs
// Created          : 2016-07-02  4:28 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Credit.Kolibre.Foundation.Sys;
using Credit.Kolibre.Foundation.Utilities;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     The default implementation of <see cref="IdentityUser{TKey}" /> which uses a string as a primary key.
    /// </summary>
    public class IdentityUser : IdentityUser<string>
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="IdentityUser" />.
        /// </summary>
        /// <remarks>
        ///     The UserId property is initialized to from a new GUID string value.
        /// </remarks>
        public IdentityUser()
        {
            UserId = ID.NewSequentialGuid().ToGuidString();
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="IdentityUser" />.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <remarks>
        ///     The UserId property is initialized to from a new GUID string value.
        /// </remarks>
        public IdentityUser(string userName) : this()
        {
            UserName = userName;
        }
    }

    /// <summary>
    ///     Represents a user in the identity system
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the user.</typeparam>
    public class IdentityUser<TKey> : IdentityUser<TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>>
        where TKey : IEquatable<TKey>
    {
    }

    /// <summary>
    ///     Represents a user in the identity system
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the user.</typeparam>
    /// <typeparam name="TUserClaim">The type representing a claim.</typeparam>
    /// <typeparam name="TUserRole">The type representing a user role.</typeparam>
    public class IdentityUser<TKey, TUserClaim, TUserRole> where TKey : IEquatable<TKey>
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="IdentityUser{TKey}" />.
        /// </summary>
        public IdentityUser()
        {
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="IdentityUser{TKey}" />.
        /// </summary>
        /// <param name="userName">The user name.</param>
        public IdentityUser(string userName) : this()
        {
            UserName = userName;
        }

        /// <summary>
        ///     Gets or sets the primary key for this user.
        /// </summary>
        public virtual TKey UserId { get; set; }

        /// <summary>
        ///     Gets or sets the user name for this user.
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        ///     Gets or sets the normalized user name for this user.
        /// </summary>
        public virtual string NormalizedUserName { get; set; }

        /// <summary>
        ///     Gets or sets the email address for this user.
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        ///     Gets or sets the normalized email address for this user.
        /// </summary>
        public virtual string NormalizedEmail { get; set; }

        /// <summary>
        ///     Gets or sets a flag indicating if a user has confirmed their email address.
        /// </summary>
        /// <value>True if the email address has been confirmed, otherwise false.</value>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        ///     Gets or sets a salted and hashed representation of the password for this user.
        /// </summary>
        public virtual string PasswordHash { get; set; }

        /// <summary>
        ///     A random value that must change whenever a user is persisted to the store
        /// </summary>
        public virtual string ConcurrencyStamp { get; set; } = ID.NewSequentialGuid().ToGuidString();

        /// <summary>
        ///     Gets or sets a telecellphone for the user.
        /// </summary>
        public virtual string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets a flag indicating if a user has confirmed their telephone address.
        /// </summary>
        /// <value>True if the telecellphone has been confirmed, otherwise false.</value>
        public virtual bool CellphoneConfirmed { get; set; }

        /// <summary>
        ///     Gets or sets the date and time, in UTC, when any user lockout ends.
        /// </summary>
        /// <remarks>
        ///     A value in the past means the user is not locked out.
        /// </remarks>
        public virtual DateTimeOffset? LockoutEnd { get; set; }

        /// <summary>
        ///     Gets or sets a flag indicating if the user could be locked out.
        /// </summary>
        /// <value>True if the user could be locked out, otherwise false.</value>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the number of unsuccessfully login attempts for the current user.
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        ///     Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual ICollection<TUserRole> Roles { get; } = new List<TUserRole>();

        /// <summary>
        ///     Navigation property for the claims this user possesses.
        /// </summary>
        public virtual ICollection<TUserClaim> Claims { get; } = new List<TUserClaim>();

        /// <summary>
        ///     Returns the username for this user.
        /// </summary>
        public override string ToString()
        {
            return UserName;
        }
    }
}