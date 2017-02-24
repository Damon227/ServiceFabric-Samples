// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Identity
// File             : UserClaimsPrincipalFactory.cs
// Created          : 2016-07-06  11:01 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Credit.Kolibre.Foundation.ServiceFabric.Identity.Options;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Provides methods to create a claims principal for a given user.
    /// </summary>
    /// <typeparam name="TUser">The type used to represent a user.</typeparam>
    /// <typeparam name="TRole">The type used to represent a role.</typeparam>
    public class UserClaimsPrincipalFactory<TUser, TRole> : IUserClaimsPrincipalFactory<TUser>
        where TUser : class
        where TRole : class
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UserClaimsPrincipalFactory{TUser, TRole}" /> class.
        /// </summary>
        /// <param name="userManager">The <see cref="UserManager{TUser}" /> to retrieve user information from.</param>
        /// <param name="roleManager">The <see cref="RoleManager{TRole}" /> to retrieve a user's roles from.</param>
        /// <param name="optionsAccessor">The configured <see cref="IdentityOptions" />.</param>
        public UserClaimsPrincipalFactory(
            UserManager<TUser> userManager,
            RoleManager<TRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
        {
            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }
            if (roleManager == null)
            {
                throw new ArgumentNullException(nameof(roleManager));
            }
            if (optionsAccessor?.Value == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
            UserManager = userManager;
            RoleManager = roleManager;
            Options = optionsAccessor.Value;
        }

        /// <summary>
        ///     Gets the <see cref="UserManager{TUser}" /> for this factory.
        /// </summary>
        /// <value>
        ///     The current <see cref="UserManager{TUser}" /> for this factory instance.
        /// </value>
        public UserManager<TUser> UserManager { get; }

        /// <summary>
        ///     Gets the <see cref="RoleManager{TRole}" /> for this factory.
        /// </summary>
        /// <value>
        ///     The current <see cref="RoleManager{TRole}" /> for this factory instance.
        /// </value>
        public RoleManager<TRole> RoleManager { get; }

        /// <summary>
        ///     Gets the <see cref="IdentityOptions" /> for this factory.
        /// </summary>
        /// <value>
        ///     The current <see cref="IdentityOptions" /> for this factory instance.
        /// </value>
        public IdentityOptions Options { get; }

        #region IUserClaimsPrincipalFactory<TUser> Members

        /// <summary>
        ///     Creates a <see cref="ClaimsPrincipal" /> from an user asynchronously.
        /// </summary>
        /// <param name="user">The user to create a <see cref="ClaimsPrincipal" /> from.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous creation operation, containing the created <see cref="ClaimsPrincipal" />.</returns>
        public virtual async Task<ClaimsPrincipal> CreateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            ClaimsIdentity id = new ClaimsIdentity(Options.Schemes.ApplicationAuthenticationScheme,
                Options.ClaimsIdentity.UserIdClaimType,
                Options.ClaimsIdentity.RoleClaimType);

            string userId = await UserManager.GetUserIdAsync(user);
            string userName = await UserManager.GetUserNameAsync(user);
            id.AddClaim(new Claim(Options.ClaimsIdentity.UserIdClaimType, userId));
            id.AddClaim(new Claim(Options.ClaimsIdentity.UserNameClaimType, userName));

            if (UserManager.SupportsUserEmail)
            {
                string email = await UserManager.GetEmailAsync(user);
                bool emailConfirmed = await UserManager.IsEmailConfirmedAsync(user);
                if (email.IsNotNullOrEmpty() && emailConfirmed)
                {
                    id.AddClaim(new Claim(Options.ClaimsIdentity.UserEmailClaimType, email));
                }
            }

            if (UserManager.SupportsUserCellphone)
            {
                string cellphone = await UserManager.GetCellphoneAsync(user);
                bool cellphoneConfirmed = await UserManager.IsCellphoneConfirmedAsync(user);
                if (cellphone.IsNotNullOrEmpty() && cellphoneConfirmed)
                {
                    id.AddClaim(new Claim(Options.ClaimsIdentity.UserCellphonelClaimType, cellphone));
                }
            }

            if (UserManager.SupportsUserRole)
            {
                IList<string> roles = await UserManager.GetRolesAsync(user);
                foreach (string roleName in roles)
                {
                    id.AddClaim(new Claim(Options.ClaimsIdentity.RoleClaimType, roleName));
                    if (RoleManager.SupportsRoleClaims)
                    {
                        TRole role = await RoleManager.FindByNameAsync(roleName);
                        if (role != null)
                        {
                            id.AddClaims(await RoleManager.GetClaimsAsync(role));
                        }
                    }
                }
            }

            if (UserManager.SupportsUserClaim)
            {
                id.AddClaims(await UserManager.GetClaimsAsync(user));
            }
            return new ClaimsPrincipal(id);
        }

        #endregion
    }
}