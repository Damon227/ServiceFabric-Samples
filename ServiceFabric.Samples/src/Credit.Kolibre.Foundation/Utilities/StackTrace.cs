// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : StackTrace.cs
// Created          : 2016-07-04  9:31 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Credit.Kolibre.Foundation.Utilities
{
    public static class StackTraceUtility
    {
        /// <summary>
        ///     获取当前正在执行的方法的名称。
        /// </summary>
        /// <returns>当前正在执行的方法的名称。</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethodName(int frameIndex = 1)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(frameIndex);

            return sf.GetMethod().Name;
        }

        /// <summary>
        ///     获取当前正在执行的方法所在类的名称。
        /// </summary>
        /// <returns>当前正在执行的方法所在类的名称。</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentTypeName(int frameIndex = 1)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(frameIndex);

            return sf.GetMethod().ReflectedType?.FullName ??
                   sf.GetMethod().DeclaringType?.FullName ?? sf.GetFileName();
        }
    }
}