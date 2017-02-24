// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Identity
// File             : UserManager.cs
// Created          : 2016-08-23  10:04 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Credit.Kolibre.Foundation.Logging;
using Credit.Kolibre.Foundation.ServiceFabric.Identity.Options;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Provides the APIs for managing user in a persistence store.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public class UserManager<TUser> : IDisposable where TUser : class
    {
        private readonly HttpContext _context;

        private bool _disposed;

        /// <summary>
        ///     Constructs a new instance of <see cref="UserManager{TUser}" />.
        /// </summary>
        /// <param name="store">The persistence store the manager will operate over.</param>
        /// <param name="optionsAccessor">The accessor used to access the <see cref="IdentityOptions" />.</param>
        /// <param name="passwordHasher">The password hashing implementation to use when saving passwords.</param>
        /// <param name="userValidators">A collection of <see cref="IUserValidator{TUser}" /> to validate users against.</param>
        /// <param name="passwordValidators">A collection of <see cref="IPasswordValidator{TUser}" /> to validate passwords against.</param>
        /// <param name="keyNormalizer">The <see cref="ILookupNormalizer" /> to use when generating index keys for users.</param>
        /// <param name="errors">The <see cref="ErrorDescriber" /> used to provider error messages.</param>
        /// <param name="services">The <see cref="IServiceProvider" /> used to resolve services.</param>
        /// <param name="logger">The logger used to log messages, warnings and errors.</param>
        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        public UserManager(IUserStore<TUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            ErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<TUser>> logger)
        {
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }
            Store = store;
            Options = optionsAccessor?.Value ?? new IdentityOptions();
            PasswordHasher = passwordHasher;
            KeyNormalizer = keyNormalizer;
            ErrorDescriber = errors;
            Logger = logger;

            if (userValidators != null)
            {
                foreach (IUserValidator<TUser> v in userValidators)
                {
                    UserValidators.Add(v);
                }
            }
            if (passwordValidators != null)
            {
                foreach (IPasswordValidator<TUser> v in passwordValidators)
                {
                    PasswordValidators.Add(v);
                }
            }

            if (services != null)
            {
                _context = services.GetService<IHttpContextAccessor>()?.HttpContext;
            }
        }

        /// <summary>
        ///     Gets a flag indicating whether the backing user store supports user passwords.
        /// </summary>
        /// <value>
        ///     true if the backing user store supports user passwords, otherwise false.
        /// </value>
        public virtual bool SupportsUserPassword
        {
            get
            {
                ThrowIfDisposed();
                return Store is IUserPasswordStore<TUser>;
            }
        }

        /// <summary>
        ///     Gets a flag indicating whether the backing user store supports user roles.
        /// </summary>
        /// <value>
        ///     true if the backing user store supports user roles, otherwise false.
        /// </value>
        public virtual bool SupportsUserRole
        {
            get
            {
                ThrowIfDisposed();
                return Store is IUserRoleStore<TUser>;
            }
        }

        /// <summary>
        ///     Gets a flag indicating whether the backing user store supports user emails.
        /// </summary>
        /// <value>
        ///     true if the backing user store supports user emails, otherwise false.
        /// </value>
        public virtual bool SupportsUserEmail
        {
            get
            {
                ThrowIfDisposed();
                return Store is IUserEmailStore<TUser>;
            }
        }

        /// <summary>
        ///     Gets a flag indicating whether the backing user store supports user telecellphones.
        /// </summary>
        /// <value>
        ///     true if the backing user store supports user telecellphones, otherwise false.
        /// </value>
        public virtual bool SupportsUserCellphone
        {
            get
            {
                ThrowIfDisposed();
                return Store is IUserCellphoneStore<TUser>;
            }
        }

        /// <summary>
        ///     Gets a flag indicating whether the backing user store supports user claims.
        /// </summary>
        /// <value>
        ///     true if the backing user store supports user claims, otherwise false.
        /// </value>
        public virtual bool SupportsUserClaim
        {
            get
            {
                ThrowIfDisposed();
                return Store is IUserClaimStore<TUser>;
            }
        }

        /// <summary>
        ///     Gets a flag indicating whether the backing user store supports user lock-outs.
        /// </summary>
        /// <value>
        ///     true if the backing user store supports user lock-outs, otherwise false.
        /// </value>
        public virtual bool SupportsUserLockout
        {
            get
            {
                ThrowIfDisposed();
                return Store is IUserLockoutStore<TUser>;
            }
        }

        /// <summary>
        ///     Gets a flag indicating whether the backing user store supports returning
        ///     <see cref="IQueryable" /> collections of information.
        /// </summary>
        /// <value>
        ///     true if the backing user store supports returning <see cref="IQueryable" /> collections of
        ///     information, otherwise false.
        /// </value>
        public virtual bool SupportsQueryableUsers
        {
            get
            {
                ThrowIfDisposed();
                return Store is IQueryableUserStore<TUser>;
            }
        }

        /// <summary>
        ///     Returns an IQueryable of users if the store is an IQueryableUserStore
        /// </summary>
        public virtual IQueryable<TUser> Users
        {
            get
            {
                IQueryableUserStore<TUser> queryableStore = Store as IQueryableUserStore<TUser>;
                if (queryableStore == null)
                {
                    throw new NotSupportedException(Resource.StoreNotIQueryableUserStore);
                }
                return queryableStore.Users;
            }
        }

        private CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;

        /// <summary>
        ///     Gets or sets the persistence store the manager operates over.
        /// </summary>
        /// <value>The persistence store the manager operates over.</value>
        protected internal IUserStore<TUser> Store { get; set; }

        /// <summary>
        ///     Gets the <see cref="ILogger" /> used to log messages from the manager.
        /// </summary>
        /// <value>
        ///     The <see cref="ILogger" /> used to log messages from the manager.
        /// </value>
        protected internal virtual ILogger Logger { get; set; }

        internal IPasswordHasher PasswordHasher { get; set; }

        internal IList<IUserValidator<TUser>> UserValidators { get; } = new List<IUserValidator<TUser>>();

        internal IList<IPasswordValidator<TUser>> PasswordValidators { get; } = new List<IPasswordValidator<TUser>>();

        internal ILookupNormalizer KeyNormalizer { get; set; }

        internal ErrorDescriber ErrorDescriber { get; set; }

        internal IdentityOptions Options { get; set; }

        #region IDisposable Members

        /// <summary>
        ///     Releases all resources used by the user manager.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        ///     Increments the access unsuccessfully count for the user as an asynchronous operation.
        ///     If the unsuccessfully access account is greater than or equal to the configured maximum number of attempts,
        ///     the user will be locked out for the configured lockout time span.
        /// </summary>
        /// <param name="user">The user whose unsuccessfully access count to increment.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" /> of the operation.</returns>
        public virtual async Task<Result> AccessFailedAsync(TUser user)
        {
            ThrowIfDisposed();

            if (!SupportsUserLockout)
            {
                throw new NotSupportedException(Resource.StoreNotIUserLockoutStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IUserLockoutStore<TUser> store = GetUserLockoutStore();
            // If this puts the user over the threshold for lockout, lock them out and reset the access unsuccessfully count
            int count = await store.IncrementAccessFailedCountAsync(user, CancellationToken);
            if (count < Options.Lockout.MaxFailedAccessAttempts)
            {
                return await UpdateUserAsync(user);
            }
            Logger.LogWarning(EventCode.CREDIT_KOLIBRE_IDENTITY_USER_LOCKED_OUT, "User {userId} is locked out.", await GetUserIdAsync(user));
            await store.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.Add(Options.Lockout.DefaultLockoutTimeSpan),
                CancellationToken);
            await store.ResetAccessFailedCountAsync(user, CancellationToken);
            return await UpdateUserAsync(user);
        }

        /// <summary>
        ///     Adds the specified <paramref name="claim" /> to the <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user to add the claim to.</param>
        /// <param name="claim">The claim to add.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual Task<Result> AddClaimAsync(TUser user, Claim claim)
        {
            ThrowIfDisposed();

            if (!SupportsUserClaim)
            {
                throw new NotSupportedException(Resource.StoreNotIUserClaimStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            return AddClaimsAsync(user, new[] { claim });
        }

        /// <summary>
        ///     Adds the specified <paramref name="claims" /> to the <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user to add the claim to.</param>
        /// <param name="claims">The claims to add.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> AddClaimsAsync(TUser user, IEnumerable<Claim> claims)
        {
            ThrowIfDisposed();

            if (!SupportsUserClaim)
            {
                throw new NotSupportedException(Resource.StoreNotIUserClaimStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            IUserClaimStore<TUser> claimStore = GetClaimStore();
            await claimStore.AddClaimsAsync(user, claims, CancellationToken);
            return await UpdateUserAsync(user);
        }

        /// <summary>
        ///     Add the specified <paramref name="user" /> to the named role.
        /// </summary>
        /// <param name="user">The user to add to the named role.</param>
        /// <param name="role">The name of the role to add the user to.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> AddToRoleAsync(TUser user, string role)
        {
            ThrowIfDisposed();

            if (!SupportsUserRole)
            {
                throw new NotSupportedException(Resource.StoreNotIUserRoleStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (role.IsNullOrEmpty())
            {
                throw new ArgumentException(Foundation.Resource.Argument_EmptyOrNullString, nameof(role));
            }

            IUserRoleStore<TUser> userRoleStore = GetUserRoleStore();
            string normalizedRole = NormalizeKey(role);
            if (await userRoleStore.IsInRoleAsync(user, normalizedRole, CancellationToken))
            {
                return await UserAlreadyInRoleError(user, role);
            }
            await userRoleStore.AddToRoleAsync(user, normalizedRole, CancellationToken);
            return await UpdateUserAsync(user);
        }

        /// <summary>
        ///     Add the specified <paramref name="user" /> to the named roles.
        /// </summary>
        /// <param name="user">The user to add to the named roles.</param>
        /// <param name="roles">The name of the roles to add the user to.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> AddToRolesAsync(TUser user, IEnumerable<string> roles)
        {
            ThrowIfDisposed();

            if (!SupportsUserRole)
            {
                throw new NotSupportedException(Resource.StoreNotIUserRoleStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (roles == null)
            {
                throw new ArgumentNullException(nameof(roles));
            }

            IUserRoleStore<TUser> userRoleStore = GetUserRoleStore();
            foreach (string role in roles.Distinct())
            {
                string normalizedRole = NormalizeKey(role);
                if (await userRoleStore.IsInRoleAsync(user, normalizedRole, CancellationToken))
                {
                    return await UserAlreadyInRoleError(user, role);
                }
                await userRoleStore.AddToRoleAsync(user, normalizedRole, CancellationToken);
            }
            return await UpdateUserAsync(user);
        }

        /// <summary>
        ///     Sets the cellphone for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose cellphone to set.</param>
        /// <param name="cellphone">The cellphone to set.</param>
        /// <param name="cellphoneConfirmed">The cellphone confirmed to set.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> ChangeCellphoneAsync(TUser user, string cellphone, bool cellphoneConfirmed = true)
        {
            ThrowIfDisposed();

            if (!SupportsUserCellphone)
            {
                throw new NotSupportedException(Resource.StoreNotIUserCellphoneStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (cellphone.IsNullOrEmpty())
            {
                throw new ArgumentException(Foundation.Resource.Argument_EmptyOrNullString, nameof(cellphone));
            }

            IUserCellphoneStore<TUser> store = GetCellphoneStore();
            await store.SetCellphoneAsync(user, cellphone, CancellationToken);
            await store.SetCellphoneConfirmedAsync(user, cellphoneConfirmed, CancellationToken);
            return await UpdateUserAsync(user);
        }

        /// <summary>
        ///     Updates a users emails  for the user.
        /// </summary>
        /// <param name="user">The user whose email should be updated.</param>
        /// <param name="email">The new email address.</param>
        /// <param name="emailConfirmed">The email confirmed to set.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> ChangeEmailAsync(TUser user, string email, bool emailConfirmed = true)
        {
            ThrowIfDisposed();

            if (!SupportsUserEmail)
            {
                throw new NotSupportedException(Resource.StoreNotIUserEmailStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IUserEmailStore<TUser> store = GetEmailStore();
            await store.SetEmailAsync(user, email, CancellationToken);
            await store.SetEmailConfirmedAsync(user, emailConfirmed, CancellationToken);
            return await UpdateUserAsync(user);
        }

        /// <summary>
        ///     Changes a user's password after confirming the specified <paramref name="currentPassword" /> is correct,
        ///     as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user whose password should be set.</param>
        /// <param name="currentPassword">The current password to validate before changing.</param>
        /// <param name="newPassword">The new password to set for the specified <paramref name="user" />.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> ChangePasswordAsync(TUser user, string currentPassword, string newPassword)
        {
            ThrowIfDisposed();

            if (!SupportsUserPassword)
            {
                throw new NotSupportedException(Resource.StoreNotIUserPasswordStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (currentPassword.IsNullOrEmpty())
            {
                throw new ArgumentException(Foundation.Resource.Argument_EmptyOrNullString, nameof(currentPassword));
            }

            if (newPassword.IsNullOrEmpty())
            {
                throw new ArgumentException(Foundation.Resource.Argument_EmptyOrNullString, nameof(newPassword));
            }

            IUserPasswordStore<TUser> passwordStore = GetPasswordStore();
            if (await VerifyPasswordAsync(passwordStore, user, currentPassword) != PasswordVerificationResult.Failed)
            {
                Result result = await UpdatePasswordHash(passwordStore, user, newPassword);
                if (!result.Succeeded)
                {
                    return result;
                }
                return await UpdateUserAsync(user);
            }
            Logger.LogWarning(EventCode.CREDIT_KOLIBRE_IDENTITY_CHANGE_PASSWORD_FAILED, "Change password unsuccessfully for user {userId}.", await GetUserIdAsync(user));
            return Result.Failed(ErrorDescriber.PasswordMismatch());
        }

        /// <summary>
        ///     Returns a flag indicating whether the given <paramref name="password" /> is valid for the
        ///     specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose password should be validated.</param>
        /// <param name="password">The password to validate</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing true if
        ///     the specified <paramref name="password" /> matches the one store for the <paramref name="user" />,
        ///     otherwise false.
        /// </returns>
        public virtual async Task<bool> CheckPasswordAsync(TUser user, string password)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                return false;
            }

            if (password.IsNullOrEmpty())
            {
                throw new ArgumentException(Foundation.Resource.Argument_EmptyOrNullString, nameof(password));
            }

            IUserPasswordStore<TUser> passwordStore = GetPasswordStore();
            PasswordVerificationResult result = await VerifyPasswordAsync(passwordStore, user, password);
            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                await UpdatePasswordHash(passwordStore, user, password, false);
                await UpdateUserAsync(user);
            }

            bool success = result != PasswordVerificationResult.Failed;
            if (!success)
            {
                Logger.LogWarning(EventCode.CREDIT_KOLIBRE_IDENTITY_INVALID_PASSWORD, "Invalid password for user {userId}.", await GetUserIdAsync(user));
            }
            return success;
        }

        /// <summary>
        ///     Validates that an cellphone confirmation token matches the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user to validate the token against.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> ConfirmCellphoneAsync(TUser user)
        {
            ThrowIfDisposed();

            if (!SupportsUserCellphone)
            {
                throw new NotSupportedException(Resource.StoreNotIUserCellphoneStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IUserCellphoneStore<TUser> store = GetCellphoneStore();
            await store.SetCellphoneConfirmedAsync(user, true, CancellationToken);
            return await UpdateUserAsync(user);
        }

        /// <summary>
        ///     Validates that an email confirmation token matches the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user to validate the token against.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> ConfirmEmailAsync(TUser user)
        {
            ThrowIfDisposed();

            if (!SupportsUserEmail)
            {
                throw new NotSupportedException(Resource.StoreNotIUserEmailStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IUserEmailStore<TUser> store = GetEmailStore();
            await store.SetEmailConfirmedAsync(user, true, CancellationToken);
            return await UpdateUserAsync(user);
        }

        /// <summary>
        ///     Creates the specified <paramref name="user" /> in the backing store with given password,
        ///     as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <param name="password">The password for the user to hash and store.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> CreateAsync(TUser user, string password)
        {
            ThrowIfDisposed();

            if (!SupportsUserPassword)
            {
                throw new NotSupportedException(Resource.StoreNotIUserPasswordStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (password.IsNullOrEmpty())
            {
                throw new ArgumentException(Foundation.Resource.Argument_EmptyOrNullString, nameof(password));
            }

            IUserPasswordStore<TUser> passwordStore = GetPasswordStore();
            Result result = await UpdatePasswordHash(passwordStore, user, password);
            if (!result.Succeeded)
            {
                return result;
            }
            result = await ValidateUserInternal(user);
            if (!result.Succeeded)
            {
                return result;
            }
            if (Options.Lockout.AllowedForNewUsers && SupportsUserLockout)
            {
                await GetUserLockoutStore().SetLockoutEnabledAsync(user, true, CancellationToken);
            }
            await UpdateNormalizedUserNameAsync(user);

            if (SupportsUserEmail)
            {
                await UpdateNormalizedEmailAsync(user);
            }

            return await Store.CreateAsync(user, CancellationToken);
        }

        /// <summary>
        ///     Creates the specified <paramref name="user" /> in the backing store with given password,
        ///     as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <param name="password">The password for the user to hash and store.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> CreateSimpleAsync(TUser user, string password)
        {
            ThrowIfDisposed();

            if (!SupportsUserPassword)
            {
                throw new NotSupportedException(Resource.StoreNotIUserPasswordStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (password.IsNullOrEmpty())
            {
                throw new ArgumentException(Foundation.Resource.Argument_EmptyOrNullString, nameof(password));
            }

            IUserPasswordStore<TUser> passwordStore = GetPasswordStore();
            Result result = await UpdatePasswordHash(passwordStore, user, password);
            if (!result.Succeeded)
            {
                return result;
            }
            result = await ValidateUserInternal(user);
            if (!result.Succeeded)
            {
                return result;
            }
            if (Options.Lockout.AllowedForNewUsers && SupportsUserLockout)
            {
                await GetUserLockoutStore().SetLockoutEnabledAsync(user, true, CancellationToken);
            }
            await UpdateNormalizedUserNameAsync(user);

            if (SupportsUserEmail)
            {
                await UpdateNormalizedEmailAsync(user);
            }

            return await Store.CreateAsync(user, CancellationToken);
        }

        /// <summary>
        ///     Deletes the specified <paramref name="user" /> from the backing store.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual Task<Result> DeleteAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Store.DeleteAsync(user, CancellationToken);
        }

        /// <summary>
        ///     Gets the user, if any, associated with the specified cellphone.
        /// </summary>
        /// <param name="cellphone">The cellphone to return the user for.</param>
        /// <returns>
        ///     The task object containing the results of the asynchronous lookup operation, the user if any associated with the specified cellphone.
        /// </returns>
        public virtual Task<TUser> FindByCellphoneAsync(string cellphone)
        {
            ThrowIfDisposed();

            if (!SupportsUserCellphone)
            {
                throw new NotSupportedException(Resource.StoreNotIUserCellphoneStore);
            }

            if (cellphone.IsNullOrEmpty())
            {
                throw new ArgumentException(Foundation.Resource.Argument_EmptyOrNullString, nameof(cellphone));
            }

            IUserCellphoneStore<TUser> store = GetCellphoneStore();
            return store.FindByCellphoneAsync(NormalizeKey(cellphone), CancellationToken);
        }

        /// <summary>
        ///     Gets the user, if any, associated with the specified, normalized email address.
        /// </summary>
        /// <param name="email">The normalized email address to return the user for.</param>
        /// <returns>
        ///     The task object containing the results of the asynchronous lookup operation, the user if any associated with the specified normalized email address.
        /// </returns>
        public virtual Task<TUser> FindByEmailAsync(string email)
        {
            ThrowIfDisposed();

            if (!SupportsUserEmail)
            {
                throw new NotSupportedException(Resource.StoreNotIUserEmailStore);
            }

            if (email.IsNullOrEmpty())
            {
                throw new ArgumentException(Foundation.Resource.Argument_EmptyOrNullString, nameof(email));
            }

            IUserEmailStore<TUser> store = GetEmailStore();
            return store.FindByEmailAsync(NormalizeKey(email), CancellationToken);
        }

        /// <summary>
        ///     Finds and returns a user, if any, who has the specified <paramref name="userId" />.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the user matching the specified <paramref name="userId" /> if it exists.
        /// </returns>
        public virtual Task<TUser> FindByIdAsync(string userId)
        {
            ThrowIfDisposed();

            if (userId.IsNullOrEmpty())
            {
                throw new ArgumentException(Foundation.Resource.Argument_EmptyOrNullString, nameof(userId));
            }

            return Store.FindByIdAsync(userId, CancellationToken);
        }

        /// <summary>
        ///     Finds and returns a user, if any, who has the specified user name.
        /// </summary>
        /// <param name="userName">The user name to search for.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the user matching the specified <paramref name="userName" /> if it exists.
        /// </returns>
        public virtual Task<TUser> FindByNameAsync(string userName)
        {
            ThrowIfDisposed();

            if (userName.IsNullOrEmpty())
            {
                throw new ArgumentException(Foundation.Resource.Argument_EmptyOrNullString, nameof(userName));
            }

            userName = NormalizeKey(userName);
            return Store.FindByNameAsync(userName, CancellationToken);
        }

        /// <summary>
        ///     Generates a value suitable for use in concurrency tracking.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation.
        /// </returns>
        public virtual Task<string> GenerateConcurrencyStampAsync()
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }

        /// <summary>
        ///     Retrieves the current number of unsuccessfully accesses for the given <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose access unsuccessfully count should be retrieved for.</param>
        /// <returns>
        ///     The <see cref="Task" /> that contains the result the asynchronous operation, the current unsuccessfully access count
        ///     for the user.
        /// </returns>
        public virtual async Task<int> GetAccessFailedCountAsync(TUser user)
        {
            ThrowIfDisposed();

            if (!SupportsUserLockout)
            {
                throw new NotSupportedException(Resource.StoreNotIUserLockoutStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IUserLockoutStore<TUser> store = GetUserLockoutStore();
            return await store.GetAccessFailedCountAsync(user, CancellationToken);
        }

        /// <summary>
        ///     Gets the telecellphone, if any, for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose telecellphone should be retrieved.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation, containing the user's telecellphone, if any.</returns>
        public virtual async Task<string> GetCellphoneAsync(TUser user)
        {
            ThrowIfDisposed();

            if (!SupportsUserCellphone)
            {
                throw new NotSupportedException(Resource.StoreNotIUserCellphoneStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IUserCellphoneStore<TUser> store = GetCellphoneStore();
            return await store.GetCellphoneAsync(user, CancellationToken);
        }

        /// <summary>
        ///     Gets a list of <see cref="Claim" />s to be belonging to the specified <paramref name="user" /> as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user whose claims to retrieve.</param>
        /// <returns>
        ///     A <see cref="Task{TResult}" /> that represents the result of the asynchronous query, a list of <see cref="Claim" />s.
        /// </returns>
        public virtual async Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            ThrowIfDisposed();

            if (!SupportsUserClaim)
            {
                throw new NotSupportedException(Resource.StoreNotIUserClaimStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IUserClaimStore<TUser> claimStore = GetClaimStore();
            return await claimStore.GetClaimsAsync(user, CancellationToken);
        }

        /// <summary>
        ///     Gets the email address for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose email should be returned.</param>
        /// <returns>The task object containing the results of the asynchronous operation, the email address for the specified <paramref name="user" />.</returns>
        public virtual async Task<string> GetEmailAsync(TUser user)
        {
            ThrowIfDisposed();

            if (!SupportsUserEmail)
            {
                throw new NotSupportedException(Resource.StoreNotIUserEmailStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IUserEmailStore<TUser> store = GetEmailStore();
            return await store.GetEmailAsync(user, CancellationToken);
        }

        /// <summary>
        ///     Retrieves a flag indicating whether user lockout can enabled for the specified user.
        /// </summary>
        /// <param name="user">The user whose ability to be locked out should be returned.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, true if a user can be locked out, otherwise false.
        /// </returns>
        public virtual async Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            ThrowIfDisposed();

            if (!SupportsUserLockout)
            {
                throw new NotSupportedException(Resource.StoreNotIUserLockoutStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IUserLockoutStore<TUser> store = GetUserLockoutStore();
            return await store.GetLockoutEnabledAsync(user, CancellationToken);
        }

        /// <summary>
        ///     Gets the last <see cref="DateTimeOffset" /> a user's last lockout expired, if any.
        ///     Any time in the past should be indicates a user is not locked out.
        /// </summary>
        /// <param name="user">The user whose lockout date should be retrieved.</param>
        /// <returns>
        ///     A <see cref="Task{TResult}" /> that represents the lookup, a <see cref="DateTimeOffset" /> containing the last time a user's lockout expired, if any.
        /// </returns>
        public virtual async Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user)
        {
            ThrowIfDisposed();

            if (!SupportsUserLockout)
            {
                throw new NotSupportedException(Resource.StoreNotIUserLockoutStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IUserLockoutStore<TUser> store = GetUserLockoutStore();
            return await store.GetLockoutEndDateAsync(user, CancellationToken);
        }

        /// <summary>
        ///     Gets a list of role names the specified <paramref name="user" /> belongs to.
        /// </summary>
        /// <param name="user">The user whose role names to retrieve.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation, containing a list of role names.</returns>
        public virtual async Task<IList<string>> GetRolesAsync(TUser user)
        {
            ThrowIfDisposed();

            if (!SupportsUserRole)
            {
                throw new NotSupportedException(Resource.StoreNotIUserRoleStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IUserRoleStore<TUser> userRoleStore = GetUserRoleStore();
            return await userRoleStore.GetRolesAsync(user, CancellationToken);
        }

        public virtual Task<TUser> GetUserAsync(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            string id = GetUserId(principal);
            return id == null ? Task.FromResult<TUser>(null) : FindByIdAsync(id);
        }

        /// <summary>
        ///     Returns the User ID claim value if present otherwise returns null.
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal" /> instance.</param>
        /// <returns>The User ID claim value, or null if the claim is not present.</returns>
        /// <remarks>The User ID claim is identified by <see cref="System.Security.Claims.ClaimTypes.NameIdentifier" />.</remarks>
        public virtual string GetUserId(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            return principal.FindFirstValue(Options.ClaimsIdentity.UserIdClaimType);
        }

        /// <summary>
        ///     Gets the user identifier for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose identifier should be retrieved.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation, containing the identifier for the specified <paramref name="user" />.</returns>
        public virtual async Task<string> GetUserIdAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            ThrowIfDisposed();
            return await Store.GetUserIdAsync(user, CancellationToken);
        }

        /// <summary>
        ///     Returns the Name claim value if present otherwise returns null.
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal" /> instance.</param>
        /// <returns>The Name claim value, or null if the claim is not present.</returns>
        /// <remarks>The Name claim is identified by <see cref="ClaimsIdentity.DefaultNameClaimType" />.</remarks>
        public virtual string GetUserName(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            return principal.FindFirstValue(Options.ClaimsIdentity.UserNameClaimType);
        }

        /// <summary>
        ///     Gets the user name for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose name should be retrieved.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation, containing the name for the specified <paramref name="user" />.</returns>
        public virtual async Task<string> GetUserNameAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return await Store.GetUserNameAsync(user, CancellationToken);
        }

        /// <summary>
        ///     Returns a list of users from the user store who have the specified <paramref name="claim" />.
        /// </summary>
        /// <param name="claim">The claim to look for.</param>
        /// <returns>
        ///     A <see cref="Task{TResult}" /> that represents the result of the asynchronous query, a list of <typeparamref name="TUser" />s who
        ///     have the specified claim.
        /// </returns>
        public virtual Task<IList<TUser>> GetUsersForClaimAsync(Claim claim)
        {
            ThrowIfDisposed();

            if (!SupportsUserClaim)
            {
                throw new NotSupportedException(Resource.StoreNotIUserClaimStore);
            }

            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            IUserClaimStore<TUser> store = GetClaimStore();
            return store.GetUsersForClaimAsync(claim, CancellationToken);
        }

        /// <summary>
        ///     Returns a list of users from the user store who are members of the specified <paramref name="roleName" />.
        /// </summary>
        /// <param name="roleName">The name of the role whose users should be returned.</param>
        /// <returns>
        ///     A <see cref="Task{TResult}" /> that represents the result of the asynchronous query, a list of <typeparamref name="TUser" />s who
        ///     are members of the specified role.
        /// </returns>
        public virtual Task<IList<TUser>> GetUsersInRoleAsync(string roleName)
        {
            ThrowIfDisposed();

            if (!SupportsUserRole)
            {
                throw new NotSupportedException(Resource.StoreNotIUserRoleStore);
            }

            if (roleName == null)
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            IUserRoleStore<TUser> store = GetUserRoleStore();
            return store.GetUsersInRoleAsync(NormalizeKey(roleName), CancellationToken);
        }

        /// <summary>
        ///     Gets a flag indicating whether the specified <paramref name="user" />'s telecellphone has been confirmed.
        /// </summary>
        /// <param name="user">The user to return a flag for, indicating whether their telecellphone is confirmed.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, returning true if the specified <paramref name="user" /> has a confirmed
        ///     telecellphone otherwise false.
        /// </returns>
        public virtual Task<bool> IsCellphoneConfirmedAsync(TUser user)
        {
            ThrowIfDisposed();

            if (!SupportsUserCellphone)
            {
                throw new NotSupportedException(Resource.StoreNotIUserCellphoneStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IUserCellphoneStore<TUser> store = GetCellphoneStore();
            return store.GetCellphoneConfirmedAsync(user, CancellationToken);
        }

        /// <summary>
        ///     Gets a flag indicating whether the email address for the specified <paramref name="user" /> has been verified, true if the cellphone address is verified otherwise
        ///     false.
        /// </summary>
        /// <param name="user">The user whose email confirmation status should be returned.</param>
        /// <returns>
        ///     The task object containing the results of the asynchronous operation, a flag indicating whether the email address for the specified <paramref name="user" />
        ///     has been confirmed or not.
        /// </returns>
        public virtual async Task<bool> IsEmailConfirmedAsync(TUser user)
        {
            ThrowIfDisposed();

            if (!SupportsUserEmail)
            {
                throw new NotSupportedException(Resource.StoreNotIUserEmailStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IUserEmailStore<TUser> store = GetEmailStore();
            return await store.GetEmailConfirmedAsync(user, CancellationToken);
        }

        /// <summary>
        ///     Returns a flag indicating whether the specified <paramref name="user" /> is a member of the give named role.
        /// </summary>
        /// <param name="user">The user whose role membership should be checked.</param>
        /// <param name="role">The name of the role to be checked.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing a flag indicating whether the specified <paramref name="user" /> is
        ///     a member of the named role.
        /// </returns>
        public virtual async Task<bool> IsInRoleAsync(TUser user, string role)
        {
            ThrowIfDisposed();

            if (!SupportsUserRole)
            {
                throw new NotSupportedException(Resource.StoreNotIUserRoleStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IUserRoleStore<TUser> userRoleStore = GetUserRoleStore();
            return await userRoleStore.IsInRoleAsync(user, NormalizeKey(role), CancellationToken);
        }

        /// <summary>
        ///     Returns a flag indicating whether the specified <paramref name="user" /> his locked out,
        ///     as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user whose locked out status should be retrieved.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, true if the specified <paramref name="user " />
        ///     is locked out, otherwise false.
        /// </returns>
        public virtual async Task<bool> IsLockedOutAsync(TUser user)
        {
            ThrowIfDisposed();

            if (!SupportsUserLockout)
            {
                throw new NotSupportedException(Resource.StoreNotIUserLockoutStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IUserLockoutStore<TUser> store = GetUserLockoutStore();
            if (!await store.GetLockoutEnabledAsync(user, CancellationToken))
            {
                return false;
            }
            DateTimeOffset? lockoutTime = await store.GetLockoutEndDateAsync(user, CancellationToken);
            return lockoutTime >= DateTimeOffset.UtcNow;
        }

        /// <summary>
        ///     Normalize a key (user name, email) for consistent comparisons.
        /// </summary>
        /// <param name="key">The key to normalize.</param>
        /// <returns>A normalized value representing the specified <paramref name="key" />.</returns>
        public virtual string NormalizeKey(string key)
        {
            return KeyNormalizer == null ? key : KeyNormalizer.Normalize(key);
        }

        /// <summary>
        ///     Removes the specified <paramref name="claim" /> from the given <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user to remove the specified <paramref name="claim" /> from.</param>
        /// <param name="claim">The <see cref="Claim" /> to remove.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual Task<Result> RemoveClaimAsync(TUser user, Claim claim)
        {
            ThrowIfDisposed();

            if (!SupportsUserClaim)
            {
                throw new NotSupportedException(Resource.StoreNotIUserClaimStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            return RemoveClaimsAsync(user, new[] { claim });
        }

        /// <summary>
        ///     Removes the specified <paramref name="claims" /> from the given <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user to remove the specified <paramref name="claims" /> from.</param>
        /// <param name="claims">A collection of <see cref="Claim" />s to remove.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims)
        {
            ThrowIfDisposed();

            if (!SupportsUserClaim)
            {
                throw new NotSupportedException(Resource.StoreNotIUserClaimStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            IUserClaimStore<TUser> claimStore = GetClaimStore();
            await claimStore.RemoveClaimsAsync(user, claims, CancellationToken);
            return await UpdateUserAsync(user);
        }

        /// <summary>
        ///     Removes the specified <paramref name="user" /> from the named role.
        /// </summary>
        /// <param name="user">The user to remove from the named role.</param>
        /// <param name="role">The name of the role to remove the user from.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> RemoveFromRoleAsync(TUser user, string role)
        {
            ThrowIfDisposed();

            if (!SupportsUserRole)
            {
                throw new NotSupportedException(Resource.StoreNotIUserRoleStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IUserRoleStore<TUser> userRoleStore = GetUserRoleStore();
            string normalizedRole = NormalizeKey(role);
            if (!await userRoleStore.IsInRoleAsync(user, normalizedRole, CancellationToken))
            {
                return await UserNotInRoleError(user, role);
            }
            await userRoleStore.RemoveFromRoleAsync(user, normalizedRole, CancellationToken);
            return await UpdateUserAsync(user);
        }

        /// <summary>
        ///     Removes the specified <paramref name="user" /> from the named roles.
        /// </summary>
        /// <param name="user">The user to remove from the named roles.</param>
        /// <param name="roles">The name of the roles to remove the user from.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> RemoveFromRolesAsync(TUser user, IEnumerable<string> roles)
        {
            ThrowIfDisposed();

            if (!SupportsUserRole)
            {
                throw new NotSupportedException(Resource.StoreNotIUserRoleStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (roles == null)
            {
                throw new ArgumentNullException(nameof(roles));
            }

            IUserRoleStore<TUser> userRoleStore = GetUserRoleStore();
            foreach (string role in roles)
            {
                string normalizedRole = NormalizeKey(role);
                if (!await userRoleStore.IsInRoleAsync(user, normalizedRole, CancellationToken))
                {
                    return await UserNotInRoleError(user, role);
                }
                await userRoleStore.RemoveFromRoleAsync(user, normalizedRole, CancellationToken);
            }
            return await UpdateUserAsync(user);
        }

        /// <summary>
        ///     Replaces the given <paramref name="claim" /> on the specified <paramref name="user" /> with the <paramref name="newClaim" />
        /// </summary>
        /// <param name="user">The user to replace the claim on.</param>
        /// <param name="claim">The claim to replace.</param>
        /// <param name="newClaim">The new claim to replace the existing <paramref name="claim" /> with.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim)
        {
            ThrowIfDisposed();

            if (!SupportsUserClaim)
            {
                throw new NotSupportedException(Resource.StoreNotIUserClaimStore);
            }

            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            if (newClaim == null)
            {
                throw new ArgumentNullException(nameof(newClaim));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IUserClaimStore<TUser> claimStore = GetClaimStore();
            await claimStore.ReplaceClaimAsync(user, claim, newClaim, CancellationToken);
            return await UpdateUserAsync(user);
        }

        /// <summary>
        ///     Resets the access unsuccessfully count for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose unsuccessfully access count should be reset.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" /> of the operation.</returns>
        public virtual async Task<Result> ResetAccessFailedCountAsync(TUser user)
        {
            ThrowIfDisposed();

            if (!SupportsUserLockout)
            {
                throw new NotSupportedException(Resource.StoreNotIUserLockoutStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IUserLockoutStore<TUser> store = GetUserLockoutStore();
            if (await GetAccessFailedCountAsync(user) == 0)
            {
                return Result.Success;
            }
            await store.ResetAccessFailedCountAsync(user, CancellationToken);
            return await UpdateUserAsync(user);
        }

        /// <summary>
        ///     Sets the cellphone for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose cellphone to set.</param>
        /// <param name="cellphone">The cellphone to set.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> SetCellphoneAsync(TUser user, string cellphone)
        {
            ThrowIfDisposed();

            if (!SupportsUserCellphone)
            {
                throw new NotSupportedException(Resource.StoreNotIUserCellphoneStore);
            }

            IUserCellphoneStore<TUser> store = GetCellphoneStore();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await store.SetCellphoneAsync(user, cellphone, CancellationToken);
            await store.SetCellphoneConfirmedAsync(user, false, CancellationToken);
            return await UpdateUserAsync(user);
        }

        /// <summary>
        ///     Sets the <paramref name="email" /> address for a <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose email should be set.</param>
        /// <param name="email">The email to set.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> SetEmailAsync(TUser user, string email)
        {
            ThrowIfDisposed();

            if (!SupportsUserEmail)
            {
                throw new NotSupportedException(Resource.StoreNotIUserEmailStore);
            }

            IUserEmailStore<TUser> store = GetEmailStore();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await store.SetEmailAsync(user, email, CancellationToken);
            await store.SetEmailConfirmedAsync(user, false, CancellationToken);
            return await UpdateUserAsync(user);
        }

        /// <summary>
        ///     Sets a flag indicating whether the specified <paramref name="user" /> is locked out,
        ///     as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user whose locked out status should be set.</param>
        /// <param name="enabled">Flag indicating whether the user is locked out or not.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, the <see cref="Result" /> of the operation
        /// </returns>
        public virtual async Task<Result> SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            ThrowIfDisposed();

            if (!SupportsUserLockout)
            {
                throw new NotSupportedException(Resource.StoreNotIUserLockoutStore);
            }

            IUserLockoutStore<TUser> store = GetUserLockoutStore();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await store.SetLockoutEnabledAsync(user, enabled, CancellationToken);
            return await UpdateUserAsync(user);
        }

        /// <summary>
        ///     Locks out a user until the specified end date has passed. Setting a end date in the past immediately unlocks a user.
        /// </summary>
        /// <param name="user">The user whose lockout date should be set.</param>
        /// <param name="lockoutEnd">The <see cref="DateTimeOffset" /> after which the <paramref name="user" />'s lockout should end.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" /> of the operation.</returns>
        public virtual async Task<Result> SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd)
        {
            ThrowIfDisposed();

            if (!SupportsUserLockout)
            {
                throw new NotSupportedException(Resource.StoreNotIUserLockoutStore);
            }

            IUserLockoutStore<TUser> store = GetUserLockoutStore();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!await store.GetLockoutEnabledAsync(user, CancellationToken))
            {
                Logger.LogWarning(EventCode.CREDIT_KOLIBRE_IDENTITY_LOCKOUT_FAILED_BECAUSE_LOCKOUT_NOT_ENABLE, "Lockout for user {userId} unsuccessfully because lockout is not enabled for this user.", await GetUserIdAsync(user));
                return Result.Failed(ErrorDescriber.UserLockoutNotEnabled());
            }
            await store.SetLockoutEndDateAsync(user, lockoutEnd, CancellationToken);
            return await UpdateUserAsync(user);
        }

        /// <summary>
        ///     Sets the <paramref name="user" />'s password to the specified <paramref name="newPassword" />.
        /// </summary>
        /// <param name="user">The user whose password should be reset.</param>
        /// <param name="newPassword">The new password to set if reset token verification fails.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> SetPasswordAsync(TUser user, string newPassword)
        {
            ThrowIfDisposed();

            if (!SupportsUserPassword)
            {
                throw new NotSupportedException(Resource.StoreNotIUserPasswordStore);
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IUserPasswordStore<TUser> passwordStore = GetPasswordStore();
            Result result = await UpdatePasswordHash(passwordStore, user, newPassword);
            if (!result.Succeeded)
            {
                return result;
            }
            return await UpdateUserAsync(user);
        }

        /// <summary>
        ///     Sets the given <paramref name="userName" /> for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose name should be set.</param>
        /// <param name="userName">The user name to set.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation.</returns>
        public virtual async Task<Result> SetUserNameAsync(TUser user, string userName)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await Store.SetUserNameAsync(user, userName, CancellationToken);
            return await UpdateUserAsync(user);
        }

        /// <summary>
        ///     Updates the specified <paramref name="user" /> in the backing store.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual Task<Result> UpdateAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return UpdateUserAsync(user);
        }

        /// <summary>
        ///     Updates the normalized email for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose email address should be normalized and updated.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task UpdateNormalizedEmailAsync(TUser user)
        {
            IUserEmailStore<TUser> store = GetEmailStore();
            if (store != null)
            {
                string email = await GetEmailAsync(user);
                await store.SetNormalizedEmailAsync(user, NormalizeKey(email), CancellationToken);
            }
        }

        /// <summary>
        ///     Updates the normalized user name for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose user name should be normalized and updated.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation.</returns>
        public virtual async Task UpdateNormalizedUserNameAsync(TUser user)
        {
            string normalizedName = NormalizeKey(await GetUserNameAsync(user));
            await Store.SetNormalizedUserNameAsync(user, normalizedName, CancellationToken);
        }

        /// <summary>
        ///     Releases the unmanaged resources used by the role manager and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                Store?.Dispose();
                _disposed = true;
            }
        }

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        ///     Returns a <see cref="PasswordVerificationResult" /> indicating the result of a password hash comparison.
        /// </summary>
        /// <param name="store">The store containing a user's password.</param>
        /// <param name="user">The user whose password should be verified.</param>
        /// <param name="password">The password to verify.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="PasswordVerificationResult" />
        ///     of the operation.
        /// </returns>
        protected virtual async Task<PasswordVerificationResult> VerifyPasswordAsync(IUserPasswordStore<TUser> store, TUser user, string password)
        {
            string hash = await store.GetPasswordHashAsync(user, CancellationToken);
            if (hash == null)
            {
                return PasswordVerificationResult.Failed;
            }
            return PasswordHasher.VerifyHashedPassword(hash, password);
        }

        internal IUserCellphoneStore<TUser> GetCellphoneStore()
        {
            IUserCellphoneStore<TUser> cast = Store as IUserCellphoneStore<TUser>;
            if (cast == null)
            {
                throw new NotSupportedException(Resource.StoreNotIUserCellphoneStore);
            }
            return cast;
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Global")]
        internal IUserEmailStore<TUser> GetEmailStore(bool throwOnFail = true)
        {
            IUserEmailStore<TUser> cast = Store as IUserEmailStore<TUser>;
            if (throwOnFail && cast == null)
            {
                throw new NotSupportedException(Resource.StoreNotIUserEmailStore);
            }
            return cast;
        }

        internal IUserLockoutStore<TUser> GetUserLockoutStore()
        {
            IUserLockoutStore<TUser> cast = Store as IUserLockoutStore<TUser>;
            if (cast == null)
            {
                throw new NotSupportedException(Resource.StoreNotIUserLockoutStore);
            }
            return cast;
        }

        internal async Task<Result> UpdatePasswordHash(IUserPasswordStore<TUser> passwordStore,
            TUser user, string newPassword, bool validatePassword = true)
        {
            if (validatePassword)
            {
                Result validate = await ValidatePasswordInternal(user, newPassword);
                if (!validate.Succeeded)
                {
                    return validate;
                }
            }
            string hash = newPassword != null ? PasswordHasher.HashPassword(newPassword) : null;
            await passwordStore.SetPasswordHashAsync(user, hash, CancellationToken);
            return Result.Success;
        }

        internal async Task<Result> UpdateSimplePasswordHash(IUserPasswordStore<TUser> passwordStore,
            TUser user, string newPassword, bool validatePassword = true)
        {
            if (validatePassword)
            {
                Result validate = await ValidatePasswordInternal(user, newPassword);
                if (!validate.Succeeded)
                {
                    return validate;
                }
            }
            string hash = newPassword != null ? PasswordHasher.HashPassword(newPassword) : null;
            await passwordStore.SetPasswordHashAsync(user, hash, CancellationToken);
            return Result.Success;
        }

        private IUserClaimStore<TUser> GetClaimStore()
        {
            IUserClaimStore<TUser> cast = Store as IUserClaimStore<TUser>;
            if (cast == null)
            {
                throw new NotSupportedException(Resource.StoreNotIUserClaimStore);
            }
            return cast;
        }

        private IUserPasswordStore<TUser> GetPasswordStore()
        {
            IUserPasswordStore<TUser> cast = Store as IUserPasswordStore<TUser>;
            if (cast == null)
            {
                throw new NotSupportedException(Resource.StoreNotIUserPasswordStore);
            }
            return cast;
        }

        private IUserRoleStore<TUser> GetUserRoleStore()
        {
            IUserRoleStore<TUser> cast = Store as IUserRoleStore<TUser>;
            if (cast == null)
            {
                throw new NotSupportedException(Resource.StoreNotIUserRoleStore);
            }
            return cast;
        }

        private async Task<Result> UpdateUserAsync(TUser user)
        {
            Result result = await ValidateUserInternal(user);
            if (!result.Succeeded)
            {
                return result;
            }

            await UpdateNormalizedUserNameAsync(user);

            if (SupportsUserEmail)
            {
                await UpdateNormalizedEmailAsync(user);
            }

            return await Store.UpdateAsync(user, CancellationToken);
        }

        private async Task<Result> UserAlreadyInRoleError(TUser user, string role)
        {
            Logger.LogWarning(EventCode.CREDIT_KOLIBRE_IDENTITY_USER_ALREADY_IN_ROLE, "User {userId} is already in role {role}.", await GetUserIdAsync(user), role);
            return Result.Failed(ErrorDescriber.UserAlreadyInRole(role));
        }

        private async Task<Result> UserNotInRoleError(TUser user, string role)
        {
            Logger.LogWarning(EventCode.CREDIT_KOLIBRE_IDENTITY_USER_NOT_IN_ROLE, "User {userId} is not in role {role}.", await GetUserIdAsync(user), role);
            return Result.Failed(ErrorDescriber.UserNotInRole(role));
        }

        private async Task<Result> ValidatePasswordInternal(TUser user, string password)
        {
            List<Error> errors = new List<Error>();
            foreach (IPasswordValidator<TUser> v in PasswordValidators)
            {
                Result result = await v.ValidateAsync(this, user, password);
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors);
                }
            }
            if (errors.Count > 0)
            {
                Logger.LogWarning(EventCode.CREDIT_KOLIBRE_IDENTITY_USER_PASSWORD_VALIDATION_FAILED, "User {userId} password validation unsuccessfully: {errors}.", await GetUserIdAsync(user), string.Join(";", errors.Select(e => e.Code)));
                return Result.Failed(errors.ToArray());
            }
            return Result.Success;
        }

        private async Task<Result> ValidateSimplePasswordInternal(TUser user, string password)
        {
            List<Error> errors = new List<Error>();
            foreach (IPasswordValidator<TUser> v in PasswordValidators)
            {
                Result result = await v.ValidateAsync(this, user, password);
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors);
                }
            }
            if (errors.Count > 0)
            {
                Logger.LogWarning(EventCode.CREDIT_KOLIBRE_IDENTITY_USER_PASSWORD_VALIDATION_FAILED, "User {userId} password validation failed: {errors}.", await GetUserIdAsync(user), string.Join(";", errors.Select(e => e.Code)));
                return Result.Failed(errors.ToArray());
            }
            return Result.Success;
        }

        private async Task<Result> ValidateUserInternal(TUser user)
        {
            List<Error> errors = new List<Error>();
            foreach (IUserValidator<TUser> v in UserValidators)
            {
                Result result = await v.ValidateAsync(this, user);
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors);
                }
            }
            if (errors.Count > 0)
            {
                Logger.LogWarning(EventCode.CREDIT_KOLIBRE_IDENTITY_USER_VALIDATION_FAILED, "User {userId} validation unsuccessfully: {errors}.", await GetUserIdAsync(user), string.Join(";", errors.Select(e => e.Code)));
                return Result.Failed(errors.ToArray());
            }
            return Result.Success;
        }
    }
}