// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : SessionIdTelemetryInitializer.cs
// Created          : 2016-06-30  4:27 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Credit.Kolibre.Foundation.Sys;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;

namespace Credit.Kolibre.Foundation.ServiceFabric.Insights.TelemetryInitializers
{
    /// <summary>
    ///     A telemetry initializer that populates session context id.
    /// </summary>
    public class SessionIdTelemetryInitializer : TelemetryInitializerBase
    {
        public SessionIdTelemetryInitializer(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (telemetry.Context.Session.Id.IsNullOrEmpty() || !telemetry.Context.Session.Id.IsGuid())
            {
                if (requestTelemetry.Context.Session.Id.IsNullOrEmpty() || !requestTelemetry.Context.Session.Id.IsGuid())
                {
                    object sessionIdItem;
                    if (platformContext.Items.TryGetValue(Constants.X_KC_SESSIONID, out sessionIdItem) && sessionIdItem is string)
                    {
                        string resultSessionId = sessionIdItem.ToString();
                        if (resultSessionId.IsGuid())
                        {
                            requestTelemetry.Context.Session.Id = resultSessionId;
                            requestTelemetry.Context.Properties["SessionId"] = resultSessionId;
                        }
                    }
                }
            }

            telemetry.Context.Session.Id = requestTelemetry.Context.Session.Id;
            telemetry.Context.Properties["SessionId"] = telemetry.Context.Session.Id;
        }
    }
}