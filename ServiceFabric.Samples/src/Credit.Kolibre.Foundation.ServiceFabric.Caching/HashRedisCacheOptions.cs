// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Caching
// File             : HashRedisCacheOptions.cs
// Created          : 2016-08-01  20:43
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.ServiceFabric.Caching
{
    public class HashRedisCacheOptions : JsonRedisCacheOptions, IOptions<HashRedisCacheOptions>
    {
        /// <summary>
        ///     相对过期时间，以小时为单位
        /// </summary>
        public int SlidingExpireHours { get; set; }

        #region IOptions<HashRedisCacheOptions> Members

        /// <summary>
        ///     The configured TOptions instance.
        /// </summary>
        HashRedisCacheOptions IOptions<HashRedisCacheOptions>.Value
        {
            get { return this; }
        }

        #endregion
    }
}