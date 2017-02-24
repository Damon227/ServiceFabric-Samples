// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Caching
// File             : JsonRedisCacheOptions.cs
// Created          : 2016-07-03  10:42 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.ServiceFabric.Caching
{
    /// <summary>
    ///     Configuration options for <see cref="JsonRedisCache" />.
    /// </summary>
    public class JsonRedisCacheOptions : IOptions<JsonRedisCacheOptions>
    {
        /// <summary>
        ///     The configuration used to connect to Redis.
        /// </summary>
        public string ConfigurationString { get; set; }

        /// <summary>
        ///     The database index.
        /// </summary>
        public int Database { get; set; }

        /// <summary>
        ///     The Redis instance name.
        /// </summary>
        public string InstanceName { get; set; }

        #region IOptions<JsonRedisCacheOptions> Members

        JsonRedisCacheOptions IOptions<JsonRedisCacheOptions>.Value
        {
            get { return this; }
        }

        #endregion
    }
}