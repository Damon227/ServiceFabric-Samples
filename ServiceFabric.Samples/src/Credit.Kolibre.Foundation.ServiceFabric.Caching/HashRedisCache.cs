// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.ServiceFabric.Caching
// File             : HashRedisCache.cs
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
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace Credit.Kolibre.Foundation.ServiceFabric.Caching
{
    public class HashRedisCache : JsonRedisCache, IDistributedHashCache
    {
        private static readonly JsonSerializerSettings s_jsonSerializerSettings = SETTING.REDIS_CACHE_JSON_SETTINGS;
        private static readonly RetryPolicy s_retryPolicy = new RetryPolicy<CacheTransientErrorDetectionStrategy>(new FixedInterval(3, 5.Seconds()));
        private readonly HashRedisCacheOptions _options;

        private readonly TelemetryClient _telemetryClient;
        private readonly ILogger<HashRedisCache> _logger;
        private readonly string _instance;
        private ConnectionMultiplexer _connection;
        private IDatabase _cache;

        public HashRedisCache(
            IOptions<HashRedisCacheOptions> optionsAccessor,
            IHttpTelemetryClientAccessor httpTelemetryClientAccessor,
            ILoggerFactory loggerFactory)
            : base(optionsAccessor, httpTelemetryClientAccessor, loggerFactory)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            _options = optionsAccessor.Value;
            _telemetryClient = httpTelemetryClientAccessor.GetTelemetryClient();
            _logger = loggerFactory.CreateLogger<HashRedisCache>();

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

        #region IDistributedHashCache Members

        public IDictionary<string, object> GetHash(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            IDictionary<string, object> result = GetAndRefresh(key, true);
            return result;
        }

        public async Task<IDictionary<string, object>> GetHashAsync(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            IDictionary<string, object> result = await GetAndRefreshAsync(key, true);
            return result;
        }

        public void SetHash<T>(string key, IDictionary<string, T> value, DistributedCacheEntryOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Connect();

            HashEntry[] hashEntries = BuildHashEntries(value, options);

            _logger.LogHashRedisSetBegin(_instance + key, null);

            _telemetryClient.TrackDependency(DepandencyType.Redis, GetType().FullName, "SetHash",
                () => _cache.HashSet(_instance + key, hashEntries));

            _cache.KeyExpire(_instance + key, new TimeSpan(0, _options.SlidingExpireHours, 0, 0));

            _logger.LogHashRedisSetEnd(_instance + key, null);
        }

        public async Task SetHashAsync<T>(string key, IDictionary<string, T> value, DistributedCacheEntryOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Connect();

            HashEntry[] hashEntries = BuildHashEntries(value, options);

            _logger.LogHashRedisSetBegin(_instance + key, null);

            await _telemetryClient.TrackDependency(DepandencyType.Redis, GetType().FullName, "SetHash",
                async () => await _cache.HashSetAsync(_instance + key, hashEntries));

            RefreshExpiry(_instance + key, null, _options.SlidingExpireHours.Hours());

            _logger.LogHashRedisSetEnd(_instance + key, null);
        }

        public void SetHashValue<T>(string key, string hashKey, T value, DistributedCacheEntryOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (hashKey == null)
            {
                throw new ArgumentNullException(nameof(hashKey));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Connect();

            HashRedisCacheModel cache = new HashRedisCacheModel
            {
                AbsoluteExpiration = GetAbsoluteExpiration(options),
                SlidingExpiration = options.SlidingExpiration,
                Payload = value.ToJson(s_jsonSerializerSettings)
            };

            _logger.LogHashRedisSetBegin(_instance + key, hashKey);

            _telemetryClient.TrackDependency(DepandencyType.Redis, GetType().FullName, "SetHashValue",
                () => _cache.HashSet(_instance + key, hashKey, cache.ToJson(s_jsonSerializerSettings)));

            RefreshExpiry(_instance + key, null, _options.SlidingExpireHours.Hours());

            _logger.LogHashRedisSetEnd(_instance + key, hashKey);
        }

        public async Task SetHashValueAsync<T>(string key, string hashKey, T value, DistributedCacheEntryOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (hashKey == null)
            {
                throw new ArgumentNullException(nameof(hashKey));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Connect();

            HashRedisCacheModel cache = new HashRedisCacheModel
            {
                AbsoluteExpiration = GetAbsoluteExpiration(options),
                SlidingExpiration = options.SlidingExpiration,
                Payload = value.ToJson(s_jsonSerializerSettings)
            };

            _logger.LogHashRedisSetBegin(_instance + key, hashKey);

            await _telemetryClient.TrackDependency(DepandencyType.Redis, GetType().FullName, "SetHashValue",
                async () => await _cache.HashSetAsync(_instance + key, hashKey, cache.ToJson(s_jsonSerializerSettings)));

            RefreshExpiry(_instance + key, null, _options.SlidingExpireHours.Hours());

            _logger.LogHashRedisSetEnd(_instance + key, hashKey);
        }

        public void SetHashValue<T>(string key, KeyValuePair<string, T> value, DistributedCacheEntryOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (default(KeyValuePair<string, T>).Equals(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            Connect();

            HashRedisCacheModel cache = new HashRedisCacheModel
            {
                AbsoluteExpiration = GetAbsoluteExpiration(options),
                SlidingExpiration = options.SlidingExpiration,
                Payload = value.ToJson(s_jsonSerializerSettings)
            };

            _logger.LogHashRedisSetBegin(_instance + key, value.Key);

            _telemetryClient.TrackDependency(DepandencyType.Redis, GetType().FullName, "SetHashValue",
                () => _cache.HashSet(_instance + key, value.Key, cache.ToJson(s_jsonSerializerSettings)));

            RefreshExpiry(_instance + key, null, _options.SlidingExpireHours.Hours());

            _logger.LogHashRedisSetEnd(_instance + key, value.Key);
        }

        public async Task SetHashValueAsync<T>(string key, KeyValuePair<string, T> value, DistributedCacheEntryOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (default(KeyValuePair<string, T>).Equals(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            Connect();

            HashRedisCacheModel cache = new HashRedisCacheModel
            {
                AbsoluteExpiration = GetAbsoluteExpiration(options),
                SlidingExpiration = options.SlidingExpiration,
                Payload = value.ToJson(s_jsonSerializerSettings)
            };

            _logger.LogHashRedisSetBegin(_instance + key, value.Key);

            await _telemetryClient.TrackDependency(DepandencyType.Redis, GetType().FullName, "SetHashValue",
                async () => await _cache.HashSetAsync(_instance + key, value.Key, cache.ToJson(s_jsonSerializerSettings)));

            RefreshExpiry(_instance + key, null, _options.SlidingExpireHours.Hours());

            _logger.LogHashRedisSetEnd(_instance + key, value.Key);
        }

        public void RemoveHashValue(string key, string hashKey)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (hashKey == null)
            {
                throw new ArgumentNullException(nameof(hashKey));
            }

            Connect();

            _logger.LogHashRedisRemoveBegin(_instance + key, hashKey);

            _telemetryClient.TrackDependency(DepandencyType.Redis, _connection.Configuration, "RemoveHashValue" + _instance + key,
                () => s_retryPolicy.ExecuteAction(() => _cache.HashDelete(_instance + key, hashKey)));

            _logger.LogHashRedisRemoveEnd(_instance + key, hashKey);
        }

        public async Task RemoveHashValueAsync(string key, string hashKey)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (hashKey == null)
            {
                throw new ArgumentNullException(nameof(hashKey));
            }

            Connect();

            _logger.LogHashRedisRemoveBegin(_instance + key, hashKey);

            await _telemetryClient.TrackDependency(DepandencyType.Redis, _connection.Configuration, "RemoveHashValue" + _instance + key,
                async () => await s_retryPolicy.ExecuteAsync(async () => await _cache.HashDeleteAsync(_instance + key, hashKey)));

            _logger.LogHashRedisRemoveEnd(_instance + key, hashKey);
        }

        public T GetHashValue<T>(string key, string hashKey)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (hashKey == null)
            {
                throw new ArgumentNullException(nameof(hashKey));
            }

            T result = GetHashEntry<T>(key, hashKey);
            return result;
        }

        public async Task<T> GetHashValueAsync<T>(string key, string hashKey)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (hashKey == null)
            {
                throw new ArgumentNullException(nameof(hashKey));
            }

            T result = await GetHashEntryAsync<T>(key, hashKey);
            return result;
        }

        #endregion

        private static HashEntry[] BuildHashEntries<T>(IDictionary<string, T> value, DistributedCacheEntryOptions options)
        {
            HashEntry[] hashEntries = { };
            int count = value.Count;
            List<string> keys = new List<string>(value.Keys);

            for (int i = 0; i < count; i++)
            {
                HashRedisCacheModel cache = new HashRedisCacheModel
                {
                    AbsoluteExpiration = GetAbsoluteExpiration(options),
                    SlidingExpiration = options.SlidingExpiration,
                    Payload = value[keys[i]].ToJson(s_jsonSerializerSettings)
                };
                hashEntries[i] = new HashEntry(keys[i], cache.ToJson(s_jsonSerializerSettings));
            }
            return hashEntries;
        }

        private IDictionary<string, object> GetAndRefresh(string key, bool refresh = false)
        {
            Connect();

            HashEntry[] result = GetAllHashEntry(key);

            if (result?.Length > 0)
            {
                Dictionary<string, object> cache = result.ToDictionary<HashEntry, string, object>(
                    hashEntry => hashEntry.Name, hashEntry => hashEntry.Value);

                if (cache.Count > 0)
                {
                    if (refresh)
                    {
                        RefreshExpiry(key, null, _options.SlidingExpireHours.Hours());
                    }

                    return cache;
                }
            }

            return null;
        }

        private async Task<IDictionary<string, object>> GetAndRefreshAsync(string key, bool refresh = false)
        {
            Connect();

            HashEntry[] result = await GetAllHashEntryAsync(key);

            if (result?.Length > 0)
            {
                Dictionary<string, object> cache = result.ToDictionary<HashEntry, string, object>(
                    hashEntry => hashEntry.Name, hashEntry => hashEntry.Value);

                if (cache.Count > 0)
                {
                    if (refresh)
                    {
                        RefreshExpiry(key, null, _options.SlidingExpireHours.Hours());
                    }

                    return cache;
                }
            }

            return null;
        }

        private HashEntry[] GetAllHashEntry(string key)
        {
            _logger.LogHashRedisGetBegin(_instance + key, null);

            HashEntry[] result = _telemetryClient.TrackDependency(DepandencyType.Redis, _connection.Configuration, "HashGetAll" + _instance + key,
                () => s_retryPolicy.ExecuteAction(() => _cache.HashGetAll(_instance + key)));

            _logger.LogHashRedisGetEnd(_instance + key, null);

            if (result?.Length > 0)
            {
                return result;
            }

            return null;
        }

        private T GetHashEntry<T>(string key, string hashKey)
        {
            _logger.LogHashRedisGetBegin(_instance + key, null);

            RedisValue result = _telemetryClient.TrackDependency(DepandencyType.Redis, _connection.Configuration, "HashGet" + _instance + key + hashKey,
                () => s_retryPolicy.ExecuteAction(() => _cache.HashGet(_instance + key, hashKey)));

            _logger.LogHashRedisGetEnd(_instance + key, null);

            if (result.HasValue && !result.IsNullOrEmpty)
            {
                return result.ToString().FromJson<T>(s_jsonSerializerSettings);
            }

            return default(T);
        }

        private async Task<HashEntry[]> GetAllHashEntryAsync(string key)
        {
            _logger.LogHashRedisGetBegin(_instance + key);

            HashEntry[] result = await _telemetryClient.TrackDependency(DepandencyType.Redis, _connection.Configuration, "HashGet" + _instance + key,
                () => s_retryPolicy.ExecuteAction(async () => await _cache.HashGetAllAsync(_instance + key)));

            _logger.LogHashRedisGetEnd(_instance + key);

            if (result?.Length > 0)
            {
                return result;
            }

            return null;
        }

        private async Task<T> GetHashEntryAsync<T>(string key, string hashKey)
        {
            Connect();

            _logger.LogHashRedisGetBegin(_instance + key);

            RedisValue result = await _telemetryClient.TrackDependency(DepandencyType.Redis, _connection.Configuration, "HashGet" + _instance + key + hashKey,
                () => s_retryPolicy.ExecuteAction(async () => await _cache.HashGetAsync(_instance + key, hashKey)));

            _logger.LogHashRedisGetEnd(_instance + key);

            if (result.HasValue && !result.IsNullOrEmpty)
            {
                JObject obj = JObject.Parse(result.ToString());
                return obj["Payload"].ToString().FromJson<T>(s_jsonSerializerSettings);
            }

            return default(T);
        }

        private bool RefreshExpiry(string key, DateTimeOffset? absoluteExpiration, TimeSpan? slidingExpiration)
        {
            TimeSpan? expiryTimeSpan = GetExpiryTimeSpan(absoluteExpiration, slidingExpiration);

            _logger.LogHashRedisRefreshBegin(_instance + key);

            bool result = _telemetryClient.TrackDependency(DepandencyType.Redis, _connection.Configuration, "RefreshExpiry" + _instance + key,
                () => s_retryPolicy.ExecuteAction(() => _cache.KeyExpire(_instance + key, expiryTimeSpan.GetValueOrDefault(TimeSpan.MaxValue))));

            _logger.LogHashRedisRefreshEnd(_instance + key);

            return result;
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

        private void Connect()
        {
            if (_connection == null || !_connection.IsConnected)
            {
                _connection = ConnectionMultiplexer.Connect(_options.ConfigurationString);
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
    }
}