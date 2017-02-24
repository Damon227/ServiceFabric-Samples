// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.Logging
// File             : Logger.cs
// Created          : 2016-10-11  2:38 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.Extensions.Logging;

namespace Credit.Kolibre.Foundation.Logging
{
    public abstract class Logger : ILogger
    {
        protected static readonly string s_loglevelPadding = ": ";
        protected static readonly string s_messagePadding;
        protected static readonly string s_newLineWithMessagePadding;
        protected Func<string, LogLevel, bool> _filter;
        protected Func<string> _operationIdAccessor;

        static Logger()
        {
            string logLevelString = GetLogLevelString(LogLevel.Information);
            s_messagePadding = new string(' ', logLevelString.Length + s_loglevelPadding.Length);
            s_newLineWithMessagePadding = Environment.NewLine + s_messagePadding;
        }

        protected Logger(string name, Func<string, LogLevel, bool> filter, Func<string> operationIdAccessor)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Filter = filter ?? ((category, logLevel) => true);
            OperationIdAccessor = operationIdAccessor ?? (() => string.Empty);
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

        public string Name { get; }

        public Func<string> OperationIdAccessor
        {
            get { return _operationIdAccessor; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _operationIdAccessor = value;
            }
        }

        #region ILogger Members

        /// <summary>Begins a logical operation scope.</summary>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>An IDisposable that ends the logical operation scope on dispose.</returns>
        public virtual IDisposable BeginScope<TState>(TState state)
        {
            return new LoggerScope();
        }

        #endregion

        #region ILogger Members

        /// <summary>
        ///     Checks if the given <paramref name="logLevel" /> is enabled.
        /// </summary>
        /// <param name="logLevel">level to be checked.</param>
        /// <returns><c>true</c> if enabled.</returns>
        public virtual bool IsEnabled(LogLevel logLevel)
        {
            return Filter(Name, logLevel);
        }

        #endregion

        #region ILogger Members

        /// <summary>Writes a log entry.</summary>
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

            if (message.IsNotNullOrEmpty() || exception != null)
            {
                WriteMessage(logLevel, Name, eventId.Id, message, exception);
            }
        }

        #endregion

        protected static string GetLogLevelString(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return "TRCE";
                case LogLevel.Debug:
                    return "DBUG";
                case LogLevel.Information:
                    return "INFO";
                case LogLevel.Warning:
                    return "WARN";
                case LogLevel.Error:
                    return "FAIL";
                case LogLevel.Critical:
                    return "CRIT";
                default:
                    return "TRCE";
            }
        }

        protected static string ReplaceMessageNewLinesAndTab(string message)
        {
            return message.Replace("\r", "\\r\\n")
                .Replace("\n", "\\r\\n")
                .Replace("\r\n", "\\r\\n")
                .Replace("\t", "  ");
        }

        protected static string ReplaceMessageNewLinesWithPadding(string message)
        {
            return message.Replace(Environment.NewLine, s_newLineWithMessagePadding);
        }

        protected abstract void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception exception);
    }

    public class LoggerScope : IDisposable
    {
        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}