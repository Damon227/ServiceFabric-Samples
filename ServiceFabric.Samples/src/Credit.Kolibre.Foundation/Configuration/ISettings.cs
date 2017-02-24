// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : ISettings.cs
// Created          : 2016-06-27  2:06 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Credit.Kolibre.Foundation.Configuration
{
    /// <summary>
    ///     用于获取配置项。
    /// </summary>
    public interface ISettings : IAppSettings
    {
        /// <summary>
        ///     指示配置中是否有指定的项。如果当获取该配置项时，获取的值为 <see cref="System.String.Empty" />，该方法会返回 <c>true</c>。
        /// </summary>
        /// <param name="key">指定的项的 key。</param>
        /// <returns>如果有返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="key" /> 为 <c>null</c>。
        /// </exception>
        bool Exists(string key);

        /// <summary>
        ///     获取指定的配置值，并且使用 JSON 反序列化的方法转换为指定的类型。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException" /> 。
        /// </summary>
        /// <typeparam name="T">需要转换的目标类型。</typeparam>
        /// <param name="key">配置项的 key。</param>
        /// <returns>
        ///     返回转换后的指定类型。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        T Get<T>(string key);

        /// <summary>
        ///     获取指定的配置值，并且使用 JSON 反序列化的方法转换为指定的类型。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException"></see> 。
        /// </summary>
        /// <typeparam name="T">需要转换的目标类型。</typeparam>
        /// <param name="key">配置项的 key。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>
        ///     返回转换后的指定类型。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        T Get<T>(string key, T defaultValue);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Boolean" />。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException" /> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <returns>
        ///     返回转换后的 <see cref="System.Boolean" /> 值。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        bool GetBool(string key);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Boolean" />。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException"></see> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>
        ///     返回转换后的 <see cref="System.Boolean" /> 值。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        bool GetBool(string key, bool defaultValue);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Boolean" /> 的列表。
        ///     如果其中任意一项转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException" /> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <remarks>列表需要使用"[]"包裹，并且元素使用","分割才能转换成功。</remarks>
        /// <returns>
        ///     返回转换后的 <see cref="System.Boolean" /> 的列表。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        IList<bool> GetBoolList(string key);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.DateTime" />。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException" /> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <returns>
        ///     返回转换后的 <see cref="System.DateTime" /> 值。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        DateTime GetDateTime(string key);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.DateTime" />。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException"></see> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>
        ///     返回转换后的 <see cref="System.DateTime" /> 值。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        DateTime GetDateTime(string key, DateTime defaultValue);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.DateTime" /> 的列表。
        ///     如果其中任意一项转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException" /> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <remarks>列表需要使用"[]"包裹，并且元素使用","分割才能转换成功。</remarks>
        /// <returns>
        ///     返回转换后的 <see cref="System.DateTime" /> 的列表。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        IList<DateTime> GetDateTimeList(string key);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Decimal" />。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException" /> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <returns>
        ///     返回转换后的 <see cref="System.Decimal" /> 值。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        decimal GetDecimal(string key);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Decimal" />。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException"></see> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>
        ///     返回转换后的 <see cref="System.Decimal" /> 值。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        decimal GetDecimal(string key, decimal defaultValue);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Decimal" /> 的列表。
        ///     如果其中任意一项转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException" /> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <remarks>列表需要使用"[]"包裹，并且元素使用","分割才能转换成功。</remarks>
        /// <returns>
        ///     返回转换后的 <see cref="System.Decimal" /> 的列表。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        IList<decimal> GetDecimalList(string key);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Double" />。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException" /> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <returns>
        ///     返回转换后的 <see cref="System.Double" /> 值。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        double GetDouble(string key);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Double" />。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException"></see> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>
        ///     返回转换后的 <see cref="System.Double" /> 值。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        double GetDouble(string key, double defaultValue);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Double" /> 的列表。
        ///     如果其中任意一项转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException" /> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <remarks>列表需要使用"[]"包裹，并且元素使用","分割才能转换成功。</remarks>
        /// <returns>
        ///     返回转换后的 <see cref="System.Double" /> 的列表。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        IList<double> GetDoubleList(string key);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Guid" />。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException" /> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <returns>
        ///     返回转换后的 <see cref="System.Guid" /> 值。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        Guid GetGuid(string key);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Guid" />。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException"></see> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>
        ///     返回转换后的 <see cref="System.Guid" /> 值。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        Guid GetGuid(string key, Guid defaultValue);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Guid" /> 的列表。
        ///     如果其中任意一项转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException" /> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <remarks>列表需要使用"[]"包裹，并且元素使用","分割才能转换成功。</remarks>
        /// <returns>
        ///     返回转换后的 <see cref="System.Guid" /> 的列表。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        IList<Guid> GetGuidList(string key);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Int32" />。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException" /> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <returns>
        ///     返回转换后的 <see cref="System.Int32" /> 值。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        int GetInt(string key);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Int32" />。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException"></see> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>
        ///     返回转换后的 <see cref="System.Int32" /> 值。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        int GetInt(string key, int defaultValue);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Int32" /> 的列表。
        ///     如果其中任意一项转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException" /> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <remarks>列表需要使用"[]"包裹，并且元素使用","分割才能转换成功。</remarks>
        /// <returns>
        ///     返回转换后的 <see cref="System.Int32" /> 的列表。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        IList<int> GetIntList(string key);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Int64" />。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException" /> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <returns>
        ///     返回转换后的 <see cref="System.Int64" /> 值。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        long GetLong(string key);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Int64" />。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException"></see> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>
        ///     返回转换后的 <see cref="System.Int64" /> 值。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        long GetLong(string key, long defaultValue);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Int64" /> 的列表。
        ///     如果其中任意一项转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException" /> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <remarks>列表需要使用"[]"包裹，并且元素使用","分割才能转换成功。</remarks>
        /// <returns>
        ///     返回转换后的 <see cref="System.Int64" /> 的列表。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        IList<long> GetLongList(string key);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Int16" />。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException" /> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <returns>
        ///     返回转换后的 <see cref="System.Int16" /> 值。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        short GetShort(string key);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Int16" />。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException"></see> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>
        ///     返回转换后的 <see cref="System.Int16" /> 值。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        short GetShort(string key, short defaultValue);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Int16" /> 的列表。
        ///     如果其中任意一项转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException" /> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <remarks>列表需要使用"[]"包裹，并且元素使用","分割才能转换成功。</remarks>
        /// <returns>
        ///     返回转换后的 <see cref="System.Int16" /> 的列表。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        IList<short> GetShortList(string key);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Single" />。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException" /> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <returns>
        ///     返回转换后的 <see cref="System.Single" /> 值。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        float GetSingle(string key);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Single" />。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        ///     如果转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException"></see> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>
        ///     返回转换后的 <see cref="System.Single" /> 值。
        ///     如果获取的配置原始字符串为 <c>null</c> 或者 <see cref="System.String.Empty" />，会返回默认值 <paramref name="defaultValue" />。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        float GetSingle(string key, float defaultValue);

        /// <summary>
        ///     获取指定的配置值，并且使用一般的转换方法转换为 <see cref="System.Single" /> 的列表。
        ///     如果其中任意一项转换失败，会抛出异常 <see cref="System.Configuration.ConfigurationErrorsException" /> 。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <remarks>列表需要使用"[]"包裹，并且元素使用","分割才能转换成功。</remarks>
        /// <returns>
        ///     返回转换后的 <see cref="System.Single" /> 的列表。
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///     转换失败。
        /// </exception>
        IList<float> GetSingleList(string key);
    }
}