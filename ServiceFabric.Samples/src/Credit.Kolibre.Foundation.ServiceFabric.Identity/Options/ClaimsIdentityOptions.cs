// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.ServiceFabric.Identity
// File             : ClaimsIdentityOptions.cs
// Created          : 2017-02-15  18:46
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity.Options
{
    /// <summary>
    ///     Options used to configure the claim types used for well known claims.
    /// </summary>
    public class ClaimsIdentityOptions
    {
        /// <summary>
        ///     Gets or sets the ClaimType used for a Role claim.
        /// </summary>
        /// <remarks>
        ///     This defaults to <see cref="ClaimTypes.Role" />.
        /// </remarks>
        public string RoleClaimType { get; set; } = ClaimTypes.Role;

        /// <summary>
        ///     Gets or sets the ClaimType used for a user cellphone claim.
        /// </summary>
        /// <remarks>
        ///     This defaults to <see cref="ClaimTypes.MobilePhone" />.
        /// </remarks>
        public string UserCellphonelClaimType { get; set; } = ClaimTypes.MobilePhone;

        /// <summary>
        ///     Gets or sets the ClaimType used for a user email claim.
        /// </summary>
        /// <remarks>
        ///     This defaults to <see cref="ClaimTypes.Email" />.
        /// </remarks>
        public string UserEmailClaimType { get; set; } = ClaimTypes.Email;

        /// <summary>
        ///     Gets or sets the ClaimType used for the user identifier claim.
        /// </summary>
        /// <remarks>
        ///     This defaults to <see cref="ClaimTypes.NameIdentifier" />.
        /// </remarks>
        public string UserIdClaimType { get; set; } = ClaimTypes.NameIdentifier;

        /// <summary>
        ///     Gets or sets the ClaimType used for the user name claim.
        /// </summary>
        /// <remarks>
        ///     This defaults to <see cref="ClaimTypes.Name" />.
        /// </remarks>
        public string UserNameClaimType { get; set; } = ClaimTypes.Name;
    }
}