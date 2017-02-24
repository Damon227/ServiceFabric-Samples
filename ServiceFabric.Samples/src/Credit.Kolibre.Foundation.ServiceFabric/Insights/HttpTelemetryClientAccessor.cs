// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : HttpTelemetryClientAccessor.cs
// Created          : 2016-07-09  2:43 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;

namespace Credit.Kolibre.Foundation.ServiceFabric.Insights
{
    public class HttpTelemetryClientAccessor : IHttpTelemetryClientAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private HttpContext HttpContext
        {
            get { return _httpContextAccessor.HttpContext; }
        }

        public HttpTelemetryClientAccessor(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }

            _httpContextAccessor = httpContextAccessor;
        }

        #region IHttpTelemetryClientAccessor Members

        public TelemetryClient GetTelemetryClient()
        {
            TelemetryClient client = new TelemetryClient();

            if (HttpContext != null)
            {
                object requestId;
                if (HttpContext.Items.TryGetValue(Constants.X_KC_REQUESTID, out requestId))
                {
                    if (requestId != null && requestId.ToString().IsGuid())
                    {
                        client.Context.Operation.Id = requestId.ToString();
                    }
                }
            }

            return client;
        }

        #endregion
    }
}