// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : LocationClientIPTelemetryInitializer.cs
// Created          : 2016-06-29  1:01 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Net;
using System.Net.Sockets;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;

namespace Credit.Kolibre.Foundation.ServiceFabric.Insights.TelemetryInitializers
{
    /// <summary>
    ///     This telemetry initializer extracts client IP address and populates telemetry.Context.Location.Ip property.
    ///     Lot's of code reuse from Microsoft.ApplicationInsights.Extensibility.Web.TelemetryInitializers.LocationClientIPTelemetryInitializer
    /// </summary>
    public class LocationClientIPTelemetryInitializer : TelemetryInitializerBase
    {
        public LocationClientIPTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
        }

        /// <summary>
        ///     Gets or sets comma separated list of request header names that is used to check client id.
        /// </summary>
        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (telemetry.Context.Location.Ip.IsNullOrEmpty())
            {
                if (requestTelemetry.Context.Location.Ip.IsNullOrEmpty())
                {
                    string resultClientIp = null;
                    object clientIpItem;
                    if (platformContext.Items.TryGetValue(Constants.X_KC_CLIENTIP, out clientIpItem) && clientIpItem is string)
                    {
                        resultClientIp = clientIpItem.ToString();
                    }

                    if (resultClientIp.IsNotNullOrEmpty() && IsCorrectIpAddress(resultClientIp))
                    {
                        requestTelemetry.Context.Location.Ip = resultClientIp;
                        requestTelemetry.Context.Properties["ClientIp"] = resultClientIp;
                    }
                }

                telemetry.Context.Location.Ip = requestTelemetry.Context.Location.Ip;
                telemetry.Context.Properties["ClientIp"] = telemetry.Context.Location.Ip;
            }
        }

        private static bool IsCorrectIpAddress(string address)
        {
            IPAddress outParameter;
            address = address.Trim();

            // Core SDK does not support setting Location.Ip to malformed ip address
            if (IPAddress.TryParse(address, out outParameter))
            {
                // Also SDK supports only ipv4!
                if (outParameter.AddressFamily == AddressFamily.InterNetwork)
                {
                    return true;
                }
            }

            return false;
        }
    }
}