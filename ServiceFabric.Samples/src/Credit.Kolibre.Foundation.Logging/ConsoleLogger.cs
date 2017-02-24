// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.Logging
// File             : ConsoleLogger.cs
// Created          : 2016-10-11  2:38 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Runtime.InteropServices;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console.Internal;
using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.Logging
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
            if (message.IsNotNullOrEmpty())
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
                exceptionText = s_messagePadding + ReplaceMessageNewLinesAndTab(exception.GetExceptionString());
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

        #region Nested type: AnsiSystemConsole

        private sealed class AnsiSystemConsole : IAnsiSystemConsole
        {
            #region IAnsiSystemConsole Members

            public void Write(string message)
            {
                System.Console.Write(message);
            }

            #endregion

            #region IAnsiSystemConsole Members

            public void WriteLine(string message)
            {
                System.Console.WriteLine(message);
            }

            #endregion
        }

        #endregion

        #region Nested type: ConsoleColors

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