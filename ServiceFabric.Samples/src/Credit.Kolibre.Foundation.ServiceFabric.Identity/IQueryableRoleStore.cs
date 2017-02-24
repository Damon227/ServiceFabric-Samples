// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Identity
// File             : IQueryableRoleStore.cs
// Created          : 2016-07-06  10:47 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Linq;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Provides an abstraction for querying roles in a Role store.
    /// </summary>
    /// <typeparam name="TRole">The type encapsulating a role.</typeparam>
    public interface IQueryableRoleStore<TRole> : IRoleStore<TRole> where TRole : class
    {
        /// <summary>
        ///     Returns an <see cref="IQueryable{T}" /> collection of roles.
        /// </summary>
        /// <value>An <see cref="IQueryable{T}" /> collection of roles.</value>
        IQueryable<TRole> Roles { get; }
    }
}