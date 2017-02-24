// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : GodLog.Foundation.Logging.ServiceFabric
// File             : ETWLoggerProvider.cs
// Created          : 2017-02-14  16:18
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Fabric;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GodLog.Foundation.Logging.ServiceFabric
{
    public class ETWLoggerProvider : LoggerProvider
    {
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
            get { return _options; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _options = value;
            }
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