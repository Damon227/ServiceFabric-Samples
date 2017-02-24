// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : IAppSettings.cs
// Created          : 2016-06-24  6:13 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Credit.Kolibre.Foundation.Configuration
{
    /// <summary>
    ///     用于定义配置处理类的接口。如果只是需要使用或者获取配置，请使用 <see cref="Credit.Kolibre.Foundation.Configuration.ISettings" />。
    /// </summary>
    public interface IAppSettings
    {
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
        bool Add(string key, string value);

        /// <summary>
        ///     获取指定的配置项中原始的字符串。如果找不到指定的配置项，则返回 <c>null</c>。<paramref name="key" /> 不能为 <c>null</c>。
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
        string Get(string key);

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
        bool Set(string key, string value);
    }
}