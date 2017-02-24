// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation
// File             : AppSettings.cs
// Created          : 2017-02-15  14:00
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Concurrent;
using Credit.Kolibre.Foundation.Static;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.Azure;

namespace Credit.Kolibre.Foundation.Configuration
{
    /// <summary>
    ///     配置处理类，继承此类自定义配置的处理。
    /// </summary>
    public class AppSettings : IAppSettings
    {
        private readonly ConcurrentDictionary<string, string> _settings = new ConcurrentDictionary<string, string>();

        #region IAppSettings Members

        /// <summary>
        ///     添加一个新的指定的配置值，如果该设置项已经存在，则添加失败，不会对配置值有任何影响。
        ///     通过该方法设置的配置值，不会更新到配置文件中，只是会保留在应用的配置类中。
        ///     <paramref name="key" /> 不能为 <c>null</c>。
        /// </summary>
        /// <param name="key">配置项的 key，不能为 <c>null</c>。</param>
        /// <param name="value">配置项的值。</param>
        /// <returns>
        ///     添加成功则返回 <c>true</c>，设置失败则返回 <c>false</c>。
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="key" /> 为 <c>null</c>。
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="key" /> 为空字符串或者仅包含空格。
        /// </exception>
        public virtual bool Add(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (key.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(key));
            }

            string setValue = _settings.AddOrUpdate(key, k => value, (k, ov) => ov);

            return string.Equals(setValue, value, StringComparison.Ordinal);
        }

        /// <summary>
        ///     获取指定的配置项中原始的字符串。如果找不到指定的配置项，则返回 <c>null</c>。<paramref name="key" /> 不能为 <c>null</c>。
        /// </summary>
        /// <param name="key">配置项的 key，不能为 <c>null</c>。</param>
        /// <returns>
        ///     返回指定的配置项中原始的字符串。如果找不到指定的配置项，则返回 <c>null</c>。
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="key" /> 为 <c>null</c>。
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="key" /> 为空字符串或者仅包含空格。
        /// </exception>
        public virtual string Get(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (key.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(key));
            }

            string value;
            if (_settings.TryGetValue(key, out value))
            {
                return value;
            }

            value = _settings[key] = GetSetting(key);

            return value;
        }

        /// <summary>
        ///     设置指定的配置值，如果该设置项已经存在，则覆盖原配置值。只能通过字符串的形式设置，设置时要考虑使用时的格式。
        ///     通过该方法设置的配置值，不会更新到配置文件中，只是会保留在应用的配置类中。
        ///     <paramref name="key" /> 不能为 <c>null</c>。
        /// </summary>
        /// <param name="key">配置项的 key，不能为 <c>null</c>。</param>
        /// <param name="value">配置项的值。</param>
        /// <returns>
        ///     设置成功则返回 <c>true</c>，设置失败则返回 <c>false</c>。
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="key" /> 为 <c>null</c>。
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="key" /> 为空字符串或者仅包含空格。
        /// </exception>
        public virtual bool Set(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (key.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(key));
            }

            _settings[key] = value;

            return true;
        }

        #endregion

        protected virtual string GetSetting(string key)
        {
            return CloudConfigurationManager.GetSetting(key, true);
        }
    }
}