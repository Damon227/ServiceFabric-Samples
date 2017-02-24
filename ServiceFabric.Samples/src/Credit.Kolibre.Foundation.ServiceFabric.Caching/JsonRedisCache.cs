// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.ServiceFabric.Caching
// File             : JsonRedisCache.cs
// Created          : 2017-02-15  17:29
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
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
    public class JsonRedisCache : IDistributedCache, IDisposable
    {
        private static readonly JsonSerializerSettings s_jsonSerializerSettings = SETTING.REDIS_CACHE_JSON_SETTINGS;

        private static readonly RetryPolicy s_retryPolicy = new RetryPolicy<CacheTransientErrorDetectionStrategy>(new FixedInterval(3, 5.Seconds()));
        private readonly JsonRedisCacheOptions _options;

        private readonly TelemetryClient _telemetryClient;
        private readonly ILogger<JsonRedisCache> _logger;
        private readonly string _instance;
        private ConnectionMultiplexer _connection;
        private IDatabase _cache;

        public JsonRedisCache(IOptions<JsonRedisCacheOptions> optionsAccessor,
            IHttpTelemetryClientAccessor httpTelemetryClientAccessor,
            ILoggerFactory loggerFactory)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            _options = optionsAccessor.Value;
            _telemetryClient = httpTelemetryClientAccessor.GetTelemetryClient();
            _logger = loggerFactory.CreateLogger<JsonRedisCache>();

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

        #region IDisposable Members

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _connection?.Close();
        }

        #endregion

        #region IDistributedCache Members

        public byte[] Get(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            string result = GetAndRefresh(key, true);
            return result?.GetBytesOfUTF8();
        }

        public async Task<byte[]> GetAsync(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            string result = await GetAndRefreshAsync(key, true);
            return result?.GetBytesOfUTF8();
        }

        public void Refresh(string key)
        {
            GetAndRefresh(key, true);
        }

        public Task RefreshAsync(string key)
        {
            return GetAndRefreshAsync(key, true);
        }

        public void Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Connect();

            _logger.LogJsonRedisRemoveBegin(_instance + key);

            _telemetryClient.TrackDependency(DepandencyType.Redis, _connection.Configuration, "Remove" + _instance + key,
                () => s_retryPolicy.ExecuteAction(() => _cache.KeyDelete(_instance + key)));

            _logger.LogJsonRedisRemoveEnd(_instance + key);
        }

        public async Task RemoveAsync(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            await ConnectAsync();

            _logger.LogJsonRedisRemoveBegin(_instance + key);

            await _telemetryClient.TrackDependency(DepandencyType.Redis, _connection.Configuration, "Remove" + _instance + key,
                async () => await s_retryPolicy.ExecuteAsync(async () => await _cache.KeyDeleteAsync(_instance + key)));

            _logger.LogJsonRedisRemoveEnd(_instance + key);
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
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

            TimeSpan? expiryTimeSpan = GetExpiryTimeSpan(options);

            JsonRedisCacheModel cache = new JsonRedisCacheModel
            {
                AbsoluteExpiration = GetAbsoluteExpiration(options),
                SlidingExpiration = options.SlidingExpiration,
                Payload = value.ToUTF8String()
            };

            _logger.LogJsonRedisSetBegin(_instance + key);

            _telemetryClient.TrackDependency(DepandencyType.Redis, GetType().FullName, "Set",
                () => _cache.StringSet(_instance + key, cache.ToJson(s_jsonSerializerSettings),
                    expiryTimeSpan.GetValueOrDefault(TimeSpan.MaxValue)));

            _logger.LogJsonRedisSetEnd(_instance + key);
        }

        public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            await ConnectAsync();

            TimeSpan? expiryTimeSpan = GetExpiryTimeSpan(options);

            JsonRedisCacheModel cache = new JsonRedisCacheModel
            {
                AbsoluteExpiration = GetAbsoluteExpiration(options),
                SlidingExpiration = options.SlidingExpiration,
                Payload = value.ToUTF8String()
            };

            _logger.LogJsonRedisSetBegin(_instance + key);

            await _telemetryClient.TrackDependencyAsync(DepandencyType.Redis, GetType().FullName, "SetAsync",
                async () => await _cache.StringSetAsync(_instance + key, cache.ToJson(s_jsonSerializerSettings),
                    expiryTimeSpan.GetValueOrDefault(TimeSpan.MaxValue)));

            _logger.LogJsonRedisSetEnd(_instance + key);
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

        private string GetAndRefresh(string key, bool refresh = false)
        {
            Connect();

            string result = GetString(key);

            if (!result.IsNullOrEmpty())
            {
                JsonRedisCacheModel cache = result.FromJson<JsonRedisCacheModel>(s_jsonSerializerSettings);
                if (cache != null)
                {
                    if (refresh)
                    {
                        RefreshExpiry(key, cache.AbsoluteExpiration, cache.SlidingExpiration);
                    }

                    return cache.Payload;
                }
            }

            return null;
        }

        private async Task<string> GetAndRefreshAsync(string key, bool refresh = false)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            await ConnectAsync();

            string result = await GetStringAsync(key);

            if (!result.IsNullOrEmpty())
            {
                JsonRedisCacheModel cache = result.FromJson<JsonRedisCacheModel>(s_jsonSerializerSettings);
                if (cache != null)
                {
                    if (refresh)
                    {
                        await RefreshExpiryAsync(key, cache.AbsoluteExpiration, cache.SlidingExpiration);
                    }

                    return cache.Payload;
                }
            }

            return null;
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


        private static TimeSpan? GetMaxTimeSpan(TimeSpan? value1, TimeSpan? value2)
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

        private string GetString(string key)
        {
            _logger.LogJsonRedisGetBegin(_instance + key);

            RedisValue result = _telemetryClient.TrackDependency(DepandencyType.Redis, _connection.Configuration, "Get" + _instance + key,
                () => s_retryPolicy.ExecuteAction(() => _cache.StringGet(_instance + key)));

            _logger.LogJsonRedisGetEnd(_instance + key);

            if (result.HasValue && !result.IsNullOrEmpty)
            {
                return result;
            }

            return null;
        }

        private async Task<string> GetStringAsync(string key)
        {
            _logger.LogJsonRedisGetBegin(_instance + key);

            RedisValue result = await _telemetryClient.TrackDependency(DepandencyType.Redis, _connection.Configuration, "Get" + _instance + key,
                async () => await s_retryPolicy.ExecuteAsync(async () => await _cache.StringGetAsync(_instance + key)));

            _logger.LogJsonRedisGetEnd(_instance + key);

            if (result.HasValue && !result.IsNullOrEmpty)
            {
                return result;
            }

            return null;
        }

        private bool RefreshExpiry(string key, DateTimeOffset? absoluteExpiration, TimeSpan? slidingExpiration)
        {
            TimeSpan? expiryTimeSpan = GetExpiryTimeSpan(absoluteExpiration, slidingExpiration);

            _logger.LogJsonRedisRefreshBegin(_instance + key);

            bool result = _telemetryClient.TrackDependency(DepandencyType.Redis, _connection.Configuration, "RefreshExpiry" + _instance + key,
                () => s_retryPolicy.ExecuteAction(() => _cache.KeyExpire(_instance + key, expiryTimeSpan.GetValueOrDefault(TimeSpan.MaxValue))));

            _logger.LogJsonRedisRefreshEnd(_instance + key);

            return result;
        }

        private async Task<bool> RefreshExpiryAsync(string key, DateTimeOffset? absoluteExpiration, TimeSpan? slidingExpiration)
        {
            TimeSpan? expiryTimeSpan = GetExpiryTimeSpan(absoluteExpiration, slidingExpiration);

            _logger.LogJsonRedisRefreshBegin(_instance + key);

            bool result = await _telemetryClient.TrackDependency(DepandencyType.Redis, _connection.Configuration, "RefreshExpiry" + _instance + key,
                async () => await s_retryPolicy.ExecuteAsync(async () => await _cache.KeyExpireAsync(_instance + key, expiryTimeSpan.GetValueOrDefault(TimeSpan.MaxValue))));

            _logger.LogJsonRedisRefreshEnd(_instance + key);

            return result;
        }
    }
}