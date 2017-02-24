using System.Collections.Generic;

namespace Credit.Kolibre.Foundation.Sys.Collections.Generic
{
    /// <summary>
    ///     分页列表。
    /// </summary>
    /// <typeparam name="T">列表中元素的类型。</typeparam>
    public interface IPaginatedList<T> : IList<T>
    {
        /// <summary>
        ///     指示是否在原数据中，该页数据是否还有下一页。
        /// </summary>
        bool HasNextPage { get; }

        /// <summary>
        ///     该页数据的页码索引，第一页的页码索引为0。
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        ///     获取分页时指定的单页元素数量。
        /// </summary>
        int PageSize { get; }

        /// <summary>
        ///     获取分页时原数据的元素总数量。
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        ///     获取分页时原数据的元素总页数。
        /// </summary>
        int TotalPageCount { get; }
    }
}