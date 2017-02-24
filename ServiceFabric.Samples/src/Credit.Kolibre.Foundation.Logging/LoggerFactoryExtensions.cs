// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.Logging
// File             : LoggerFactoryExtensions.cs
// Created          : 2016-10-11  2:38 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.Logging
{
    /// <summary>
    ///     Extension methods for the <see cref="ILoggerFactory" /> class.
    /// </summary>
    public static class LoggerFactoryExtensions
    {
        /// <summary>
        ///     Adds a console logger that is enabled as defined by the configuration.
        /// </summary>
        /// <param name="factory">The extension method argument.</param>
        /// <param name="operationIdAccessor">The operation id accessor for <see cref="ConsoleLogger" />.</param>
        /// <param name="serviceProvider">The service provider that provide necessary service.</param>
        /// <returns>The <see cref="ILoggerFactory" /> so that additional calls can be chained.</returns>
        public static ILoggerFactory AddConsole(this ILoggerFactory factory, Func<string> operationIdAccessor, IServiceProvider serviceProvider)
        {
            IConfiguration config = serviceProvider.GetService<IConfiguration>();

            IOptions<ConsoleLoggerOptions> options =
                serviceProvider.GetService<IOptions<ConsoleLoggerOptions>>() ??
                new ConsoleLoggerOptions
                {
                    Colored = true,
                    MinLevel = LogLevel.Trace
                };

            return config == null ? AddConsole(factory, LogLevel.Trace, operationIdAccessor, options) :
                AddConsole(factory, config, operationIdAccessor, options);
        }

        /// <summary>
        ///     Adds a console logger that is enabled as defined by the configuration.
        /// </summary>
        /// <param name="factory">The extension method argument.</param>
        /// <param name="operationIdAccessor">The operation id accessor for <see cref="ConsoleLogger" />.</param>
        /// <param name="options">The options of <see cref="ConsoleLogger" />.</param>
        /// <returns>The <see cref="ILoggerFactory" /> so that additional calls can be chained.</returns>
        public static ILoggerFactory AddConsole(this ILoggerFactory factory, Func<string> operationIdAccessor, IOptions<ConsoleLoggerOptions> options)
        {
            return AddConsole(factory, LogLevel.Trace, operationIdAccessor, options);
        }

        /// <summary>
        ///     Adds a console logger that is enabled as defined by the configuration.
        /// </summary>
        /// <param name="factory">The extension method argument.</param>
        /// <param name="minLevel">The minimum <see cref="LogLevel" /> to be logged.</param>
        /// <param name="operationIdAccessor">The operation id accessor for <see cref="ConsoleLogger" />.</param>
        /// <param name="options">The options of <see cref="ConsoleLogger" />.</param>
        /// <returns>The <see cref="ILoggerFactory" /> so that additional calls can be chained.</returns>
        public static ILoggerFactory AddConsole(this ILoggerFactory factory, LogLevel minLevel, Func<string> operationIdAccessor, IOptions<ConsoleLoggerOptions> options)
        {
            return AddConsole(factory, (_, logLevel) => logLevel != LogLevel.None && logLevel >= minLevel, operationIdAccessor, options);
        }

        /// <summary>
        ///     Adds a console logger that is enabled as defined by the configuration.
        /// </summary>
        /// <param name="factory">The extension method argument.</param>
        /// <param name="filter">The events filter based on the log level.</param>
        /// <param name="operationIdAccessor">The operation id accessor for <see cref="ConsoleLogger" />.</param>
        /// <param name="options">The options of <see cref="ConsoleLogger" />.</param>
        /// <returns>The <see cref="ILoggerFactory" /> so that additional calls can be chained.</returns>
        public static ILoggerFactory AddConsole(this ILoggerFactory factory, Func<string, LogLevel, bool> filter, Func<string> operationIdAccessor, IOptions<ConsoleLoggerOptions> options)
        {
            factory.AddProvider(new ConsoleLoggerProvider(filter, operationIdAccessor, options));
            return factory;
        }

        /// <summary>
        ///     Adds a console logger that is enabled as defined by the configuration.
        /// </summary>
        /// <param name="factory">The extension method argument.</param>
        /// <param name="configuration">The configuration used to filter events based on the log level.</param>
        /// <param name="operationIdAccessor">The operation id accessor for <see cref="ConsoleLogger" />.</param>
        /// <param name="options">The options of <see cref="ConsoleLogger" />.</param>
        /// <returns>The <see cref="ILoggerFactory" /> so that additional calls can be chained.</returns>
        public static ILoggerFactory AddConsole(this ILoggerFactory factory, IConfiguration configuration, Func<string> operationIdAccessor, IOptions<ConsoleLoggerOptions> options)
        {
            factory.AddProvider(new ConsoleLoggerProvider(configuration, operationIdAccessor, options));
            return factory;
        }

        /// <summary>
        ///     Adds a trace logger that is enabled as defined by the configuration.
        /// </summary>
        /// <param name="factory">The extension method argument.</param>
        /// <param name="serviceProvider">The service provider that provide necessary service.</param>
        /// <param name="operationIdAccessor">The operation id accessor for <see cref="TraceLogger" />.</param>
        /// <returns>The <see cref="ILoggerFactory" /> so that additional calls can be chained.</returns>
        public static ILoggerFactory AddTrace(this ILoggerFactory factory, IServiceProvider serviceProvider, Func<string> operationIdAccessor = null)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            IConfiguration config = serviceProvider.GetService<IConfiguration>();

            IOptions<TraceLoggerOptions> options =
                serviceProvider.GetService<IOptions<TraceLoggerOptions>>() ??
                new TraceLoggerOptions
                {
                    MinLevel = LogLevel.Trace
                };

            return config == null ? AddTrace(factory, LogLevel.Trace, operationIdAccessor, options) :
                AddTrace(factory, config, operationIdAccessor, options);
        }

        /// <summary>
        ///     Adds a trace logger that is enabled as defined by the configuration.
        /// </summary>
        /// <param name="factory">The extension method argument.</param>
        /// <param name="operationIdAccessor">The operation id accessor for <see cref="TraceLogger" />.</param>
        /// <param name="options">The options of <see cref="TraceLogger" />.</param>
        /// <returns>The <see cref="ILoggerFactory" /> so that additional calls can be chained.</returns>
        public static ILoggerFactory AddTrace(this ILoggerFactory factory, Func<string> operationIdAccessor = null, IOptions<TraceLoggerOptions> options = null)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            return AddTrace(factory, LogLevel.Trace, operationIdAccessor, options);
        }

        /// <summary>
        ///     Adds a trace logger that is enabled as defined by the configuration.
        /// </summary>
        /// <param name="factory">The extension method argument.</param>
        /// <param name="minLevel">The minimum <see cref="LogLevel" /> to be logged.</param>
        /// <param name="operationIdAccessor">The operation id accessor for <see cref="TraceLogger" />.</param>
        /// <param name="options">The options of <see cref="TraceLogger" />.</param>
        /// <returns>The <see cref="ILoggerFactory" /> so that additional calls can be chained.</returns>
        public static ILoggerFactory AddTrace(this ILoggerFactory factory, LogLevel minLevel, Func<string> operationIdAccessor = null, IOptions<TraceLoggerOptions> options = null)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            return AddTrace(factory, (_, logLevel) => logLevel != LogLevel.None && logLevel >= minLevel, operationIdAccessor, options);
        }

        /// <summary>
        ///     Adds a trace logger that is enabled as defined by the configuration.
        /// </summary>
        /// <param name="factory">The extension method argument.</param>
        /// <param name="filter">The events filter based on the log level.</param>
        /// <param name="operationIdAccessor">The operation id accessor for <see cref="TraceLogger" />.</param>
        /// <param name="options">The options of <see cref="TraceLogger" />.</param>
        /// <returns>The <see cref="ILoggerFactory" /> so that additional calls can be chained.</returns>
        public static ILoggerFactory AddTrace(this ILoggerFactory factory, Func<string, LogLevel, bool> filter, Func<string> operationIdAccessor = null, IOptions<TraceLoggerOptions> options = null)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (operationIdAccessor == null)
            {
                operationIdAccessor = () => string.Empty;
            }

            factory.AddProvider(new TraceLoggerProvider(filter, operationIdAccessor, options));
            return factory;
        }

        /// <summary>
        ///     Adds a trace logger that is enabled as defined by the configuration.
        /// </summary>
        /// <param name="factory">The extension method argument.</param>
        /// <param name="configuration">The configuration used to filter events based on the log level.</param>
        /// <param name="operationIdAccessor">The operation id accessor for <see cref="TraceLogger" />.</param>
        /// <param name="options">The options of <see cref="TraceLogger" />.</param>
        /// <returns>The <see cref="ILoggerFactory" /> so that additional calls can be chained.</returns>
        public static ILoggerFactory AddTrace(this ILoggerFactory factory, IConfiguration configuration, Func<string> operationIdAccessor = null, IOptions<TraceLoggerOptions> options = null)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (operationIdAccessor == null)
            {
                operationIdAccessor = () => string.Empty;
            }

            factory.AddProvider(new TraceLoggerProvider(configuration, operationIdAccessor, options));
            return factory;
        }
    }
}