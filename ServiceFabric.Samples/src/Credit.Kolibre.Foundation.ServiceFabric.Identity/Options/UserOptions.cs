// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Identity
// File             : UserOptions.cs
// Created          : 2016-07-01  8:12 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity.Options
{
    /// <summary>
    ///     Options for user validation.
    /// </summary>
    public class UserOptions
    {
        /// <summary>
        ///     Gets or sets the list of allowed characters in the username used to validate user names.
        /// </summary>
        /// <value>
        ///     The list of allowed characters in the username used to validate user names.
        /// </value>
        //public string AllowedUserNameCharacters { get; set; } = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        public string AllowedUserNameCharacters { get; set; } = "";

        /// <summary>
        ///     Gets or sets a flag indicating whether the application requires unique emails for its users.
        /// </summary>
        /// <value>
        ///     True if the application requires each user to have their own, unique email, otherwise false.
        /// </value>
        public bool RequireUniqueEmail { get; set; } = true;
    }
}