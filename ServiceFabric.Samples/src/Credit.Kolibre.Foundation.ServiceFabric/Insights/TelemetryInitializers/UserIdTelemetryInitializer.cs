// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : UserIdTelemetryInitializer.cs
// Created          : 2016-06-30  4:47 PM
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
    ///     Telemetry initializer populates user agent (telemetry.Context.User.Id) for
    ///     all telemetry data items.
    /// </summary>
    public class UserIdTelemetryInitializer : TelemetryInitializerBase
    {
        public UserIdTelemetryInitializer(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (telemetry.Context.User.Id.IsNullOrEmpty() || !telemetry.Context.User.Id.IsGuid())
            {
                if (requestTelemetry.Context.User.Id.IsNullOrEmpty() || !requestTelemetry.Context.User.Id.IsGuid())
                {
                    object userIdItem;
                    if (platformContext.Items.TryGetValue(Constants.X_KC_USERID, out userIdItem) && userIdItem is string)
                    {
                        string resultUserId = userIdItem.ToString();
                        if (resultUserId.IsGuid())
                        {
                            requestTelemetry.Context.User.Id = resultUserId;
                            telemetry.Context.Properties["UserId"] = resultUserId;
                        }
                    }
                }

                telemetry.Context.User.Id = requestTelemetry.Context.User.Id;
                telemetry.Context.Properties["UserId"] = telemetry.Context.User.Id;
            }
        }
    }
}