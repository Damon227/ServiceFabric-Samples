// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : ExceptionExtensions.cs
// Created          : 2016-06-29  11:43 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using Credit.Kolibre.Foundation.Static;

namespace Credit.Kolibre.Foundation.Sys
{
    /// <summary>
    ///     <see cref="System.Exception" /> 的扩展类。
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        ///     将 <see cref="System.Reflection.ReflectionTypeLoadException" /> 转换为 <see cref="System.AggregateException" />
        /// </summary>
        public static AggregateException FlattenToAggregateException(this ReflectionTypeLoadException exception)
        {
            // if ReflectionTypeLoadException is thrown, we need to provide the
            // LoaderExceptions property in order to make it meaningful.
            List<Exception> all = new List<Exception> { exception };
            all.AddRange(exception.LoaderExceptions);
            throw new AggregateException("A ReflectionTypeLoadException has been thrown. The original exception and the contents of the LoaderExceptions property have been aggregated for your convenience.", all);
        }

        /// <summary>
        ///     获取 <see cref="System.Exception" /> 的详细错误信息。
        /// </summary>
        public static string GetExceptionString(this Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            CreateExceptionString(sb, exception, "  ");
            return sb.ToString();
        }

        private static void CreateExceptionString(StringBuilder sb, Exception exception, string indent)
        {
            sb = sb ?? new StringBuilder();

            while (true)
            {
                if (exception == null)
                {
                    throw new ArgumentNullException(nameof(exception));
                }

                if (indent == null)
                {
                    indent = string.Empty;
                }
                else if (indent.Length > 0)
                {
                    sb.Append($"{indent}Inner ");
                }

                sb.AppendLine("Exception(s) Found:");
                sb.AppendLine($"{indent}Type: {exception.GetType().FullName}");
                sb.AppendLine($"{indent}Message: {exception.Message}");
                sb.AppendLine($"{indent}Source: {exception.Source}");
                sb.AppendLine($"{indent}DataJson: {exception.Data.ToJson("\\N", SETTING.EXCEPTION_JSON_SETTINGS)}");
                sb.AppendLine($"{indent}Stacktrace:");
                sb.AppendLine($"{exception.StackTrace}");

                if (exception is ReflectionTypeLoadException)
                {
                    Exception[] loaderExceptions = ((ReflectionTypeLoadException)exception).LoaderExceptions;
                    if (loaderExceptions.Length == 0)
                    {
                        sb.AppendLine($"{indent}No LoaderExceptions found.");
                    }
                    else
                    {
                        foreach (Exception e in loaderExceptions)
                        {
                            CreateExceptionString(sb, e, indent + "  ");
                        }
                    }
                }
                else if (exception is AggregateException)
                {
                    ReadOnlyCollection<Exception> innerExceptions = ((AggregateException)exception).InnerExceptions;
                    if (innerExceptions.Count == 0)
                    {
                        sb.AppendLine($"{indent}No InnerExceptions found.");
                    }
                    else
                    {
                        foreach (Exception e in innerExceptions)
                        {
                            CreateExceptionString(sb, e, indent + "  ");
                        }
                    }
                }
                else if (exception.InnerException != null)
                {
                    sb.AppendLine();
                    exception = exception.InnerException;
                    indent = indent + "  ";
                    continue;
                }
                break;
            }
        }
    }
}