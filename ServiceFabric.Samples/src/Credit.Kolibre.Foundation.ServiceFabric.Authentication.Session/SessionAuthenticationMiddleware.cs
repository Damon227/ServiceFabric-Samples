// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Authentication.Session
// File             : SessionAuthenticationMiddleware.cs
// Created          : 2016-07-05  1:14 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.AspNetCore.Authentication.Session
{
    public class SessionAuthenticationMiddleware : AuthenticationMiddleware<SessionAuthenticationOptions>
    {
        private readonly ISessionManager _sessionManager;

        public SessionAuthenticationMiddleware(
            RequestDelegate next,
            ISessionManager sessionManager,
            IOptions<SessionAuthenticationOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder)
            : base(next, options, loggerFactory, encoder)
        {
            if (sessionManager == null)
            {
                throw new ArgumentNullException(nameof(sessionManager));
            }

            _sessionManager = sessionManager;
        }

        protected override AuthenticationHandler<SessionAuthenticationOptions> CreateHandler()
        {
            return new SessionAuthenticationHandler(_sessionManager);
        }
    }
}