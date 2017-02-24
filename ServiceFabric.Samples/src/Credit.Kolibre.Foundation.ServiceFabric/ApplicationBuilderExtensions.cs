// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : ApplicationBuilderExtensions.cs
// Created          : 2016-07-10  12:21 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Credit.Kolibre.Foundation.ServiceFabric.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Credit.Kolibre.Foundation.ServiceFabric
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseClientIpAccessing(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<ClientIpAccessingMiddleware>();
        }

        public static IApplicationBuilder UseInternalServerErrorExceptionHandler(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<InternalServerErrorExceptionHandlerMiddleware>();
        }

        public static IApplicationBuilder UseKolibreCreditConstants(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<KolibreCreditConstantsMiddleware>();
        }
    }
}