// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : ByteExtensions.cs
// Created          : 2016-07-25  12:06 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Credit.Kolibre.Foundation.Static;

namespace Credit.Kolibre.Foundation.Sys
{
    /// <summary>
    ///     <see cref="System.Byte" /> 的扩展类。
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        ///     使用 <see cref="System.Text.Encoding.ASCII" /> 将指定字节数组中的所有字节解码为一个字符串。
        /// </summary>
        /// <param name="bytes">包含要解码的字节序列的字节数组。</param>
        /// <returns>包含指定字节序列解码结果的 <see cref="System.String" />。</returns>
        /// <exception cref="T:System.ArgumentException">字节数组中包含无效的 ASCII 码位。</exception>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="bytes" /> 为 null。
        /// </exception>
        /// <exception cref="T:System.Text.DecoderFallbackException">发生回退（请参见.NET Framework 中的字符编码以获得完整的解释）－和－<see cref="P:System.Text.Encoding.DecoderFallback" /> 设置为 <see cref="T:System.Text.DecoderExceptionFallback" />。</exception>
        public static string ToASCIIString(this byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes), SR.ArgumentNull_Array);
            }

            return Encoding.ASCII.GetString(bytes.FixBom());
        }

        /// <summary>
        ///     使用 Base64 算法将指定字节数组中的所有字节解码为一个字符串。
        /// </summary>
        /// <param name="bytes">包含要解码的字节序列的字节数组。</param>
        /// <returns>包含指定字节序列解码结果的 <see cref="System.String" />。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="bytes" /> 为 null。
        /// </exception>
        public static string ToBase64String(this byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes), SR.ArgumentNull_Array);
            }

            return Convert.ToBase64String(bytes.FixBom());
        }

        /// <summary>
        ///     使用十六进制将指定字节数组中的所有字节解码为一个字符串。
        /// </summary>
        /// <param name="bytes">包含要解码的字节序列的字节数组。</param>
        /// <returns>包含指定字节序列解码结果的 <see cref="System.String" />。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="bytes" /> 为 null。
        /// </exception>
        [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Global")]
        public static string ToPrintableString(this byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes), SR.ArgumentNull_Array);
            }

            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString().ToUpperInvariant();
        }

        /// <summary>
        ///     使用指定的编码将指定字节数组中的所有字节解码为一个字符串。
        /// </summary>
        /// <param name="bytes">包含要解码的字节序列的字节数组。</param>
        /// <param name="encodingName">指定编码的代码页名称。</param>
        /// <returns>包含指定字节序列解码结果的 <see cref="System.String" />。</returns>
        /// <exception cref="T:System.ArgumentException">字节数组中包含无效的 UTF8 码位。</exception>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="bytes" /> 为 null。
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="encodingName" /> 不是有效的代码页名称。
        /// </exception>
        /// <exception cref="T:System.Text.DecoderFallbackException">发生回退（请参见.NET Framework 中的字符编码以获得完整的解释）－和－<see cref="P:System.Text.Encoding.DecoderFallback" /> 设置为 <see cref="T:System.Text.DecoderExceptionFallback" />。</exception>
        public static string ToStringOfEncoding(this byte[] bytes, string encodingName)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes), SR.ArgumentNull_Array);
            }
            if (encodingName.IsNullOrEmpty())
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(encodingName));
            }

            return Encoding.UTF8.GetString(bytes.FixBom());
        }

        /// <summary>
        ///     使用 <see cref="System.Text.Encoding.Unicode" /> 将指定字节数组中的所有字节解码为一个字符串。
        /// </summary>
        /// <param name="bytes">包含要解码的字节序列的字节数组。</param>
        /// <returns>包含指定字节序列解码结果的 <see cref="System.String" />。</returns>
        /// <exception cref="T:System.ArgumentException">字节数组中包含无效的 Unicode 码位。</exception>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="bytes" /> 为 null。
        /// </exception>
        /// <exception cref="T:System.Text.DecoderFallbackException">发生回退（请参见.NET Framework 中的字符编码以获得完整的解释）－和－<see cref="P:System.Text.Encoding.DecoderFallback" /> 设置为 <see cref="T:System.Text.DecoderExceptionFallback" />。</exception>
        public static string ToUnicodeString(this byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes), SR.ArgumentNull_Array);
            }

            return Encoding.Unicode.GetString(bytes.FixBom());
        }

        /// <summary>
        ///     使用 <see cref="System.Text.Encoding.UTF8" /> 将指定字节数组中的所有字节解码为一个字符串。
        /// </summary>
        /// <param name="bytes">包含要解码的字节序列的字节数组。</param>
        /// <returns>包含指定字节序列解码结果的 <see cref="System.String" />。</returns>
        /// <exception cref="T:System.ArgumentException">字节数组中包含无效的 UTF8 码位。</exception>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="bytes" /> 为 null。
        /// </exception>
        /// <exception cref="T:System.Text.DecoderFallbackException">发生回退（请参见.NET Framework 中的字符编码以获得完整的解释）－和－<see cref="P:System.Text.Encoding.DecoderFallback" /> 设置为 <see cref="T:System.Text.DecoderExceptionFallback" />。</exception>
        public static string ToUTF8String(this byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes), SR.ArgumentNull_Array);
            }

            return Encoding.UTF8.GetString(bytes.FixBom());
        }

        private static byte[] FixBom(this byte[] valueToFix)
        {
            //see BOM - Byte Order Mark : http://en.wikipedia.org/wiki/Byte_order_mark
            //    http://www.verious.com/qa/-239-187-191-characters-appended-to-the-beginning-of-each-file/
            //    http://social.msdn.microsoft.com/Forums/en-US/8956758d-9814-4bd4-9812-e82903640b2f/recieving-239187191-character-symbols-when-loading-text-files-not-containing-them
            if (valueToFix.Length > 3 && valueToFix[0] == '\xEF' && valueToFix[1] == '\xBB' && valueToFix[2] == '\xBF')
            {
                int size = valueToFix.Length - 3;
                byte[] value = new byte[size];
                Array.Copy(valueToFix, 3, value, 0, size);
                return value;
            }

            return valueToFix;
        }
    }
}