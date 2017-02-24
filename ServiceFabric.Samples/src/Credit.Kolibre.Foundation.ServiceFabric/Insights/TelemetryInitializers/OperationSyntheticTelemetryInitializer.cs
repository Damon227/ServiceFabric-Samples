// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.AspNet
// File             : OperationSyntheticTelemetryInitializer.cs
// Created          : 2017-02-15  15:01
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Credit.Kolibre.Foundation.Sys;
using Credit.Kolibre.Foundation.Sys.Collections.Generic;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Credit.Kolibre.Foundation.ServiceFabric.Insights.TelemetryInitializers
{
    /// <summary>
    ///     This will allow to mark synthetic traffic from availability tests
    /// </summary>
    public class OperationSyntheticTelemetryInitializer : TelemetryInitializerBase
    {
        private const string SYNTHETIC_TEST_RUN_ID = "SyntheticTest-RunId";
        private const string SYNTHETIC_TEST_LOCATION = "SyntheticTest-Location";

        private const string SYNTHETIC_SOURCE_HEADER_VALUE = "Application Insights Availability Monitoring";

        public OperationSyntheticTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
        }

        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (telemetry.Context.Operation.SyntheticSource.IsNullOrEmpty())
            {
                if (requestTelemetry.Context.Operation.SyntheticSource.IsNullOrEmpty())
                {
                    string resultSyntheticSource = GetApplicationInsightsSyntheticSource(platformContext, telemetry);

                    if (resultSyntheticSource.IsNullOrEmpty())
                    {
                        resultSyntheticSource = GetKolibreCreditSyntheticSource(platformContext);
                    }

                    requestTelemetry.Context.Operation.SyntheticSource = resultSyntheticSource;
                }

                telemetry.Context.Operation.SyntheticSource = requestTelemetry.Context.Operation.SyntheticSource;
            }
        }

        private static string GetApplicationInsightsSyntheticSource(HttpContext platformContext, ITelemetry telemetry)
        {
            StringValues runIdHeader = platformContext.Request.Headers[SYNTHETIC_TEST_RUN_ID];
            StringValues locationHeader = platformContext.Request.Headers[SYNTHETIC_TEST_LOCATION];

            if (runIdHeader.IsNotNullOrEmpty() && locationHeader.IsNotNullOrEmpty())
            {
                telemetry.Context.Operation.SyntheticSource = SYNTHETIC_SOURCE_HEADER_VALUE;

                telemetry.Context.User.Id = locationHeader + "_" + runIdHeader;
                telemetry.Context.Session.Id = runIdHeader;

                return SYNTHETIC_SOURCE_HEADER_VALUE;
            }

            return null;
        }

        private static string GetKolibreCreditSyntheticSource(HttpContext platformContext)
        {
            object syntheticSourceItem;
            if (platformContext.Items.TryGetValue(Constants.X_KC_SOURCE, out syntheticSourceItem) && syntheticSourceItem is string)
            {
                return syntheticSourceItem.ToString();
            }
            return null;
        }
    }
}