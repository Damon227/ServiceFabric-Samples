// ***********************************************************************
// Solution         : GodLog
// Project          : GodLog.Foundation.Logging
// File             : LoggerFactoryExtensions.cs
// Created          : 2017-02-14  14:58
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

namespace GodLog.Foundation.Logging
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
    }
}