// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Session
// File             : LoggingExtensions.cs
// Created          : 2016-08-23  10:04 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Credit.Kolibre.Foundation.Logging;
using Microsoft.Extensions.Logging;

namespace Credit.Kolibre.Foundation.ServiceFabric.Seesion
{
    internal static class LoggingExtensions
    {
        private static readonly Action<ILogger, string, Exception> s_errorCommitTheSession;
        private static readonly Action<ILogger, string, Exception> s_initSession;
        private static readonly Action<ILogger, string, Exception> s_sessionMissing;
        private static readonly Action<ILogger, string, int, Exception> s_sessionLoaded;
        private static readonly Action<ILogger, string, int, Exception> s_sessionStored;
        private static readonly Action<ILogger, string, Exception> s_errorSessionLoadOrInit;
        private static readonly Action<ILogger, string, string, Exception> s_sessionIdMissmatch;
        private static readonly Action<ILogger, string, string, string, Exception> s_sessionUserIdMissmatch;

        static LoggingExtensions()
        {
            s_errorCommitTheSession = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_SESSION_ERROR_COMMIT,
                logLevel: LogLevel.Warning,
                formatString: "ERROR - commit the session, Key:{sessionKey}");
            s_initSession = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_SESSION_INIT,
                logLevel: LogLevel.Trace,
                formatString: "Ssession inited, Key:{sessionKey}");
            s_sessionMissing = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_SESSION_SESSION_MISSING,
                logLevel: LogLevel.Warning,
                formatString: "Session missing, Key:{sessionKey}");
            s_sessionLoaded = LoggerMessage.Define<string, int>(
                eventId: EventCode.CREDIT_KOLIBRE_SESSION_SESSION_LOADED,
                logLevel: LogLevel.Trace,
                formatString: "Session loaded, Key:{sessionKey}, Count:{count}");
            s_sessionStored = LoggerMessage.Define<string, int>(
                eventId: EventCode.CREDIT_KOLIBRE_SESSION_SESSION_STORED,
                logLevel: LogLevel.Trace,
                formatString: "Session stored, Key:{sessionKey}, Count:{count}");
            s_errorSessionLoadOrInit = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_SESSION_ERROR_SESSION_LOAD_OR_INIT,
                logLevel: LogLevel.Warning,
                formatString: "ERROR - session load or init exception, Key:{sessionKey}");
            s_sessionIdMissmatch = LoggerMessage.Define<string, string>(
                eventId: EventCode.CREDIT_KOLIBRE_SESSION_ID_MISSMATCH,
                logLevel: LogLevel.Warning,
                formatString: "Session id missmatch, Key:{sessionKey}, SessionId:{sessionId}");
            s_sessionUserIdMissmatch = LoggerMessage.Define<string, string, string>(
                eventId: EventCode.CREDIT_KOLIBRE_SESSION_USER_ID_MISSMATCH,
                logLevel: LogLevel.Warning,
                formatString: "Session user id missmatch, Key:{sessionKey}, UserId:{userId}, SessionUserId:{sessionUserId}");
        }

        public static void LogErrorCommitTheSession(this ILogger logger, string sessionKey, Exception exception)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                s_errorCommitTheSession(logger, sessionKey, exception);
            }
        }

        public static void LogErrorSessionLoadOrInit(this ILogger logger, string sessionKey, Exception exception)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                s_errorSessionLoadOrInit(logger, sessionKey, exception);
            }
        }

        public static void LogSessionLoaded(this ILogger logger, string sessionKey, int count)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_sessionLoaded(logger, sessionKey, count, null);
            }
        }

        public static void LogSessionMissing(this ILogger logger, string sessionKey)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                s_sessionMissing(logger, sessionKey, null);
            }
        }

        public static void LogSessionStored(this ILogger logger, string sessionKey, int count)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_sessionStored(logger, sessionKey, count, null);
            }
        }

        internal static void LogInitSession(this ILogger logger, string sessionKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_initSession(logger, sessionKey, null);
            }
        }

        internal static void LogSessionIdMissmatch(this ILogger logger, string sessionKey, string sessionId)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                s_sessionIdMissmatch(logger, sessionKey, sessionId, null);
            }
        }

        internal static void LogSessionUserIdMissmatch(this ILogger logger, string sessionKey, string userId, string sourceUserId)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                s_sessionUserIdMissmatch(logger, sessionKey, userId, sourceUserId, null);
            }
        }
    }
}