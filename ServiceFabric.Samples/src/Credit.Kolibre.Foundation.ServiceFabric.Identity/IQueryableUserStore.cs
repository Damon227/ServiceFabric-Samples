// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Identity
// File             : IQueryableUserStore.cs
// Created          : 2016-07-01  7:23 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Linq;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Provides an abstraction for querying roles in a User store.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public interface IQueryableUserStore<TUser> : IUserStore<TUser> where TUser : class
    {
        /// <summary>
        ///     Returns an <see cref="IQueryable{T}" /> collection of users.
        /// </summary>
        /// <value>An <see cref="IQueryable{T}" /> collection of users.</value>
        IQueryable<TUser> Users { get; }
    }
}