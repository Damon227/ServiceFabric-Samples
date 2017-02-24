// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Caching
// File             : IDistributedListCache.cs
// Created          : 2016-11-26  14:32
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Credit.Kolibre.Foundation.ServiceFabric.Caching
{
    public interface IDistributedListCache : IDistributedCache
    {
        /// <summary>
        ///     从 list 结构中获取第一个元素
        /// </summary>
        /// <param name="key">The key.</param>
        string GetFirst(string key);

        /// <summary>
        ///     从 list 结构中获取第一个元素
        /// </summary>
        /// <param name="key">The key.</param>
        Task<string> GetFirstAsync(string key);

        /// <summary>
        /// 设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <param name="options">The options.</param>
        void SetList(string key, RedisValue[] values, DistributedCacheEntryOptions options);

        /// <summary>
        /// 设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <param name="options">The options.</param>
        Task SetListAsync(string key, RedisValue[] values, DistributedCacheEntryOptions options);

        /// <summary>
        /// 设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="list">The list.</param>
        /// <param name="options">The options.</param>
        void SetList(string key, List<string> list, DistributedCacheEntryOptions options);

        /// <summary>
        /// 设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="list">The list.</param>
        /// <param name="options">The options.</param>
        Task SetListAsync(string key, List<string> list, DistributedCacheEntryOptions options);

        /// <summary>
        /// 设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="array">The array.</param>
        /// <param name="options">The options.</param>
        void SetList(string key, string[] array, DistributedCacheEntryOptions options);

        /// <summary>
        /// 设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="array">The array.</param>
        /// <param name="options">The options.</param>
        Task SetListAsync(string key, string[] array, DistributedCacheEntryOptions options);

        /// <summary>
        /// 设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="list">The list.</param>
        /// <param name="options">The options.</param>
        void SetList(string key, List<int> list, DistributedCacheEntryOptions options);

        /// <summary>
        /// 设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="list">The list.</param>
        /// <param name="options">The options.</param>
        Task SetListAsync(string key, List<int> list, DistributedCacheEntryOptions options);

        /// <summary>
        /// 设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="array">The array.</param>
        /// <param name="options">The options.</param>
        void SetList(string key, int[] array, DistributedCacheEntryOptions options);

        /// <summary>
        /// 设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="array">The array.</param>
        /// <param name="options">The options.</param>
        Task SetListAsync(string key, int[] array, DistributedCacheEntryOptions options);
    }
}