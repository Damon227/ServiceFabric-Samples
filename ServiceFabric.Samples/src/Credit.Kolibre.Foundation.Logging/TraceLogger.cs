// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.Logging
// File             : TraceLogger.cs
// Created          : 2016-10-11  2:38 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics;
using System.Text;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.Logging
{
    public class TraceLogger : Logger
    {
        private static readonly TraceLoggerOptions s_defaultOptions = new TraceLoggerOptions { MinLevel = LogLevel.Trace };
        private TraceLoggerOptions _options;

        protected internal TraceLogger(string name, Func<string, LogLevel, bool> filter, Func<string> operationIdAccessor, IOptions<TraceLoggerOptions> options) : base(name, filter, operationIdAccessor)
        {
            Options = options?.Value;
        }

        public TraceLoggerOptions Options
        {
            get { return _options; }
            set { _options = value ?? s_defaultOptions; }
        }

        public override bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None && logLevel >= _options.MinLevel && base.IsEnabled(logLevel);
        }

        protected override void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception exception)
        {
            string exceptionText = string.Empty;
            bool printLog = false;

            DateTime now = DateTime.UtcNow;

            string logLevelString = GetLogLevelString(logLevel);
            string logIdentifier = $"{logName}\t{eventId}\t{OperationIdAccessor.Invoke()}\t{Guid.NewGuid().ToGuidString()}\t{now:O}\t{now.ToChinaStandardTime():O}";

            if (message.IsNotNullOrEmpty())
            {
                // message
                message = ReplaceMessageNewLinesAndTab(message);
                printLog = true;
            }

            if (exception != null)
            {
                // exception message
                exceptionText = ReplaceMessageNewLinesAndTab(exception.GetExceptionString());
                printLog = true;
            }

            if (printLog)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(logLevelString);

                if (logIdentifier.IsNotNullOrEmpty())
                {
                    sb.Append("\t");
                    sb.Append(logIdentifier);
                }

                if (message.IsNotNullOrEmpty())
                {
                    sb.Append("\t");
                    sb.Append(message);
                }

                if (exceptionText.IsNotNullOrEmpty())
                {
                    sb.Append("\t");
                    sb.Append(exceptionText);
                }

                TraceMessage(logLevel, sb.ToString());
            }
        }

        private static void TraceMessage(LogLevel logLevel, string message)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    Trace.WriteLine(message);
                    break;
                case LogLevel.Debug:
                    Trace.WriteLine(message);
                    break;
                case LogLevel.Information:
                    Trace.TraceInformation(message);
                    break;
                case LogLevel.Warning:
                    Trace.TraceWarning(message);
                    break;
                case LogLevel.Error:
                    Trace.TraceError(message);
                    break;
                case LogLevel.Critical:
                    Trace.TraceError(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }
    }
}