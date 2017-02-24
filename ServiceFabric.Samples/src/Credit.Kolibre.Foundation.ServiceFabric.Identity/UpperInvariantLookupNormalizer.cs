// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Identity
// File             : UpperInvariantLookupNormalizer.cs
// Created          : 2016-07-01  8:57 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Implements <see cref="ILookupNormalizer" /> by converting keys to their upper cased invariant culture representation.
    /// </summary>
    public class UpperInvariantLookupNormalizer : ILookupNormalizer
    {
        #region ILookupNormalizer Members

        /// <summary>
        ///     Returns a normalized representation of the specified <paramref name="key" />
        ///     by converting keys to their upper cased invariant culture representation.
        /// </summary>
        /// <param name="key">The key to normalize.</param>
        /// <returns>A normalized representation of the specified <paramref name="key" />.</returns>
        public virtual string Normalize(string key)
        {
            return key?.Normalize().ToUpperInvariant();
        }

        #endregion
    }
}