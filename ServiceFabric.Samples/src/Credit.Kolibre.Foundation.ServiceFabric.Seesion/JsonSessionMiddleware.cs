// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Session
// File             : JsonSessionMiddleware.cs
// Created          : 2016-07-20  12:25 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.ServiceFabric.Seesion
{
    /// <summary>
    ///     Enables the session state for the application.
    /// </summary>
    public class JsonSessionMiddleware
    {
        private static readonly Func<bool> s_returnTrue = () => true;
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly ISessionIdProvider _sessionIdProvider;
        private readonly ISessionStore _sessionStore;
        private readonly SessionOptions _options;

        /// <summary>
        ///     Creates a new <see cref="JsonSessionMiddleware" />.
        /// </summary>
        /// <param name="next">The <see cref="RequestDelegate" /> representing the next middleware in the pipeline.</param>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory" /> representing the factory that used to create logger instances.</param>
        /// <param name="sessionIdProvider">The <see cref="ISessionIdProvider" /> used to get and init session id.</param>
        /// <param name="sessionStore">The <see cref="ISessionStore" /> representing the session store.</param>
        /// <param name="options">The session configuration options.</param>
        public JsonSessionMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            ISessionIdProvider sessionIdProvider,
            ISessionStore sessionStore,
            IOptions<SessionOptions> options)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (sessionIdProvider == null)
            {
                throw new ArgumentNullException(nameof(sessionIdProvider));
            }

            if (sessionStore == null)
            {
                throw new ArgumentNullException(nameof(sessionStore));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _next = next;
            _logger = loggerFactory.CreateLogger<JsonSessionMiddleware>();
            _sessionIdProvider = sessionIdProvider;
            _sessionStore = sessionStore;
            _options = options.Value ?? new SessionOptions();
        }

        /// <summary>
        ///     Invokes the logic of the middleware.
        /// </summary>
        /// <param name="httpContext">The <see cref="HttpContext" />.</param>
        /// <returns>A <see cref="Task" /> that completes when the middleware has completed processing.</returns>
        public async Task Invoke(HttpContext httpContext)
        {
            bool isNewSessionKey = false;

            string sessionId = _sessionIdProvider.GetSessionId();
            if (sessionId.IsNullOrEmpty())
            {
                sessionId = _sessionIdProvider.InitAndSetSessionId();
                isNewSessionKey = true;
            }

            SessionFeature feature = new SessionFeature
            {
                Session = _sessionStore.Create(sessionId, _options.IdleTimeout, s_returnTrue, isNewSessionKey)
            };

            httpContext.Features.Set<ISessionFeature>(feature);

            try
            {
                await _next(httpContext);
            }
            finally
            {
                httpContext.Features.Set<ISessionFeature>(null);

                if (feature.Session != null)
                {
                    try
                    {
                        await feature.Session.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogErrorCommitTheSession(sessionId, ex);
                    }

                    if (!feature.Session.Id.Equals(sessionId, StringComparison.Ordinal))
                    {
                        sessionId = feature.Session.Id;
                        _sessionIdProvider.SetSessionId(sessionId);
                    }
                }
            }
        }
    }
}