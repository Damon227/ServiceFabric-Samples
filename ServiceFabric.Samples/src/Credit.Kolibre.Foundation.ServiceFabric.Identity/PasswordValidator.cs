// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Identity
// File             : PasswordValidator.cs
// Created          : 2016-08-23  10:31
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Credit.Kolibre.Foundation.ServiceFabric.Identity.Options;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Provides the default password policy for Identity.
    /// </summary>
    /// <typeparam name="TUser">The type that represents a user.</typeparam>
    public class PasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : class
    {
        /// <summary>
        ///     Constructions a new instance of <see cref="PasswordValidator{TUser}" />.
        /// </summary>
        /// <param name="errors">The <see cref="ErrorDescriber" /> to retrieve error text from.</param>
        public PasswordValidator(ErrorDescriber errors = null)
        {
            Describer = errors ?? new ErrorDescriber();
        }

        /// <summary>
        ///     Gets the <see cref="ErrorDescriber" /> used to supply error text.
        /// </summary>
        /// <value>The <see cref="ErrorDescriber" /> used to supply error text.</value>
        public ErrorDescriber Describer { get; }

        #region IPasswordValidator<TUser> Members

        /// <summary>
        ///     Validates a password as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager{TUser}" /> to retrieve the <paramref name="user" /> properties from.</param>
        /// <param name="user">The user whose password should be validated.</param>
        /// <param name="password">The password supplied for validation</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual Task<Result> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            List<Error> errors = new List<Error>();
            PasswordOptions options = manager.Options.Password;
            if (string.IsNullOrWhiteSpace(password) || password.Length < options.RequiredLength)
            {
                errors.Add(Describer.PasswordTooShort(options.RequiredLength));
            }
            //if (options.RequireNonAlphanumeric && password.All(IsLetterOrDigit))
            //{
            //    errors.Add(Describer.PasswordRequiresNonAlphanumeric());
            //}
            //if (options.RequireDigit && !password.Any(IsDigit))
            //{
            //    errors.Add(Describer.PasswordRequiresDigit());
            //}
            //if (options.RequireLowercase && !password.Any(IsLower))
            //{
            //    errors.Add(Describer.PasswordRequiresLower());
            //}
            //if (options.RequireUppercase && !password.Any(IsUpper))
            //{
            //    errors.Add(Describer.PasswordRequiresUpper());
            //}
            return
                Task.FromResult(errors.Count == 0
                    ? Result.Success
                    : Result.Failed(errors.ToArray()));
        }

        /// <summary>
        ///     Validates a password as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager{TUser}" /> to retrieve the <paramref name="user" /> properties from.</param>
        /// <param name="user">The user whose password should be validated.</param>
        /// <param name="password">The password supplied for validation</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual Task<Result> ValidateSimpleAsync(UserManager<TUser> manager, TUser user, string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            List<Error> errors = new List<Error>();
            PasswordOptions options = manager.Options.Password;
            if (string.IsNullOrWhiteSpace(password) || password.Length < options.RequiredLength)
            {
                errors.Add(Describer.PasswordTooShort(options.RequiredLength));
            }
            //if (options.RequireNonAlphanumeric && password.All(IsLetterOrDigit))
            //{
            //    errors.Add(Describer.PasswordRequiresNonAlphanumeric());
            //}
            //if (options.RequireDigit && !password.Any(IsDigit))
            //{
            //    errors.Add(Describer.PasswordRequiresDigit());
            //}
            //if (options.RequireLowercase && !password.Any(IsLower))
            //{
            //    errors.Add(Describer.PasswordRequiresLower());
            //}
            //if (options.RequireUppercase && !password.Any(IsUpper))
            //{
            //    errors.Add(Describer.PasswordRequiresUpper());
            //}
            return
                Task.FromResult(errors.Count == 0
                    ? Result.Success
                    : Result.Failed(errors.ToArray()));
        }

        #endregion

        /// <summary>
        ///     Returns a flag indicting whether the supplied character is a digit.
        /// </summary>
        /// <param name="c">The character to check if it is a digit.</param>
        /// <returns>True if the character is a digit, otherwise false.</returns>
        public virtual bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        /// <summary>
        ///     Returns a flag indicting whether the supplied character is an ASCII letter or digit.
        /// </summary>
        /// <param name="c">The character to check if it is an ASCII letter or digit.</param>
        /// <returns>True if the character is an ASCII letter or digit, otherwise false.</returns>
        public virtual bool IsLetterOrDigit(char c)
        {
            return IsUpper(c) || IsLower(c) || IsDigit(c);
        }

        /// <summary>
        ///     Returns a flag indicting whether the supplied character is a lower case ASCII letter.
        /// </summary>
        /// <param name="c">The character to check if it is a lower case ASCII letter.</param>
        /// <returns>True if the character is a lower case ASCII letter, otherwise false.</returns>
        public virtual bool IsLower(char c)
        {
            return c >= 'a' && c <= 'z';
        }

        /// <summary>
        ///     Returns a flag indicting whether the supplied character is an upper case ASCII letter.
        /// </summary>
        /// <param name="c">The character to check if it is an upper case ASCII letter.</param>
        /// <returns>True if the character is an upper case ASCII letter, otherwise false.</returns>
        public virtual bool IsUpper(char c)
        {
            return c >= 'A' && c <= 'Z';
        }
    }
}