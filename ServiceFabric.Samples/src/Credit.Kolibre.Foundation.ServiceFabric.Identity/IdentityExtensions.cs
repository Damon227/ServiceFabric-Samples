// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.ServiceFabric.Identity
// File             : IdentityExtensions.cs
// Created          : 2017-02-15  18:46
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Credit.Kolibre.Foundation.AspNetCore.Authentication.Session;
using Credit.Kolibre.Foundation.ServiceFabric.Identity.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Identity extensions for <see cref="IApplicationBuilder" />.
    /// </summary>
    public static class IdentityExtensions
    {
        /// <summary>
        ///     Adds an Entity Framework implementation of identity information stores.
        /// </summary>
        /// <typeparam name="TContext">The Entity Framework database context to use.</typeparam>
        /// <param name="builder">The <see cref="IdentityBuilder" /> instance this method extends.</param>
        /// <returns>The <see cref="IdentityBuilder" /> instance this method extends.</returns>
        public static IdentityBuilder AddEntityFrameworkStores<TContext>(this IdentityBuilder builder)
            where TContext : DbContext
        {
            builder.Services.TryAdd(GetDefaultServices(builder.UserType, builder.RoleType, typeof (TContext)));
            return builder;
        }

        /// <summary>
        ///     Adds an Entity Framework implementation of identity information stores.
        /// </summary>
        /// <typeparam name="TContext">The Entity Framework database context to use.</typeparam>
        /// <typeparam name="TKey">The type of the primary key used for the users and roles.</typeparam>
        /// <param name="builder">The <see cref="IdentityBuilder" /> instance this method extends.</param>
        /// <returns>The <see cref="IdentityBuilder" /> instance this method extends.</returns>
        public static IdentityBuilder AddEntityFrameworkStores<TContext, TKey>(this IdentityBuilder builder)
            where TContext : DbContext
            where TKey : IEquatable<TKey>
        {
            builder.Services.TryAdd(GetDefaultServices(builder.UserType, builder.RoleType, typeof (TContext), typeof (TKey)));
            return builder;
        }

        /// <summary>
        ///     Adds the default identity system configuration for the specified User and Role types.
        /// </summary>
        /// <typeparam name="TUser">The type representing a User in the system.</typeparam>
        /// <typeparam name="TRole">The type representing a Role in the system.</typeparam>
        /// <param name="services">The services available in the application.</param>
        /// <returns>An <see cref="IdentityBuilder" /> for creating and configuring the identity system.</returns>
        public static IdentityBuilder AddIdentity<TUser, TRole>(
            this IServiceCollection services)
            where TUser : class
            where TRole : class
        {
            return services.AddIdentity<TUser, TRole>(null);
        }

        /// <summary>
        ///     Adds and configures the identity system for the specified User and Role types.
        /// </summary>
        /// <typeparam name="TUser">The type representing a User in the system.</typeparam>
        /// <typeparam name="TRole">The type representing a Role in the system.</typeparam>
        /// <param name="services">The services available in the application.</param>
        /// <param name="setupAction">An action to configure the <see cref="IdentityOptions" />.</param>
        /// <returns>An <see cref="IdentityBuilder" /> for creating and configuring the identity system.</returns>
        public static IdentityBuilder AddIdentity<TUser, TRole>(
            this IServiceCollection services,
            Action<IdentityOptions> setupAction)
            where TUser : class
            where TRole : class
        {
            // Services used by identity
            services.AddAuthentication();

            // Hosting doesn't add IHttpContextAccessor by default
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<ISessionManager, SessionManager>();

            // Identity services
            services.TryAddSingleton<IdentityMarkerService>();
            services.TryAddScoped<IUserValidator<TUser>, UserValidator<TUser>>();
            services.TryAddScoped<IPasswordValidator<TUser>, PasswordValidator<TUser>>();
            services.TryAddScoped<IPasswordHasher, PasswordHasher>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.TryAddScoped<IRoleValidator<TRole>, RoleValidator<TRole>>();

            // No interface for the error describer so we can add errors without rev'ing the interface
            services.TryAddScoped<ErrorDescriber>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<TUser>, UserClaimsPrincipalFactory<TUser, TRole>>();
            services.TryAddScoped<UserManager<TUser>, UserManager<TUser>>();
            services.TryAddScoped<SignInManager<TUser>, SignInManager<TUser>>();
            services.TryAddScoped<RoleManager<TRole>, RoleManager<TRole>>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return new IdentityBuilder(typeof (TUser), typeof (TRole), services);
        }

        /// <summary>
        ///     Enables ASP.NET identity for the current application.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" /> instance this method extends.</param>
        /// <returns>The <see cref="IApplicationBuilder" /> instance this method extends.</returns>
        public static IApplicationBuilder UseIdentity(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            IdentityMarkerService marker = app.ApplicationServices.GetService<IdentityMarkerService>();
            if (marker == null)
            {
                throw new InvalidOperationException(Resource.MustCallAddIdentity);
            }

            IdentityOptions options = app.ApplicationServices.GetRequiredService<IOptions<IdentityOptions>>().Value;
            //app.UseSessionAuthentication(options.Schemes.ExternalAuthenticationScheme);
            //app.UseSessionAuthentication(options.Schemes.TwoFactorRememberMeAuthenticationScheme);
            //app.UseSessionAuthentication(options.Schemes.TwoFactorUserIdAuthenticationScheme);
            app.UseSessionAuthentication(options.Schemes.ApplicationAuthenticationScheme);
            return app;
        }

        private static IServiceCollection GetDefaultServices(Type userType, Type roleType, Type contextType, Type keyType = null)
        {
            keyType = keyType ?? typeof (string);
            Type userStoreType = typeof (UserStore<,,,>).MakeGenericType(userType, roleType, contextType, keyType);
            Type roleStoreType = typeof (RoleStore<,,>).MakeGenericType(roleType, contextType, keyType);

            ServiceCollection services = new ServiceCollection();
            services.AddScoped(
                typeof (IUserStore<>).MakeGenericType(userType),
                userStoreType);
            services.AddScoped(
                typeof (IRoleStore<>).MakeGenericType(roleType),
                roleStoreType);
            return services;
        }
    }
}