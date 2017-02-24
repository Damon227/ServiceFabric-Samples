// ***********************************************************************
// Solution         : GodLog
// Project          : GodLog.Foundation.Logging
// File             : ConsoleLogger.cs
// Created          : 2017-02-14  15:00
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GodLog.Foundation.Logging
{
    public class ConsoleLogger : Logger
    {
        private static readonly object s_lock = new object();
        private IConsole _console;
        private ConsoleLoggerOptions _options;

        public ConsoleLogger(string name, Func<string, LogLevel, bool> filter, Func<string> operationIdAccessor, IOptions<ConsoleLoggerOptions> options) : base(name, filter, operationIdAccessor)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console = new WindowsLogConsole();
            }
            else
            {
                Console = new AnsiLogConsole(new AnsiSystemConsole());
            }

            Options = options.Value;
        }

        public IConsole Console
        {
            get { return _console; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _console = value;
            }
        }

        public ConsoleLoggerOptions Options
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

        public override bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None && logLevel >= _options.MinLevel && base.IsEnabled(logLevel);
        }

        /// <summary>
        ///     获取 <see cref="System.Exception" /> 的详细错误信息。
        /// </summary>
        public static string GetExceptionString(Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            CreateExceptionString(sb, exception, "  ");
            return sb.ToString();
        }

        protected override void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception exception)
        {
            ConsoleColors logLevelColors = default(ConsoleColors);
            string logLevelString = string.Empty;
            string logIdentifier = string.Empty;
            string exceptionText = string.Empty;
            bool printLog = false;

            // Example:
            // INFO: System ConsoleApp.Program [10] 
            //       Request received
            if (!string.IsNullOrEmpty(message))
            {
                logLevelColors = GetLogLevelConsoleColors(logLevel);
                logLevelString = GetLogLevelString(logLevel);
                // category and event id
                logIdentifier = s_loglevelPadding + logName + " [" + eventId + "] " + OperationIdAccessor.Invoke() + " " + DateTime.UtcNow.ToLocalTime().ToString("O");

                // message
                message = s_messagePadding + ReplaceMessageNewLinesAndTab(message);
                printLog = true;
            }

            // Example:
            // System.InvalidOperationException
            //    at Namespace.Class.Function() in File:line X
            if (exception != null)
            {
                // exception message
                exceptionText = s_messagePadding + ReplaceMessageNewLinesAndTab(GetExceptionString(exception));
                printLog = true;
            }

            if (printLog)
            {
                lock (s_lock)
                {
                    if (!string.IsNullOrEmpty(logLevelString))
                    {
                        // log level string
                        Console.Write(
                            logLevelString,
                            logLevelColors.Background,
                            logLevelColors.Foreground);
                    }

                    // use default colors from here on
                    if (!string.IsNullOrEmpty(logIdentifier))
                    {
                        Console.WriteLine(logIdentifier, null, null);
                    }
                    if (!string.IsNullOrEmpty(message))
                    {
                        Console.WriteLine(message, null, null);
                    }
                    if (!string.IsNullOrEmpty(exceptionText))
                    {
                        Console.WriteLine(exceptionText, null, null);
                    }

                    // In case of AnsiLogConsole, the messages are not yet written to the console,
                    // this would flush them instead.
                    Console.Flush();
                }
            }
        }

        private static ConsoleColors GetLogLevelConsoleColors(LogLevel logLevel)
        {
            // We must explicitly set the background color if we are setting the foreground color,
            // since just setting one can look bad on the users console.
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return new ConsoleColors(ConsoleColor.Magenta, null);
                case LogLevel.Error:
                    return new ConsoleColors(ConsoleColor.Red, null);
                case LogLevel.Warning:
                    return new ConsoleColors(ConsoleColor.Yellow, null);
                case LogLevel.Information:
                    return new ConsoleColors(ConsoleColor.White, null);
                case LogLevel.Debug:
                    return new ConsoleColors(ConsoleColor.Gray, null);
                case LogLevel.Trace:
                    return new ConsoleColors(ConsoleColor.Gray, null);
                default:
                    return new ConsoleColors(null, null);
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

        #region Nested type: ${0}

        private sealed class AnsiSystemConsole : IAnsiSystemConsole
        {
            #region IAnsiSystemConsole Members

            public void Write(string message)
            {
                System.Console.Write(message);
            }

            public void WriteLine(string message)
            {
                System.Console.WriteLine(message);
            }

            #endregion
        }

        private struct ConsoleColors
        {
            public ConsoleColors(ConsoleColor? foreground, ConsoleColor? background)
            {
                Foreground = foreground;
                Background = background;
            }

            public ConsoleColor? Foreground { get; }

            public ConsoleColor? Background { get; }
        }

        #endregion
    }
}