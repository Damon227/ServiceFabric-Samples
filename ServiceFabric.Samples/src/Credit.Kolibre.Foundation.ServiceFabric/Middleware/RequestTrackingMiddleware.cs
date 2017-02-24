// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : RequestTrackingMiddleware.cs
// Created          : 2016-08-23  10:04 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Credit.Kolibre.Foundation.Configuration;
using Credit.Kolibre.Foundation.Logging;
using Credit.Kolibre.Foundation.ServiceFabric.Http;
using Credit.Kolibre.Foundation.ServiceFabric.Utilities;
using Credit.Kolibre.Foundation.Static;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Credit.Kolibre.Foundation.ServiceFabric.Middleware
{
    public class RequestTrackingMiddleware
    {
        private readonly ILogger<RequestTrackingMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly ISettings _settings;
        private readonly string _sdkVersion;
        private readonly TelemetryClient _telemetryClient;


        public RequestTrackingMiddleware(RequestDelegate next, ISettings settings, ILogger<RequestTrackingMiddleware> logger, TelemetryClient telemetryClient)
        {
            _logger = logger;
            _next = next;
            _settings = settings;
            _sdkVersion = SdkVersionUtils.GetAssemblyVersion();
            _telemetryClient = telemetryClient;
        }

        public async Task Invoke(HttpContext httpContext, RequestTelemetry telemetry)
        {
            telemetry.Timestamp = DateTimeOffset.UtcNow;
            telemetry.HttpMethod = httpContext.Request.Method;
            telemetry.Url = httpContext.Request.GetUri();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            bool requestFailed = false;

            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception)
            {
                requestFailed = true;
                throw;
            }
            finally
            {
                sw.Stop();

                telemetry.Duration = sw.Elapsed;
                telemetry.ResponseCode = httpContext.Response.StatusCode.ToString();
                telemetry.Success = !requestFailed && (httpContext.Response.StatusCode < 400);
                telemetry.Context.GetInternalContext().SdkVersion = _sdkVersion;

                _telemetryClient.TrackRequest(telemetry);
                int eventCode = EventCode.CREDIT_KOLIBRE_FOUNDATION_ASPNETCORE_BASE + httpContext.Response.StatusCode;
                _logger.LogInformation(eventCode, "Request:" + BuildRequestData(telemetry).ToJson(SETTING.LOG_JSON_SETTINGS));
            }
        }

        private RequestData BuildRequestData(RequestTelemetry telemetry)
        {
            return new RequestData
            {
                App = _settings.Get("app"),
                AppVersion = _settings.Get("version"),
                ClientIPAddress = telemetry.Context.Location.Ip,
                ClientType = telemetry.Context.Properties.FirstOrDefault(i => i.Key == "ClientType").Value,
                PageName = telemetry.Context.Properties.FirstOrDefault(i => i.Key == "PageName").Value,
                HttpMethod = telemetry.HttpMethod.ToUpperInvariant(),
                RequestId = telemetry.Id,
                RequestName = telemetry.Name,
                RequestTime = telemetry.StartTime.UtcDateTime.ToString("O"),
                RequestUrl = telemetry.Url.AbsoluteUri,
                ResponseCode = telemetry.ResponseCode,
                ResponseTime = telemetry.Duration.TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + " ms",
                RoleInstance = telemetry.Context.Cloud.RoleInstance,
                RoleName = telemetry.Context.Cloud.RoleName,
                Success = telemetry.Success.GetValueOrDefault() ? "true" : "false"
            };
        }
    }
}