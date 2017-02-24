// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Identity
// File             : UserValidator.cs
// Created          : 2016-07-02  4:28 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Credit.Kolibre.Foundation.Static;
using Credit.Kolibre.Foundation.Sys;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Provides validation services for user classes.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public class UserValidator<TUser> : IUserValidator<TUser> where TUser : class
    {
        /// <summary>
        ///     Creates a new instance of <see cref="UserValidator{TUser}" />/
        /// </summary>
        /// <param name="errors">The <see cref="ErrorDescriber" /> used to provider error messages.</param>
        public UserValidator(ErrorDescriber errors = null)
        {
            Describer = errors ?? new ErrorDescriber();
        }

        /// <summary>
        ///     Gets the <see cref="ErrorDescriber" /> used to provider error messages for the current <see cref="UserValidator{TUser}" />.
        /// </summary>
        /// <value>The <see cref="ErrorDescriber" /> used to provider error messages for the current <see cref="UserValidator{TUser}" />.</value>
        public ErrorDescriber Describer { get; }

        #region IUserValidator<TUser> Members

        /// <summary>
        ///     Validates the specified <paramref name="user" /> as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager{TUser}" /> that can be used to retrieve user properties.</param>
        /// <param name="user">The user to validate.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" /> of the validation operation.</returns>
        public virtual async Task<Result> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            List<Error> errors = new List<Error>();

            await ValidateUserName(manager, user, errors);

            if (manager.SupportsUserEmail)
            {
                await ValidateEmail(manager, user, errors);
            }
            return errors.Count > 0 ? Result.Failed(errors.ToArray()) : Result.Success;
        }

        #endregion

        // make sure email is not empty, valid, and unique
        [SuppressMessage("ReSharper", "UnusedVariable")]
        private async Task ValidateEmail(UserManager<TUser> manager, TUser user, ICollection<Error> errors)
        {
            string email = await manager.GetEmailAsync(user);

            // Kolibre User Email and NormalizedEmail can be null
            //if (email.IsNullOrWhiteSpace())
            //{
            //    errors.Add(Describer.InvalidEmail(email));
            //    return;
            //}

            if (email != null && !REGEX.EMAIL_REGEX.IsMatch(email))
            {
                errors.Add(Describer.InvalidEmail(email));
                return;
            }

            if (email.IsNotNullOrEmpty() && manager.Options.User.RequireUniqueEmail)
            {
                TUser owner = await manager.FindByEmailAsync(email);
                if (owner != null &&
                    !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
                {
                    errors.Add(Describer.DuplicateEmail(email));
                }
            }
        }

        private async Task ValidateUserName(UserManager<TUser> manager, TUser user, ICollection<Error> errors)
        {
            string userName = await manager.GetUserNameAsync(user);
            if (userName.IsNullOrEmpty())
            {
                errors.Add(Describer.InvalidUserName(userName));
            }
            else if (manager.Options.User.AllowedUserNameCharacters.IsNotNullOrEmpty() &&
                     userName.Any(c => !manager.Options.User.AllowedUserNameCharacters.Contains(c)))
            {
                errors.Add(Describer.InvalidUserName(userName));
            }
            else
            {
                TUser owner = await manager.FindByNameAsync(userName);
                if (owner != null &&
                    !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user), StringComparison.InvariantCultureIgnoreCase))
                {
                    errors.Add(Describer.DuplicateUserName(userName));
                }
            }
        }
    }
}