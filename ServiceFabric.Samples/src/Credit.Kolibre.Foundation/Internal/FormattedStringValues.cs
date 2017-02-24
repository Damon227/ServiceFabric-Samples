// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : FormattedStringValues.cs
// Created          : 2016-07-08  12:51 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Credit.Kolibre.Foundation.Internal
{
    /// <summary>
    ///     LogValues to enable formatting options supported by <see cref="M:string.Format" />.
    ///     This also enables using {NamedformatItem} in the format string.
    /// </summary>
    public class FormattedStringValues : IReadOnlyList<KeyValuePair<string, object>>
    {
        private static readonly ConcurrentDictionary<string, StringValuesFormatter> s_formatters = new ConcurrentDictionary<string, StringValuesFormatter>();
        private readonly StringValuesFormatter _formatter;
        private readonly object[] _values;
        private readonly string _originalMessage;

        public FormattedStringValues(string format, params object[] values)
        {
            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

            if (values.Length != 0)
            {
                _formatter = s_formatters.GetOrAdd(format, f => new StringValuesFormatter(f));
            }

            _originalMessage = format;
            _values = values;
        }

        #region IReadOnlyList<KeyValuePair<string,object>> Members

        public KeyValuePair<string, object> this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException(nameof(index));
                }

                if (index == Count - 1)
                {
                    return new KeyValuePair<string, object>("{OriginalFormat}", _originalMessage);
                }

                return _formatter.GetValue(_values, index);
            }
        }

        public int Count
        {
            get
            {
                if (_formatter == null)
                {
                    return 1;
                }

                return _formatter.ValueNames.Count + 1;
            }
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            for (int i = 0; i < Count; ++i)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public override string ToString()
        {
            if (_formatter == null)
            {
                return _originalMessage;
            }

            return _formatter.Format(_values);
        }
    }
}