// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : Int64Extensions.cs
// Created          : 2016-06-29  11:43 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Credit.Kolibre.Foundation.Static;

namespace Credit.Kolibre.Foundation.Sys
{
    /// <summary>
    ///     <see cref="System.long" /> 的扩展类。
    /// </summary>
    public static class Int64Extensions
    {
        private const ulong TYPE_CODE_DATA_MASK = 0xFFFFFFFF; // Lowest 4 bytes

        /// <summary>
        ///     使用 BASE36 算法将指定的 <see cref="System.long" /> 字符串中的所有字符解码为一个字符串。
        /// </summary>
        /// <param name="l">要解码的数值。</param>
        /// <returns>包含对指定的数值进行解码结果的 <see cref="System.String" />。</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="l" /> 为负值。
        /// </exception>
        public static string EncodeBase36(this long l)
        {
            if (l < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(l), l, SR.ArgumentOutOfRange_MustBeNonNegNum);
            }

            char[] clistarr = CONST.BASE36_CHARACTERS.ToCharArray();
            Stack<char> result = new Stack<char>();
            while (l != 0)
            {
                result.Push(clistarr[l % 36]);
                l /= 36;
            }
            return new string(result.ToArray());
        }

        /// <summary>
        ///     截取 <see cref="System.ulong" /> 的整数部分，截取后的类型为 <see cref="System.uint" /> 。
        /// </summary>
        /// <param name="value">待截取的无符号长整型。</param>
        /// <returns>截取后的无符号整型。</returns>
        public static uint SliceIntPart(this ulong value)
        {
            return (uint)(value & TYPE_CODE_DATA_MASK);
        }

        /// <summary>
        ///     将指定的 <see cref="System.long" /> 对象作为FileTime转换为 <see cref="System.DateTime" />对象，并且指定 <see cref="System.DateTime" />对象的 <see cref="System.DateTimeKind" />。
        /// </summary>
        /// <param name="fileTime">指定的 <see cref="System.long" /> 对象。</param>
        /// <param name="dateTimeKind">指定的 <see cref="System.DateTime.Kind" />。</param>
        /// <returns>转换后的中国标准时区的 <see cref="System.DateTime" />，并且对象的 <see cref="System.DateTime.Kind" /> 为 <see cref="System.DateTimeKind.Local" />。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     <paramref name="fileTime" /> 的值超过合理范围。
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="dateTimeKind" /> 不是合法的 <see cref="System.DateTimeKind" />。
        /// </exception>
        public static DateTime ToDateTimeFromFileTime(this long fileTime, DateTimeKind dateTimeKind = DateTimeKind.Utc)
        {
            long ticks = fileTime + CONST.FILE_TIME_OFFSET;

            if (ticks > DateTime.MaxValue.Ticks || ticks < DateTime.MinValue.Ticks)
            {
                throw new ArgumentOutOfRangeException(nameof(fileTime), fileTime, SR.ArgumentOutOfRange_DateTimeBadTicks);
            }
            if (!Enum.IsDefined(typeof(DateTimeKind), dateTimeKind))
            {
                throw new ArgumentException(SR.Argument_EnumIllegalVal.FormatWith(nameof(dateTimeKind)), nameof(dateTimeKind));
            }

            return new DateTime(ticks, dateTimeKind);
        }

        /// <summary>
        ///     将指定的 <see cref="System.long" /> 对象的值作为 Ticks 值转换为 <see cref="System.DateTime" />对象，并且指定 <see cref="System.DateTime" />对象的 <see cref="System.DateTimeKind" />。
        /// </summary>
        /// <param name="ticks">指定的 <see cref="System.long" /> 对象。</param>
        /// <param name="dateTimeKind">指定的 <see cref="System.DateTime.Kind" />。</param>
        /// <returns>转换后的中国标准时区的 <see cref="System.DateTime" />，并且对象的 <see cref="System.DateTime.Kind" /> 为 <see cref="System.DateTimeKind.Local" />。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     <paramref name="ticks" /> 的值超过合理范围。
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="dateTimeKind" /> 不是合法的 <see cref="System.DateTimeKind" />。
        /// </exception>
        public static DateTime ToDateTimeFromTicks(this long ticks, DateTimeKind dateTimeKind = DateTimeKind.Utc)
        {
            if (ticks > DateTime.MaxValue.Ticks || ticks < DateTime.MinValue.Ticks)
            {
                throw new ArgumentOutOfRangeException(nameof(ticks), ticks, SR.ArgumentOutOfRange_DateTimeBadTicks);
            }
            if (!Enum.IsDefined(typeof(DateTimeKind), dateTimeKind))
            {
                throw new ArgumentException(SR.Argument_EnumIllegalVal.FormatWith(nameof(dateTimeKind)), nameof(dateTimeKind));
            }

            return new DateTime(ticks, dateTimeKind);
        }

        /// <summary>
        ///     将指定的 <see cref="System.long" /> 对象的值作为 UnixTimestamp 值转换为 <see cref="System.DateTime" />对象，并且指定 <see cref="System.DateTime" />对象的 <see cref="System.DateTimeKind" />。
        /// </summary>
        /// <param name="timeStamp">指定的 <see cref="System.long" /> 对象。</param>
        /// <param name="dateTimeKind">指定的 <see cref="System.DateTime.Kind" />。</param>
        /// <returns>转换后的中国标准时区的 <see cref="System.DateTime" />，并且对象的 <see cref="System.DateTime.Kind" /> 为 <see cref="System.DateTimeKind.Local" />。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     <paramref name="timeStamp" /> 的值超过合理范围。
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="dateTimeKind" /> 不是合法的 <see cref="System.DateTimeKind" />。
        /// </exception>
        public static DateTime ToDateTimeFromUnixTimestamp(this long timeStamp, DateTimeKind dateTimeKind = DateTimeKind.Utc)
        {
            long seconds = timeStamp + CONST.EPOCH_SECONDS;
            long ticks = seconds * 1000 * CONST.MILLISECOND_TICKS;

            if (ticks > DateTime.MaxValue.Ticks || ticks < DateTime.MinValue.Ticks)
            {
                throw new ArgumentOutOfRangeException(nameof(timeStamp), timeStamp, SR.ArgumentOutOfRange_DateTimeBadTicks);
            }
            if (!Enum.IsDefined(typeof(DateTimeKind), dateTimeKind))
            {
                throw new ArgumentException(SR.Argument_EnumIllegalVal.FormatWith(nameof(dateTimeKind)), nameof(dateTimeKind));
            }

            return new DateTime(ticks, dateTimeKind);
        }

        /// <summary>
        ///     如果值小于0，则返回0，否则返回原值。
        /// </summary>
        /// <param name="value">原值。</param>
        /// <returns>如果值小于0，则返回0，否则返回原值。</returns>
        public static long ToZeroIfNegNum(this long value)
        {
            return value < 0 ? 0 : value;
        }
    }
}