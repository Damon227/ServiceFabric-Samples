// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : OperationNameTelemetryInitializer.cs
// Created          : 2016-06-29  1:03 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics;
using System.Linq;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Tree;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DiagnosticAdapter;

namespace Credit.Kolibre.Foundation.ServiceFabric.Insights.TelemetryInitializers
{
    /// <summary>
    ///     A telemetry initializer that populates operation context name.
    /// </summary>
    public class OperationNameTelemetryInitializer : TelemetryInitializerBase
    {
        public const string BEFORE_ACTION_NOTIFICATION_NAME = "Microsoft.AspNetCore.Mvc.BeforeAction";

        public OperationNameTelemetryInitializer(IHttpContextAccessor httpContextAccessor, DiagnosticListener telemetryListener)
            : base(httpContextAccessor)
        {
            if (telemetryListener == null)
            {
                throw new ArgumentNullException(nameof(telemetryListener));
            }

            telemetryListener.SubscribeWithAdapter(this);
        }

        public OperationNameTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
            : this(httpContextAccessor, null)
        {
        }

        [DiagnosticName(BEFORE_ACTION_NOTIFICATION_NAME)]
        public void OnBeforeAction(ActionDescriptor actionDescriptor, HttpContext httpContext, RouteData routeData)
        {
            RequestTelemetry telemetry = httpContext.RequestServices.GetService<RequestTelemetry>();
            if (telemetry != null && telemetry.Name.IsNullOrEmpty())
            {
                string name = GetNameFromRouteContext(routeData);

                if (name.IsNotNullOrEmpty())
                {
                    name = httpContext.Request.Method + " " + name;
                    telemetry.Name = name;
                }
            }
        }

        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (telemetry.Context.Operation.Name.IsNullOrEmpty())
            {
                if (requestTelemetry.Context.Operation.Name.IsNullOrEmpty())
                {
                    // We didn't get BeforeAction notification
                    string resultOperationName = platformContext.Request.Method.ToUpperInvariant() + " " + platformContext.Request.Path.Value;

                    requestTelemetry.Name = resultOperationName;
                    requestTelemetry.Context.Operation.Name = resultOperationName;
                }

                telemetry.Context.Operation.Name = requestTelemetry.Context.Operation.Name;
            }
        }

        private static string GetNameFromRouteContext(RouteData routeData)
        {
            string name = null;

            if (routeData.Values.Count > 0)
            {
                RouteValueDictionary routeValues = routeData.Values;

                object controller;
                routeValues.TryGetValue("controller", out controller);
                string controllerString = controller == null ? string.Empty : controller.ToString();

                if (controllerString.IsNotNullOrEmpty())
                {
                    name = controllerString;

                    object action;
                    routeValues.TryGetValue("action", out action);
                    string actionString = action == null ? string.Empty : action.ToString();

                    if (actionString.IsNotNullOrEmpty())
                    {
                        name += "/" + actionString;
                    }

                    if (routeValues.Keys.Count > 2)
                    {
                        // Add parameters
                        string[] sortedKeys = routeValues.Keys
                            .Where(key =>
                                !string.Equals(key, "controller", StringComparison.OrdinalIgnoreCase) &&
                                !string.Equals(key, "action", StringComparison.OrdinalIgnoreCase) &&
                                !string.Equals(key, TreeRouter.RouteGroupKey, StringComparison.OrdinalIgnoreCase))
                            .OrderBy(key => key, StringComparer.OrdinalIgnoreCase)
                            .ToArray();

                        if (sortedKeys.Length > 0)
                        {
                            string arguments = string.Join(@"/", sortedKeys);
                            name += " [" + arguments + "]";
                        }
                    }
                }
            }

            return name;
        }
    }
}