// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Caching
// File             : ListRedisCacheOptions.cs
// Created          : 2016-11-26  14:38
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.ServiceFabric.Caching
{
    public class ListRedisCacheOptions : JsonRedisCacheOptions, IOptions<ListRedisCacheOptions>
    {
        /// <summary>
        ///     相对过期时间，以小时为单位
        /// </summary>
        public int SlidingExpireHours { get; set; }

        #region IOptions<ListRedisCacheOptions> Members

        /// <summary>
        ///     The configured TOptions instance.
        /// </summary>
        ListRedisCacheOptions IOptions<ListRedisCacheOptions>.Value
        {
            get { return this; }
        }

        #endregion
    }
}