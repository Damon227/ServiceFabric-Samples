// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Identity
// File             : ILookupNormalizer.cs
// Created          : 2016-07-01  5:05 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Provides an abstraction for normalizing keys for lookup purposes.
    /// </summary>
    public interface ILookupNormalizer
    {
        /// <summary>
        ///     Returns a normalized representation of the specified <paramref name="key" />.
        /// </summary>
        /// <param name="key">The key to normalize.</param>
        /// <returns>A normalized representation of the specified <paramref name="key" />.</returns>
        string Normalize(string key);
    }
}