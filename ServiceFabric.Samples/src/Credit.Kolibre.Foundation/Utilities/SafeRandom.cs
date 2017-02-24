// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : SafeRandom.cs
// Created          : 2016-08-31  11:12 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Credit.Kolibre.Foundation.Utilities
{
    /// <summary>
    ///     Thread-safe random number generator.
    ///     Has same API as System.Random but is thread safe, similar to the implementation by Steven Toub: http://blogs.msdn.com/b/pfxteam/archive/2014/10/20/9434171.aspx
    /// </summary>
    public class SafeRandom
    {
        private static readonly RandomNumberGenerator s_globalCryptoProvider = RandomNumberGenerator.Create();

        [ThreadStatic]
        private static Random s_random;

        public int Next()
        {
            return GetRandom().Next();
        }

        public int Next(int maxValue)
        {
            return GetRandom().Next(maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            return GetRandom().Next(minValue, maxValue);
        }

        public void NextBytes(byte[] buffer)
        {
            GetRandom().NextBytes(buffer);
        }

        public double NextDouble()
        {
            return GetRandom().NextDouble();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Random GetRandom()
        {
            if (s_random == null)
            {
                byte[] buffer = new byte[4];
                s_globalCryptoProvider.GetBytes(buffer);
                s_random = new Random(BitConverter.ToInt32(buffer, 0));
            }

            return s_random;
        }
    }
}