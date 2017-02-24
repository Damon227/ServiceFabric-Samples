// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : UserAgentTelemetryInitializer.cs
// Created          : 2016-06-29  1:03 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Credit.Kolibre.Foundation.ServiceFabric.Http.Headers;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Credit.Kolibre.Foundation.ServiceFabric.Insights.TelemetryInitializers
{
    /// <summary>
    ///     Telemetry initializer populates user agent (telemetry.Context.User.UserAgent) for
    ///     all telemetry data items.
    /// </summary>
    public class UserAgentTelemetryInitializer : TelemetryInitializerBase
    {
        public UserAgentTelemetryInitializer(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (telemetry.Context.User.UserAgent.IsNullOrEmpty())
            {
                if (requestTelemetry.Context.User.UserAgent.IsNullOrEmpty())
                {
                    string resultUserAgent = platformContext.Request.Headers[HeaderNames.UserAgent];

                    if (resultUserAgent.IsNullOrEmpty())
                    {
                        IHttpRequestFeature httpRequestFeature = platformContext.Features.Get<IHttpRequestFeature>();

                        if (httpRequestFeature != null)
                        {
                            resultUserAgent = httpRequestFeature.Headers[HeaderNames.UserAgent];
                        }
                    }

                    requestTelemetry.Context.User.UserAgent = resultUserAgent;
                }

                telemetry.Context.User.UserAgent = requestTelemetry.Context.User.UserAgent;
            }
        }
    }
}