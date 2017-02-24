// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.ServiceFabric.Caching
// File             : ListRedisCache.cs
// Created          : 2017-02-15  17:29
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credit.Kolibre.Foundation.ServiceFabric.Insights;
using Credit.Kolibre.Foundation.Static;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Credit.Kolibre.Foundation.ServiceFabric.Caching
{
    public class ListRedisCache : JsonRedisCache, IDistributedListCache
    {
        private static readonly JsonSerializerSettings s_jsonSerializerSettings = SETTING.REDIS_CACHE_JSON_SETTINGS;
        private static readonly RetryPolicy s_retryPolicy = new RetryPolicy<CacheTransientErrorDetectionStrategy>(new FixedInterval(3, 5.Seconds()));
        private readonly ListRedisCacheOptions _options;

        private readonly TelemetryClient _telemetryClient;
        private readonly ILogger<ListRedisCache> _logger;
        private readonly string _instance;
        private ConnectionMultiplexer _connection;
        private IDatabase _cache;

        public ListRedisCache(
            IOptions<ListRedisCacheOptions> optionsAccessor,
            IHttpTelemetryClientAccessor httpTelemetryClientAccessor,
            ILoggerFactory loggerFactory)
            : base(optionsAccessor, httpTelemetryClientAccessor, loggerFactory)
        {
            _options = optionsAccessor.Value;
            _telemetryClient = httpTelemetryClientAccessor.GetTelemetryClient();
            _logger = loggerFactory.CreateLogger<ListRedisCache>();

            // This allows partitioning a single backend cache for use with multiple apps/services.
            if (_options.InstanceName.IsNotNullOrEmpty())
            {
                _instance = _options.InstanceName;

                if (!_options.InstanceName.GetLast().Equals(":"))
                {
                    _instance += ":";
                }
            }
            else
            {
                _instance = string.Empty;
            }
        }

        #region IDistributedListCache Members

        /// <summary>
        ///     从 list 结构中获取第一个元素
        /// </summary>
        /// <param name="key">The key.</param>
        public string GetFirst(string key)
        {
            _logger.LogListRedisGetBegin(_instance + key, null);

            Connect();

            RedisValue result = _telemetryClient.TrackDependency(DepandencyType.Redis, _connection.Configuration, "Get" + _instance + key,
                () => s_retryPolicy.ExecuteAction(() => _cache.ListLeftPop(_instance + key)));

            _logger.LogListRedisGetEnd(_instance + key, null);

            if (result.HasValue && !result.IsNullOrEmpty)
            {
                return result;
            }

            return null;
        }

        /// <summary>
        ///     从 list 结构中获取第一个元素
        /// </summary>
        /// <param name="key">The key.</param>
        public async Task<string> GetFirstAsync(string key)
        {
            _logger.LogListRedisGetBegin(_instance + key, null);

            Connect();

            RedisValue result = await _telemetryClient.TrackDependency(DepandencyType.Redis, _connection.Configuration, "Get" + _instance + key,
                async () => await s_retryPolicy.ExecuteAsync(async () => await _cache.ListLeftPopAsync(_instance + key)));

            _logger.LogListRedisGetEnd(_instance + key, null);

            if (result.HasValue && !result.IsNullOrEmpty)
            {
                return result;
            }

            return null;
        }

        /// <summary>
        ///     设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <param name="options">The options.</param>
        public void SetList(string key, RedisValue[] values, DistributedCacheEntryOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Connect();

            _logger.LogListRedisSetBegin(_instance + key, null);

            _telemetryClient.TrackDependency(DepandencyType.Redis, GetType().FullName, "SetListValue",
                () => _cache.ListRightPush(_instance + key, values));

            RefreshExpiry(_instance + key, options.AbsoluteExpiration, options.SlidingExpiration);

            _logger.LogListRedisSetEnd(_instance + key, null);
        }

        /// <summary>
        ///     设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <param name="options">The options.</param>
        public async Task SetListAsync(string key, RedisValue[] values, DistributedCacheEntryOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            await ConnectAsync();

            _logger.LogListRedisSetBegin(_instance + key, null);

            await _telemetryClient.TrackDependency(DepandencyType.Redis, GetType().FullName, "SetListValue",
                async () => await _cache.ListRightPushAsync(_instance + key, values));

            RefreshExpiry(_instance + key, options.AbsoluteExpiration, options.SlidingExpiration);

            _logger.LogListRedisSetEnd(_instance + key, null);
        }

        /// <summary>
        ///     设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="list">The list.</param>
        /// <param name="options">The options.</param>
        public void SetList(string key, List<string> list, DistributedCacheEntryOptions options)
        {
            RedisValue[] redisValues = list.Select(t => (RedisValue)t).ToArray();

            SetList(key, redisValues, options);
        }

        /// <summary>
        ///     设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="list">The list.</param>
        /// <param name="options">The options.</param>
        public async Task SetListAsync(string key, List<string> list, DistributedCacheEntryOptions options)
        {
            RedisValue[] redisValues = list.Select(t => (RedisValue)t).ToArray();

            await SetListAsync(key, redisValues, options);
        }

        /// <summary>
        ///     设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="array">The array.</param>
        /// <param name="options">The options.</param>
        public void SetList(string key, string[] array, DistributedCacheEntryOptions options)
        {
            RedisValue[] redisValues = array.Select(t => (RedisValue)t).ToArray();

            SetList(key, redisValues, options);
        }

        /// <summary>
        ///     设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="array">The array.</param>
        /// <param name="options">The options.</param>
        public async Task SetListAsync(string key, string[] array, DistributedCacheEntryOptions options)
        {
            RedisValue[] redisValues = array.Select(t => (RedisValue)t).ToArray();

            await SetListAsync(key, redisValues, options);
        }

        /// <summary>
        ///     设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="list">The list.</param>
        /// <param name="options">The options.</param>
        public void SetList(string key, List<int> list, DistributedCacheEntryOptions options)
        {
            RedisValue[] redisValues = list.Select(t => (RedisValue)t).ToArray();

            SetList(key, redisValues, options);
        }

        /// <summary>
        ///     设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="list">The list.</param>
        /// <param name="options">The options.</param>
        public async Task SetListAsync(string key, List<int> list, DistributedCacheEntryOptions options)
        {
            RedisValue[] redisValues = list.Select(t => (RedisValue)t).ToArray();

            await SetListAsync(key, redisValues, options);
        }


        /// <summary>
        ///     设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="array">The array.</param>
        /// <param name="options">The options.</param>
        public void SetList(string key, int[] array, DistributedCacheEntryOptions options)
        {
            RedisValue[] redisValues = array.Select(t => (RedisValue)t).ToArray();

            SetList(key, redisValues, options);
        }

        /// <summary>
        ///     设置 list
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="array">The array.</param>
        /// <param name="options">The options.</param>
        public async Task SetListAsync(string key, int[] array, DistributedCacheEntryOptions options)
        {
            RedisValue[] redisValues = array.Select(t => (RedisValue)t).ToArray();

            await SetListAsync(key, redisValues, options);
        }

        #endregion

        private void Connect()
        {
            if (_connection == null || !_connection.IsConnected)
            {
                _connection = ConnectionMultiplexer.Connect(_options.ConfigurationString);
                _cache = _connection.GetDatabase(_options.Database);
            }
        }

        private async Task ConnectAsync()
        {
            if (_connection == null || !_connection.IsConnected)
            {
                _connection = await ConnectionMultiplexer.ConnectAsync(_options.ConfigurationString);
                _cache = _connection.GetDatabase(_options.Database);
            }
        }

        private static DateTimeOffset? GetAbsoluteExpiration(DistributedCacheEntryOptions options)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            if (options.AbsoluteExpiration.HasValue && options.AbsoluteExpiration <= now)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(DistributedCacheEntryOptions.AbsoluteExpiration),
                    options.AbsoluteExpiration.Value,
                    "The absolute expiration value must be in the future.");
            }

            if (options.AbsoluteExpirationRelativeToNow.HasValue)
            {
                return GetMaxDateTimeOffset(now + options.AbsoluteExpirationRelativeToNow, options.AbsoluteExpiration);
            }

            return options.AbsoluteExpiration;
        }

        private static DateTimeOffset? GetMaxDateTimeOffset(DateTimeOffset? value1, DateTimeOffset? value2)
        {
            if (value1.HasValue && value2.HasValue)
            {
                return value1.Value >= value2.Value ? value1.Value : value2.Value;
            }

            if (value1.HasValue)
            {
                return value1;
            }
            return value2;
        }

        private bool RefreshExpiry(string key, DateTimeOffset? absoluteExpiration, TimeSpan? slidingExpiration)
        {
            TimeSpan? expiryTimeSpan = GetExpiryTimeSpan(absoluteExpiration, slidingExpiration);

            _logger.LogListRedisRefreshBegin(_instance + key);

            bool result = _telemetryClient.TrackDependency(DepandencyType.Redis, _connection.Configuration, "RefreshExpiry" + _instance + key,
                () => s_retryPolicy.ExecuteAction(() => _cache.KeyExpire(_instance + key, expiryTimeSpan.GetValueOrDefault(TimeSpan.MaxValue))));

            _logger.LogListRedisRefreshEnd(_instance + key);

            return result;
        }

        private static TimeSpan? GetExpiryTimeSpan(DistributedCacheEntryOptions options)
        {
            DateTimeOffset? absoluteExpiration = GetAbsoluteExpiration(options);

            return GetExpiryTimeSpan(absoluteExpiration, options.SlidingExpiration);
        }

        private static TimeSpan? GetExpiryTimeSpan(DateTimeOffset? absoluteExpiration, TimeSpan? slidingExpiration)
        {
            TimeSpan? expiryTimeSpan = absoluteExpiration?.Subtract(DateTimeOffset.UtcNow);
            if (slidingExpiration.HasValue)
            {
                return GetMinTimeSpan(expiryTimeSpan, slidingExpiration);
            }

            return expiryTimeSpan;
        }

        private static TimeSpan? GetMinTimeSpan(TimeSpan? value1, TimeSpan? value2)
        {
            if (value1.HasValue && value2.HasValue)
            {
                return value1.Value <= value2.Value ? value1.Value : value2.Value;
            }

            if (value1.HasValue)
            {
                return value1;
            }
            return value2;
        }
    }
}