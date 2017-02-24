// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.Logging
// File             : TraceLoggerProvider.cs
// Created          : 2016-10-11  2:38 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.Logging
{
    public class TraceLoggerProvider : LoggerProvider
    {
        private static readonly IOptions<TraceLoggerOptions> s_defaultOptions =
            new TraceLoggerOptions { MinLevel = LogLevel.Trace };

        private IOptions<TraceLoggerOptions> _options;

        public TraceLoggerProvider(Func<string, LogLevel, bool> filter, Func<string> operationIdAccessor, IOptions<TraceLoggerOptions> options) : base(filter, operationIdAccessor)
        {
            Options = options;
        }

        public TraceLoggerProvider(IConfiguration configuration, Func<string> operationIdAccessor, IOptions<TraceLoggerOptions> options) : base(configuration, operationIdAccessor)
        {
            Options = options;
        }

        protected override string LogLevelSettionsSectionKey
        {
            get { return "Logging: TraceLogger: LogLevel"; }
        }

        public IOptions<TraceLoggerOptions> Options
        {
            get { return _options; }
            set { _options = value ?? s_defaultOptions; }
        }

        public override ILogger CreateLogger(string name)
        {
            return new TraceLogger(name, _filter ?? GetFilter(), OperationIdAccessor, Options);
        }
    }
}