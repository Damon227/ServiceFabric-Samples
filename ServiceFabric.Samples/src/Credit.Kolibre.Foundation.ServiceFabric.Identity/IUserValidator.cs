// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Identity
// File             : IUserValidator.cs
// Created          : 2016-07-01  8:12 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Provides an abstraction for user validation.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public interface IUserValidator<TUser> where TUser : class
    {
        /// <summary>
        ///     Validates the specified <paramref name="user" /> as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager{TUser}" /> that can be used to retrieve user properties.</param>
        /// <param name="user">The user to validate.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" /> of the validation operation.</returns>
        Task<Result> ValidateAsync(UserManager<TUser> manager, TUser user);
    }
}