// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : OperationIdTelemetryInitializer.cs
// Created          : 2016-06-29  1:02 PM
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
    ///     A telemetry initializer that populates operation context id.
    /// </summary>
    public class OperationIdTelemetryInitializer : TelemetryInitializerBase
    {
        public OperationIdTelemetryInitializer(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (requestTelemetry.Context.Operation.Id.IsNullOrEmpty() || !requestTelemetry.Context.Operation.Id.IsGuid())
            {
                object operationIdItem;
                if (platformContext.Items.TryGetValue(Constants.X_KC_REQUESTID, out operationIdItem) && operationIdItem is string)
                {
                    string resultOperationId = operationIdItem.ToString();
                    if (resultOperationId.IsGuid())
                    {
                        requestTelemetry.Id = resultOperationId;
                        requestTelemetry.Context.Operation.Id = resultOperationId;
                        requestTelemetry.Context.Properties["OperationId"] = resultOperationId;
                    }
                }
            }

            telemetry.Context.Operation.Id = requestTelemetry.Context.Operation.Id;
            telemetry.Context.Properties["OperationId"] = telemetry.Context.Operation.Id;
        }
    }
}