// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : CloudRoleNameTelemetryInitializer.cs
// Created          : 2016-06-30  12:24 PM
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
    ///     A telemetry initializer that populates cloud context role name.
    /// </summary>
    public class CloudRoleNameTelemetryInitializer : TelemetryInitializerBase
    {
        public CloudRoleNameTelemetryInitializer(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (telemetry.Context.Cloud.RoleName.IsNullOrEmpty())
            {
                if (requestTelemetry.Context.Cloud.RoleName.IsNullOrEmpty())
                {
                    string resultRoleName = platformContext.Request.Host.Value;

                    if (resultRoleName.IsNullOrEmpty())
                    {
                        resultRoleName = platformContext.Request.Headers[HeaderNames.Host];
                    }

                    if (resultRoleName.IsNullOrEmpty())
                    {
                        IHttpRequestFeature httpRequestFeature = platformContext.Features.Get<IHttpRequestFeature>();

                        if (httpRequestFeature != null)
                        {
                            resultRoleName = httpRequestFeature.Headers[HeaderNames.Host];
                        }
                    }

                    if (resultRoleName.IsNullOrEmpty())
                    {
                        object roleNameItem;
                        if (platformContext.Items.TryGetValue(Constants.X_KC_HOST, out roleNameItem) && roleNameItem is string)
                        {
                            resultRoleName = roleNameItem.ToString();
                        }
                    }

                    if (resultRoleName.IsNotNullOrEmpty())
                    {
                        requestTelemetry.Context.Cloud.RoleName = resultRoleName;
                    }
                }

                telemetry.Context.Cloud.RoleName = requestTelemetry.Context.Cloud.RoleName;
            }
        }
    }
}