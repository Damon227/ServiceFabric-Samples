// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Session
// File             : DistributedJsonSessionStore.cs
// Created          : 2016-07-03  9:39 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Credit.Kolibre.Foundation.Static;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Credit.Kolibre.Foundation.ServiceFabric.Seesion
{
    public class DistributedJsonSessionStore : ISessionStore
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDistributedCache _cache;
        private readonly ILoggerFactory _loggerFactory;

        public DistributedJsonSessionStore(
            IHttpContextAccessor httpContextAccessor,
            IDistributedCache cache,
            ILoggerFactory loggerFactory)
        {
            if (httpContextAccessor == null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }

            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _loggerFactory = loggerFactory;
        }

        #region ISessionStore Members

        public ISession Create(string sessionKey, TimeSpan idleTimeout, Func<bool> tryEstablishSession, bool isNewSessionKey)
        {
            if (sessionKey.IsNullOrEmpty())
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(sessionKey));
            }

            if (tryEstablishSession == null)
            {
                throw new ArgumentNullException(nameof(tryEstablishSession));
            }

            return new DistributedJsonSession(_httpContextAccessor.HttpContext, _cache, sessionKey, idleTimeout, tryEstablishSession, _loggerFactory, isNewSessionKey);
        }

        #endregion
    }
}