// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : UrlPath.cs
// Created          : 2016-06-27  2:07 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Credit.Kolibre.Foundation.Sys;

namespace Credit.Kolibre.Foundation.Utilities
{
    /// <summary>
    ///     处理Url路径。
    /// </summary>
    public static class UrlPath
    {
        /// <summary>
        ///     将两个Url字符串拼接起来，并且返回 <see cref="System.Uri" /> 实例。
        /// </summary>
        /// <param name="path1">第一个Url的字符串。</param>
        /// <param name="path2">第二个Url的字符串。</param>
        /// <returns>拼接后的<see cref="System.Uri" /> 实例。</returns>
        public static Uri CombineUrlPaths(string path1, string path2)
        {
            if (path2.IsNullOrEmpty())
                return new Uri(path1);

            if (path1.IsNullOrEmpty())
                return new Uri(path2);

            if (path2.StartsWith("http://", StringComparison.Ordinal) || path2.StartsWith("https://", StringComparison.Ordinal))
                return new Uri(path2);

            char ch = path1[path1.Length - 1];

            return ch != '/' ? new Uri(path1.TrimEnd('/') + '/' + path2.TrimStart('/')) : new Uri(path1 + path2);
        }
    }
}