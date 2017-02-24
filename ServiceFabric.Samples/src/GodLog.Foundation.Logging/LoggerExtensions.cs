// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.Logging
// File             : LoggerExtensions.cs
// Created          : 2016-10-11  2:38 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.Extensions.Logging;

namespace GodLog.Foundation.Logging
{
    public static class LoggerExtensions
    {
        public static void Critical(this ILogger logger, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Critical))
            {
                logger.LogCritical(message, args);
            }
        }

        public static void Critical(this ILogger logger, int eventId, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Critical))
            {
                logger.LogCritical(eventId, message, args);
            }
        }

        public static void Critical(this ILogger logger, int eventId, Exception exception, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Critical))
            {
                logger.LogCritical(eventId, exception, message, args);
            }
        }

        public static void Debug(this ILogger logger, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(message, args);
            }
        }

        public static void Debug(this ILogger logger, int eventId, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(eventId, message, args);
            }
        }

        public static void Debug(this ILogger logger, int eventId, Exception exception, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(eventId, exception, message, args);
            }
        }

        public static void Error(this ILogger logger, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogError(message, args);
            }
        }

        public static void Error(this ILogger logger, int eventId, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogError(eventId, message, args);
            }
        }

        public static void Error(this ILogger logger, int eventId, Exception exception, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogError(eventId, exception, message, args);
            }
        }

        public static void Info(this ILogger logger, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(message, args);
            }
        }

        public static void Info(this ILogger logger, int eventId, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(eventId, message, args);
            }
        }

        public static void Info(this ILogger logger, int eventId, Exception exception, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(eventId, exception, message, args);
            }
        }

        public static void Trace(this ILogger logger, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace(message, args);
            }
        }

        public static void Trace(this ILogger logger, int eventId, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace(eventId, message, args);
            }
        }

        public static void Trace(this ILogger logger, int eventId, Exception exception, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace(eventId, exception, message, args);
            }
        }

        public static void Warn(this ILogger logger, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                logger.LogWarning(message, args);
            }
        }

        public static void Warn(this ILogger logger, int eventId, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                logger.LogWarning(eventId, message, args);
            }
        }

        public static void Warn(this ILogger logger, int eventId, Exception exception, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                logger.LogWarning(eventId, exception, message, args);
            }
        }
    }
}