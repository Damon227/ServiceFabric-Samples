// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : AppInfoTelemetryInitializer.cs
// Created          : 2016-06-30  10:35 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Credit.Kolibre.Foundation.Sys;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

// ReSharper disable SuggestBaseTypeForParameter

namespace Credit.Kolibre.Foundation.ServiceFabric.Insights.TelemetryInitializers
{
    public class AppInfoTelemetryInitializer : TelemetryInitializerBase
    {
        private const string APP_NAME_KEY = "AppName";
        private const string APP_VERSION_KEY = "AppVersion";

        private readonly IConfiguration _configuration;

        public AppInfoTelemetryInitializer(IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpContextAccessor)
        {
            _configuration = configuration;
        }

        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            InitAppName(requestTelemetry, telemetry);
            InitAppVersion(requestTelemetry, telemetry);
        }

        private void InitAppName(RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (!telemetry.Context.Properties.ContainsKey(APP_NAME_KEY))
            {
                if (!requestTelemetry.Context.Properties.ContainsKey(APP_NAME_KEY))
                {
                    string appName = _configuration["App:Name"];
                    if (appName.IsNotNullOrEmpty())
                    {
                        requestTelemetry.Context.Properties[APP_NAME_KEY] = appName;
                    }
                }

                telemetry.Context.Properties[APP_NAME_KEY] = requestTelemetry.Context.Properties[APP_NAME_KEY];
            }
        }

        private void InitAppVersion(RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (telemetry.Context.Component.Version.IsNullOrEmpty())
            {
                if (requestTelemetry.Context.Component.Version.IsNullOrEmpty())
                {
                    string appName = _configuration["App:Version"];
                    if (appName.IsNotNullOrEmpty())
                    {
                        requestTelemetry.Context.Properties[APP_VERSION_KEY] = appName;
                    }
                }

                telemetry.Context.Properties[APP_VERSION_KEY] = requestTelemetry.Context.Properties[APP_VERSION_KEY];
            }
        }
    }
}