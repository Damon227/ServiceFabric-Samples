// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : PageTrackingMiddleware.cs
// Created          : 2016-07-10  1:51 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Credit.Kolibre.Foundation.ServiceFabric.Http.Headers;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;

namespace Credit.Kolibre.Foundation.ServiceFabric.Middleware
{
    public class PageTrackingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TelemetryClient _telemetryClient;

        public PageTrackingMiddleware(RequestDelegate next, TelemetryClient telemetryClient)
        {
            _next = next;
            _telemetryClient = telemetryClient;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            object pageNameItem;
            if (httpContext.Items.TryGetValue(Constants.X_KC_PAGENAME, out pageNameItem) && pageNameItem is string)
            {
                PageViewTelemetry telemetry = new PageViewTelemetry(pageNameItem.ToString());

                try
                {
                    string referer = httpContext.Request.Headers[HeaderNames.Referer];
                    if (referer.IsNotNullOrEmpty())
                    {
                        telemetry.Url = new Uri(referer);
                    }
                }
                catch
                {
                    //ignore
                }

                _telemetryClient.TrackPageView(telemetry);
            }
            await _next.Invoke(httpContext);
        }
    }
}