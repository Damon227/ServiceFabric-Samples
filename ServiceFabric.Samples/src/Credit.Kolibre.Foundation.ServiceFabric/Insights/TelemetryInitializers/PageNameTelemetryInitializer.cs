// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : PageNameTelemetryInitializer.cs
// Created          : 2016-06-30  9:48 PM
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
    public class PageNameTelemetryInitializer : TelemetryInitializerBase
    {
        private const string PAGE_NAME_KEY = "PageName";

        public PageNameTelemetryInitializer(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (!telemetry.Context.Properties.ContainsKey(PAGE_NAME_KEY))
            {
                if (!requestTelemetry.Context.Properties.ContainsKey(PAGE_NAME_KEY))
                {
                    object pageNameItem;
                    if (platformContext.Items.TryGetValue(Constants.X_KC_PAGENAME, out pageNameItem) && pageNameItem is string)
                    {
                        string resultPageName = pageNameItem.ToString();
                        if (resultPageName.IsNotNullOrEmpty())
                        {
                            requestTelemetry.Context.Properties[PAGE_NAME_KEY] = resultPageName;
                        }
                    }
                }

                telemetry.Context.Properties[PAGE_NAME_KEY] = requestTelemetry.Context.Properties[PAGE_NAME_KEY];
            }
        }
    }
}