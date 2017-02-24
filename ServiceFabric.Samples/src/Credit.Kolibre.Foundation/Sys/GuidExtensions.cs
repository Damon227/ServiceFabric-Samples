using System;

namespace Credit.Kolibre.Foundation.Sys
{
    /// <summary>
    ///     <see cref="System.Guid" /> 的扩展类。
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        ///     将指定的 <see cref="System.Guid" /> 实例转化为字符串形式。会使用 "N" 作为格式化标识符，并且返回的字符串是全大写。
        /// </summary>
        /// <param name="value">指定的 <see cref="System.Guid" /> 实例。</param>
        /// <example>DFF7732A0948496DA560473AE83AAFB4</example>
        /// <returns>转换后的字符串。</returns>
        public static string ToGuidString(this Guid value)
        {
            return value.ToString("N").ToUpperInvariant();
        }
    }
}