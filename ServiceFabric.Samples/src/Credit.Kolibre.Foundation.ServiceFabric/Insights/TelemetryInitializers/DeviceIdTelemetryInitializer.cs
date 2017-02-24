// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : DeviceIdTelemetryInitializer.cs
// Created          : 2016-06-30  3:32 PM
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
    ///     A telemetry initializer that populates device context id.
    /// </summary>
    public class DeviceIdTelemetryInitializer : TelemetryInitializerBase
    {
        public DeviceIdTelemetryInitializer(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (telemetry.Context.Device.Id.IsNullOrEmpty() || !telemetry.Context.Device.Id.IsGuid())
            {
                if (requestTelemetry.Context.Device.Id.IsNullOrEmpty() || !requestTelemetry.Context.Device.Id.IsGuid())
                {
                    object deviceIdItem;

                    if (platformContext.Items.TryGetValue(Constants.X_KC_DEVICEID, out deviceIdItem) && deviceIdItem is string)
                    {
                        string resultDeviceId = deviceIdItem.ToString();
                        if (resultDeviceId.IsGuid())
                        {
                            requestTelemetry.Context.Device.Id = resultDeviceId;
                            requestTelemetry.Context.Properties["DeviceId"] = resultDeviceId;
                        }
                    }
                }

                telemetry.Context.Device.Id = requestTelemetry.Context.Device.Id;
                telemetry.Context.Properties["DeviceId"] = telemetry.Context.Device.Id;
            }
        }
    }
}