﻿// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Identity
// File             : IPasswordValidator.cs
// Created          : 2016-08-23  10:31
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Provides an abstraction for validating passwords.
    /// </summary>
    /// <typeparam name="TUser">The type that represents a user.</typeparam>
    public interface IPasswordValidator<TUser> where TUser : class
    {
        /// <summary>
        ///     Validates a password as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager{TUser}" /> to retrieve the <paramref name="user" /> properties from.</param>
        /// <param name="user">The user whose password should be validated.</param>
        /// <param name="password">The password supplied for validation</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<Result> ValidateAsync(UserManager<TUser> manager, TUser user, string password);

        /// <summary>
        ///     Validates a password as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager{TUser}" /> to retrieve the <paramref name="user" /> properties from.</param>
        /// <param name="user">The user whose password should be validated.</param>
        /// <param name="password">The password supplied for validation</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<Result> ValidateSimpleAsync(UserManager<TUser> manager, TUser user, string password);
    }
}