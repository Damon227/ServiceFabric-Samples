// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Authentication.Session
// File             : SessionAuthenticationServiceCollectionExtensions.cs
// Created          : 2016-07-10  2:53 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Credit.Kolibre.Foundation.AspNetCore.Authentication.Session
{
    public static class SessionAuthenticationServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds Redis distributed caching services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An action to configure the <see cref="SessionAuthenticationOptions" />.</param>
        /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddSessionAuthenticationService(this IServiceCollection services, Action<SessionAuthenticationOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            return services.Configure(setupAction);
        }

        /// <summary>
        ///     Adds Redis distributed caching services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The configuration to configure the <see cref="SessionAuthenticationOptions" />.</param>
        /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddSessionAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration != null)
            {
                services.Configure<SessionAuthenticationOptions>(configuration);
            }

            return services;
        }
    }
}