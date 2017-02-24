// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : ExceptionTrackingMiddleware.cs
// Created          : 2016-07-20  12:25 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Credit.Kolibre.Foundation.Logging;
using Credit.Kolibre.Foundation.ServiceFabric.Utilities;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Credit.Kolibre.Foundation.ServiceFabric.Middleware
{
    public class ExceptionTrackingMiddleware
    {
        private readonly ILogger<RequestTrackingMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly TelemetryClient _telemetryClient;
        private readonly string _sdkVersion;

        public ExceptionTrackingMiddleware(RequestDelegate next, TelemetryClient telemetryClient, ILogger<RequestTrackingMiddleware> logger)
        {
            _next = next;
            _telemetryClient = telemetryClient;
            _logger = logger;
            _sdkVersion = SdkVersionUtils.GetAssemblyVersion();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception exception)
            {
                ExceptionTelemetry exceptionTelemetry = new ExceptionTelemetry(exception)
                {
                    HandledAt = ExceptionHandledAt.Platform,
                    SeverityLevel = SeverityLevel.Critical,
                    Timestamp = DateTimeOffset.UtcNow
                };
                exceptionTelemetry.Context.GetInternalContext().SdkVersion = _sdkVersion;
                _telemetryClient.Track(exceptionTelemetry);
                _logger.LogCritical(EventCode.CREDIT_KOLIBRE_FOUNDATION_ASPNETCORE_ERROR_UNEXPECTED, exception, exception.Message);
                throw;
            }
        }
    }
}