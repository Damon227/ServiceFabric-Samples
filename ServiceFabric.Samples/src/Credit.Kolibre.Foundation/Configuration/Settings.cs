// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : Settings.cs
// Created          : 2016-06-27  2:06 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using Credit.Kolibre.Foundation.Static;
using Credit.Kolibre.Foundation.Sys;
using Credit.Kolibre.Foundation.Sys.Collections.Generic;

namespace Credit.Kolibre.Foundation.Configuration
{
    /// <summary>
    ///     应用的默认配置类，用于获取或者设置全局配置，初始配置来源于配置文件。
    /// </summary>
    public class Settings : ISettings
    {
        /// <summary>
        ///     配置项值格式错误时的格式化报错字符串。
        /// </summary>
        protected const string ERROR_APPSETTING_INVALLID_FORMAT = "The {0} setting had an invalid format. The value \"{1}\" could not be cast to type {2}";

        /// <summary>
        ///     无法找到指定配置项时的格式化报错字符串。
        /// </summary>
        protected const string ERROR_APPSETTING_NOT_FOUND = "Unable to find App Setting: {0}";

        private readonly IAppSettings _appSettings;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Credit.Kolibre.Foundation.Configuration.Settings" /> class.
        /// </summary>
        /// <param name="appSettings"><see cref="Credit.Kolibre.Foundation.Configuration.IAppSettings" /> 实例。</param>
        public Settings(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Credit.Kolibre.Foundation.Configuration.Settings" /> class.
        /// </summary>
        private Settings()
        {
            _appSettings = new AppSettings();
        }

        /// <summary>
        ///     当前应用的默认配置类。
        /// </summary>
        public static ISettings Current { get; } = new Settings();

        /// <summary>
        ///     配置值的显式 <c>null</c> 值形式。"{null}"。
        /// </summary>
        public static string NullSettingValue
        {
            get { return "{null}"; }
        }

        #region ISettings Members

        /// <summary>
        ///     添加一个新的指定的配置值，如果该设置项已经存在，则添加失败，不会对配置值有任何影响。
        ///     通过该方法设置的配置值，不会更新到配置文件中，只是会保留在应用的配置类中。
        ///     <paramref name="key" /> 和 <paramref name="value" /> 不能为 <c>null</c>，配置值需要设置为 <c>null</c> 的情况下，需要显式进行设置为 "{null}"。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <param name="value">配置项的值。</param>
        /// <returns>
        ///     添加成功则返回 <c>true</c>，设置失败则返回 <c>false</c>。
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="key" /> 或者 <paramref name="value" /> 为 <c>null</c>。
        /// </exception>
        public bool Add(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (key.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(key));
            }

            return _appSettings.Add(key, value);
        }

        /// <summary>
        ///     指示配置中是否有指定的项。如果当获取该配置项时，获取的值为 <see cref="System.String.Empty" />，该方法会返回 <c>true</c>。
        /// </summary>
        /// <param name="key">指定的项的 key。</param>
        /// <returns>如果有返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="key" /> 为 <c>null</c>。
        /// </exception>
        public virtual bool Exists(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return GetNullableString(key) != null;
        }

        /// <summary>
        ///     获取指定的配置项中原始的字符串。如果找不到指定的配置项，或者配置项中显式地配置为 <c>null</c>，则返回 <c>null</c>。<paramref name="key" /> 不能为 <c>null</c>。
        /// </summary>
        /// <param name="key">配置项的 key，不能为 <c>null</c>。</param>
        /// <remarks>将配置项设置为 "{null}"，即为显式的 <c>null</c> 值。</remarks>
        /// <returns>
        ///     返回指定的配置项中原始的字符串。如果找不到指定的配置项，或者配置项中显示地配置为 <c>null</c>，则返回 <c>null</c>。
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="key" /> 为 <c>null</c>。
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="key" /> 为空字符串或者仅包含空格。
        /// </exception>
        public string Get(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (key.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(key));
            }

            return GetNullableString(key);
        }

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
        public virtual T Get<T>(string key)
        {
            string stringValue = GetNullableString(key);

            try
            {
                return stringValue.FromJson<T>();
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(T).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }
        }

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
        public virtual T Get<T>(string key, T defaultValue)
        {
            string stringValue = GetNullableString(key);

            try
            {
                if (stringValue.IsNotNullOrEmpty())
                {
                    return stringValue.FromJson<T>();
                }
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(T).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }

            return defaultValue;
        }

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
        public bool GetBool(string key)
        {
            string stringValue = GetNullableString(key);

            try
            {
                return stringValue.ToBool();
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(bool).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }
        }

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
        public bool GetBool(string key, bool defaultValue)
        {
            string stringValue = GetNullableString(key);

            try
            {
                if (stringValue.IsNotNullOrEmpty())
                {
                    return stringValue.ToBool();
                }
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(bool).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }

            return defaultValue;
        }

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
        public IList<bool> GetBoolList(string key)
        {
            IList<bool> setting = new List<bool>();
            string stringValue = GetNullableString(key);

            stringValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ForEach(i =>
            {
                try
                {
                    setting.Add(i.ToBool());
                }
                catch (Exception ex)
                {
                    string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, i, typeof(bool).FullName);
                    throw new ConfigurationErrorsException(message, ex);
                }
            });

            return setting;
        }

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
        public DateTime GetDateTime(string key)
        {
            string stringValue = GetNullableString(key);

            try
            {
                return stringValue.ToDateTime();
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(DateTime).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }
        }

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
        public DateTime GetDateTime(string key, DateTime defaultValue)
        {
            string stringValue = GetNullableString(key);

            try
            {
                if (stringValue.IsNotNullOrEmpty())
                {
                    return stringValue.ToDateTime();
                }
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(DateTime).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }

            return defaultValue;
        }

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
        public IList<DateTime> GetDateTimeList(string key)
        {
            IList<DateTime> setting = new List<DateTime>();
            string stringValue = GetNullableString(key);

            stringValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ForEach(i =>
            {
                try
                {
                    setting.Add(i.ToDateTime());
                }
                catch (Exception ex)
                {
                    string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, i, typeof(DateTime).FullName);
                    throw new ConfigurationErrorsException(message, ex);
                }
            });

            return setting;
        }

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
        public decimal GetDecimal(string key)
        {
            string stringValue = GetNullableString(key);

            try
            {
                return stringValue.ToDecimal();
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(decimal).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }
        }

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
        public decimal GetDecimal(string key, decimal defaultValue)
        {
            string stringValue = GetNullableString(key);

            try
            {
                if (stringValue.IsNotNullOrEmpty())
                {
                    return stringValue.ToDecimal();
                }
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(decimal).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }

            return defaultValue;
        }

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
        public IList<decimal> GetDecimalList(string key)
        {
            IList<decimal> setting = new List<decimal>();
            string stringValue = GetNullableString(key);

            stringValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ForEach(i =>
            {
                try
                {
                    setting.Add(i.ToDecimal());
                }
                catch (Exception ex)
                {
                    string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, i, typeof(decimal).FullName);
                    throw new ConfigurationErrorsException(message, ex);
                }
            });

            return setting;
        }

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
        public double GetDouble(string key)
        {
            string stringValue = GetNullableString(key);

            try
            {
                return stringValue.ToDouble();
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(double).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }
        }

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
        public double GetDouble(string key, double defaultValue)
        {
            string stringValue = GetNullableString(key);

            try
            {
                if (stringValue.IsNotNullOrEmpty())
                {
                    return stringValue.ToDouble();
                }
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(double).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }

            return defaultValue;
        }

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
        public IList<double> GetDoubleList(string key)
        {
            IList<double> setting = new List<double>();
            string stringValue = GetNullableString(key);

            stringValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ForEach(i =>
            {
                try
                {
                    setting.Add(i.ToDouble());
                }
                catch (Exception ex)
                {
                    string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, i, typeof(double).FullName);
                    throw new ConfigurationErrorsException(message, ex);
                }
            });

            return setting;
        }

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
        public Guid GetGuid(string key)
        {
            string stringValue = GetNullableString(key);

            try
            {
                return stringValue.ToGuid();
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(Guid).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }
        }

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
        public Guid GetGuid(string key, Guid defaultValue)
        {
            string stringValue = GetNullableString(key);

            try
            {
                if (stringValue.IsNotNullOrEmpty())
                {
                    return stringValue.ToGuid();
                }
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(Guid).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }

            return defaultValue;
        }

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
        public IList<Guid> GetGuidList(string key)
        {
            IList<Guid> setting = new List<Guid>();
            string stringValue = GetNullableString(key);

            stringValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ForEach(i =>
            {
                try
                {
                    setting.Add(i.ToGuid());
                }
                catch (Exception ex)
                {
                    string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, i, typeof(Guid).FullName);
                    throw new ConfigurationErrorsException(message, ex);
                }
            });

            return setting;
        }

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
        public int GetInt(string key)
        {
            string stringValue = GetNullableString(key);

            try
            {
                return stringValue.ToInt32();
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(int).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }
        }

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
        public int GetInt(string key, int defaultValue)
        {
            string stringValue = GetNullableString(key);

            try
            {
                if (stringValue.IsNotNullOrEmpty())
                {
                    return stringValue.ToInt32();
                }
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(int).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }

            return defaultValue;
        }

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
        public IList<int> GetIntList(string key)
        {
            IList<int> setting = new List<int>();
            string stringValue = GetNullableString(key);

            stringValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ForEach(i =>
            {
                try
                {
                    setting.Add(i.ToInt32());
                }
                catch (Exception ex)
                {
                    string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, i, typeof(int).FullName);
                    throw new ConfigurationErrorsException(message, ex);
                }
            });

            return setting;
        }

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
        public long GetLong(string key)
        {
            string stringValue = GetNullableString(key);

            try
            {
                return stringValue.ToInt64();
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(long).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }
        }

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
        public long GetLong(string key, long defaultValue)
        {
            string stringValue = GetNullableString(key);

            try
            {
                if (stringValue.IsNotNullOrEmpty())
                {
                    return stringValue.ToInt64();
                }
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(long).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }

            return defaultValue;
        }

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
        public IList<long> GetLongList(string key)
        {
            IList<long> setting = new List<long>();
            string stringValue = GetNullableString(key);

            stringValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ForEach(i =>
            {
                try
                {
                    setting.Add(i.ToInt64());
                }
                catch (Exception ex)
                {
                    string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, i, typeof(long).FullName);
                    throw new ConfigurationErrorsException(message, ex);
                }
            });

            return setting;
        }

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
        public short GetShort(string key)
        {
            string stringValue = GetNullableString(key);

            try
            {
                return stringValue.ToInt16();
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(short).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }
        }

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
        public short GetShort(string key, short defaultValue)
        {
            string stringValue = GetNullableString(key);

            try
            {
                if (stringValue.IsNotNullOrEmpty())
                {
                    return stringValue.ToInt16();
                }
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(short).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }

            return defaultValue;
        }

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
        public IList<short> GetShortList(string key)
        {
            IList<short> setting = new List<short>();
            string stringValue = GetNullableString(key);

            stringValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ForEach(i =>
            {
                try
                {
                    setting.Add(i.ToInt16());
                }
                catch (Exception ex)
                {
                    string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, i, typeof(short).FullName);
                    throw new ConfigurationErrorsException(message, ex);
                }
            });

            return setting;
        }

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
        public float GetSingle(string key)
        {
            string stringValue = GetNullableString(key);

            try
            {
                return stringValue.ToSingle();
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(float).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }
        }

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
        public float GetSingle(string key, float defaultValue)
        {
            string stringValue = GetNullableString(key);

            try
            {
                if (stringValue.IsNotNullOrEmpty())
                {
                    return stringValue.ToSingle();
                }
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(float).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }

            return defaultValue;
        }

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
        public IList<float> GetSingleList(string key)
        {
            IList<float> setting = new List<float>();
            string stringValue = GetNullableString(key);

            stringValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ForEach(i =>
            {
                try
                {
                    setting.Add(i.ToSingle());
                }
                catch (Exception ex)
                {
                    string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, i, typeof(float).FullName);
                    throw new ConfigurationErrorsException(message, ex);
                }
            });

            return setting;
        }

        /// <summary>
        ///     设置指定的配置值，如果该设置项已经存在，则覆盖原配置值。只能通过字符串的形式设置，设置时要考虑使用时的格式。
        ///     通过该方法设置的配置值，不会更新到配置文件中，只是会保留在应用的配置类中。
        ///     <paramref name="key" /> 和 <paramref name="value" /> 不能为 <c>null</c>，配置值需要设置为 <c>null</c> 的情况下，需要显式进行设置为 "{null}"。
        /// </summary>
        /// <param name="key">配置项的 key。</param>
        /// <param name="value">配置项的值。</param>
        /// <returns>
        ///     设置成功则返回 <c>true</c>，设置失败则返回 <c>false</c>。
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="key" /> 或者 <paramref name="value" /> 为 <c>null</c>。
        /// </exception>
        public bool Set(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (key.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(key));
            }

            return _appSettings.Set(key, value);
        }

        #endregion ISettings Members

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
        public float GetFloat(string key)
        {
            string stringValue = GetNullableString(key);

            try
            {
                return stringValue.ToFloat();
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(float).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }
        }

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
        public float GetFloat(string key, float defaultValue)
        {
            string stringValue = GetNullableString(key);

            try
            {
                if (stringValue.IsNotNullOrEmpty())
                {
                    return stringValue.ToFloat();
                }
            }
            catch (Exception ex)
            {
                string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, stringValue, typeof(float).FullName);
                throw new ConfigurationErrorsException(message, ex);
            }

            return defaultValue;
        }

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
        public IList<float> GetFloatList(string key)
        {
            IList<float> setting = new List<float>();
            string stringValue = GetNullableString(key);

            stringValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ForEach(i =>
            {
                try
                {
                    setting.Add(i.ToFloat());
                }
                catch (Exception ex)
                {
                    string message = ERROR_APPSETTING_INVALLID_FORMAT.FormatWith(key, i, typeof(float).FullName);
                    throw new ConfigurationErrorsException(message, ex);
                }
            });

            return setting;
        }

        private string GetNullableString(string key)
        {
            string value = _appSettings.Get(key);
            return NullSettingValue.Equals(value, StringComparison.InvariantCultureIgnoreCase) ? null : value;
        }
    }
}