// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.ServiceFabric.Caching
// File             : RedisCache.cs
// Created          : 2017-02-15  17:29
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Credit.Kolibre.Foundation.ServiceFabric.Insights;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.ServiceFabric.Caching
{
    public class RedisCache : HashRedisCache
    {
        public RedisCache(
            IOptions<HashRedisCacheOptions> optionsAccessor,
            IHttpTelemetryClientAccessor httpTelemetryClientAccessor,
            ILoggerFactory loggerFactory)
            : base(optionsAccessor, httpTelemetryClientAccessor, loggerFactory)
        {
        }
    }
}