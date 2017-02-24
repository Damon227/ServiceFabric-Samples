// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : CloudRoleInstanceTelemetryInitializer.cs
// Created          : 2016-06-30  2:18 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;

namespace Credit.Kolibre.Foundation.ServiceFabric.Insights.TelemetryInitializers
{
    /// <summary>
    ///     A telemetry initializer that populates cloud context role instance.
    /// </summary>
    public class CloudRoleInstanceTelemetryInitializer : TelemetryInitializerBase
    {
        private string _roleInstanceName;

        public CloudRoleInstanceTelemetryInitializer(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (telemetry.Context.Cloud.RoleInstance.IsNullOrEmpty())
            {
                if (requestTelemetry.Context.Cloud.RoleInstance.IsNullOrEmpty())
                {
                    string name = LazyInitializer.EnsureInitialized(ref _roleInstanceName, GetMachineName);
                    requestTelemetry.Context.Cloud.RoleInstance = name;
                }

                telemetry.Context.Cloud.RoleInstance = requestTelemetry.Context.Cloud.RoleInstance;
            }
        }

        private static string GetMachineName()
        {
            string hostName = Dns.GetHostName();

            // Issue #61: For dnxcore machine name does not have domain name like in full framework 
#if !NETSTANDARD1_6
            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            if (!hostName.EndsWith(domainName, StringComparison.OrdinalIgnoreCase))
            {
                hostName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", hostName, domainName);
            }
#endif
            return hostName;
        }
    }
}