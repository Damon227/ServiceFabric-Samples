// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : InsightsExtensions.cs
// Created          : 2016-06-27  9:13 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Credit.Kolibre.Foundation.ServiceFabric.Insights.TelemetryInitializers;
using Credit.Kolibre.Foundation.ServiceFabric.Middleware;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Credit.Kolibre.Foundation.ServiceFabric.Insights
{
    public static class InsightsExtensions
    {
        private const string INSTRUMENTATION_KEY_FROM_CONFIG = "ApplicationInsights:InstrumentationKey";
        private const string DEVELOPER_MODE_FROM_CONFIG = "ApplicationInsights:TelemetryChannel:DeveloperMode";
        private const string ENDPOINT_ADDRESS_FROM_CONFIG = "ApplicationInsights:TelemetryChannel:EndpointAddress";

        private const string INSTRUMENTATION_KEY_FOR_WEB_SITES = "APPINSIGHTS_INSTRUMENTATIONKEY";
        private const string DEVELOPER_MODE_FOR_WEB_SITES = "APPINSIGHTS_DEVELOPER_MODE";
        private const string ENDPOINT_ADDRESS_FOR_WEB_SITES = "APPINSIGHTS_ENDPOINTADDRESS";

        public static IConfigurationBuilder AddInsightsSettings(
            this IConfigurationBuilder configurationSourceRoot,
            bool? developerMode = null,
            string endpointAddress = null,
            string instrumentationKey = null)
        {
            List<KeyValuePair<string, string>> telemetryConfigValues = new List<KeyValuePair<string, string>>();

            bool wasAnythingSet = false;

            if (developerMode != null)
            {
                telemetryConfigValues.Add(new KeyValuePair<string, string>(DEVELOPER_MODE_FOR_WEB_SITES, developerMode.Value.ToString()));
                wasAnythingSet = true;
            }

            if (instrumentationKey != null)
            {
                telemetryConfigValues.Add(new KeyValuePair<string, string>(INSTRUMENTATION_KEY_FOR_WEB_SITES, instrumentationKey));
                wasAnythingSet = true;
            }

            if (endpointAddress != null)
            {
                telemetryConfigValues.Add(new KeyValuePair<string, string>(ENDPOINT_ADDRESS_FOR_WEB_SITES, endpointAddress));
                wasAnythingSet = true;
            }

            if (wasAnythingSet)
            {
                configurationSourceRoot.Add(new MemoryConfigurationSource { InitialData = telemetryConfigValues });
            }

            return configurationSourceRoot;
        }

        public static IServiceCollection AddInsightsTelemetry(this IServiceCollection services, IConfiguration config, ApplicationInsightsServiceOptions serviceOptions = null)
        {
            ApplicationInsightsServiceOptions options = serviceOptions ?? new ApplicationInsightsServiceOptions();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddSingleton<ITelemetryInitializer, AppInfoTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, ClientTypeTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, CloudRoleInstanceTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, CloudRoleNameTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, DeviceIdTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, LocationClientIPTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, OperationIdTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, OperationNameTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, OperationSyntheticTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, PageNameTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, SessionIdTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, UserAgentTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, UserAgentTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, UserIdTelemetryInitializer>();

            services.AddSingleton<ITelemetryModule, PerformanceCollectorModule>();
            services.AddSingleton<ITelemetryModule, DependencyTrackingTelemetryModule>();

            services.AddSingleton(serviceProvider =>
            {
                TelemetryConfiguration telemetryConfiguration = TelemetryConfiguration.Active;
                AddTelemetryChannelAndProcessorsForFullFramework(serviceProvider, telemetryConfiguration, options);
                telemetryConfiguration.TelemetryChannel = serviceProvider.GetService<ITelemetryChannel>() ?? telemetryConfiguration.TelemetryChannel;
                AddTelemetryConfiguration(config, telemetryConfiguration);
                AddServicesToCollection(serviceProvider, telemetryConfiguration.TelemetryInitializers);
                InitializeModulesWithConfiguration(serviceProvider, telemetryConfiguration);
                return telemetryConfiguration;
            });

            services.AddScoped<TelemetryClient>();
            services.AddScoped(svcs =>
            {
                // Default constructor need to be used
                RequestTelemetry rt = new RequestTelemetry();
                return rt;
            });

            return services;
        }


        public static IApplicationBuilder UseInsightsService(this IApplicationBuilder app)
        {
            app.UseMiddleware<PageTrackingMiddleware>();
            app.UseMiddleware<RequestTrackingMiddleware>();
            app.UseMiddleware<ExceptionTrackingMiddleware>();
            return app;
        }

        private static void AddServicesToCollection<T>(IServiceProvider serviceProvider, ICollection<T> collection)
        {
            IEnumerable<T> services = serviceProvider.GetService<IEnumerable<T>>();
            foreach (T service in services)
            {
                collection.Add(service);
            }
        }

        [SuppressMessage("ReSharper", "OperatorIsCanBeUsed")]
        private static void AddTelemetryChannelAndProcessorsForFullFramework(IServiceProvider serviceProvider, TelemetryConfiguration configuration, ApplicationInsightsServiceOptions serviceOptions)
        {
            // Adding Server Telemetry Channel if services doesn't have an existing channel
            configuration.TelemetryChannel = serviceProvider.GetService<ITelemetryChannel>() ?? new ServerTelemetryChannel();

            if (configuration.TelemetryChannel is ServerTelemetryChannel)
            {
                // Enabling Quick Pulse Metric Stream 
                if (serviceOptions.EnableQuickPulseMetricStream)
                {
                    QuickPulseTelemetryModule quickPulseModule = new QuickPulseTelemetryModule();
                    quickPulseModule.Initialize(configuration);

                    QuickPulseTelemetryProcessor processor;
                    configuration.TelemetryProcessorChainBuilder.Use(next =>
                    {
                        processor = new QuickPulseTelemetryProcessor(next);
                        quickPulseModule.RegisterTelemetryProcessor(processor);
                        return processor;
                    });
                }

                // Enabling Adaptive Sampling and initializing server telemetry channel with configuration
                if (configuration.TelemetryChannel.GetType() == typeof(ServerTelemetryChannel))
                {
                    if (serviceOptions.EnableAdaptiveSampling)
                    {
                        configuration.TelemetryProcessorChainBuilder.UseAdaptiveSampling();
                    }
                    (configuration.TelemetryChannel as ServerTelemetryChannel).Initialize(configuration);
                }

                configuration.TelemetryProcessorChainBuilder.Build();
            }
        }

        /// <summary>
        ///     Read from configuration
        ///     Config.json will look like this:
        ///     "ApplicationInsights": {
        ///     "InstrumentationKey": "11111111-2222-3333-4444-555555555555"
        ///     "TelemetryChannel": {
        ///     EndpointAddress: "http://dc.services.visualstudio.com/v2/track",
        ///     DeveloperMode: true
        ///     }
        ///     }
        ///     Values can also be read from environment variables to support azure web sites configuration:
        /// </summary>
        /// <param name="config">Configuration to read variables from.</param>
        /// <param name="telemetryConfiguration">Telemetry configuration to populate</param>
        private static void AddTelemetryConfiguration(IConfiguration config, TelemetryConfiguration telemetryConfiguration)
        {
            string instrumentationKey = config[INSTRUMENTATION_KEY_FOR_WEB_SITES];
            if (string.IsNullOrWhiteSpace(instrumentationKey))
            {
                instrumentationKey = config[INSTRUMENTATION_KEY_FROM_CONFIG];
            }

            if (!string.IsNullOrWhiteSpace(instrumentationKey))
            {
                telemetryConfiguration.InstrumentationKey = instrumentationKey;
            }

            string developerModeValue = config[DEVELOPER_MODE_FOR_WEB_SITES];
            if (string.IsNullOrWhiteSpace(developerModeValue))
            {
                developerModeValue = config[DEVELOPER_MODE_FROM_CONFIG];
            }

            if (!string.IsNullOrWhiteSpace(developerModeValue))
            {
                bool developerMode;
                if (bool.TryParse(developerModeValue, out developerMode))
                {
                    telemetryConfiguration.TelemetryChannel.DeveloperMode = developerMode;
                }
            }

            string endpointAddress = config[ENDPOINT_ADDRESS_FOR_WEB_SITES];
            if (string.IsNullOrWhiteSpace(endpointAddress))
            {
                endpointAddress = config[ENDPOINT_ADDRESS_FROM_CONFIG];
            }

            if (!string.IsNullOrWhiteSpace(endpointAddress))
            {
                telemetryConfiguration.TelemetryChannel.EndpointAddress = endpointAddress;
            }
        }

        private static void InitializeModulesWithConfiguration(IServiceProvider serviceProvider, TelemetryConfiguration configuration)
        {
            IEnumerable<ITelemetryModule> services = serviceProvider.GetService<IEnumerable<ITelemetryModule>>();
            foreach (ITelemetryModule service in services)
            {
                service.Initialize(configuration);
            }
        }
    }
}