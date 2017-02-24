// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Identity
// File             : IRoleValidator.cs
// Created          : 2016-07-06  10:38 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Provides an abstraction for a validating a role.
    /// </summary>
    /// <typeparam name="TRole">The type encapsulating a role.</typeparam>
    public interface IRoleValidator<TRole> where TRole : class
    {
        /// <summary>
        ///     Validates a role as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="RoleManager{TRole}" /> managing the role store.</param>
        /// <param name="role">The role to validate.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the <see cref="Result" /> of the asynchronous validation.</returns>
        Task<Result> ValidateAsync(RoleManager<TRole> manager, TRole role);
    }
}