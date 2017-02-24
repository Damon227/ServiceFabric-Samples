// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Authentication.Session
// File             : SessionAuthenticationOptions.cs
// Created          : 2016-07-05  1:25 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.AspNetCore.Authentication.Session
{
    /// <summary>
    ///     Configuration options for <see cref="SessionAuthenticationMiddleware" />.
    /// </summary>
    public class SessionAuthenticationOptions : AuthenticationOptions, IOptions<SessionAuthenticationOptions>
    {
        /// <summary>
        ///     Create an instance of the options initialized with the default values
        /// </summary>
        public SessionAuthenticationOptions()
        {
            AuthenticationScheme = SessionAuthenticationDefaults.AUTHENTICATION_SCHEME;
            AutomaticAuthenticate = SessionAuthenticationDefaults.AUTOMATIC_AUTHENTICATE;
            AutomaticChallenge = SessionAuthenticationDefaults.AUTOMATIC_CHALLENGE;
        }

        public string SessionTicketName { get; set; } = SessionAuthenticationDefaults.DEFAULT_SESSION_TICKET_NAME;

        public TimeSpan ExpireTimeSpan { get; set; } = SessionAuthenticationDefaults.DEFAULT_EXPIRE_TIME_SPAN;

        #region IOptions<SessionAuthenticationOptions> Members

        SessionAuthenticationOptions IOptions<SessionAuthenticationOptions>.Value
        {
            get { return this; }
        }

        #endregion
    }
}