// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : ServiceCollectionExtensions.cs
// Created          : 2016-09-09  20:08
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable InconsistentNaming

namespace Credit.Kolibre.Foundation.ServiceFabric.DI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSingletonService<ITProvider, TProvider>(this IServiceCollection services)
            where TProvider : class, ITProvider
            where ITProvider : class
        {
            services.AddSingleton<ITProvider, TProvider>();

            return services;
        }

        public static IServiceCollection AddSingletonService<ITProvider, TProvider, TOptions>(this IServiceCollection services, Action<TOptions> setupAction)
            where TProvider : class, ITProvider
            where ITProvider : class
            where TOptions : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return services.AddSingletonService<ITProvider, TProvider>();
        }

        public static IServiceCollection AddSingletonService<ITProvider, TProvider, TOptions>(this IServiceCollection services, IConfiguration configuration)
            where TProvider : class, ITProvider
            where ITProvider : class
            where TOptions : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration != null)
            {
                services.Configure<TOptions>(configuration);
            }

            return services.AddSingletonService<ITProvider, TProvider>();
        }

        public static IServiceCollection AddScopedService<ITProvider, TProvider>(this IServiceCollection services)
            where TProvider : class, ITProvider
            where ITProvider : class
        {
            services.AddScoped<ITProvider, TProvider>();

            return services;
        }

        public static IServiceCollection AddScopedService<ITProvider, TProvider, TOptions>(this IServiceCollection services, Action<TOptions> setupAction)
            where TProvider : class, ITProvider
            where ITProvider : class
            where TOptions : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return services.AddScopedService<ITProvider, TProvider>();
        }

        public static IServiceCollection AddScopedService<ITProvider, TProvider, TOptions>(this IServiceCollection services, IConfiguration configuration)
            where TProvider : class, ITProvider
            where ITProvider : class
            where TOptions : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration != null)
            {
                services.Configure<TOptions>(configuration);
            }

            return services.AddScopedService<ITProvider, TProvider>();
        }
    }
}