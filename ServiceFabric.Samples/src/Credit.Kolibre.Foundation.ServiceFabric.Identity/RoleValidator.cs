// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Identity
// File             : RoleValidator.cs
// Created          : 2016-07-06  11:00 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Provides the default validation of roles.
    /// </summary>
    /// <typeparam name="TRole">The type encapsulating a role.</typeparam>
    public class RoleValidator<TRole> : IRoleValidator<TRole> where TRole : class
    {
        /// <summary>
        ///     Creates a new instance of <see cref="RoleValidator{TRole}" />/
        /// </summary>
        /// <param name="errors">The <see cref="ErrorDescriber" /> used to provider error messages.</param>
        public RoleValidator(ErrorDescriber errors = null)
        {
            Describer = errors ?? new ErrorDescriber();
        }

        private ErrorDescriber Describer { get; }

        #region IRoleValidator<TRole> Members

        /// <summary>
        ///     Validates a role as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="RoleManager{TRole}" /> managing the role store.</param>
        /// <param name="role">The role to validate.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the <see cref="Result" /> of the asynchronous validation.</returns>
        public virtual async Task<Result> ValidateAsync(RoleManager<TRole> manager, TRole role)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            List<Error> errors = new List<Error>();
            await ValidateRoleName(manager, role, errors);
            if (errors.Count > 0)
            {
                return Result.Failed(errors.ToArray());
            }
            return Result.Success;
        }

        #endregion

        private async Task ValidateRoleName(RoleManager<TRole> manager, TRole role,
            ICollection<Error> errors)
        {
            string roleName = await manager.GetRoleNameAsync(role);
            if (string.IsNullOrWhiteSpace(roleName))
            {
                errors.Add(Describer.InvalidRoleName(roleName));
            }
            else
            {
                TRole owner = await manager.FindByNameAsync(roleName);
                if (owner != null &&
                    !string.Equals(await manager.GetRoleIdAsync(owner), await manager.GetRoleIdAsync(role)))
                {
                    errors.Add(Describer.DuplicateRoleName(roleName));
                }
            }
        }
    }
}