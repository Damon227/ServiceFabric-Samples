// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.ServiceFabric.Seesion
// File             : DistributedJsonSession.cs
// Created          : 2017-02-15  19:02
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Credit.Kolibre.Foundation.ServiceFabric.Caching;
using Credit.Kolibre.Foundation.Static;
using Credit.Kolibre.Foundation.Sys;
using Credit.Kolibre.Foundation.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Credit.Kolibre.Foundation.ServiceFabric.Seesion
{
    public class DistributedJsonSession : ISession
    {
        private static readonly DistributedCacheEntryOptions s_cacheEntryOptions = new DistributedCacheEntryOptions();
        private readonly HttpContext _httpContext;
        private readonly IDistributedCache _cache;
        private readonly TimeSpan _idleTimeout;
        private readonly Func<bool> _tryEstablishSession;
        private readonly ILogger _logger;
        private readonly bool _isNewSessionKey;
        private string _sessionKey;
        private IDictionary<string, string> _store;
        private bool _isModified;
        private bool _loaded;
        private bool _isAvailable;

        public DistributedJsonSession(
            HttpContext httpContext,
            IDistributedCache cache,
            string sessionKey,
            TimeSpan idleTimeout,
            Func<bool> tryEstablishSession,
            ILoggerFactory loggerFactory,
            bool isNewSessionKey)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (sessionKey.IsNullOrEmpty())
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(sessionKey));
            }

            if (tryEstablishSession == null)
            {
                throw new ArgumentNullException(nameof(tryEstablishSession));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _httpContext = httpContext;
            _cache = cache;
            _sessionKey = sessionKey;
            _idleTimeout = idleTimeout;
            _tryEstablishSession = tryEstablishSession;
            _logger = loggerFactory.CreateLogger<DistributedJsonSession>();
            _isNewSessionKey = isNewSessionKey;
            _store = new Dictionary<string, string>();
        }

        #region ISession Members

        public bool IsAvailable
        {
            get
            {
                LoadOrInit();
                return _isAvailable;
            }
        }

        public string Id
        {
            get
            {
                LoadOrInit();
                return _sessionKey;
            }
        }

        public IEnumerable<string> Keys
        {
            get
            {
                LoadOrInit();
                return _store.Keys;
            }
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            if (key == null)
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(key));
            }

            LoadOrInit();
            string stringValue;
            if (_store.TryGetValue(key, out stringValue))
            {
                value = stringValue.GetBytesOfUTF8();
                return true;
            }

            value = null;
            return false;
        }

        public void Set(string key, byte[] value)
        {
            if (key == null)
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            LoadOrInit();

            if (IsAvailable)
            {
                _store[key] = value.ToUTF8String();
                _isModified = true;
            }
        }

        public void Remove(string key)
        {
            LoadOrInit();
            _isModified |= _store.Remove(key);
        }

        public void Clear()
        {
            LoadOrInit();
            _isModified |= _store.Count > 0;
            _store.Clear();
        }

        // This will throw if called directly and a failure occurs. The user is expected to handle the failures.
        public async Task LoadAsync()
        {
            if (!_loaded)
            {
                if (!_isNewSessionKey)
                {
                    Dictionary<string, string> data = await _cache.GetObjectAsync<Dictionary<string, string>>("Session:" + _sessionKey);
                    if (data == null)
                    {
                        _logger.LogSessionMissing(_sessionKey);
                    }
                    else
                    {
                        _logger.LogSessionLoaded(_sessionKey, data.Count);

                        RefreshSession(data);
                    }
                }

                if (!_store.ContainsKey("Metadata") && _tryEstablishSession())
                {
                    _store["Metadata"] = SessionMetadata.InitSessionMetadata(_sessionKey, _httpContext, _idleTimeout).ToJson(SETTING.DATA_JSON_SETTINGS);
                    _isModified = true;
                    _logger.LogInitSession(_sessionKey);
                }

                _isAvailable = true;
                _loaded = true;
            }
        }

        public async Task CommitAsync()
        {
            if (_isModified)
            {
                await _cache.SetObjectAsync("Session:" + _sessionKey, _store, s_cacheEntryOptions);

                if (_isNewSessionKey)
                {
                    Dictionary<string, DateTimeOffset> sessionKeys = await _cache.GetObjectAsync<Dictionary<string, DateTimeOffset>>("Session:User:" + _sessionKey);
                    sessionKeys = sessionKeys ?? new Dictionary<string, DateTimeOffset>();
                    sessionKeys[_sessionKey] = DateTimeOffset.UtcNow;
                    await _cache.SetObjectAsync("Session:User:" + _sessionKey, sessionKeys, new DistributedCacheEntryOptions());
                }

                _isModified = false;
                _logger.LogSessionStored(_sessionKey, _store.Count);
            }
            else
            {
                await _cache.RefreshAsync(_sessionKey);
            }
        }

        #endregion

        private string GetUserId()
        {
            object userIdItem;
            if (_httpContext != null && _httpContext.Items.TryGetValue(Constants.X_KC_USERID, out userIdItem))
            {
                if (userIdItem is string && userIdItem.ToString().IsGuid())
                {
                    return userIdItem.ToString();
                }
            }

            return null;
        }

        private void LoadOrInit()
        {
            if (!_loaded)
            {
                try
                {
                    if (!_isNewSessionKey)
                    {
                        Dictionary<string, string> data = _cache.GetObject<Dictionary<string, string>>("Session:" + _sessionKey);
                        if (data == null)
                        {
                            _logger.LogSessionMissing(_sessionKey);
                        }
                        else
                        {
                            _logger.LogSessionLoaded(_sessionKey, data.Count);
                            _store = data;
                        }
                    }

                    if (!_store.ContainsKey("Metadata") && _tryEstablishSession())
                    {
                        _store["Metadata"] = SessionMetadata.InitSessionMetadata(_sessionKey, _httpContext, _idleTimeout).ToJson(SETTING.DATA_JSON_SETTINGS);
                        _isModified = true;
                        _logger.LogInitSession(_sessionKey);
                    }

                    _isAvailable = true;
                }
                catch (Exception exception)
                {
                    _logger.LogErrorSessionLoadOrInit(_sessionKey, exception);
                    _isAvailable = false;
                }
                finally
                {
                    _loaded = true;
                }
            }
        }

        private void RefreshSession(IDictionary<string, string> data)
        {
            SessionMetadata sessionMetadata = data["Metadata"].FromJson<SessionMetadata>(SETTING.DATA_JSON_SETTINGS);

            if (sessionMetadata.SessionId != _sessionKey)
            {
                _logger.LogSessionIdMissmatch(_sessionKey, sessionMetadata.SessionId);
            }
            else
            {
                DateTimeOffset sessionExpiryTime = sessionMetadata.ExpiryTime;

                sessionMetadata.ExpiryTime = DateTimeOffset.UtcNow.Add(_idleTimeout);
                sessionMetadata.IsFirstRequest = false;

                if (sessionExpiryTime <= DateTimeOffset.UtcNow)
                {
                    sessionMetadata.SourceSessionId = _sessionKey;
                    RefreshSessionId();
                    sessionMetadata.SessionId = _sessionKey;
                }

                data["Metadata"] = sessionMetadata.ToJson(SETTING.DATA_JSON_SETTINGS);
                _store = data;
            }
        }

        private void RefreshSessionId()
        {
            _sessionKey = ID.NewSequentialGuid().ToGuidString();
        }
    }
}