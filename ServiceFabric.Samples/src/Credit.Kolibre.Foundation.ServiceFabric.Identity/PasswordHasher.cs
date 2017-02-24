﻿// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.ServiceFabric.Identity
// File             : PasswordHasher.cs
// Created          : 2017-02-15  18:46
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Credit.Kolibre.Foundation.ServiceFabric.Identity.Options;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Implements the standard Identity password hashing.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        /* =======================
         * HASHED PASSWORD FORMATS
         * =======================
         * 
         * Version 2:
         * PBKDF2 with HMAC-SHA1, 128-bit salt, 256-bit subkey, 1000 iterations.
         * (See also: SDL crypto guidelines v5.1, Part III)
         * Format: { 0x00, salt, subkey }
         *
         * Version 3:
         * PBKDF2 with HMAC-SHA256, 128-bit salt, 256-bit subkey, 10000 iterations.
         * Format: { 0x01, prf (UInt32), iter count (UInt32), salt length (UInt32), salt, subkey }
         * (All UInt32s are stored big-endian.)
         */

        private readonly PasswordHasherCompatibilityMode _compatibilityMode;
        private readonly int _iterCount;
        private readonly RandomNumberGenerator _rng;

        /// <summary>
        ///     Creates a new instance of <see cref="PasswordHasher" />.
        /// </summary>
        /// <param name="optionsAccessor">The options for this instance.</param>
        public PasswordHasher(IOptions<PasswordHasherOptions> optionsAccessor = null)
        {
            PasswordHasherOptions options = optionsAccessor?.Value ?? new PasswordHasherOptions();

            _compatibilityMode = options.CompatibilityMode;
            switch (_compatibilityMode)
            {
                case PasswordHasherCompatibilityMode.IdentityV2:
                    // nothing else to do
                    break;

                case PasswordHasherCompatibilityMode.IdentityV3:
                    _iterCount = options.IterationCount;
                    if (_iterCount < 1)
                    {
                        throw new InvalidOperationException(Resource.InvalidPasswordHasherIterationCount);
                    }
                    break;

                default:
                    throw new InvalidOperationException(Resource.InvalidPasswordHasherCompatibilityMode);
            }

            _rng = options.Rng;
        }

        #region IPasswordHasher Members

        /// <summary>
        ///     Returns a hashed representation of the supplied <paramref name="password" /> for the specified user.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>A hashed representation of the supplied <paramref name="password" /> for the specified user.</returns>
        public virtual string HashPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            return Convert.ToBase64String(_compatibilityMode == PasswordHasherCompatibilityMode.IdentityV2 ? HashPasswordV2(password, _rng) : HashPasswordV3(password, _rng));
        }

        /// <summary>
        ///     Returns a <see cref="PasswordVerificationResult" /> indicating the result of a password hash comparison.
        /// </summary>
        /// <param name="hashedPassword">The hash value for a user's stored password.</param>
        /// <param name="providedPassword">The password supplied for comparison.</param>
        /// <returns>A <see cref="PasswordVerificationResult" /> indicating the result of a password hash comparison.</returns>
        /// <remarks>Implementations of this method should be time consistent.</remarks>
        public virtual PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (hashedPassword == null)
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }
            if (providedPassword == null)
            {
                throw new ArgumentNullException(nameof(providedPassword));
            }

            byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);

            // read the format marker from the hashed password
            if (decodedHashedPassword.Length == 0)
            {
                return PasswordVerificationResult.Failed;
            }
            switch (decodedHashedPassword[0])
            {
                case 0x00:
                    if (VerifyHashedPasswordV2(decodedHashedPassword, providedPassword))
                    {
                        // This is an old password hash format - the caller needs to rehash if we're not running in an older compat mode.
                        return _compatibilityMode == PasswordHasherCompatibilityMode.IdentityV3
                            ? PasswordVerificationResult.SuccessRehashNeeded
                            : PasswordVerificationResult.Success;
                    }
                    return PasswordVerificationResult.Failed;

                case 0x01:
                    int embeddedIterCount;
                    if (VerifyHashedPasswordV3(decodedHashedPassword, providedPassword, out embeddedIterCount))
                    {
                        // If this hasher was configured with a higher iteration count, change the entry now.
                        return embeddedIterCount < _iterCount
                            ? PasswordVerificationResult.SuccessRehashNeeded
                            : PasswordVerificationResult.Success;
                    }
                    return PasswordVerificationResult.Failed;

                default:
                    return PasswordVerificationResult.Failed; // unknown format marker
            }
        }

        #endregion

        // Compares two byte arrays for equality. The method is specifically written so that the loop is not optimized.
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(IReadOnlyList<byte> a, IReadOnlyList<byte> b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Count != b.Count)
            {
                return false;
            }
            bool areSame = true;
            for (int i = 0; i < a.Count; i++)
            {
                areSame &= a[i] == b[i];
            }
            return areSame;
        }

        private static byte[] HashPasswordV2(string password, RandomNumberGenerator rng)
        {
            const KeyDerivationPrf PBKDF2_PRF = KeyDerivationPrf.HMACSHA1; // default for Rfc2898DeriveBytes
            const int PBKDF2_ITER_COUNT = 1000; // default for Rfc2898DeriveBytes
            const int PBKDF2_SUBKEY_LENGTH = 256 / 8; // 256 bits
            const int SALT_SIZE = 128 / 8; // 128 bits

            // Produce a version 2 (see comment above) text hash.
            byte[] salt = new byte[SALT_SIZE];
            rng.GetBytes(salt);
            byte[] subkey = KeyDerivation.Pbkdf2(password, salt, PBKDF2_PRF, PBKDF2_ITER_COUNT, PBKDF2_SUBKEY_LENGTH);

            byte[] outputBytes = new byte[1 + SALT_SIZE + PBKDF2_SUBKEY_LENGTH];
            outputBytes[0] = 0x00; // format marker
            Buffer.BlockCopy(salt, 0, outputBytes, 1, SALT_SIZE);
            Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SALT_SIZE, PBKDF2_SUBKEY_LENGTH);
            return outputBytes;
        }

        private byte[] HashPasswordV3(string password, RandomNumberGenerator rng)
        {
            return HashPasswordV3(password, rng,
                KeyDerivationPrf.HMACSHA256,
                _iterCount,
                128 / 8,
                256 / 8);
        }

        private static byte[] HashPasswordV3(string password, RandomNumberGenerator rng, KeyDerivationPrf prf, int iterCount, int saltSize, int numBytesRequested)
        {
            // Produce a version 3 (see comment above) text hash.
            byte[] salt = new byte[saltSize];
            rng.GetBytes(salt);
            byte[] subkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);

            byte[] outputBytes = new byte[13 + salt.Length + subkey.Length];
            outputBytes[0] = 0x01; // format marker
            WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
            WriteNetworkByteOrder(outputBytes, 5, (uint)iterCount);
            WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);
            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);
            return outputBytes;
        }

        [SuppressMessage("ReSharper", "RedundantCast")]
        private static uint ReadNetworkByteOrder(IReadOnlyList<byte> buffer, int offset)
        {
            return ((uint)buffer[offset + 0] << 24)
                   | ((uint)buffer[offset + 1] << 16)
                   | ((uint)buffer[offset + 2] << 8)
                   | (uint)buffer[offset + 3];
        }

        private static bool VerifyHashedPasswordV2(byte[] hashedPassword, string password)
        {
            const KeyDerivationPrf PBKDF2_PRF = KeyDerivationPrf.HMACSHA1; // default for Rfc2898DeriveBytes
            const int PBKDF2_ITER_COUNT = 1000; // default for Rfc2898DeriveBytes
            const int PBKDF2_SUBKEY_LENGTH = 256 / 8; // 256 bits
            const int SALT_SIZE = 128 / 8; // 128 bits

            // We know ahead of time the exact length of a valid hashed password payload.
            if (hashedPassword.Length != 1 + SALT_SIZE + PBKDF2_SUBKEY_LENGTH)
            {
                return false; // bad size
            }

            byte[] salt = new byte[SALT_SIZE];
            Buffer.BlockCopy(hashedPassword, 1, salt, 0, salt.Length);

            byte[] expectedSubkey = new byte[PBKDF2_SUBKEY_LENGTH];
            Buffer.BlockCopy(hashedPassword, 1 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

            // Hash the incoming password and verify it
            byte[] actualSubkey = KeyDerivation.Pbkdf2(password, salt, PBKDF2_PRF, PBKDF2_ITER_COUNT, PBKDF2_SUBKEY_LENGTH);
            return ByteArraysEqual(actualSubkey, expectedSubkey);
        }

        private static bool VerifyHashedPasswordV3(byte[] hashedPassword, string password, out int iterCount)
        {
            iterCount = default(int);

            try
            {
                // Read header information
                KeyDerivationPrf prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPassword, 1);
                iterCount = (int)ReadNetworkByteOrder(hashedPassword, 5);
                int saltLength = (int)ReadNetworkByteOrder(hashedPassword, 9);

                // Read the salt: must be >= 128 bits
                if (saltLength < 128 / 8)
                {
                    return false;
                }
                byte[] salt = new byte[saltLength];
                Buffer.BlockCopy(hashedPassword, 13, salt, 0, salt.Length);

                // Read the subkey (the rest of the payload): must be >= 128 bits
                int subkeyLength = hashedPassword.Length - 13 - salt.Length;
                if (subkeyLength < 128 / 8)
                {
                    return false;
                }
                byte[] expectedSubkey = new byte[subkeyLength];
                Buffer.BlockCopy(hashedPassword, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

                // Hash the incoming password and verify it
                byte[] actualSubkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, subkeyLength);
                return ByteArraysEqual(actualSubkey, expectedSubkey);
            }
            catch
            {
                // This should never occur except in the case of a malformed payload, where
                // we might go off the end of the array. Regardless, a malformed payload
                // implies verification unsuccessfully.
                return false;
            }
        }

        private static void WriteNetworkByteOrder(IList<byte> buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }
    }
}