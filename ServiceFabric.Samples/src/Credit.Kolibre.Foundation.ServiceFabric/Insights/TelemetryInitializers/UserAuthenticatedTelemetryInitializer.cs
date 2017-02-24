// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : UserAuthenticatedTelemetryInitializer.cs
// Created          : 2016-06-30  4:55 PM
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
    public class UserAuthenticatedTelemetryInitializer : TelemetryInitializerBase
    {
        public UserAuthenticatedTelemetryInitializer(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (telemetry.Context.User.AuthenticatedUserId.IsNullOrEmpty() || !telemetry.Context.User.AuthenticatedUserId.IsGuid())
            {
                if (requestTelemetry.Context.User.AuthenticatedUserId.IsNullOrEmpty() || !requestTelemetry.Context.User.AuthenticatedUserId.IsGuid())
                {
                    if (platformContext.User.Identity.IsAuthenticated && platformContext.User.Identity.Name.IsNotNullOrEmpty())
                    {
                        requestTelemetry.Context.User.AuthenticatedUserId = platformContext.User.Identity.Name;
                        requestTelemetry.Context.Properties["IsAuthenticated"] = platformContext.User.Identity.IsAuthenticated.ToString();
                    }
                }

                telemetry.Context.User.AuthenticatedUserId = requestTelemetry.Context.User.AuthenticatedUserId;
                telemetry.Context.Properties["IsAuthenticated"] = platformContext.User.Identity.IsAuthenticated.ToString();
            }
        }
    }
}