// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : StringValuesFormatter.cs
// Created          : 2016-07-08  12:52 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Credit.Kolibre.Foundation.Internal
{
    /// <summary>
    ///     Formatter to convert the named format items like {NamedformatItem} to <see cref="M:string.Format" /> format.
    /// </summary>
    public class StringValuesFormatter
    {
        private readonly string _format;

        public StringValuesFormatter(string format)
        {
            OriginalFormat = format;

            StringBuilder sb = new StringBuilder();
            int scanIndex = 0;
            int endIndex = format.Length;

            while (scanIndex < endIndex)
            {
                int openBraceIndex = FindBraceIndex(format, '{', scanIndex, endIndex);
                int closeBraceIndex = FindBraceIndex(format, '}', openBraceIndex, endIndex);

                // Format item syntax : { index[,alignment][ :formatString] }.
                int formatDelimiterIndex = FindIndexOf(format, ',', openBraceIndex, closeBraceIndex);
                if (formatDelimiterIndex == closeBraceIndex)
                {
                    formatDelimiterIndex = FindIndexOf(format, ':', openBraceIndex, closeBraceIndex);
                }

                if (closeBraceIndex == endIndex)
                {
                    sb.Append(format, scanIndex, endIndex - scanIndex);
                    scanIndex = endIndex;
                }
                else
                {
                    sb.Append(format, scanIndex, openBraceIndex - scanIndex + 1);
                    sb.Append(ValueNames.Count.ToString(CultureInfo.InvariantCulture));
                    ValueNames.Add(format.Substring(openBraceIndex + 1, formatDelimiterIndex - openBraceIndex - 1));
                    sb.Append(format, formatDelimiterIndex, closeBraceIndex - formatDelimiterIndex + 1);

                    scanIndex = closeBraceIndex + 1;
                }
            }

            _format = sb.ToString();
        }

        public string OriginalFormat { get; }
        public List<string> ValueNames { get; } = new List<string>();

        public string Format(object[] values)
        {
            if (values != null)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    object value = values[i];

                    if (value == null)
                    {
                        continue;
                    }

                    // since 'string' implements IEnumerable, special case it
                    if (value is string)
                    {
                        continue;
                    }

                    // if the value implements IEnumerable, build a comma separated string.
                    IEnumerable enumerable = value as IEnumerable;
                    if (enumerable != null)
                    {
                        values[i] = string.Join(", ", enumerable.Cast<object>().Where(obj => obj != null));
                    }
                }
            }

            return string.Format(CultureInfo.InvariantCulture, _format, values);
        }

        public KeyValuePair<string, object> GetValue(object[] values, int index)
        {
            if (index < 0 || index > ValueNames.Count)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }

            if (ValueNames.Count > index)
            {
                return new KeyValuePair<string, object>(ValueNames[index], values[index]);
            }

            return new KeyValuePair<string, object>("{OriginalFormat}", OriginalFormat);
        }

        public IEnumerable<KeyValuePair<string, object>> GetValues(object[] values)
        {
            KeyValuePair<string, object>[] valueArray = new KeyValuePair<string, object>[values.Length + 1];
            for (int index = 0; index != ValueNames.Count; ++index)
            {
                valueArray[index] = new KeyValuePair<string, object>(ValueNames[index], values[index]);
            }

            valueArray[valueArray.Length - 1] = new KeyValuePair<string, object>("{OriginalFormat}", OriginalFormat);
            return valueArray;
        }

        private static int FindBraceIndex(string format, char brace, int startIndex, int endIndex)
        {
            // Example: {{prefix{{{Argument}}}suffix}}.
            int braceIndex = endIndex;
            int scanIndex = startIndex;
            int braceOccurenceCount = 0;

            while (scanIndex < endIndex)
            {
                if (braceOccurenceCount > 0 && format[scanIndex] != brace)
                {
                    if (braceOccurenceCount % 2 == 0)
                    {
                        // Even number of '{' or '}' found. Proceed search with next occurence of '{' or '}'.
                        braceOccurenceCount = 0;
                        braceIndex = endIndex;
                    }
                    else
                    {
                        // An unescaped '{' or '}' found.
                        break;
                    }
                }
                else if (format[scanIndex] == brace)
                {
                    if (brace == '}')
                    {
                        if (braceOccurenceCount == 0)
                        {
                            // For '}' pick the first occurence.
                            braceIndex = scanIndex;
                        }
                    }
                    else
                    {
                        // For '{' pick the last occurence.
                        braceIndex = scanIndex;
                    }

                    braceOccurenceCount++;
                }

                scanIndex++;
            }

            return braceIndex;
        }

        private static int FindIndexOf(string format, char ch, int startIndex, int endIndex)
        {
            int findIndex = format.IndexOf(ch, startIndex, endIndex - startIndex);
            return findIndex == -1 ? endIndex : findIndex;
        }
    }
}