// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Identity
// File             : IdentityOptions.cs
// Created          : 2016-07-02  4:28 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity.Options
{
    /// <summary>
    ///     Represents all the options you can use to configure the identity system.
    /// </summary>
    public class IdentityOptions
    {
        /// <summary>
        ///     Gets or sets the <see cref="ClaimsIdentityOptions" /> for the identity system.
        /// </summary>
        /// <value>
        ///     The <see cref="ClaimsIdentityOptions" /> for the identity system.
        /// </value>
        public ClaimsIdentityOptions ClaimsIdentity { get; set; } = new ClaimsIdentityOptions();

        /// <summary>
        ///     Gets or sets the <see cref="UserOptions" /> for the identity system.
        /// </summary>
        /// <value>
        ///     The <see cref="UserOptions" /> for the identity system.
        /// </value>
        public UserOptions User { get; set; } = new UserOptions();

        /// <summary>
        ///     Gets or sets the <see cref="PasswordOptions" /> for the identity system.
        /// </summary>
        /// <value>
        ///     The <see cref="PasswordOptions" /> for the identity system.
        /// </value>
        public PasswordOptions Password { get; set; } = new PasswordOptions();

        /// <summary>
        ///     Gets or sets the <see cref="LockoutOptions" /> for the identity system.
        /// </summary>
        /// <value>
        ///     The <see cref="LockoutOptions" /> for the identity system.
        /// </value>
        public LockoutOptions Lockout { get; set; } = new LockoutOptions();

        /// <summary>
        ///     Gets or sets the <see cref="SignInOptions" /> for the identity system.
        /// </summary>
        /// <value>
        ///     The <see cref="SignInOptions" /> for the identity system.
        /// </value>
        public SignInOptions SignIn { get; set; } = new SignInOptions();

        /// <summary>
        ///     Gets or sets the <see cref="IdentitySchemeOptions" /> for the identity system.
        /// </summary>
        /// <value>
        ///     The <see cref="IdentitySchemeOptions" /> for the identity system.
        /// </value>
        public IdentitySchemeOptions Schemes { get; set; } = new IdentitySchemeOptions();

        /// <summary>
        ///     Gets or sets the <see cref="TimeSpan" /> after which security stamps are re-validated.
        /// </summary>
        /// <value>
        ///     The <see cref="TimeSpan" /> after which security stamps are re-validated.
        /// </value>
        public TimeSpan SecurityStampValidationInterval { get; set; } = TimeSpan.FromMinutes(30);
    }
}