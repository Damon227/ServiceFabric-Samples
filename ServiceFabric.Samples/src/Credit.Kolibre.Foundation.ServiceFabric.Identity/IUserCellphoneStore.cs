// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Identity
// File             : IUserPhoneNumberStore.cs
// Created          : 2016-07-02  4:28 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading;
using System.Threading.Tasks;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Provides an abstraction for a store containing users' telecellphones.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public interface IUserCellphoneStore<TUser> : IUserStore<TUser> where TUser : class
    {
        /// <summary>
        ///     Finds and returns a user, if any, who has the specified cellphone.
        /// </summary>
        /// <param name="cellphone">The cellphone to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the user matching the specified <paramref name="cellphone" /> if it exists.
        /// </returns>
        Task<TUser> FindByCellphoneAsync(string cellphone, CancellationToken cancellationToken);

        /// <summary>
        ///     Gets the telecellphone, if any, for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose telecellphone should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation, containing the user's telecellphone, if any.</returns>
        Task<string> GetCellphoneAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        ///     Gets a flag indicating whether the specified <paramref name="user" />'s telecellphone has been confirmed.
        /// </summary>
        /// <param name="user">The user to return a flag for, indicating whether their telecellphone is confirmed.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, returning true if the specified <paramref name="user" /> has a confirmed
        ///     telecellphone otherwise false.
        /// </returns>
        Task<bool> GetCellphoneConfirmedAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        ///     Sets the telecellphone for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose telecellphone should be set.</param>
        /// <param name="cellphone">The telecellphone to set.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation.</returns>
        Task SetCellphoneAsync(TUser user, string cellphone, CancellationToken cancellationToken);

        /// <summary>
        ///     Sets a flag indicating if the specified <paramref name="user" />'s cellphone has been confirmed..
        /// </summary>
        /// <param name="user">The user whose telecellphone confirmation status should be set.</param>
        /// <param name="confirmed">A flag indicating whether the user's telecellphone has been confirmed.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation.</returns>
        Task SetCellphoneConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken);
    }
}