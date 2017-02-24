﻿// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : DateTimeExtensions.cs
// Created          : 2016-06-29  11:43 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Credit.Kolibre.Foundation.Static;

namespace Credit.Kolibre.Foundation.Sys
{
    /// <summary>
    ///     <see cref="System.DateTime" /> 的扩展类。
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        ///     计算指定的 <see cref="System.DateTime" /> 对象的值距离 <see cref="System.DateTime.Now" /> 的差值。
        /// </summary>
        /// <param name="dateTime">指定的 <see cref="System.DateTime" /> 对象。</param>
        /// <returns>计算出的差值。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="dateTime" /> 为 <c>null</c>。
        /// </exception>
        public static TimeSpan DurationToNow(this DateTime dateTime)
        {
            if (dateTime == null)
            {
                throw new ArgumentNullException(nameof(dateTime), SR.ArgumentNull_Generic);
            }

            return DateTime.Now - dateTime;
        }

        /// <summary>
        ///     指示原 <see cref="System.DateTime" /> 对象的值是否晚于指定的 <see cref="System.DateTime" /> 对象的值。
        /// </summary>
        /// <param name="source">原 <see cref="System.DateTime" /> 对象。</param>
        /// <param name="destination">指定的 <see cref="System.DateTime" /> 对象的。</param>
        /// <returns>如果原 <see cref="System.DateTime" /> 对象的值晚于指定的 <see cref="System.DateTime" /> 对象的值，返回 <c>true</c>；否则返回 <c>false</c>。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="source" /> 或者 <paramref name="destination" /> 为 <c>null</c>。
        /// </exception>
        public static bool IsAfter(this DateTime source, DateTime destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), SR.ArgumentNull_Generic);
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination), SR.ArgumentNull_Generic);
            }

            return source > destination;
        }

        /// <summary>
        ///     指示原 <see cref="System.DateTime" /> 对象的值是否晚于指定的 <see cref="System.DateTime" /> 对象的值，并且需要指定冗余量。
        /// </summary>
        /// <param name="source">原 <see cref="System.DateTime" /> 对象。</param>
        /// <param name="destination">指定的 <see cref="System.DateTime" /> 对象的。</param>
        /// <param name="redundancy">冗余量。</param>
        /// <returns>如果原 <see cref="System.DateTime" /> 对象的值晚于指定的 <see cref="System.DateTime" /> 对象的值，返回 <c>true</c>；否则返回 <c>false</c>。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="source" /> 或者 <paramref name="destination" /> 或者 <paramref name="redundancy" /> 为 <c>null</c>。
        /// </exception>
        public static bool IsAfter(this DateTime source, DateTime destination, TimeSpan redundancy)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), SR.ArgumentNull_Generic);
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination), SR.ArgumentNull_Generic);
            }
            if (redundancy == null)
            {
                throw new ArgumentNullException(nameof(redundancy), SR.ArgumentNull_Generic);
            }

            return source - destination > redundancy;
        }

        /// <summary>
        ///     指示原 <see cref="System.DateTime" /> 对象的值是否晚于或者等于指定的 <see cref="System.DateTime" /> 对象的值，并且需要指定冗余量。
        /// </summary>
        /// <param name="source">原 <see cref="System.DateTime" /> 对象。</param>
        /// <param name="destination">指定的 <see cref="System.DateTime" /> 对象的。</param>
        /// <param name="redundancy">冗余量。</param>
        /// <returns>指示原 <see cref="System.DateTime" /> 对象的值晚于或者等于指定的 <see cref="System.DateTime" /> 对象的值，返回 <c>true</c>；否则返回 <c>false</c>。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="source" /> 或者 <paramref name="destination" /> 或者 <paramref name="redundancy" /> 为 <c>null</c>。
        /// </exception>
        public static bool IsAfterOrEqual(this DateTime source, DateTime destination, TimeSpan redundancy)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), SR.ArgumentNull_Generic);
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination), SR.ArgumentNull_Generic);
            }
            if (redundancy == null)
            {
                throw new ArgumentNullException(nameof(redundancy), SR.ArgumentNull_Generic);
            }

            return source - destination >= redundancy;
        }

        /// <summary>
        ///     指示原 <see cref="System.DateTime" /> 对象的值是否晚于或者等于指定的 <see cref="System.DateTime" /> 对象的值。
        /// </summary>
        /// <param name="source">原 <see cref="System.DateTime" /> 对象。</param>
        /// <param name="destination">指定的 <see cref="System.DateTime" /> 对象的。</param>
        /// <returns>指示原 <see cref="System.DateTime" /> 对象的值晚于或者等于指定的 <see cref="System.DateTime" /> 对象的值，返回 <c>true</c>；否则返回 <c>false</c>。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="source" /> 或者 <paramref name="destination" /> 为 <c>null</c>。
        /// </exception>
        public static bool IsAfterOrEqual(this DateTime source, DateTime destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), SR.ArgumentNull_Generic);
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination), SR.ArgumentNull_Generic);
            }

            return source >= destination;
        }

        /// <summary>
        ///     指示原 <see cref="System.DateTime" /> 对象的值是否早于指定的 <see cref="System.DateTime" /> 对象的值。
        /// </summary>
        /// <param name="source">原 <see cref="System.DateTime" /> 对象。</param>
        /// <param name="destination">指定的 <see cref="System.DateTime" /> 对象的。</param>
        /// <param name="redundancy">冗余量。</param>
        /// <returns>指示原 <see cref="System.DateTime" /> 对象的值早于指定的 <see cref="System.DateTime" /> 对象的值，返回 <c>true</c>；否则返回 <c>false</c>。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="source" /> 或者 <paramref name="destination" /> 或者 <paramref name="redundancy" /> 为 <c>null</c>。
        /// </exception>
        public static bool IsBefore(this DateTime source, DateTime destination, TimeSpan redundancy)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), SR.ArgumentNull_Generic);
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination), SR.ArgumentNull_Generic);
            }
            if (redundancy == null)
            {
                throw new ArgumentNullException(nameof(redundancy), SR.ArgumentNull_Generic);
            }

            return destination - source > redundancy;
        }

        /// <summary>
        ///     指示原 <see cref="System.DateTime" /> 对象的值是否早于指定的 <see cref="System.DateTime" /> 对象的值。
        /// </summary>
        /// <param name="source">原 <see cref="System.DateTime" /> 对象。</param>
        /// <param name="destination">指定的 <see cref="System.DateTime" /> 对象的。</param>
        /// <returns>指示原 <see cref="System.DateTime" /> 对象的值早于指定的 <see cref="System.DateTime" /> 对象的值，返回 <c>true</c>；否则返回 <c>false</c>。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="source" /> 或者 <paramref name="destination" /> 为 <c>null</c>。
        /// </exception>
        public static bool IsBefore(this DateTime source, DateTime destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), SR.ArgumentNull_Generic);
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination), SR.ArgumentNull_Generic);
            }

            return destination > source;
        }

        /// <summary>
        ///     指示原 <see cref="System.DateTime" /> 对象的值是否早于指定的 <see cref="System.DateTime" /> 对象的值。
        /// </summary>
        /// <param name="source">原 <see cref="System.DateTime" /> 对象。</param>
        /// <param name="destination">指定的 <see cref="System.DateTime" /> 对象的。</param>
        /// <returns>指示原 <see cref="System.DateTime" /> 对象的值早于或者等于指定的 <see cref="System.DateTime" /> 对象的值，返回 <c>true</c>；否则返回 <c>false</c>。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="source" /> 或者 <paramref name="destination" /> 为 <c>null</c>。
        /// </exception>
        public static bool IsBeforeOrEqual(this DateTime source, DateTime destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), SR.ArgumentNull_Generic);
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination), SR.ArgumentNull_Generic);
            }

            return destination >= source;
        }

        /// <summary>
        ///     指示原 <see cref="System.DateTime" /> 对象的值是否早于指定的 <see cref="System.DateTime" /> 对象的值。
        /// </summary>
        /// <param name="source">原 <see cref="System.DateTime" /> 对象。</param>
        /// <param name="destination">指定的 <see cref="System.DateTime" /> 对象的。</param>
        /// <param name="redundancy">冗余量。</param>
        /// <returns>指示原 <see cref="System.DateTime" /> 对象的值早于或者等于指定的 <see cref="System.DateTime" /> 对象的值，返回 <c>true</c>；否则返回 <c>false</c>。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="source" /> 或者 <paramref name="destination" /> 或者 <paramref name="redundancy" /> 为 <c>null</c>。
        /// </exception>
        public static bool IsBeforeOrEqual(this DateTime source, DateTime destination, TimeSpan redundancy)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), SR.ArgumentNull_Generic);
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination), SR.ArgumentNull_Generic);
            }
            if (redundancy == null)
            {
                throw new ArgumentNullException(nameof(redundancy), SR.ArgumentNull_Generic);
            }

            return destination - source >= redundancy;
        }

        /// <summary>
        ///     指示指定的 <see cref="System.DateTime" /> 对象的 <see cref="System.DateTime.Date" /> 是否是当月的第一天。
        /// </summary>
        /// <param name="date">指定的 <see cref="System.DateTime" /> 对象。</param>
        /// <returns>如果是，返回 <c>true</c>；否则返回 <c>false</c>。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="date" /> 为 <c>null</c>。
        /// </exception>
        public static bool IsFirstDayOfThisMonth(this DateTime date)
        {
            if (date == null)
            {
                throw new ArgumentNullException(nameof(date), SR.ArgumentNull_Generic);
            }

            return date.Month != date.AddDays(-1).Month;
        }

        /// <summary>
        ///     指示指定的 <see cref="System.DateTime" /> 对象的 <see cref="System.DateTime.Date" /> 是否是当年的第一天。
        /// </summary>
        /// <param name="date">指定的 <see cref="System.DateTime" /> 对象。</param>
        /// <returns>如果是，返回 <c>true</c>；否则返回 <c>false</c>。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="date" /> 为 <c>null</c>。
        /// </exception>
        public static bool IsFirstDayOfThisYear(this DateTime date)
        {
            if (date == null)
            {
                throw new ArgumentNullException(nameof(date), SR.ArgumentNull_Generic);
            }

            return date.Year != date.AddDays(-1).Year;
        }

        /// <summary>
        ///     指示指定的 <see cref="System.DateTime" /> 对象的值是否是指定日期内的时间。
        /// </summary>
        /// <param name="time">指定的 <see cref="System.DateTime" /> 对象。</param>
        /// <param name="date">指定日期。</param>
        /// <returns>如果是，返回 <c>true</c>；否则返回 <c>false</c>。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="time" /> 或者 <paramref name="date" /> 为 <c>null</c>。
        /// </exception>
        public static bool IsInTheDay(this DateTime time, DateTime date)
        {
            if (time == null)
            {
                throw new ArgumentNullException(nameof(time), SR.ArgumentNull_Generic);
            }
            if (date == null)
            {
                throw new ArgumentNullException(nameof(date), SR.ArgumentNull_Generic);
            }

            return time >= date.Date && time < date.Date.AddDays(1);
        }

        /// <summary>
        ///     指示指定的 <see cref="System.DateTime" /> 对象的 <see cref="System.DateTime.Date" /> 是否是当月的最后一天。
        /// </summary>
        /// <param name="date">指定的 <see cref="System.DateTime" /> 对象。</param>
        /// <returns>如果是，返回 <c>true</c>；否则返回 <c>false</c>。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="date" /> 为 <c>null</c>。
        /// </exception>
        public static bool IsLastDayOfThisMonth(this DateTime date)
        {
            if (date == null)
            {
                throw new ArgumentNullException(nameof(date), SR.ArgumentNull_Generic);
            }

            return date.Month != date.AddDays(1).Month;
        }

        /// <summary>
        ///     指示指定的 <see cref="System.DateTime" /> 对象的 <see cref="System.DateTime.Date" /> 是否是当年的最后一天。
        /// </summary>
        /// <param name="date">指定的 <see cref="System.DateTime" /> 对象。</param>
        /// <returns>如果是，返回 <c>true</c>；否则返回 <c>false</c>。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="date" /> 为 <c>null</c>。
        /// </exception>
        public static bool IsLastDayOfThisYear(this DateTime date)
        {
            if (date == null)
            {
                throw new ArgumentNullException(nameof(date), SR.ArgumentNull_Generic);
            }

            return date.Year != date.AddDays(1).Year;
        }

        /// <summary>
        ///     将指定的 <see cref="System.DateTime" /> 对象转换为中国标准时区的 <see cref="System.DateTime" />。
        /// </summary>
        /// <param name="dateTime">指定的 <see cref="System.DateTime" /> 对象。</param>
        /// <returns>转换后的中国标准时区的 <see cref="System.DateTime" />，并且对象的 <see cref="System.DateTime.Kind" /> 为 <see cref="System.DateTimeKind.Local" />。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="dateTime" /> 为 <c>null</c>。
        /// </exception>
        public static DateTime ToChinaStandardTime(this DateTime dateTime)
        {
            if (dateTime == null)
            {
                throw new ArgumentNullException(nameof(dateTime), SR.ArgumentNull_Generic);
            }

            dateTime = dateTime.ToUniversalTime().AddHours(8);
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, DateTimeKind.Local);
        }

        /// <summary>
        ///     将指定的 <see cref="System.DateTimeOffset" /> 对象转换为中国标准时区的 <see cref="System.DateTimeOffset" />。
        /// </summary>
        /// <param name="dateTime">指定的 <see cref="System.DateTimeOffset" /> 对象。</param>
        /// <returns>转换后的中国标准时区的 <see cref="System.DateTimeOffset" />，并且对象的 <see cref="System.DateTimeOffset.Offset" /> 为 [08:00:00]。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="dateTime" /> 为 <c>null</c>。
        /// </exception>
        public static DateTimeOffset ToChinaStandardTime(this DateTimeOffset dateTime)
        {
            if (dateTime == null)
            {
                throw new ArgumentNullException(nameof(dateTime), SR.ArgumentNull_Generic);
            }

            dateTime = dateTime.ToUniversalTime().AddHours(8);
            return new DateTimeOffset(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, TimeSpan.FromHours(8));
        }

        /// <summary>
        ///     将指定的 <see cref="System.DateTime" /> 对象转换为JavaScript的整型时间。
        /// </summary>
        /// <param name="dateTime">指定的 <see cref="System.DateTime" /> 对象。</param>
        /// <returns>转换后的JavaScript的整型时间。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="dateTime" /> 为 <c>null</c>。
        /// </exception>
        public static long ToJSDate(this DateTime dateTime)
        {
            if (dateTime == null)
            {
                throw new ArgumentNullException(nameof(dateTime), SR.ArgumentNull_Generic);
            }

            DateTime utc = dateTime.ToUniversalTime();
            return (utc.Ticks - CONST.EPOCH_TICKS) / 10000;
        }

        /// <summary>
        ///     将指定的 <see cref="System.DateTime" /> 对象转换为日志中使用的 <see cref="System.String" />。
        /// </summary>
        /// <param name="dateTime">指定的 <see cref="System.DateTime" /> 对象。</param>
        /// <example>转换后的字符串会类似 “2016-08-31T23:31:40.5610456+08:00” 。</example>
        /// <returns>转换后的时间字符串。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="dateTime" /> 为 <c>null</c>。
        /// </exception>
        public static string ToLogFormatString(this DateTime dateTime)
        {
            if (dateTime == null)
            {
                throw new ArgumentNullException(nameof(dateTime), SR.ArgumentNull_Generic);
            }

            return dateTime.ToString("O");
        }

        /// <summary>
        ///     将指定的 <see cref="System.DateTimeOffset" /> 对象转换为日志中使用的 <see cref="System.String" />。
        /// </summary>
        /// <param name="dateTime">指定的 <see cref="System.DateTime" /> 对象。</param>
        /// <example>转换后的字符串会类似 “2016-08-31T23:31:40.5610456+08:00” 。</example>
        /// <returns>转换后的时间字符串。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="dateTime" /> 为 <c>null</c>。
        /// </exception>
        public static string ToLogFormatString(this DateTimeOffset dateTime)
        {
            if (dateTime == null)
            {
                throw new ArgumentNullException(nameof(dateTime), SR.ArgumentNull_Generic);
            }

            return dateTime.ToString("O");
        }

        /// <summary>
        ///     将指定的 <see cref="System.DateTime" /> 对象转换为易于阅读的格式的 <see cref="System.String" />。
        /// </summary>
        /// <param name="dateTime">指定的 <see cref="System.DateTime" />。</param>
        /// <example>转换后的字符串会类似 “2016-06-06 15:01:03” 。</example>
        /// <returns>转换后的时间字符串。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="dateTime" /> 为 <c>null</c>。
        /// </exception>
        public static string ToReadableString(this DateTime dateTime)
        {
            if (dateTime == null)
            {
                throw new ArgumentNullException(nameof(dateTime), SR.ArgumentNull_Generic);
            }

            return dateTime.ToString("s").Replace('T', ' ');
        }

        /// <summary>
        ///     将指定的 <see cref="System.DateTimeOffset" /> 对象转换为易于阅读的格式的 <see cref="System.String" />。
        /// </summary>
        /// <param name="dateTime">指定的 <see cref="System.DateTimeOffset" />。</param>
        /// <example>转换后的字符串会类似 “2016-06-06 15:01:03” 。</example>
        /// <returns>转换后的时间字符串。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="dateTime" /> 为 <c>null</c>。
        /// </exception>
        public static string ToReadableString(this DateTimeOffset dateTime)
        {
            if (dateTime == null)
            {
                throw new ArgumentNullException(nameof(dateTime), SR.ArgumentNull_Generic);
            }

            return dateTime.ToString("s").Replace('T', ' ');
        }

        /// <summary>
        ///     将指定的 <see cref="System.DateTime" /> 对象转换为UNIX的时间戳。
        /// </summary>
        /// <param name="dateTime">指定的 <see cref="System.DateTime" /> 对象。</param>
        /// <returns>转换后的时间戳。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="dateTime" /> 为 <c>null</c>。
        /// </exception>
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            if (dateTime == null)
            {
                throw new ArgumentNullException(nameof(dateTime), SR.ArgumentNull_Generic);
            }

            DateTime utc = dateTime.ToUniversalTime();
            return (utc.Ticks - CONST.EPOCH_TICKS) / 10000000;
        }

        /// <summary>
        ///     将指定的 <see cref="System.DateTime" /> 对象的值转换为协调世界时 (UTC)。
        /// </summary>
        /// <param name="dateTime">指定的 <see cref="System.DateTime" />。</param>
        /// <returns>一个对象，其 <see cref="P:System.DateTime.Kind" /> 属性为 <see cref="System.DateTimeKind.Utc" />，并且其值为等效于指定的 <see cref="System.DateTime" /> 对象的值的 UTC时间；如果经转换的值过大以至于不能由 MaxValue 对象表示，则为 <see cref="System.DateTime" /> 对象，或者，如果经转换的值过小以至于不能表示为 MinValue 对象，则为 <see cref="System.DateTime" /> 对象。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="dateTime" /> 为 <c>null</c>。
        /// </exception>
        public static DateTime ToUTC(this DateTime dateTime)
        {
            if (dateTime == null)
            {
                throw new ArgumentNullException(nameof(dateTime), SR.ArgumentNull_Generic);
            }

            return dateTime.ToUniversalTime();
        }
    }
}