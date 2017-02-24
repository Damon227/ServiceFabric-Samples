// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Session
// File             : SessionMiddlewareExtensions.cs
// Created          : 2016-07-03  9:47 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace Credit.Kolibre.Foundation.ServiceFabric.Seesion
{
    /// <summary>
    ///     Extension methods for adding the <see cref="JsonSessionMiddleware" /> to an application.
    /// </summary>
    public static class SessionMiddlewareExtensions
    {
        /// <summary>
        ///     Adds the <see cref="JsonSessionMiddleware" /> to automatically enable session state for the application.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>The <see cref="IApplicationBuilder" />.</returns>
        public static IApplicationBuilder UseJsonSession(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            IDistributedCache distributedCache = app.ApplicationServices.GetService<IDistributedCache>();
            if (distributedCache == null)
            {
                throw new InvalidOperationException("No IDistributedCache is registered.");
            }

            return app.UseMiddleware<JsonSessionMiddleware>();
        }
    }
}