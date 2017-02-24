// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Identity
// File             : IdentityErrorDescriber.cs
// Created          : 2016-08-23  10:04 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Credit.Kolibre.Foundation.Logging;
using Credit.Kolibre.Foundation.Sys;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Service to enable localization for application facing errors.
    /// </summary>
    /// <remarks>
    ///     These errors are returned to controllers and are generally used as display messages to end users.
    /// </remarks>
    public class ErrorDescriber
    {
        /// <summary>
        ///     Returns an <see cref="Error" /> indicating a concurrency failure.
        /// </summary>
        /// <returns>An <see cref="Error" /> indicating a concurrency failure.</returns>
        public virtual Error ConcurrencyFailure()
        {
            return new Error
            {
                Code = EventCode.CREDIT_KOLIBRE_IDENTITY_ERROR_CONCURRENCY_FAILURE,
                Message = Resource.ConcurrencyFailure
            };
        }

        /// <summary>
        ///     Returns the default <see cref="Error" />.
        /// </summary>
        /// <returns>The default <see cref="Error" />,</returns>
        public virtual Error DefaultError()
        {
            return new Error
            {
                Code = EventCode.CREDIT_KOLIBRE_IDENTITY_ERROR_DEFAULT,
                Message = Resource.DefaultError
            };
        }

        /// <summary>
        ///     Returns an <see cref="Error" /> indicating the specified <paramref name="email" /> is already associated with an account.
        /// </summary>
        /// <param name="email">The email that is already associated with an account.</param>
        /// <returns>An <see cref="Error" /> indicating the specified <paramref name="email" /> is already associated with an account.</returns>
        public virtual Error DuplicateEmail(string email)
        {
            return new Error
            {
                Code = EventCode.CREDIT_KOLIBRE_IDENTITY_ERROR_DUPLICATE_EMAIL,
                Message = Resource.DuplicateEmail.FormatWith(email)
            };
        }

        /// <summary>
        ///     Returns an <see cref="Error" /> indicating the specified <paramref name="role" /> name already exists.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="Error" /> indicating the specific role <paramref name="role" /> name already exists.</returns>
        public virtual Error DuplicateRoleName(string role)
        {
            return new Error
            {
                Code = EventCode.CREDIT_KOLIBRE_IDENTITY_ERROR_DUPLICATE_ROLE_NAME,
                Message = Resource.DuplicateRoleName.FormatWith(role)
            };
        }

        /// <summary>
        ///     Returns an <see cref="Error" /> indicating the specified <paramref name="userName" /> already exists.
        /// </summary>
        /// <param name="userName">The user name that already exists.</param>
        /// <returns>An <see cref="Error" /> indicating the specified <paramref name="userName" /> already exists.</returns>
        public virtual Error DuplicateUserName(string userName)
        {
            return new Error
            {
                Code = EventCode.CREDIT_KOLIBRE_IDENTITY_ERROR_DUPLICATE_USER_NAME,
                Message = Resource.DuplicateUserName.FormatWith(userName)
            };
        }

        /// <summary>
        ///     Returns an <see cref="Error" /> indicating the specified <paramref name="email" /> is invalid.
        /// </summary>
        /// <param name="email">The email that is invalid.</param>
        /// <returns>An <see cref="Error" /> indicating the specified <paramref name="email" /> is invalid.</returns>
        public virtual Error InvalidEmail(string email)
        {
            return new Error
            {
                Code = EventCode.CREDIT_KOLIBRE_IDENTITY_ERROR_INVALID_EMAIL,
                Message = Resource.InvalidEmail.FormatWith(email)
            };
        }

        /// <summary>
        ///     Returns an <see cref="Error" /> indicating the specified <paramref name="role" /> name is invalid.
        /// </summary>
        /// <param name="role">The invalid role.</param>
        /// <returns>An <see cref="Error" /> indicating the specific role <paramref name="role" /> name is invalid.</returns>
        public virtual Error InvalidRoleName(string role)
        {
            return new Error
            {
                Code = EventCode.CREDIT_KOLIBRE_IDENTITY_ERROR_INVALID_ROLE_NAME,
                Message = Resource.InvalidRoleName.FormatWith(role)
            };
        }

        /// <summary>
        ///     Returns an <see cref="Error" /> indicating the specified user <paramref name="userName" /> is invalid.
        /// </summary>
        /// <param name="userName">The user name that is invalid.</param>
        /// <returns>An <see cref="Error" /> indicating the specified user <paramref name="userName" /> is invalid.</returns>
        public virtual Error InvalidUserName(string userName)
        {
            return new Error
            {
                Code = EventCode.CREDIT_KOLIBRE_IDENTITY_ERROR_INVALID_USER_NAME,
                Message = Resource.InvalidUserName.FormatWith(userName)
            };
        }

        /// <summary>
        ///     Returns an <see cref="Error" /> indicating a password mismatch.
        /// </summary>
        /// <returns>An <see cref="Error" /> indicating a password mismatch.</returns>
        public virtual Error PasswordMismatch()
        {
            return new Error
            {
                Code = EventCode.CREDIT_KOLIBRE_IDENTITY_ERROR_PASSWORD_MISMATCH,
                Message = Resource.PasswordMismatch
            };
        }

        /// <summary>
        ///     Returns an <see cref="Error" /> indicating a password entered does not contain a numeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="Error" /> indicating a password entered does not contain a numeric character.</returns>
        public virtual Error PasswordRequiresDigit()
        {
            return new Error
            {
                Code = EventCode.CREDIT_KOLIBRE_IDENTITY_ERROR_PASSWORD_REQUIRES_DIGIT,
                Message = Resource.PasswordRequiresDigit
            };
        }

        /// <summary>
        ///     Returns an <see cref="Error" /> indicating a password entered does not contain a lower case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="Error" /> indicating a password entered does not contain a lower case letter.</returns>
        public virtual Error PasswordRequiresLower()
        {
            return new Error
            {
                Code = EventCode.CREDIT_KOLIBRE_IDENTITY_ERROR_PASSWORD_REQUIRES_LOWER,
                Message = Resource.PasswordRequiresLower
            };
        }

        /// <summary>
        ///     Returns an <see cref="Error" /> indicating a password entered does not contain a non-alphanumeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="Error" /> indicating a password entered does not contain a non-alphanumeric character.</returns>
        public virtual Error PasswordRequiresNonAlphanumeric()
        {
            return new Error
            {
                Code = EventCode.CREDIT_KOLIBRE_IDENTITY_ERROR_PASSWORD_REQUIRES_NON_ALPHAUMERIC,
                Message = Resource.PasswordRequiresNonAlphanumeric
            };
        }

        /// <summary>
        ///     Returns an <see cref="Error" /> indicating a password entered does not contain an upper case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="Error" /> indicating a password entered does not contain an upper case letter.</returns>
        public virtual Error PasswordRequiresUpper()
        {
            return new Error
            {
                Code = EventCode.CREDIT_KOLIBRE_IDENTITY_ERROR_PASSWORD_REQUIRES_UPPER,
                Message = Resource.PasswordRequiresUpper
            };
        }

        /// <summary>
        ///     Returns an <see cref="Error" /> indicating a password of the specified <paramref name="length" /> does not meet the minimum length requirements.
        /// </summary>
        /// <param name="length">The length that is not long enough.</param>
        /// <returns>An <see cref="Error" /> indicating a password of the specified <paramref name="length" /> does not meet the minimum length requirements.</returns>
        public virtual Error PasswordTooShort(int length)
        {
            return new Error
            {
                Code = EventCode.CREDIT_KOLIBRE_IDENTITY_ERROR_PASSWORD_TOO_SHORT,
                Message = Resource.PasswordTooShort.FormatWith(length)
            };
        }

        /// <summary>
        ///     Returns an <see cref="Error" /> indicating a user already has a password.
        /// </summary>
        /// <returns>An <see cref="Error" /> indicating a user already has a password.</returns>
        public virtual Error UserAlreadyHasPassword()
        {
            return new Error
            {
                Code = EventCode.CREDIT_KOLIBRE_IDENTITY_ERROR_PASSWORD_TOO_SHORT,
                Message = Resource.UserAlreadyHasPassword
            };
        }

        /// <summary>
        ///     Returns an <see cref="Error" /> indicating a user is already in the specified <paramref name="role" />.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="Error" /> indicating a user is already in the specified <paramref name="role" />.</returns>
        public virtual Error UserAlreadyInRole(string role)
        {
            return new Error
            {
                Code = EventCode.CREDIT_KOLIBRE_IDENTITY_ERROR_USER_ALREADY_IN_ROLE,
                Message = Resource.UserAlreadyInRole.FormatWith(role)
            };
        }

        /// <summary>
        ///     Returns an <see cref="Error" /> indicating user lockout is not enabled.
        /// </summary>
        /// <returns>An <see cref="Error" /> indicating user lockout is not enabled..</returns>
        public virtual Error UserLockoutNotEnabled()
        {
            return new Error
            {
                Code = EventCode.CREDIT_KOLIBRE_IDENTITY_ERROR_USER_LOCKOUT_NOT_ENABLED,
                Message = Resource.UserLockoutNotEnabled
            };
        }

        /// <summary>
        ///     Returns an <see cref="Error" /> indicating a user is not in the specified <paramref name="role" />.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="Error" /> indicating a user is not in the specified <paramref name="role" />.</returns>
        public virtual Error UserNotInRole(string role)
        {
            return new Error
            {
                Code = EventCode.CREDIT_KOLIBRE_IDENTITY_ERROR_USER_NOT_IN_ROLE,
                Message = Resource.UserNotInRole.FormatWith(role)
            };
        }
    }
}