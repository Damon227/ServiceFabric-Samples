// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Authentication.Session
// File             : SessionAuthenticationMiddlewareExtensions.cs
// Created          : 2016-07-06  12:29 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.AspNetCore.Builder;

namespace Credit.Kolibre.Foundation.AspNetCore.Authentication.Session
{
    /// <summary>
    ///     Extension methods to add session authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class SessionAuthenticationMiddlewareExtensions
    {
        /// <summary>
        ///     Adds the <see cref="SessionAuthenticationMiddleware" /> middleware to the specified <see cref="IApplicationBuilder" />, which enables session authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" /> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseSessionAuthentication(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<SessionAuthenticationMiddleware>();
        }

        /// <summary>
        ///     Adds the <see cref="SessionAuthenticationMiddleware" /> middleware to the specified <see cref="IApplicationBuilder" />, which enables session authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" /> to add the middleware to.</param>
        /// <param name="authenticationScheme">The AuthenticationScheme to configure the middleware.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseSessionAuthentication(this IApplicationBuilder app, string authenticationScheme)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            SessionAuthenticationOptions options = new SessionAuthenticationOptions
            {
                AuthenticationScheme = authenticationScheme
            };

            return app.UseMiddleware<SessionAuthenticationMiddleware>(options);
        }

        /// <summary>
        ///     Adds the <see cref="SessionAuthenticationMiddleware" /> middleware to the specified <see cref="IApplicationBuilder" />, which enables session authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" /> to add the middleware to.</param>
        /// <param name="options">A <see cref="SessionAuthenticationOptions" /> that specifies options for the middleware.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseSessionAuthentication(this IApplicationBuilder app, SessionAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<SessionAuthenticationMiddleware>(options);
        }
    }
}