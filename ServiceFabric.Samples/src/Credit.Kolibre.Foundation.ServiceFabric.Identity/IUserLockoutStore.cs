// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Identity
// File             : IUserLockoutStore.cs
// Created          : 2016-07-01  7:23 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Provides an abstraction for a storing information which can be used to implement account lockout,
    ///     including access failures and lockout status
    /// </summary>
    /// <typeparam name="TUser">The type that represents a user.</typeparam>
    public interface IUserLockoutStore<TUser> : IUserStore<TUser> where TUser : class
    {
        /// <summary>
        ///     Retrieves the current unsuccessfully access count for the specified <paramref name="user" />..
        /// </summary>
        /// <param name="user">The user whose unsuccessfully access count should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation, containing the unsuccessfully access count.</returns>
        Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        ///     Retrieves a flag indicating whether user lockout can enabled for the specified user.
        /// </summary>
        /// <param name="user">The user whose ability to be locked out should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, true if a user can be locked out, otherwise false.
        /// </returns>
        Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        ///     Gets the last <see cref="DateTimeOffset" /> a user's last lockout expired, if any.
        ///     Any time in the past should be indicates a user is not locked out.
        /// </summary>
        /// <param name="user">The user whose lockout date should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        ///     A <see cref="Task{TResult}" /> that represents the result of the asynchronous query, a <see cref="DateTimeOffset" /> containing the last time
        ///     a user's lockout expired, if any.
        /// </returns>
        Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        ///     Records that a unsuccessfully access has occurred, incrementing the unsuccessfully access count.
        /// </summary>
        /// <param name="user">The user whose cancellation count should be incremented.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation, containing the incremented unsuccessfully access count.</returns>
        Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        ///     Resets a user's unsuccessfully access count.
        /// </summary>
        /// <param name="user">The user whose unsuccessfully access count should be reset.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>This is typically called after the account is successfully accessed.</remarks>
        Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        ///     Set the flag indicating if the specified <paramref name="user" /> can be locked out..
        /// </summary>
        /// <param name="user">The user whose ability to be locked out should be set.</param>
        /// <param name="enabled">A flag indicating if lock out can be enabled for the specified <paramref name="user" />.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation.</returns>
        Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken);

        /// <summary>
        ///     Locks out a user until the specified end date has passed. Setting a end date in the past immediately unlocks a user.
        /// </summary>
        /// <param name="user">The user whose lockout date should be set.</param>
        /// <param name="lockoutEnd">The <see cref="DateTimeOffset" /> after which the <paramref name="user" />'s lockout should end.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation.</returns>
        Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken);
    }
}