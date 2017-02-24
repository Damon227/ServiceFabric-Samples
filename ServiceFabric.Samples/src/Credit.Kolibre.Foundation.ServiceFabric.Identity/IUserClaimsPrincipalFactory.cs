﻿// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Identity
// File             : IUserClaimsPrincipalFactory.cs
// Created          : 2016-07-02  3:34 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Security.Claims;
using System.Threading.Tasks;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Provides an abstraction for a factory to create a <see cref="ClaimsPrincipal" /> from a user.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public interface IUserClaimsPrincipalFactory<in TUser>
        where TUser : class
    {
        /// <summary>
        ///     Creates a <see cref="ClaimsPrincipal" /> from an user asynchronously.
        /// </summary>
        /// <param name="user">The user to create a <see cref="ClaimsPrincipal" /> from.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous creation operation, containing the created <see cref="ClaimsPrincipal" />.</returns>
        Task<ClaimsPrincipal> CreateAsync(TUser user);
    }
}