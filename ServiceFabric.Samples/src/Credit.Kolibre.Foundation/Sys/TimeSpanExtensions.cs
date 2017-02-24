// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : TimeSpanExtensions.cs
// Created          : 2016-08-31  11:09 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Credit.Kolibre.Foundation.Utilities;

namespace Credit.Kolibre.Foundation.Sys
{
    /// <summary>
    ///     <see cref="System.TimeSpan" /> 的扩展类。
    /// </summary>
    public static class TimeSpanExtensions
    {
        public static TimeSpan Divide(this TimeSpan timeSpan, double value)
        {
            double ticksD = timeSpan.Ticks / value;
            long ticks = checked((long)ticksD);
            return TimeSpan.FromTicks(ticks);
        }

        public static double Divide(this TimeSpan first, TimeSpan second)
        {
            double ticks1 = first.Ticks;
            double ticks2 = second.Ticks;
            return ticks1 / ticks2;
        }

        public static TimeSpan Max(TimeSpan first, TimeSpan second)
        {
            return first >= second ? first : second;
        }

        public static TimeSpan Min(TimeSpan first, TimeSpan second)
        {
            return first < second ? first : second;
        }

        public static TimeSpan Multiply(this TimeSpan timeSpan, double value)
        {
            double ticksD = timeSpan.Ticks * value;
            long ticks = checked((long)ticksD);
            return TimeSpan.FromTicks(ticks);
        }

        public static TimeSpan NextTimeSpan(this SafeRandom random, TimeSpan timeSpan)
        {
            if (timeSpan <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(timeSpan), timeSpan, "SafeRandom.NextTimeSpan timeSpan must be a positive number.");
            }

            double ticksD = timeSpan.Ticks * random.NextDouble();
            long ticks = checked((long)ticksD);
            return TimeSpan.FromTicks(ticks);
        }

        public static TimeSpan NextTimeSpan(this SafeRandom random, TimeSpan minValue, TimeSpan maxValue)
        {
            if (minValue <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue), minValue, "SafeRandom.NextTimeSpan minValue must be a positive number.");
            }
            if (minValue >= maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue), minValue, "SafeRandom.NextTimeSpan minValue must be greater than maxValue.");
            }
            TimeSpan span = maxValue - minValue;
            return minValue + random.NextTimeSpan(span);
        }
    }
}