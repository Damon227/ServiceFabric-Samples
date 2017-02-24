// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Identity
// File             : IdentitySchemeOptions.cs
// Created          : 2016-07-02  4:28 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity.Options
{
    /// <summary>
    ///     Represents all the options you can use to configure the middleware uesd by the identity system.
    /// </summary>
    public class IdentitySchemeOptions
    {
        private const string SHEME_PREFIX = "KC-Identity";
        private const string DEFAULT_APPLICATION_SCHEME = SHEME_PREFIX + "-Application";
        //private const string DEFAULT_EXTERNAL_SCHEME = SHEME_PREFIX + "-External";
        //private const string DEFAULT_TWO_FACTOR_REMEMBER_ME_SCHEME = SHEME_PREFIX + "-TwoFactorRememberMe";
        //private const string DEFAULT_TWO_FACTOR_USER_ID_SCHEME = SHEME_PREFIX + "-TwoFactorUserId";

        /// <summary>
        ///     Gets the scheme used to identify application authentication.
        /// </summary>
        /// <value>The scheme used to identify application authentication.</value>
        public string ApplicationAuthenticationScheme { get; set; } = DEFAULT_APPLICATION_SCHEME;

        ///// </summary>
        /////     Gets the scheme used to identify external authentication.

        ///// <summary>
        ///// <value>The scheme used to identify external authentication.</value>
        //public string ExternalAuthenticationScheme { get; set; } = DEFAULT_EXTERNAL_SCHEME;

        ///// <summary>
        /////     Gets the scheme used to identify Two Factor authentication for round tripping user identities.
        ///// </summary>
        ///// <value>The scheme used to identify user identity 2fa authentication.</value>
        //public string TwoFactorUserIdAuthenticationScheme { get; set; } = DEFAULT_TWO_FACTOR_USER_ID_SCHEME;

        ///// <summary>
        /////     Gets the scheme used to identify Two Factor authentication for saving the Remember Me state.
        ///// </summary>
        ///// <value>The scheme used to identify remember me application authentication.</value>
        //public string TwoFactorRememberMeAuthenticationScheme { get; set; } = DEFAULT_TWO_FACTOR_REMEMBER_ME_SCHEME;
    }
}