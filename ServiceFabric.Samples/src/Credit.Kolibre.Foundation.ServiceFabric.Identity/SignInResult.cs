// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Identity
// File             : SignInResult.cs
// Created          : 2016-07-02  4:28 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Represents the result of a sign-in operation.
    /// </summary>
    public class SignInResult
    {
        /// <summary>
        ///     Returns a flag indication whether the sign-in was successful.
        /// </summary>
        /// <value>True if the sign-in was successful, otherwise false.</value>
        public bool Succeeded { get; protected set; }

        /// <summary>
        ///     Returns a flag indication whether the user attempting to sign-in is locked out.
        /// </summary>
        /// <value>True if the user attempting to sign-in is locked out, otherwise false.</value>
        public bool IsLockedOut { get; protected set; }

        /// <summary>
        ///     Returns a flag indication whether the user attempting to sign-in is not allowed to sign-in.
        /// </summary>
        /// <value>True if the user attempting to sign-in is not allowed to sign-in, otherwise false.</value>
        public bool IsNotAllowed { get; protected set; }

        /// <summary>
        ///     Returns a <see cref="SignInResult" /> that represents a successful sign-in.
        /// </summary>
        /// <returns>A <see cref="SignInResult" /> that represents a successful sign-in.</returns>
        public static SignInResult Success { get; } = new SignInResult { Succeeded = true };

        /// <summary>
        ///     Returns a <see cref="SignInResult" /> that represents a unsuccessfully sign-in.
        /// </summary>
        /// <returns>A <see cref="SignInResult" /> that represents a unsuccessfully sign-in.</returns>
        public static SignInResult Failed { get; } = new SignInResult();

        /// <summary>
        ///     Returns a <see cref="SignInResult" /> that represents a sign-in attempt that unsuccessfully because
        ///     the user was logged out.
        /// </summary>
        /// <returns>
        ///     A <see cref="SignInResult" /> that represents sign-in attempt that unsuccessfully due to the
        ///     user being locked out.
        /// </returns>
        public static SignInResult LockedOut { get; } = new SignInResult { IsLockedOut = true };

        /// <summary>
        ///     Returns a <see cref="SignInResult" /> that represents a sign-in attempt that unsuccessfully because
        ///     the user is not allowed to sign-in.
        /// </summary>
        /// <returns>
        ///     A <see cref="SignInResult" /> that represents sign-in attempt that unsuccessfully due to the
        ///     user is not allowed to sign-in.
        /// </returns>
        public static SignInResult NotAllowed { get; } = new SignInResult { IsNotAllowed = true };

        /// <summary>
        ///     Converts the value of the current <see cref="SignInResult" /> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of value of the current <see cref="SignInResult" /> object.</returns>
        public override string ToString()
        {
            return IsLockedOut ? "Lockedout" :
                IsNotAllowed ? "NotAllowed" :
                    Succeeded ? "Succeeded" : "Failed";
        }
    }
}