﻿// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : IntExtensions.cs
// Created          : 2016-06-27  2:07 PM
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
    ///     Interface ILoopIterator
    /// </summary>
    public interface ILoopIterator
    {
        /// <summary>
        ///     执行指定的操作。
        /// </summary>
        /// <param name="action">指定的操作。</param>
        void Do(Action action);

        /// <summary>
        ///     执行指定的操作。
        /// </summary>
        /// <param name="action">指定的操作。</param>
        void Do(Action<int> action);
    }

    /// <summary>
    ///     <see cref="System.Int32" /> 的扩展类。
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        ///     初始化一个为指定天数的 <see cref="System.TimeSpan" /> 实例。
        /// </summary>
        /// <param name="value">指定的天数。</param>
        /// <returns>初始化后的实例。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     根据 <paramref name="value" /> 的值构造的 <see cref="System.TimeSpan" /> 小于 <see cref="System.TimeSpan.MinValue" /> 或大于 <see cref="System.TimeSpan.MaxValue" />。
        /// </exception>
        public static TimeSpan Days(this int value)
        {
            if (value > TimeSpan.MaxValue.TotalDays || value < TimeSpan.MinValue.TotalDays)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value,
                    SR.ArgumentOutOfRange_Bounds_Lower_Upper.FormatWith((int)TimeSpan.MinValue.TotalDays, (int)TimeSpan.MaxValue.TotalDays));
            }

            return new TimeSpan(value, 0, 0, 0);
        }

        /// <summary>
        ///     初始化一个为指定小时数的 <see cref="System.TimeSpan" /> 实例。
        /// </summary>
        /// <param name="value">指定的小时数。</param>
        /// <returns>初始化后的实例。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     根据 <paramref name="value" /> 的值构造的 <see cref="System.TimeSpan" /> 小于 <see cref="System.TimeSpan.MinValue" /> 或大于 <see cref="System.TimeSpan.MaxValue" />。
        /// </exception>
        public static TimeSpan Hours(this int value)
        {
            if (value > TimeSpan.MaxValue.TotalHours || value < TimeSpan.MinValue.TotalHours)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value,
                    SR.ArgumentOutOfRange_Bounds_Lower_Upper.FormatWith((int)TimeSpan.MinValue.TotalHours, (int)TimeSpan.MaxValue.TotalHours));
            }

            return new TimeSpan(value, 0, 0);
        }

        /// <summary>
        ///     初始化一个为指定毫秒数的 <see cref="System.TimeSpan" /> 实例。
        /// </summary>
        /// <param name="value">指定的毫秒数。</param>
        /// <returns>初始化后的实例。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     根据 <paramref name="value" /> 的值构造的 <see cref="System.TimeSpan" /> 小于 <see cref="System.TimeSpan.MinValue" /> 或大于 <see cref="System.TimeSpan.MaxValue" />。
        /// </exception>
        public static TimeSpan Milliseconds(this int value)
        {
            if (value > TimeSpan.MaxValue.TotalMilliseconds || value < TimeSpan.MinValue.TotalMilliseconds)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value,
                    SR.ArgumentOutOfRange_Bounds_Lower_Upper.FormatWith((int)TimeSpan.MinValue.TotalMilliseconds, (int)TimeSpan.MaxValue.TotalMilliseconds));
            }

            return new TimeSpan(0, 0, 0, 0, value);
        }

        /// <summary>
        ///     初始化一个为指定分钟数的 <see cref="System.TimeSpan" /> 实例。
        /// </summary>
        /// <param name="value">指定的分钟数。</param>
        /// <returns>初始化后的实例。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     根据 <paramref name="value" /> 的值构造的 <see cref="System.TimeSpan" /> 小于 <see cref="System.TimeSpan.MinValue" /> 或大于 <see cref="System.TimeSpan.MaxValue" />。
        /// </exception>
        public static TimeSpan Minutes(this int value)
        {
            if (value > TimeSpan.MaxValue.TotalMinutes || value < TimeSpan.MinValue.TotalMinutes)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value,
                    SR.ArgumentOutOfRange_Bounds_Lower_Upper.FormatWith((int)TimeSpan.MinValue.TotalMinutes, (int)TimeSpan.MaxValue.TotalMinutes));
            }

            return new TimeSpan(0, 0, value, 0);
        }

        /// <summary>
        ///     初始化一个为指定秒数的 <see cref="System.TimeSpan" /> 实例。
        /// </summary>
        /// <param name="value">指定的秒数。</param>
        /// <returns>初始化后的实例。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     根据 <paramref name="value" /> 的值构造的 <see cref="System.TimeSpan" /> 小于 <see cref="System.TimeSpan.MinValue" /> 或大于 <see cref="System.TimeSpan.MaxValue" />。
        /// </exception>
        public static TimeSpan Seconds(this int value)
        {
            if (value > TimeSpan.MaxValue.TotalSeconds || value < TimeSpan.MinValue.TotalSeconds)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value,
                    SR.ArgumentOutOfRange_Bounds_Lower_Upper.FormatWith((int)TimeSpan.MinValue.TotalSeconds, (int)TimeSpan.MaxValue.TotalSeconds));
            }

            return new TimeSpan(0, 0, value);
        }

        /// <summary>
        ///     初始化一个 <see cref="ILoopIterator" /> 实例，可以进行指定次数的循环。
        /// </summary>
        /// <param name="count">指定的循环次数。</param>
        /// <returns> <see cref="ILoopIterator" /> 实例。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     <paramref name="count" /> 的值不能为非负值。
        /// </exception>
        public static ILoopIterator Times(this int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, SR.ArgumentOutOfRange_MustBeNonNegNum);
            }

            return new LoopIterator(count);
        }

        /// <summary>
        ///     如果值小于0，则返回0，否则返回原值。
        /// </summary>
        /// <param name="value">原值。</param>
        /// <returns>如果值小于0，则返回0，否则返回原值。</returns>
        public static int ToZeroIfNegNum(this int value)
        {
            return value < 0 ? 0 : value;
        }
    }

    internal class LoopIterator : ILoopIterator
    {
        private readonly int _end;

        private readonly int _start;

        internal LoopIterator(int count)
        {
            if (count < 0)
            {
                count = 0;
            }

            _start = 0;
            _end = count - 1;
        }

        internal LoopIterator(int start, int end)
        {
            if (start > end)
            {
                start = end;
            }

            _start = start;
            _end = end;
        }

        #region ILoopIterator Members

        public void Do(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            for (int i = _start; i <= _end; i++)
            {
                action();
            }
        }

        public void Do(Action<int> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            for (int i = _start; i <= _end; i++)
            {
                action(i);
            }
        }

        #endregion ILoopIterator Members
    }
}