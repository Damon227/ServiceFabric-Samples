// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : EnumerableExtensions.cs
// Created          : 2016-06-29  11:43 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Credit.Kolibre.Foundation.Static;

namespace Credit.Kolibre.Foundation.Sys.Collections.Generic
{
    /// <summary>
    ///     <see cref="System.Collections.Generic.IEnumerable{T}" /> 的扩展类。
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     对 <paramref name="sequence" /> 的每一个元素执行指定操作。
        /// </summary>
        /// <typeparam name="T"><paramref name="sequence" /> 中的元素类型。</typeparam>
        /// <param name="sequence">要对其元素执行操作。</param>
        /// <param name="action">要执行的指定操作。</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="sequence" /> 或者 <paramref name="action" /> 为 <c>null</c>。
        /// </exception>
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence), SR.ArgumentNull_Generic);
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action), SR.ArgumentNull_Generic);
            }

            IList<T> values = sequence as IList<T> ?? sequence.ToList();
            foreach (T value in values)
            {
                action(value);
            }
        }

        /// <summary>
        ///     对 <paramref name="sequence" /> 的每一个元素执行指定操作。
        /// </summary>
        /// <typeparam name="T"><paramref name="sequence" /> 中的元素类型。</typeparam>
        /// <typeparam name="TResult">指定操作的返回值类型。</typeparam>
        /// <returns>对 <paramref name="sequence" /> 的每一个元素执行指定操作后的返回值按原顺序组成的序列。</returns>
        /// <param name="sequence">要对其元素执行操作。</param>
        /// <param name="action">要执行的指定操作。</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="sequence" /> 或者 <paramref name="action" /> 为 <c>null</c>。
        /// </exception>
        public static IEnumerable<TResult> ForEach<T, TResult>(this IEnumerable<T> sequence, Func<T, TResult> action)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence), SR.ArgumentNull_Generic);
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action), SR.ArgumentNull_Generic);
            }

            IList<T> values = sequence as IList<T> ?? sequence.ToList();
            return values.Select(value => action(value)).ToList();
        }

        /// <summary>
        ///     对 <paramref name="sequence" /> 的每一个元素执行指定操作。
        /// </summary>
        /// <typeparam name="T"><paramref name="sequence" /> 中的元素类型。</typeparam>
        /// <param name="sequence">要对其元素执行操作。</param>
        /// <param name="action">要执行的指定操作。</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="sequence" /> 或者 <paramref name="action" /> 为 <c>null</c>。
        /// </exception>
        public static Task ForEach<T>(this IEnumerable<T> sequence, Func<T, Task> action)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence), SR.ArgumentNull_Generic);
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action), SR.ArgumentNull_Generic);
            }

            IList<T> values = sequence as IList<T> ?? sequence.ToList();
            IEnumerable<Task> tasks = values.Select(value => action(value));
            return Task.WhenAll(tasks);
        }

        /// <summary>
        ///     对 <paramref name="sequence" /> 的每一个元素执行指定操作。
        /// </summary>
        /// <typeparam name="T"><paramref name="sequence" /> 中的元素类型。</typeparam>
        /// <typeparam name="TResult">指定操作的返回值类型。</typeparam>
        /// <returns>对 <paramref name="sequence" /> 的每一个元素执行指定操作后的返回值按原顺序组成的序列。</returns>
        /// <param name="sequence">要对其元素执行操作。</param>
        /// <param name="action">要执行的指定操作。</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="sequence" /> 或者 <paramref name="action" /> 为 <c>null</c>。
        /// </exception>
        public static async Task<IEnumerable<TResult>> ForEach<T, TResult>(this IEnumerable<T> sequence, Func<T, Task<TResult>> action)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence), SR.ArgumentNull_Generic);
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action), SR.ArgumentNull_Generic);
            }

            IList<T> values = sequence as IList<T> ?? sequence.ToList();
            IList<TResult> results = new List<TResult>();
            foreach (T value in values)
            {
                results.Add(await action(value));
            }
            return results;
        }

        /// <summary>
        ///     对 <paramref name="sequence" /> 按照指定的单页元素数量进行分页，并且取指定的页码索引中的元素序列。第一页的页码索引为0。
        /// </summary>
        /// <typeparam name="T"><paramref name="sequence" /> 中的元素类型。</typeparam>
        /// <param name="sequence">元素序列。</param>
        /// <param name="pageIndex">指定的页码索引。第一页的页码索引为0。</param>
        /// <param name="pageSize">指定的单页元素数量。</param>
        /// <returns>指定页码索引的元素序列。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="sequence" /> 为 <c>null</c>。
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     <paramref name="pageIndex" /> 不能为负值。
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     <paramref name="pageSize" /> 不能为负值。
        /// </exception>
        public static IEnumerable<T> GetPage<T>(this IEnumerable<T> sequence, int pageIndex, int pageSize)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence), SR.ArgumentNull_Generic);
            }
            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex), pageIndex, SR.ArgumentOutOfRange_MustBeNonNegNum);
            }
            if (pageSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, SR.ArgumentOutOfRange_MustBeNonNegNum);
            }

            IList<T> values = sequence as IList<T> ?? sequence.ToList();
            return values.Skip(pageIndex * pageSize).Take(pageSize);
        }

        /// <summary>
        ///     指示 <paramref name="sequence" /> 是否不是 <c>null</c> 并且元素数量不为0。
        /// </summary>
        /// <typeparam name="T"><paramref name="sequence" /> 中的元素类型。</typeparam>
        /// <param name="sequence">元素序列。</param>
        /// <returns> 如果 <paramref name="sequence" />不是 <c>null</c> 并且元素数量不为0，则返回 <c>true</c>；否则，返回<c>false</c>。</returns>
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> sequence)
        {
            return sequence != null && sequence.Any();
        }

        /// <summary>
        ///     指示 <paramref name="sequence" /> 是否是 <c>null</c> 或者元素数量为0。
        /// </summary>
        /// <param name="sequence">元素序列。</param>
        /// <returns> 如果 <paramref name="sequence" />是 <c>null</c> 或者元素数量为0，则返回 <c>true</c>；否则，返回<c>false</c>。</returns>
        public static bool IsNullOrEmpty(this IEnumerable sequence)
        {
            return sequence == null || sequence.IsSequenceNullOrEmpty();
        }

        /// <summary>
        ///     指示 <paramref name="sequence" /> 是否是 <c>null</c> 或者元素数量为0。
        /// </summary>
        /// <typeparam name="T"><paramref name="sequence" /> 中的元素类型。</typeparam>
        /// <param name="sequence">元素序列。</param>
        /// <returns> 如果 <paramref name="sequence" />是 <c>null</c> 或者元素数量为0，则返回 <c>true</c>；否则，返回<c>false</c>。</returns>
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null)
            {
                return true;
            }

            ICollection<T> collection = sequence as ICollection<T>;

            if (collection != null)
            {
                // We expect this to be the normal flow.
                return collection.Count == 0;
            }

            // We expect this to be the exceptional flow, because most collections implement
            // ICollection<T>.
            return sequence.IsSequenceNullOrEmpty();
        }

        /// <summary>
        ///     将 <paramref name="sequence" /> 拼接为一个字符串，并且使用 <paramref name="separator" /> 作为分隔符。如果 <paramref name="sequence" /> 为 <c>null</c>，则返回 <paramref name="nullValue" />。
        /// </summary>
        /// <typeparam name="T"><paramref name="sequence" /> 中的元素类型。</typeparam>
        /// <param name="sequence">原元素序列。</param>
        /// <param name="separator">分隔符，默认为", "。</param>
        /// <param name="nullValue">如果 <paramref name="sequence" /> 为 <c>null</c>，需要返回的值。</param>
        /// <returns> 拼接后的字符串。</returns>
        public static string Join<T>(this IEnumerable<T> sequence, string separator = ", ", string nullValue = null)
        {
            separator = separator ?? ",";
            return sequence == null ? nullValue : string.Join(separator, sequence);
        }

        /// <summary>
        ///     初始化 <see cref="System.Collections.Generic.HashSet{T}" /> 类的一个新实例，该实例使用集类型的默认相等比较器，包含从指定的集合复制的元素，并且有足够的容量容纳所复制的这些元素。
        /// </summary>
        /// <typeparam name="T">集合中元素的类型。</typeparam>
        /// <param name="items">元素被复制到新集合中的集合。</param>
        /// <remarks>
        ///     容量 <see cref="System.Collections.Generic.HashSet{T}" /> 对象是该对象可以容纳的元素数目。一个 <see cref="System.Collections.Generic.HashSet{T}" /> 对象的容量将自动增加如元素添加到对象。
        ///     如果 collection 包含重复的元素，该集将包含一个唯一的每个元素。将不引发任何异常。因此，结果集的大小并不完全相同的大小 collection。
        ///     此构造函数是 O(n) 操作，其中 n 是 <paramref name="items" /> 中的元素数。
        /// </remarks>
        /// <returns>新的集合，其中包含所有从 <paramref name="items" /> 中复制来的元素。</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="items" /> 为 <c>null</c>。
        /// </exception>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            return new HashSet<T>(items);
        }

        /// <summary>
        ///     将 <paramref name="sequence" /> 转换为只读集合。
        /// </summary>
        /// <typeparam name="T"><paramref name="sequence" /> 中的元素类型。</typeparam>
        /// <param name="sequence">原元素序列。</param>
        /// <returns> 只读的集合。</returns>
        public static IEnumerable<T> ToReadOnlyCollection<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence), SR.ArgumentNull_Generic);
            }

            return new ReadOnlyCollection<T>(sequence.ToList());
        }

        /// <summary>
        ///     返回一个可以表示集合内容的，并且可阅读性更强的字符串。如果集合为 <c>null</c>，则返回 "null" 或者 "[]"。
        /// </summary>
        /// <typeparam name="T"><paramref name="collection" /> 集合中元素的类型。</typeparam>
        /// <param name="collection">要描述的集合。</param>
        /// <param name="toString">可以指定的针对每个元素转换为字符串的方法。</param>
        /// <param name="separator">每个元素之间的分隔符，默认为", "。</param>
        /// <param name="putInBrackets">元素是否使用"[]"包裹。</param>
        /// <returns>可以表示集合的可阅读字符串。</returns>
        public static string ToString<T>(this IEnumerable<T> collection, Func<T, string> toString = null,
            string separator = ", ", bool putInBrackets = true)
        {
            if (collection == null)
            {
                return putInBrackets ? "[]" : "null";
            }

            StringBuilder sb = new StringBuilder();
            if (putInBrackets)
            {
                sb.Append("[");
            }

            IEnumerator<T> enumerator = collection.GetEnumerator();
            bool firstDone = false;
            while (enumerator.MoveNext())
            {
                T value = enumerator.Current;
                string val;
                if (toString != null)
                {
                    val = toString(value);
                }
                else
                {
                    val = value == null ? "null" : value.ToString();
                }

                if (firstDone)
                {
                    sb.Append(separator);
                    sb.Append(val);
                }
                else
                {
                    sb.Append(val);
                    firstDone = true;
                }
            }

            if (putInBrackets)
            {
                sb.Append("]");
            }

            return sb.ToString();
        }

        private static bool IsEnumerableEmpty(IEnumerable sequence)
        {
            IEnumerator enumerator = sequence.GetEnumerator();

            try
            {
                return !enumerator.MoveNext();
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                disposable?.Dispose();
            }
        }

        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
        private static bool IsSequenceNullOrEmpty(this IEnumerable sequence)
        {
            if (sequence == null)
            {
                return true;
            }

            ICollection collection = sequence as ICollection;

            if (collection != null)
            {
                // We expect this to be the normal flow.
                return collection.Count == 0;
            }

            // We expect this to be the exceptional flow, because most collections implement ICollection.
            return IsEnumerableEmpty(sequence);
        }
    }
}