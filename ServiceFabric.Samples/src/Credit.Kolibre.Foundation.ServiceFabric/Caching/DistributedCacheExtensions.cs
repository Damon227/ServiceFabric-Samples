// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : DistributedCacheExtensions.cs
// Created          : 2016-09-09  17:44
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Credit.Kolibre.Foundation.Static;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Credit.Kolibre.Foundation.ServiceFabric.Caching
{
    public static class DistributedCacheExtensions
    {
        private static readonly JsonSerializerSettings s_jsonSerializerSettings = SETTING.DATA_JSON_SETTINGS;

        public static T GetObject<T>(this IDistributedCache cache, string key) where T : class
        {
            return cache.Get(key)?.ToUTF8String().FromJson<T>(s_jsonSerializerSettings);
        }

        public static async Task<T> GetObjectAsync<T>(this IDistributedCache cache, string key) where T : class
        {
            return (await cache.GetAsync(key))?.ToUTF8String().FromJson<T>(s_jsonSerializerSettings);
        }

        /*public static string GetString(this IDistributedCache cache, string key)
        {
            return cache.Get(key)?.ToUTF8String();
        }

        //与 微软内置的 GetStringAsync 方法重名
        public static async Task<string> GetStringAsync(this IDistributedCache cache, string key)
        {
            return (await cache.GetAsync(key))?.ToUTF8String();
        }*/

        public static void SetObject<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options) where T : class
        {
            cache.Set(key, value.ToJson(s_jsonSerializerSettings).GetBytesOfUTF8(), options);
        }

        public static Task SetObjectAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options) where T : class
        {
            return cache.SetAsync(key, value.ToJson(s_jsonSerializerSettings).GetBytesOfUTF8(), options);
        }

        /*public static void SetString(this IDistributedCache cache, string key, string value, DistributedCacheEntryOptions options)
        {
            cache.Set(key, value.GetBytesOfUTF8(), options);
        }

        public static Task SetStringAsync(this IDistributedCache cache, string key, string value, DistributedCacheEntryOptions options)
        {
            return cache.SetAsync(key, value.GetBytesOfUTF8(), options);
        }*/
    }
}