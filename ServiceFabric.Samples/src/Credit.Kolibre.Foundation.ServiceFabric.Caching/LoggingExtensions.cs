// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Caching
// File             : LoggingExtensions.cs
// Created          : 2016-11-26  13:07
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Credit.Kolibre.Foundation.Logging;
using Microsoft.Extensions.Logging;

namespace Credit.Kolibre.Foundation.ServiceFabric.Caching
{
    internal static class LoggingExtensions
    {
        private static readonly Action<ILogger, string, Exception> s_jsonRedisGetBegin;
        private static readonly Action<ILogger, string, Exception> s_jsonRedisGetEnd;
        private static readonly Action<ILogger, string, Exception> s_jsonRedisSetBegin;
        private static readonly Action<ILogger, string, Exception> s_jsonRedisSetEnd;
        private static readonly Action<ILogger, string, Exception> s_jsonRedisRefreshBegin;
        private static readonly Action<ILogger, string, Exception> s_jsonRedisRefreshEnd;
        private static readonly Action<ILogger, string, Exception> s_jsonRedisRemoveBegin;
        private static readonly Action<ILogger, string, Exception> s_jsonRedisRemoveEnd;

        private static readonly Action<ILogger, string, Exception> s_hashRedisGetBegin;
        private static readonly Action<ILogger, string, Exception> s_hashRedisGetEnd;
        private static readonly Action<ILogger, string, Exception> s_hashRedisSetBegin;
        private static readonly Action<ILogger, string, Exception> s_hashRedisSetEnd;
        private static readonly Action<ILogger, string, Exception> s_hashRedisRefreshBegin;
        private static readonly Action<ILogger, string, Exception> s_hashRedisRefreshEnd;
        private static readonly Action<ILogger, string, Exception> s_hashRedisRemoveBegin;
        private static readonly Action<ILogger, string, Exception> s_hashRedisRemoveEnd;

        private static readonly Action<ILogger, string, Exception> s_listRedisGetBegin;
        private static readonly Action<ILogger, string, Exception> s_listRedisGetEnd;
        private static readonly Action<ILogger, string, Exception> s_listRedisSetBegin;
        private static readonly Action<ILogger, string, Exception> s_listRedisSetEnd;
        private static readonly Action<ILogger, string, Exception> s_listRedisRefreshBegin;
        private static readonly Action<ILogger, string, Exception> s_listRedisRefreshEnd;
        private static readonly Action<ILogger, string, Exception> s_listRedisRemoveBegin;
        private static readonly Action<ILogger, string, Exception> s_listRedisRemoveEnd;

        static LoggingExtensions()
        {
            s_jsonRedisGetBegin = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_JSON_REDIS_GET_BEGIN,
                logLevel: LogLevel.Trace,
                formatString: "BEGIN - get value from redis, Key:{cacheKey}");
            s_jsonRedisGetEnd = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_JSON_REDIS_GET_END,
                logLevel: LogLevel.Trace,
                formatString: "END - get value from redis, Key:{cacheKey}");
            s_jsonRedisSetBegin = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_JSON_REDIS_SET_BEGIN,
                logLevel: LogLevel.Trace,
                formatString: "BEGIN - set value from redis, Key:{cacheKey}");
            s_jsonRedisSetEnd = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_JSON_REDIS_SET_END,
                logLevel: LogLevel.Trace,
                formatString: "END - set value from redis, Key:{cacheKey}");
            s_jsonRedisRefreshBegin = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_JSON_REDIS_REFRESH_BEGIN,
                logLevel: LogLevel.Trace,
                formatString: "BEGIN - refresh expiry for redis item, Key:{cacheKey}");
            s_jsonRedisRefreshEnd = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_JSON_REDIS_REFRESH_END,
                logLevel: LogLevel.Trace,
                formatString: "END - refresh expiry for redis item, Key:{cacheKey}");
            s_jsonRedisRemoveBegin = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_JSON_REDIS_REMOVE_BEGIN,
                logLevel: LogLevel.Trace,
                formatString: "BEGIN - remove redis item, Key:{cacheKey}");
            s_jsonRedisRemoveEnd = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_JSON_REDIS_REMOVE_END,
                logLevel: LogLevel.Trace,
                formatString: "END - remove redis item, Key:{cacheKey}");

            s_hashRedisGetBegin = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_HASH_REDIS_GET_BEGIN,
                logLevel: LogLevel.Trace,
                formatString: "BEGIN - get hash value from redis, Key:{cacheKey}");
            s_hashRedisGetEnd = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_HASH_REDIS_GET_END,
                logLevel: LogLevel.Trace,
                formatString: "END - get hash value from redis, Key:{cacheKey}");
            s_hashRedisSetBegin = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_HASH_REDIS_SET_BEGIN,
                logLevel: LogLevel.Trace,
                formatString: "BEGIN - set hash value from redis, Key:{cacheKey}");
            s_hashRedisSetEnd = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_HASH_REDIS_SET_END,
                logLevel: LogLevel.Trace,
                formatString: "END - set hash value from redis, Key:{cacheKey}");
            s_hashRedisRefreshBegin = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_HASH_REDIS_REFRESH_BEGIN,
                logLevel: LogLevel.Trace,
                formatString: "BEGIN - refresh expiry for redis item, Key:{cacheKey}");
            s_hashRedisRefreshEnd = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_HASH_REDIS_REFRESH_END,
                logLevel: LogLevel.Trace,
                formatString: "END - refresh expiry for redis item, Key:{cacheKey}");
            s_hashRedisRemoveBegin = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_HASH_REDIS_REMOVE_BEGIN,
                logLevel: LogLevel.Trace,
                formatString: "BEGIN - remove redis item, Key:{cacheKey}");
            s_hashRedisRemoveEnd = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_HASH_REDIS_REMOVE_END,
                logLevel: LogLevel.Trace,
                formatString: "END - remove redis item, Key:{cacheKey}");

            s_listRedisGetBegin = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_LIST_REDIS_GET_BEGIN,
                logLevel: LogLevel.Trace,
                formatString: "BEGIN - get value from redis, Key:{cacheKey}");
            s_listRedisGetEnd = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_LIST_REDIS_GET_END,
                logLevel: LogLevel.Trace,
                formatString: "END - get value from redis, Key:{cacheKey}");
            s_listRedisSetBegin = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_LIST_REDIS_SET_BEGIN,
                logLevel: LogLevel.Trace,
                formatString: "BEGIN - set list value from redis, Key:{cacheKey}");
            s_listRedisSetEnd = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_LIST_REDIS_SET_END,
                logLevel: LogLevel.Trace,
                formatString: "END - set list value from redis, Key:{cacheKey}");
            s_listRedisRefreshBegin = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_LIST_REDIS_REFRESH_BEGIN,
                logLevel: LogLevel.Trace,
                formatString: "BEGIN - refresh expiry for redis item, Key:{cacheKey}");
            s_listRedisRefreshEnd = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_LIST_REDIS_REFRESH_END,
                logLevel: LogLevel.Trace,
                formatString: "END - refresh expiry for redis item, Key:{cacheKey}");
            s_listRedisRemoveBegin = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_LIST_REDIS_REMOVE_BEGIN,
                logLevel: LogLevel.Trace,
                formatString: "BEGIN - remove redis item, Key:{cacheKey}");
            s_listRedisRemoveEnd = LoggerMessage.Define<string>(
                eventId: EventCode.CREDIT_KOLIBRE_CACHE_LIST_REDIS_REMOVE_END,
                logLevel: LogLevel.Trace,
                formatString: "END - remove redis item, Key:{cacheKey}");
        }

        internal static void LogHashRedisGetBegin(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_hashRedisGetBegin(logger, cacheKey, null);
            }
        }

        internal static void LogHashRedisGetEnd(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_hashRedisGetEnd(logger, cacheKey, null);
            }
        }

        internal static void LogHashRedisGetBegin(this ILogger logger, string cacheKey, string hashKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_hashRedisGetBegin(logger, cacheKey + ", hashKey:" + hashKey, null);
            }
        }

        internal static void LogHashRedisGetEnd(this ILogger logger, string cacheKey, string hashKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_hashRedisGetBegin(logger, cacheKey + ", hashKey:" + hashKey, null);
            }
        }

        internal static void LogHashRedisRefreshBegin(this ILogger logger, string cacheKey, string hashKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_hashRedisGetBegin(logger, cacheKey + ", hashKey:" + hashKey, null);
            }
        }

        internal static void LogHashRedisRefreshEnd(this ILogger logger, string cacheKey, string hashKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_hashRedisGetBegin(logger, cacheKey + ", hashKey:" + hashKey, null);
            }
        }

        internal static void LogHashRedisRemoveBegin(this ILogger logger, string cacheKey, string hashKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_hashRedisGetBegin(logger, cacheKey + ", hashKey:" + hashKey, null);
            }
        }

        internal static void LogHashRedisRemoveEnd(this ILogger logger, string cacheKey, string hashKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_hashRedisGetBegin(logger, cacheKey + ", hashKey:" + hashKey, null);
            }
        }

        internal static void LogHashRedisSetBegin(this ILogger logger, string cacheKey, string hashKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_hashRedisSetBegin(logger, cacheKey + ", hashKey:" + hashKey, null);
            }
        }

        internal static void LogHashRedisSetEnd(this ILogger logger, string cacheKey, string hashKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_hashRedisSetEnd(logger, cacheKey + ", hashKey:" + hashKey, null);
            }
        }

        internal static void LogHashRedisRefreshBegin(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_hashRedisRefreshBegin(logger, cacheKey, null);
            }
        }

        internal static void LogHashRedisRefreshEnd(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_hashRedisRefreshEnd(logger, cacheKey, null);
            }
        }

        internal static void LogHashRedisRemoveBegin(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_hashRedisRemoveBegin(logger, cacheKey, null);
            }
        }

        internal static void LogHashRedisRemoveEnd(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_hashRedisRemoveEnd(logger, cacheKey, null);
            }
        }

        internal static void LogJsonRedisGetBegin(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_jsonRedisGetBegin(logger, cacheKey, null);
            }
        }

        internal static void LogJsonRedisGetEnd(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_jsonRedisGetEnd(logger, cacheKey, null);
            }
        }

        internal static void LogJsonRedisRefreshBegin(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_jsonRedisRefreshBegin(logger, cacheKey, null);
            }
        }

        internal static void LogJsonRedisRefreshEnd(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_jsonRedisRefreshEnd(logger, cacheKey, null);
            }
        }

        internal static void LogJsonRedisRemoveBegin(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_jsonRedisRemoveBegin(logger, cacheKey, null);
            }
        }

        internal static void LogJsonRedisRemoveEnd(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_jsonRedisRemoveEnd(logger, cacheKey, null);
            }
        }

        internal static void LogJsonRedisSetBegin(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_jsonRedisSetBegin(logger, cacheKey, null);
            }
        }

        internal static void LogJsonRedisSetEnd(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_jsonRedisSetEnd(logger, cacheKey, null);
            }
        }

        internal static void LogListRedisGetBegin(this ILogger logger, string cacheKey, string hashKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_listRedisGetBegin(logger, cacheKey + ", hashKey:" + hashKey, null);
            }
        }

        internal static void LogListRedisGetEnd(this ILogger logger, string cacheKey, string hashKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_listRedisGetBegin(logger, cacheKey + ", hashKey:" + hashKey, null);
            }
        }

        internal static void LogListRedisGetBegin(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_listRedisGetBegin(logger, cacheKey, null);
            }
        }

        internal static void LogListRedisGetEnd(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_listRedisGetEnd(logger, cacheKey, null);
            }
        }

        internal static void LogListRedisSetBegin(this ILogger logger, string cacheKey, string hashKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_listRedisSetBegin(logger, cacheKey + ", hashKey:" + hashKey, null);
            }
        }

        internal static void LogListRedisSetEnd(this ILogger logger, string cacheKey, string hashKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_listRedisSetEnd(logger, cacheKey + ", hashKey:" + hashKey, null);
            }
        }

        internal static void LogListRedisRefreshBegin(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_listRedisRefreshBegin(logger, cacheKey, null);
            }
        }

        internal static void LogListRedisRefreshEnd(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_listRedisRefreshEnd(logger, cacheKey, null);
            }
        }

        internal static void LogListRedisRemoveBegin(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_listRedisRemoveBegin(logger, cacheKey, null);
            }
        }

        internal static void LogListRedisRemoveEnd(this ILogger logger, string cacheKey)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                s_listRedisRemoveEnd(logger, cacheKey, null);
            }
        }
    }
}