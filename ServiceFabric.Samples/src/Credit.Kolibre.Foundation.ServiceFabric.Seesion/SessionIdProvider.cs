// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Session
// File             : SessionIdProvider.cs
// Created          : 2016-07-03  5:14 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Credit.Kolibre.Foundation.Static;
using Credit.Kolibre.Foundation.Sys;
using Credit.Kolibre.Foundation.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.ServiceFabric.Seesion
{
    public class SessionIdProvider : ISessionIdProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SessionOptions _sessionOptions;

        public SessionIdProvider(IHttpContextAccessor httpContextAccessor, IOptions<SessionOptions> optionsAccessor)
        {
            if (httpContextAccessor == null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }

            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(SR.Argument_EmptyOrNullString, nameof(optionsAccessor));
            }

            _httpContextAccessor = httpContextAccessor;
            _sessionOptions = optionsAccessor.Value ?? new SessionOptions();
        }

        private HttpContext HttpContext
        {
            get { return _httpContextAccessor.HttpContext; }
        }

        #region ISessionIdProvider Members

        public string GetSessionId()
        {
            if (HttpContext == null)
            {
                throw new ArgumentNullException(nameof(HttpContext));
            }

            if (_sessionOptions.SessionIdName.IsNullOrEmpty())
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(_sessionOptions.SessionIdName));
            }

            object item;
            if (HttpContext.Items.TryGetValue(_sessionOptions.SessionIdName, out item) && IsValidSessionId(item.ToString()))
            {
                return item.ToString();
            }

            return null;
        }

        public string InitAndSetSessionId()
        {
            if (HttpContext == null)
            {
                throw new ArgumentNullException(nameof(HttpContext));
            }

            if (_sessionOptions.SessionIdName.IsNullOrEmpty())
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(_sessionOptions.SessionIdName));
            }

            string sessionId = ID.NewSequentialGuid().ToGuidString();
            return SetSessionId(sessionId);
        }

        public string SetSessionId(string sessionId)
        {
            if (HttpContext == null)
            {
                throw new ArgumentNullException(nameof(HttpContext));
            }

            if (_sessionOptions.SessionIdName.IsNullOrEmpty())
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(_sessionOptions.SessionIdName));
            }

            HttpContext.Items[_sessionOptions.SessionIdName] = sessionId;
            return sessionId;
        }

        #endregion

        private static bool IsValidSessionId(string sessionId)
        {
            return sessionId.IsGuid();
        }
    }
}