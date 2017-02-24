// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : TelemetryInitializerBase.cs
// Created          : 2016-06-29  1:03 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Credit.Kolibre.Foundation.ServiceFabric.Insights.TelemetryInitializers
{
    public abstract class TelemetryInitializerBase : ITelemetryInitializer
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected TelemetryInitializerBase(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }

            _httpContextAccessor = httpContextAccessor;
        }

        #region ITelemetryInitializer Members

        public void Initialize(ITelemetry telemetry)
        {
            HttpContext context = _httpContextAccessor.HttpContext;

            RequestTelemetry request = context?.RequestServices?.GetService<RequestTelemetry>();

            if (request == null)
            {
                return;
            }

            OnInitializeTelemetry(context, request, telemetry);
        }

        #endregion

        protected abstract void OnInitializeTelemetry(
            HttpContext platformContext,
            RequestTelemetry requestTelemetry,
            ITelemetry telemetry);
    }
}