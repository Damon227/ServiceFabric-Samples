// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Identity
// File             : SignInManager.cs
// Created          : 2016-10-31  15:14
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
using System.Threading.Tasks;
using Credit.Kolibre.Foundation.Logging;
using Credit.Kolibre.Foundation.ServiceFabric.Identity.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Provides the APIs for user sign in.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public class SignInManager<TUser> where TUser : class
    {
        private const string LOGIN_PROVIDER_KEY = "LoginProvider";
        private const string XSRF_KEY = "XsrfId";

        private readonly IHttpContextAccessor _contextAccessor;
        private HttpContext _context;

        /// <summary>
        ///     Creates a new instance of <see cref="SignInManager{TUser}" />.
        /// </summary>
        /// <param name="userManager">An instance of <see cref="UserManager" /> used to retrieve users from and persist users.</param>
        /// <param name="contextAccessor">The accessor used to access the <see cref="HttpContext" />.</param>
        /// <param name="claimsFactory">The factory to use to create claims principals for a user.</param>
        /// <param name="optionsAccessor">The accessor used to access the <see cref="IdentityOptions" />.</param>
        /// <param name="logger">The logger used to log messages, warnings and errors.</param>
        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        public SignInManager(UserManager<TUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<TUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<TUser>> logger)
        {
            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }
            if (contextAccessor == null)
            {
                throw new ArgumentNullException(nameof(contextAccessor));
            }
            if (claimsFactory == null)
            {
                throw new ArgumentNullException(nameof(claimsFactory));
            }

            UserManager = userManager;
            _contextAccessor = contextAccessor;
            ClaimsFactory = claimsFactory;
            Options = optionsAccessor?.Value ?? new IdentityOptions();
            Logger = logger;
        }

        internal IUserClaimsPrincipalFactory<TUser> ClaimsFactory { get; set; }

        internal HttpContext Context
        {
            get
            {
                HttpContext context = _context ?? _contextAccessor?.HttpContext;
                if (context == null)
                {
                    throw new InvalidOperationException("HttpContext must not be null.");
                }
                return context;
            }
            set { _context = value; }
        }

        internal IdentityOptions Options { get; set; }

        /// <summary>
        ///     Gets the <see cref="ILogger" /> used to log messages from the manager.
        /// </summary>
        /// <value>
        ///     The <see cref="ILogger" /> used to log messages from the manager.
        /// </value>
        protected internal virtual ILogger Logger { get; set; }

        protected internal UserManager<TUser> UserManager { get; set; }

        /// <summary>
        ///     Returns a flag indicating whether the specified user can sign in.
        /// </summary>
        /// <param name="user">The user whose sign-in status should be returned.</param>
        /// <returns>
        ///     The task object representing the asynchronous operation, containing a flag that is true
        ///     if the specified user can sign-in, otherwise false.
        /// </returns>
        public virtual async Task<bool> CanSignInAsync(TUser user)
        {
            if (Options.SignIn.RequireConfirmedEmail && !await UserManager.IsEmailConfirmedAsync(user))
            {
                Logger.LogInformation(EventCode.CREDIT_KOLIBRE_IDENTITY_USER_CAN_NOT_SIGN_WITHOUT_COMFIRMED_EMAIL, "User {userId} cannot sign in without a confirmed email.", await UserManager.GetUserIdAsync(user));
                return false;
            }
            if (Options.SignIn.RequireConfirmedCellphone && !await UserManager.IsCellphoneConfirmedAsync(user))
            {
                Logger.LogInformation(EventCode.CREDIT_KOLIBRE_IDENTITY_USER_CAN_NOT_SIGN_WITHOUT_COMFIRMED_PHONE_NUMBER, "User {userId} cannot sign in without a confirmed cellphone.", await UserManager.GetUserIdAsync(user));
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Creates a <see cref="ClaimsPrincipal" /> for the specified <paramref name="user" />, as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to create a <see cref="ClaimsPrincipal" /> for.</param>
        /// <returns>The task object representing the asynchronous operation, containing the ClaimsPrincipal for the specified user.</returns>
        public virtual async Task<ClaimsPrincipal> CreateUserPrincipalAsync(TUser user) => await ClaimsFactory.CreateAsync(user);

        /// <summary>
        ///     Returns true if the principal has an identity with the application cookie identity
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal" /> instance.</param>
        /// <returns>True if the user is logged in with identity.</returns>
        public virtual bool IsSignedIn(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            //return principal.Identities != null &&
            //    principal.Identities.Any(i => i.AuthenticationType == Options.Schemes.ApplicationAuthenticationScheme);

            return principal.Identities != null && principal.Identities.Any(i => i.IsAuthenticated);
        }

        /// <summary>
        ///     Attempts to sign in the specified <paramref name="user" /> and <paramref name="password" /> combination
        ///     as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if the sign in fails.</param>
        /// <returns>
        ///     The task object representing the asynchronous operation containing the <see name="SignInResult" />
        ///     for the sign-in attempt.
        /// </returns>
        public virtual async Task<SignInResult> PasswordSignInAsync(TUser user, string password, bool lockoutOnFailure)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (!await CanSignInAsync(user))
            {
                return SignInResult.NotAllowed;
            }
            if (await IsLockedOut(user))
            {
                return await LockedOutResult(user);
            }
            if (await UserManager.CheckPasswordAsync(user, password))
            {
                await ResetLockout(user);
                await SignInAsync(user);
                return SignInResult.Success;
            }
            Logger.LogWarning(EventCode.CREDIT_KOLIBRE_IDENTITY_USER_FAILED_TO_PROVIDE_PASSWORD, "User {userId} unsuccessfully to provide the correct password.", await UserManager.GetUserIdAsync(user));

            if (UserManager.SupportsUserLockout && lockoutOnFailure)
            {
                // If lockout is requested, increment access unsuccessfully count which might lock out the user
                await UserManager.AccessFailedAsync(user);
                if (await UserManager.IsLockedOutAsync(user))
                {
                    return await LockedOutResult(user);
                }
            }
            return SignInResult.Failed;
        }

        /// <summary>
        ///     Attempts to sign in the specified <paramref name="cellphone" /> and <paramref name="password" /> combination
        ///     as an asynchronous operation.
        /// </summary>
        /// <param name="cellphone">The user cellphone to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if the sign in fails.</param>
        /// <returns>
        ///     The task object representing the asynchronous operation containing the <see name="SignInResult" />
        ///     for the sign-in attempt.
        /// </returns>
        public virtual async Task<SignInResult> PasswordSignInByCellphoneAsync(string cellphone, string password, bool lockoutOnFailure)
        {
            TUser user = await UserManager.FindByCellphoneAsync(cellphone);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await PasswordSignInAsync(user, password, lockoutOnFailure);
        }

        /// <summary>
        ///     Attempts to sign in the specified <paramref name="email" /> and <paramref name="password" /> combination
        ///     as an asynchronous operation.
        /// </summary>
        /// <param name="email">The user email to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if the sign in fails.</param>
        /// <returns>
        ///     The task object representing the asynchronous operation containing the <see name="SignInResult" />
        ///     for the sign-in attempt.
        /// </returns>
        public virtual async Task<SignInResult> PasswordSignInByEmailAsync(string email, string password, bool lockoutOnFailure)
        {
            TUser user = await UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await PasswordSignInAsync(user, password, lockoutOnFailure);
        }

        /// <summary>
        ///     Attempts to sign in the specified <paramref name="userName" /> and <paramref name="password" /> combination
        ///     as an asynchronous operation.
        /// </summary>
        /// <param name="userName">The user name to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if the sign in fails.</param>
        /// <returns>
        ///     The task object representing the asynchronous operation containing the <see name="SignInResult" />
        ///     for the sign-in attempt.
        /// </returns>
        public virtual async Task<SignInResult> PasswordSignInByUserNameAsync(string userName, string password, bool lockoutOnFailure)
        {
            TUser user = await UserManager.FindByNameAsync(userName);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await PasswordSignInAsync(user, password, lockoutOnFailure);
        }

        /// <summary>
        ///     Regenerates the user's application cookie, whilst preserving the existing
        ///     AuthenticationProperties like rememberMe, as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user whose sign-in cookie should be refreshed.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task RefreshSignInAsync(TUser user)
        {
            AuthenticateContext auth = new AuthenticateContext(Options.Schemes.ApplicationAuthenticationScheme);
            await Context.Authentication.AuthenticateAsync(auth);
            string authenticationMethod = auth.Principal?.FindFirstValue(System.Security.Claims.ClaimTypes.AuthenticationMethod);
            await SignInAsync(user, new AuthenticationProperties(auth.Properties), authenticationMethod);
        }

        /// <summary>
        ///     Signs in the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user to sign-in.</param>
        /// <param name="authenticationMethod">Name of the method used to authenticate the user.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual Task SignInAsync(TUser user, string authenticationMethod = null)
        {
            return SignInAsync(user, new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.UtcNow
            }, authenticationMethod);
        }

        /// <summary>
        ///     Signs in the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user to sign-in.</param>
        /// <param name="expiryTimeSpan">Time span before this authentication ticket expires.</param>
        /// <param name="authenticationMethod">Name of the method used to authenticate the user.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual Task SignInAsync(TUser user, TimeSpan expiryTimeSpan, string authenticationMethod = null)
        {
            return SignInAsync(user, new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.UtcNow,
                ExpiresUtc = DateTimeOffset.UtcNow.Add(expiryTimeSpan)
            }, authenticationMethod);
        }

        /// <summary>
        ///     Signs in the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user to sign-in.</param>
        /// <param name="items">State values about the authentication session.</param>
        /// <param name="authenticationMethod">Name of the method used to authenticate the user.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual Task SignInAsync(TUser user, Dictionary<string, string> items, string authenticationMethod = null)
        {
            return SignInAsync(user, new AuthenticationProperties(items), authenticationMethod);
        }

        /// <summary>
        ///     Signs in the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user to sign-in.</param>
        /// <param name="authenticationProperties">Properties applied to the login and authentication cookie.</param>
        /// <param name="authenticationMethod">Name of the method used to authenticate the user.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task SignInAsync(TUser user, AuthenticationProperties authenticationProperties, string authenticationMethod = null)
        {
            authenticationMethod = authenticationMethod ?? Options.Schemes.ApplicationAuthenticationScheme;
            ClaimsPrincipal userPrincipal = await CreateUserPrincipalAsync(user);
            // Review: should we guard against CreateUserPrincipal returning null?
            if (authenticationMethod != null)
            {
                userPrincipal.Identities.First().AddClaim(new Claim(System.Security.Claims.ClaimTypes.AuthenticationMethod, authenticationMethod));
            }
            await Context.Authentication.SignInAsync(Options.Schemes.ApplicationAuthenticationScheme,
                userPrincipal,
                authenticationProperties ?? new AuthenticationProperties());
        }

        /// <summary>
        ///     Signs the current user out of the application.
        /// </summary>
        public virtual async Task SignOutAsync()
        {
            await Context.Authentication.SignOutAsync(Options.Schemes.ApplicationAuthenticationScheme);
        }

        private async Task<bool> IsLockedOut(TUser user)
        {
            return UserManager.SupportsUserLockout && await UserManager.IsLockedOutAsync(user);
        }

        private async Task<SignInResult> LockedOutResult(TUser user)
        {
            Logger.LogInformation(EventCode.CREDIT_KOLIBRE_IDENTITY_USER_CAN_NOT_SIGN_WHILE_LOCKED_OUT, "User {userId} is currently locked out.", await UserManager.GetUserIdAsync(user));
            return SignInResult.LockedOut;
        }


        private Task ResetLockout(TUser user)
        {
            if (UserManager.SupportsUserLockout)
            {
                return UserManager.ResetAccessFailedCountAsync(user);
            }
            return Task.FromResult(0);
        }
    }
}