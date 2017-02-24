// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.ServiceFabric.Logging
// File             : ETWLoggerProvider.cs
// Created          : 2017-02-15  16:38
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Fabric;
using Credit.Kolibre.Foundation.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.ServiceFabric.Logging
{
    public class ETWLoggerProvider : LoggerProvider
    {
        private static readonly IOptions<ETWLoggerOptions> s_defaultOptions = new ETWLoggerOptions { MinLevel = LogLevel.Trace };
        private readonly ServiceContext _serviceContext;
        private IOptions<ETWLoggerOptions> _options;

        public ETWLoggerProvider(
            ServiceContext serviceContext,
            Func<string, LogLevel, bool> filter,
            Func<string> operationIdAccessor,
            IOptions<ETWLoggerOptions> options)
            : base(filter, null)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            _serviceContext = serviceContext;
            _options = options;
        }

        public ETWLoggerProvider(
            ServiceContext serviceContext,
            Func<string, LogLevel, bool> filter,
            Func<string> operationIdAccessor,
            IConfiguration configuration,
            IOptions<ETWLoggerOptions> options
            )
            : base(configuration, null)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            _serviceContext = serviceContext;
            _options = options;
        }

        public IOptions<ETWLoggerOptions> Options
        {
            get { return _options ?? s_defaultOptions; }
            set { _options = value; }
        }

        /// <summary>
        ///     Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns />
        public override ILogger CreateLogger(string categoryName)
        {
            return new ETWLogger(_serviceContext, categoryName, GetFilter(), Options);
        }
    }
}