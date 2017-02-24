// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Identity
// File             : RoleManager.cs
// Created          : 2016-07-06  10:39 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Provides the APIs for managing roles in a persistence store.
    /// </summary>
    /// <typeparam name="TRole">The type encapsulating a role.</typeparam>
    public class RoleManager<TRole> : IDisposable where TRole : class
    {
        private readonly HttpContext _context;
        private bool _disposed;

        /// <summary>
        ///     Constructs a new instance of <see cref="RoleManager{TRole}" />.
        /// </summary>
        /// <param name="store">The persistence store the manager will operate over.</param>
        /// <param name="roleValidators">A collection of validators for roles.</param>
        /// <param name="keyNormalizer">The normalizer to use when normalizing role names to keys.</param>
        /// <param name="errors">The <see cref="ErrorDescriber" /> used to provider error messages.</param>
        /// <param name="logger">The logger used to log messages, warnings and errors.</param>
        /// <param name="contextAccessor">The accessor used to access the <see cref="HttpContext" />.</param>
        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        public RoleManager(IRoleStore<TRole> store,
            IEnumerable<IRoleValidator<TRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            ErrorDescriber errors,
            ILogger<RoleManager<TRole>> logger,
            IHttpContextAccessor contextAccessor)
        {
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }
            Store = store;
            KeyNormalizer = keyNormalizer ?? new UpperInvariantLookupNormalizer();
            ErrorDescriber = errors ?? new ErrorDescriber();
            _context = contextAccessor?.HttpContext;
            Logger = logger;

            if (roleValidators != null)
            {
                foreach (IRoleValidator<TRole> v in roleValidators)
                {
                    RoleValidators.Add(v);
                }
            }
        }

        /// <summary>
        ///     Gets an IQueryable collection of Roles if the persistence store is an <see cref="IQueryableRoleStore{TRole}" />,
        ///     otherwise throws a <see cref="NotSupportedException" />.
        /// </summary>
        /// <value>An IQueryable collection of Roles if the persistence store is an <see cref="IQueryableRoleStore{TRole}" />.</value>
        /// <exception cref="NotSupportedException">Thrown if the persistence store is not an <see cref="IQueryableRoleStore{TRole}" />.</exception>
        /// <remarks>
        ///     Callers to this property should use <see cref="SupportsQueryableRoles" /> to ensure the backing role store supports
        ///     returning an IQueryable list of roles.
        /// </remarks>
        public virtual IQueryable<TRole> Roles
        {
            get
            {
                IQueryableRoleStore<TRole> queryableStore = Store as IQueryableRoleStore<TRole>;
                if (queryableStore == null)
                {
                    throw new NotSupportedException(Resource.StoreNotIQueryableRoleStore);
                }
                return queryableStore.Roles;
            }
        }

        /// <summary>
        ///     Gets a flag indicating whether the underlying persistence store supports returning an <see cref="IQueryable" /> collection of roles.
        /// </summary>
        /// <value>
        ///     true if the underlying persistence store supports returning an <see cref="IQueryable" /> collection of roles, otherwise false.
        /// </value>
        public virtual bool SupportsQueryableRoles
        {
            get
            {
                ThrowIfDisposed();
                return Store is IQueryableRoleStore<TRole>;
            }
        }

        /// <summary>
        ///     Gets a flag indicating whether the underlying persistence store supports <see cref="Claim" />s for roles.
        /// </summary>
        /// <value>
        ///     true if the underlying persistence store supports <see cref="Claim" />s for roles, otherwise false.
        /// </value>
        public virtual bool SupportsRoleClaims
        {
            get
            {
                ThrowIfDisposed();
                return Store is IRoleClaimStore<TRole>;
            }
        }

        /// <summary>
        ///     Gets the <see cref="ErrorDescriber" /> used to provider error messages.
        /// </summary>
        /// <value>
        ///     The <see cref="ErrorDescriber" /> used to provider error messages.
        /// </value>
        internal ErrorDescriber ErrorDescriber { get; set; }

        /// <summary>
        ///     Gets the normalizer to use when normalizing role names to keys.
        /// </summary>
        /// <value>
        ///     The normalizer to use when normalizing role names to keys.
        /// </value>
        internal ILookupNormalizer KeyNormalizer { get; set; }

        /// <summary>
        ///     Gets a list of validators for roles to call before persistence.
        /// </summary>
        /// <value>A list of validators for roles to call before persistence.</value>
        internal IList<IRoleValidator<TRole>> RoleValidators { get; } = new List<IRoleValidator<TRole>>();

        /// <summary>
        ///     Gets the <see cref="ILogger" /> used to log messages from the manager.
        /// </summary>
        /// <value>
        ///     The <see cref="ILogger" /> used to log messages from the manager.
        /// </value>
        protected internal virtual ILogger Logger { get; set; }

        /// <summary>
        ///     Gets the persistence store this instance operates over.
        /// </summary>
        /// <value>The persistence store this instance operates over.</value>
        protected IRoleStore<TRole> Store { get; }

        private CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;

        #region IDisposable Members

        /// <summary>
        ///     Releases all resources used by the role manager.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        ///     Adds a claim to a role.
        /// </summary>
        /// <param name="role">The role to add the claim to.</param>
        /// <param name="claim">The claim to add.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> AddClaimAsync(TRole role, Claim claim)
        {
            ThrowIfDisposed();

            if (!SupportsRoleClaims)
            {
                throw new NotSupportedException(Resource.StoreNotIRoleClaimStore);
            }

            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            IRoleClaimStore<TRole> claimStore = GetClaimStore();
            await claimStore.AddClaimAsync(role, claim, CancellationToken);
            return await UpdateRoleAsync(role);
        }

        /// <summary>
        ///     Creates the specified <paramref name="role" /> in the persistence store.
        /// </summary>
        /// <param name="role">The role to create.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation.
        /// </returns>
        public virtual async Task<Result> CreateAsync(TRole role)
        {
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            Result result = await ValidateRoleInternal(role);
            if (!result.Succeeded)
            {
                return result;
            }
            await UpdateNormalizedRoleNameAsync(role);
            result = await Store.CreateAsync(role, CancellationToken);
            return result;
        }

        /// <summary>
        ///     Deletes the specified <paramref name="role" />.
        /// </summary>
        /// <param name="role">The role to delete.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" /> for the delete.
        /// </returns>
        public virtual Task<Result> DeleteAsync(TRole role)
        {
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return Store.DeleteAsync(role, CancellationToken);
        }

        /// <summary>
        ///     Finds the role associated with the specified <paramref name="roleId" /> if any.
        /// </summary>
        /// <param name="roleId">The role ID whose role should be returned.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the role
        ///     associated with the specified <paramref name="roleId" />
        /// </returns>
        public virtual Task<TRole> FindByIdAsync(string roleId)
        {
            ThrowIfDisposed();

            if (roleId.IsNullOrEmpty())
            {
                throw new ArgumentException(Foundation.Resource.Argument_EmptyOrNullString, nameof(roleId));
            }

            return Store.FindByIdAsync(roleId, CancellationToken);
        }

        /// <summary>
        ///     Finds the role associated with the specified <paramref name="roleName" /> if any.
        /// </summary>
        /// <param name="roleName">The name of the role to be returned.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the role
        ///     associated with the specified <paramref name="roleName" />
        /// </returns>
        public virtual Task<TRole> FindByNameAsync(string roleName)
        {
            ThrowIfDisposed();

            if (roleName.IsNullOrEmpty())
            {
                throw new ArgumentException(Foundation.Resource.Argument_EmptyOrNullString, nameof(roleName));
            }

            return Store.FindByNameAsync(NormalizeKey(roleName), CancellationToken);
        }

        /// <summary>
        ///     Gets a list of claims associated with the specified <paramref name="role" />.
        /// </summary>
        /// <param name="role">The role whose claims should be returned.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the list of <see cref="Claim" />s
        ///     associated with the specified <paramref name="role" />.
        /// </returns>
        public virtual Task<IList<Claim>> GetClaimsAsync(TRole role)
        {
            ThrowIfDisposed();

            if (!SupportsRoleClaims)
            {
                throw new NotSupportedException(Resource.StoreNotIRoleClaimStore);
            }

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            IRoleClaimStore<TRole> claimStore = GetClaimStore();
            return claimStore.GetClaimsAsync(role, CancellationToken);
        }

        /// <summary>
        ///     Gets the ID of the specified <paramref name="role" />.
        /// </summary>
        /// <param name="role">The role whose ID should be retrieved.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the ID of the
        ///     specified <paramref name="role" />.
        /// </returns>
        public virtual Task<string> GetRoleIdAsync(TRole role)
        {
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return Store.GetRoleIdAsync(role, CancellationToken);
        }

        /// <summary>
        ///     Gets the name of the specified <paramref name="role" />.
        /// </summary>
        /// <param name="role">The role whose name should be retrieved.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the name of the
        ///     specified <paramref name="role" />.
        /// </returns>
        public virtual Task<string> GetRoleNameAsync(TRole role)
        {
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return Store.GetRoleNameAsync(role, CancellationToken);
        }

        /// <summary>
        ///     Gets a normalized representation of the specified <paramref name="key" />.
        /// </summary>
        /// <param name="key">The value to normalize.</param>
        /// <returns>A normalized representation of the specified <paramref name="key" />.</returns>
        public virtual string NormalizeKey(string key)
        {
            return KeyNormalizer == null ? key : KeyNormalizer.Normalize(key);
        }

        /// <summary>
        ///     Removes a claim from a role.
        /// </summary>
        /// <param name="role">The role to remove the claim from.</param>
        /// <param name="claim">The claim to remove.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> RemoveClaimAsync(TRole role, Claim claim)
        {
            ThrowIfDisposed();

            if (!SupportsRoleClaims)
            {
                throw new NotSupportedException(Resource.StoreNotIRoleClaimStore);
            }

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            IRoleClaimStore<TRole> claimStore = GetClaimStore();
            await claimStore.RemoveClaimAsync(role, claim, CancellationToken);
            return await UpdateRoleAsync(role);
        }

        /// <summary>
        ///     Gets a flag indicating whether the specified <paramref name="roleName" /> exists.
        /// </summary>
        /// <param name="roleName">The role name whose existence should be checked.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing true if the role name exists, otherwise false.
        /// </returns>
        public virtual async Task<bool> RoleExistsAsync(string roleName)
        {
            ThrowIfDisposed();
            if (roleName == null)
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            return await FindByNameAsync(NormalizeKey(roleName)) != null;
        }

        /// <summary>
        ///     Sets the name of the specified <paramref name="role" />.
        /// </summary>
        /// <param name="role">The role whose name should be set.</param>
        /// <param name="name">The name to set.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" />
        ///     of the operation.
        /// </returns>
        public virtual async Task<Result> SetRoleNameAsync(TRole role, string name)
        {
            ThrowIfDisposed();

            await Store.SetRoleNameAsync(role, name, CancellationToken);
            await UpdateNormalizedRoleNameAsync(role);
            return Result.Success;
        }

        /// <summary>
        ///     Updates the specified <paramref name="role" />.
        /// </summary>
        /// <param name="role">The role to updated.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="Result" /> for the update.
        /// </returns>
        public virtual Task<Result> UpdateAsync(TRole role)
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return UpdateRoleAsync(role);
        }

        /// <summary>
        ///     Updates the normalized name for the specified <paramref name="role" />.
        /// </summary>
        /// <param name="role">The role whose normalized name needs to be updated.</param>
        /// <returns>
        ///     The <see cref="Task" /> that represents the asynchronous operation.
        /// </returns>
        public virtual async Task UpdateNormalizedRoleNameAsync(TRole role)
        {
            string name = await GetRoleNameAsync(role);
            await Store.SetNormalizedRoleNameAsync(role, NormalizeKey(name), CancellationToken);
        }

        /// <summary>
        ///     Releases the unmanaged resources used by the role manager and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                Store.Dispose();
            }
            _disposed = true;
        }

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        // IRoleClaimStore methods
        private IRoleClaimStore<TRole> GetClaimStore()
        {
            IRoleClaimStore<TRole> cast = Store as IRoleClaimStore<TRole>;
            if (cast == null)
            {
                throw new NotSupportedException(Resource.StoreNotIRoleClaimStore);
            }
            return cast;
        }

        private async Task<Result> UpdateRoleAsync(TRole role)
        {
            Result result = await ValidateRoleInternal(role);
            if (!result.Succeeded)
            {
                return result;
            }
            await UpdateNormalizedRoleNameAsync(role);
            return await Store.UpdateAsync(role, CancellationToken);
        }

        private async Task<Result> ValidateRoleInternal(TRole role)
        {
            List<Error> errors = new List<Error>();
            foreach (IRoleValidator<TRole> v in RoleValidators)
            {
                Result result = await v.ValidateAsync(this, role);
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors);
                }
            }
            if (errors.Count > 0)
            {
                Logger.LogWarning(0, "Role {roleId} validation unsuccessfully: {errors}.", await GetRoleIdAsync(role), string.Join(";", errors.Select(e => e.Code)));
                return Result.Failed(errors.ToArray());
            }
            return Result.Success;
        }
    }
}