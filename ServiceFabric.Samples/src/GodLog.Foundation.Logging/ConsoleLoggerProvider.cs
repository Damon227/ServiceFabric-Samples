// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : GodLog.Foundation.Logging
// File             : ConsoleLoggerProvider.cs
// Created          : 2017-02-14  15:29
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GodLog.Foundation.Logging
{
    public class ConsoleLoggerProvider : LoggerProvider
    {
        private static readonly IOptions<ConsoleLoggerOptions> s_defaultOptions =
            new ConsoleLoggerOptions { Colored = true, MinLevel = LogLevel.Trace };

        private IOptions<ConsoleLoggerOptions> _options;

        public ConsoleLoggerProvider(Func<string, LogLevel, bool> filter, Func<string> operationIdAccessor, IOptions<ConsoleLoggerOptions> options)
            : base(filter, operationIdAccessor)
        {
            Options = options;
        }

        public ConsoleLoggerProvider(IConfiguration configuration, Func<string> operationIdAccessor, IOptions<ConsoleLoggerOptions> options)
            : base(configuration, operationIdAccessor)
        {
            Options = options;
        }

        public IOptions<ConsoleLoggerOptions> Options
        {
            get { return _options ?? s_defaultOptions; }
            set { _options = value; }
        }

        public override ILogger CreateLogger(string name)
        {
            return new ConsoleLogger(name, _filter ?? GetFilter(), OperationIdAccessor, Options);
        }
    }
}