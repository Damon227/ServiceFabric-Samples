// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : GodLog.Foundation.Logging.ServiceFabric
// File             : ETWLogger.cs
// Created          : 2017-02-14  16:17
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Fabric;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GodLog.Foundation.Logging.ServiceFabric
{
    public class ETWLogger : EventSource, ILogger
    {
        private static readonly string s_loglevelPadding = ": ";
        private static readonly string s_messagePadding;
        private static readonly string s_newLineWithMessagePadding;
        private ETWLoggerOptions _options;
        private ServiceContext _serviceContext;
        private Func<string, LogLevel, bool> _filter;

        static ETWLogger()
        {
            string logLevelString = GetLogLevelString(LogLevel.Information);
            s_messagePadding = new string(' ', logLevelString.Length + s_loglevelPadding.Length);
            s_newLineWithMessagePadding = Environment.NewLine + s_messagePadding;
        }

        public ETWLogger(ServiceContext serviceContext, string name, Func<string, LogLevel, bool> filter, IOptions<ETWLoggerOptions> options)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            ServiceContext = serviceContext;
            Name = name;
            Filter = filter ?? ((category, logLevel) => true);
            _options = options.Value;
        }

        public ETWLoggerOptions Options
        {
            get { return _options; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException(nameof(value));
                }

                _options = value;
            }
        }

        public ServiceContext ServiceContext
        {
            get { return _serviceContext; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _serviceContext = value;
            }
        }

        public Func<string, LogLevel, bool> Filter
        {
            get { return _filter; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _filter = value;
            }
        }

        public new string Name { get; }

        #region ILogger Members

        /// <summary>
        ///     Writes a log entry.
        /// </summary>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a <c>string</c> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            string message = formatter(state, exception);

            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                WriteMessage(logLevel, Name, eventId.Id, message, exception);
            }
        }

        /// <summary>
        ///     Checks if the given <paramref name="logLevel" /> is enabled.
        /// </summary>
        /// <param name="logLevel">level to be checked.</param>
        /// <returns>
        ///     <c>true</c> if enabled.
        /// </returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return Filter(Name, logLevel);
        }


        /// <summary>
        ///     Begins a logical operation scope.
        /// </summary>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>
        ///     An IDisposable that ends the logical operation scope on dispose.
        /// </returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return new ETWLoggerScope();
        }

        #endregion

        /// <summary>
        ///     获取 <see cref="System.Exception" /> 的详细错误信息。
        /// </summary>
        public static string GetExceptionString(Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            CreateExceptionString(sb, exception, "  ");
            return sb.ToString();
        }

        protected virtual void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception exception)
        {
            string logLevelString = string.Empty;
            string logIdentifier = string.Empty;
            string exceptionText = string.Empty;
            bool printLog = false;

            // Example:
            // INFO: ConsoleApp.Program[10]
            //       Request received
            if (!string.IsNullOrEmpty(message))
            {
                logLevelString = GetLogLevelString(logLevel);
                // category and event id
                logIdentifier = s_loglevelPadding + logName + " [" + eventId + "]";

                // message
                message = s_messagePadding + ReplaceMessageNewLinesWithPadding(message);
                printLog = true;
            }

            // Example:
            // System.InvalidOperationException
            //    at Namespace.Class.Function() in File:line X
            if (exception != null)
            {
                // exception message
                exceptionText = s_messagePadding + ReplaceMessageNewLinesWithPadding(GetExceptionString(exception));
                printLog = true;
            }

            if (printLog)
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(logLevelString))
                {
                    sb.Append(logLevelString);
                }

                if (!string.IsNullOrEmpty(logIdentifier))
                {
                    sb.AppendLine(logIdentifier);
                }

                if (!string.IsNullOrEmpty(message))
                {
                    sb.AppendLine(message);
                }

                if (!string.IsNullOrEmpty(exceptionText))
                {
                    sb.AppendLine(exceptionText);
                }

                WriteEvent(
                    eventId,
                    ServiceContext.ServiceName.ToString(),
                    ServiceContext.ServiceTypeName,
                    ServiceContext.ReplicaOrInstanceId,
                    ServiceContext.PartitionId,
                    ServiceContext.CodePackageActivationContext.ApplicationName,
                    ServiceContext.CodePackageActivationContext.ApplicationTypeName,
                    ServiceContext.NodeContext.NodeName,
                    message);
            }
        }

        private static void CreateExceptionString(StringBuilder sb, Exception exception, string indent)
        {
            sb = sb ?? new StringBuilder();

            while (true)
            {
                if (exception == null)
                {
                    throw new ArgumentNullException(nameof(exception));
                }

                if (indent == null)
                {
                    indent = string.Empty;
                }
                else if (indent.Length > 0)
                {
                    sb.Append($"{indent}Inner ");
                }

                sb.AppendLine("Exception(s) Found:");
                sb.AppendLine($"{indent}Type: {exception.GetType().FullName}");
                sb.AppendLine($"{indent}Message: {exception.Message}");
                sb.AppendLine($"{indent}Source: {exception.Source}");
                sb.AppendLine($"{indent}DataJson: {JsonConvert.SerializeObject(exception.Data)}");
                sb.AppendLine($"{indent}Stacktrace:");
                sb.AppendLine($"{exception.StackTrace}");

                if (exception is ReflectionTypeLoadException)
                {
                    Exception[] loaderExceptions = ((ReflectionTypeLoadException)exception).LoaderExceptions;
                    if (loaderExceptions.Length == 0)
                    {
                        sb.AppendLine($"{indent}No LoaderExceptions found.");
                    }
                    else
                    {
                        foreach (Exception e in loaderExceptions)
                        {
                            CreateExceptionString(sb, e, indent + "  ");
                        }
                    }
                }
                else if (exception is AggregateException)
                {
                    ReadOnlyCollection<Exception> innerExceptions = ((AggregateException)exception).InnerExceptions;
                    if (innerExceptions.Count == 0)
                    {
                        sb.AppendLine($"{indent}No InnerExceptions found.");
                    }
                    else
                    {
                        foreach (Exception e in innerExceptions)
                        {
                            CreateExceptionString(sb, e, indent + "  ");
                        }
                    }
                }
                else if (exception.InnerException != null)
                {
                    sb.AppendLine();
                    exception = exception.InnerException;
                    indent = indent + "  ";
                    continue;
                }
                break;
            }
        }

        private static string ReplaceMessageNewLinesWithPadding(string message)
        {
            return message.Replace(Environment.NewLine, s_newLineWithMessagePadding);
        }

        private static string GetLogLevelString(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return "trce";
                case LogLevel.Debug:
                    return "dbug";
                case LogLevel.Information:
                    return "info";
                case LogLevel.Warning:
                    return "warn";
                case LogLevel.Error:
                    return "fail";
                case LogLevel.Critical:
                    return "crit";
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }
    }

    public class ETWLoggerScope : IDisposable
    {
        #region IDisposable Members

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion
    }
}