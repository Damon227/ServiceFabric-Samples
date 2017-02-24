// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Identity
// File             : Rfc6238AuthenticationService.cs
// Created          : 2016-07-02  4:28 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Credit.Kolibre.Foundation.Sys;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    public static class Rfc6238AuthenticationService
    {
        private static readonly DateTime s_unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly TimeSpan s_timestep = TimeSpan.FromMinutes(10);
        private static readonly Encoding s_encoding = new UTF8Encoding(false, true);

        public static int GenerateCode(byte[] securityToken, string modifier = null)
        {
            if (securityToken == null)
            {
                throw new ArgumentNullException(nameof(securityToken));
            }

            // Allow a variance of no greater than 5 minutes in either direction
            ulong currentTimeStep = GetCurrentTimeStepNumber();
            using (HMACSHA1 hashAlgorithm = new HMACSHA1(securityToken))
            {
                return ComputeTotp(hashAlgorithm, currentTimeStep, modifier);
            }
        }

        public static bool ValidateCode(byte[] securityToken, int code, string modifier = null)
        {
            if (securityToken == null)
            {
                throw new ArgumentNullException(nameof(securityToken));
            }

            // Allow a variance of no greater than 5 minutes in either direction
            ulong currentTimeStep = GetCurrentTimeStepNumber();
            using (HMACSHA1 hashAlgorithm = new HMACSHA1(securityToken))
            {
                int computedTotp = ComputeTotp(hashAlgorithm, currentTimeStep, modifier);
                if (computedTotp == code)
                {
                    return true;
                }
            }

            // No match
            return false;
        }

        private static byte[] ApplyModifier(byte[] input, string modifier)
        {
            if (modifier.IsNullOrEmpty())
            {
                return input;
            }

            byte[] modifierBytes = s_encoding.GetBytes(modifier);
            byte[] combined = new byte[checked(input.Length + modifierBytes.Length)];
            Buffer.BlockCopy(input, 0, combined, 0, input.Length);
            Buffer.BlockCopy(modifierBytes, 0, combined, input.Length, modifierBytes.Length);
            return combined;
        }

        private static int ComputeTotp(HashAlgorithm hashAlgorithm, ulong timestepNumber, string modifier)
        {
            // # of 0's = length of pin
            const int MOD = 1000000;

            // See https://tools.ietf.org/html/rfc4226
            // We can add an optional modifier
            byte[] timestepAsBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long)timestepNumber));
            byte[] hash = hashAlgorithm.ComputeHash(ApplyModifier(timestepAsBytes, modifier));

            // Generate DT string
            int offset = hash[hash.Length - 1] & 0xf;
            Debug.Assert(offset + 4 < hash.Length);
            int binaryCode = (hash[offset] & 0x7f) << 24
                             | (hash[offset + 1] & 0xff) << 16
                             | (hash[offset + 2] & 0xff) << 8
                             | (hash[offset + 3] & 0xff);

            return binaryCode % MOD;
        }

        // More info: https://tools.ietf.org/html/rfc6238#section-4
        private static ulong GetCurrentTimeStepNumber()
        {
            TimeSpan delta = DateTime.UtcNow - s_unixEpoch;
            return (ulong)(delta.Ticks / s_timestep.Ticks);
        }
    }
}