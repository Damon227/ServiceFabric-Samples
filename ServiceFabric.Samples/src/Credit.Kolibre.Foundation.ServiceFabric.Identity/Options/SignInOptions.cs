// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Identity
// File             : SignInOptions.cs
// Created          : 2016-07-02  4:28 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity.Options
{
    /// <summary>
    ///     Options for configuring sign in..
    /// </summary>
    public class SignInOptions
    {
        /// <summary>
        ///     Gets or sets a flag indicating whether a confirmed email address is required to sign in.
        /// </summary>
        /// <value>True if a user must have a confirmed email address before they can sign in, otherwise false.</value>
        public bool RequireConfirmedEmail { get; set; } = true;

        /// <summary>
        ///     Gets or sets a flag indicating whether a confirmed telecellphone is required to sign in.
        /// </summary>
        /// <value>True if a user must have a confirmed telecellphone before they can sign in, otherwise false.</value>
        public bool RequireConfirmedCellphone { get; set; } = true;
    }
}