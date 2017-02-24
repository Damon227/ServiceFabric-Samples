// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : GodLog.Foundation.Logging.ServiceFabric
// File             : ETWLoggerFactoryExtensions.cs
// Created          : 2017-02-15  10:35
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Fabric;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GodLog.Foundation.Logging.ServiceFabric
{
    public static class ETWLoggerFactoryExtensions
    {
        /// <summary>
        ///     Add ETWLoggerProvider to ILoggerFactory.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="serviceContext">Service Fabric's ServiceContext</param>
        /// <param name="filter">The filter.</param>
        /// <param name="options">The options.</param>
        public static ILoggerFactory AddETW(this ILoggerFactory loggerFactory, ServiceContext serviceContext, Func<string, LogLevel, bool> filter, IOptions<ETWLoggerOptions> options)
        {
            loggerFactory.AddProvider(new ETWLoggerProvider(serviceContext, filter, () => null, options));
            return loggerFactory;
        }
    }
}