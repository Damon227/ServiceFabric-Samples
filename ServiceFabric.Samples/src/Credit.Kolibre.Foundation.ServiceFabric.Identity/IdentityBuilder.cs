// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.ServiceFabric.Identity
// File             : IdentityBuilder.cs
// Created          : 2017-02-15  18:46
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Reflection;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.Extensions.DependencyInjection;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Helper functions for configuring identity services.
    /// </summary>
    public class IdentityBuilder
    {
        /// <summary>
        ///     Creates a new instance of <see cref="IdentityBuilder" />.
        /// </summary>
        /// <param name="user">The <see cref="Type" /> to use for the users.</param>
        /// <param name="role">The <see cref="Type" /> to use for the roles.</param>
        /// <param name="services">The <see cref="IServiceCollection" /> to attach to.</param>
        public IdentityBuilder(Type user, Type role, IServiceCollection services)
        {
            UserType = user;
            RoleType = role;
            Services = services;
        }

        /// <summary>
        ///     Gets the <see cref="Type" /> used for users.
        /// </summary>
        /// <value>
        ///     The <see cref="Type" /> used for users.
        /// </value>
        public Type UserType { get; }


        /// <summary>
        ///     Gets the <see cref="Type" /> used for roles.
        /// </summary>
        /// <value>
        ///     The <see cref="Type" /> used for roles.
        /// </value>
        public Type RoleType { get; }

        /// <summary>
        ///     Gets the <see cref="IServiceCollection" /> services are attached to.
        /// </summary>
        /// <value>
        ///     The <see cref="IServiceCollection" /> services are attached to.
        /// </value>
        public IServiceCollection Services { get; }

        /// <summary>
        ///     Adds an <see cref="ErrorDescriber" />.
        /// </summary>
        /// <typeparam name="TDescriber">The type of the error describer.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder" /> instance.</returns>
        public virtual IdentityBuilder AddErrorDescriber<TDescriber>() where TDescriber : ErrorDescriber
        {
            Services.AddScoped<ErrorDescriber, TDescriber>();
            return this;
        }

        /// <summary>
        ///     Adds an <see cref="IPasswordValidator{TUser}" /> for the <seealso cref="UserType" />.
        /// </summary>
        /// <typeparam name="T">The user type whose password will be validated.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder" /> instance.</returns>
        public virtual IdentityBuilder AddPasswordValidator<T>() where T : class
        {
            return AddScoped(typeof (IPasswordValidator<>).MakeGenericType(UserType), typeof (T));
        }

        /// <summary>
        ///     Adds a <see cref="RoleManager{TRole}" /> for the <seealso cref="RoleType" />.
        /// </summary>
        /// <typeparam name="TRoleManager">The type of the role manager to add.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder" /> instance.</returns>
        public virtual IdentityBuilder AddRoleManager<TRoleManager>() where TRoleManager : class
        {
            Type managerType = typeof (RoleManager<>).MakeGenericType(RoleType);
            Type customType = typeof (TRoleManager);
            if (managerType == customType ||
                !managerType.GetTypeInfo().IsAssignableFrom(customType.GetTypeInfo()))
            {
                throw new InvalidOperationException(Resource.InvalidManagerType.FormatWith(customType.Name, "RoleManager", RoleType.Name));
            }
            Services.AddScoped(typeof (TRoleManager), services => services.GetRequiredService(managerType));
            return AddScoped(managerType, typeof (TRoleManager));
        }

        /// <summary>
        ///     Adds a <see cref="IRoleStore{TRole}" /> for the <seealso cref="RoleType" />.
        /// </summary>
        /// <typeparam name="T">The role type held in the store.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder" /> instance.</returns>
        public virtual IdentityBuilder AddRoleStore<T>() where T : class
        {
            return AddScoped(typeof (IRoleStore<>).MakeGenericType(RoleType), typeof (T));
        }

        /// <summary>
        ///     Adds an <see cref="IRoleValidator{TRole}" /> for the <seealso cref="RoleType" />.
        /// </summary>
        /// <typeparam name="T">The role type to validate.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder" /> instance.</returns>
        public virtual IdentityBuilder AddRoleValidator<T>() where T : class
        {
            return AddScoped(typeof (IRoleValidator<>).MakeGenericType(RoleType), typeof (T));
        }

        /// <summary>
        ///     Adds a <see cref="UserManager{TUser}" /> for the <seealso cref="UserType" />.
        /// </summary>
        /// <typeparam name="TUserManager">The type of the user manager to add.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder" /> instance.</returns>
        public virtual IdentityBuilder AddUserManager<TUserManager>() where TUserManager : class
        {
            Type userManagerType = typeof (UserManager<>).MakeGenericType(UserType);
            Type customType = typeof (TUserManager);
            if (userManagerType == customType ||
                !userManagerType.GetTypeInfo().IsAssignableFrom(customType.GetTypeInfo()))
            {
                throw new InvalidOperationException(Resource.InvalidManagerType.FormatWith(customType.Name, "UserManager", UserType.Name));
            }
            Services.AddScoped(customType, services => services.GetRequiredService(userManagerType));
            return AddScoped(userManagerType, customType);
        }

        /// <summary>
        ///     Adds an <see cref="IUserStore{TUser}" /> for the <seealso cref="UserType" />.
        /// </summary>
        /// <typeparam name="T">The user type whose password will be validated.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder" /> instance.</returns>
        public virtual IdentityBuilder AddUserStore<T>() where T : class
        {
            return AddScoped(typeof (IUserStore<>).MakeGenericType(UserType), typeof (T));
        }

        /// <summary>
        ///     Adds an <see cref="IUserValidator{TUser}" /> for the <seealso cref="UserType" />.
        /// </summary>
        /// <typeparam name="T">The user type to validate.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder" /> instance.</returns>
        public virtual IdentityBuilder AddUserValidator<T>() where T : class
        {
            return AddScoped(typeof (IUserValidator<>).MakeGenericType(UserType), typeof (T));
        }

        private IdentityBuilder AddScoped(Type serviceType, Type concreteType)
        {
            Services.AddScoped(serviceType, concreteType);
            return this;
        }
    }
}