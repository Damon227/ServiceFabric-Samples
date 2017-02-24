// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Caching
// File             : IDistributedHashCache.cs
// Created          : 2016-11-26  13:07
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Credit.Kolibre.Foundation.ServiceFabric.Caching
{
    public interface IDistributedHashCache : IDistributedCache
    {
        IDictionary<string, object> GetHash(string key);

        Task<IDictionary<string, object>> GetHashAsync(string key);

        T GetHashValue<T>(string key, string hashKey);

        Task<T> GetHashValueAsync<T>(string key, string hashKey);

        void SetHash<T>(string key, IDictionary<string, T> value, DistributedCacheEntryOptions options);

        Task SetHashAsync<T>(string key, IDictionary<string, T> value, DistributedCacheEntryOptions options);

        void SetHashValue<T>(string key, string hashKey, T value, DistributedCacheEntryOptions options);

        Task SetHashValueAsync<T>(string key, string hashKey, T value, DistributedCacheEntryOptions options);

        void SetHashValue<T>(string key, KeyValuePair<string, T> value, DistributedCacheEntryOptions options);

        Task SetHashValueAsync<T>(string key, KeyValuePair<string, T> value, DistributedCacheEntryOptions options);

        void RemoveHashValue(string key, string hashKey);

        Task RemoveHashValueAsync(string key, string hashKey);
    }
}