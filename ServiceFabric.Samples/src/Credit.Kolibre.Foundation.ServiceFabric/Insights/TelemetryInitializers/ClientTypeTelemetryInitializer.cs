// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : ClientTypeTelemetryInitializer.cs
// Created          : 2016-06-30  9:01 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;

namespace Credit.Kolibre.Foundation.ServiceFabric.Insights.TelemetryInitializers
{
    public class ClientTypeTelemetryInitializer : TelemetryInitializerBase
    {
        private const string CLIENT_TYPE_KEY = "ClientType";

        public ClientTypeTelemetryInitializer(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (!telemetry.Context.Properties.ContainsKey(CLIENT_TYPE_KEY))
            {
                if (!requestTelemetry.Context.Properties.ContainsKey(CLIENT_TYPE_KEY))
                {
                    string resultClientType;
                    object clientTypeItem;
                    if (platformContext.Items.TryGetValue(Constants.X_KC_CLIENTTYPE, out clientTypeItem) && clientTypeItem is string)
                    {
                        resultClientType = clientTypeItem.ToString();
                    }
                    else
                    {
                        resultClientType = "unknown";
                    }

                    requestTelemetry.Context.Properties[CLIENT_TYPE_KEY] = resultClientType;
                }

                telemetry.Context.Properties[CLIENT_TYPE_KEY] = requestTelemetry.Context.Properties[CLIENT_TYPE_KEY];
            }
        }
    }
}