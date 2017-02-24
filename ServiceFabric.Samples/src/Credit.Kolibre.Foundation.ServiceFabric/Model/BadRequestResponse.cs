// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : BadRequestResponse.cs
// Created          : 2016-07-20  12:25 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Credit.Kolibre.Foundation.ServiceFabric.Model
{
    /// <summary>
    ///     4XX - BadRequest
    /// </summary>
    public class BadRequestResponse
    {
        /// <summary>
        ///     错误码，如果没有指定会默认为 0。
        /// </summary>
        [Required]
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary>
        ///     报错信息，信息中的内容可以使用被额外数据中的内容进行格式化。
        /// </summary>
        [Required]
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        ///     额外数据，用以格式化报错信息，该数组不会为 null，但是可能不包含任何元素。
        /// </summary>
        [Required]
        [JsonProperty("data")]
        public List<string> Data { get; set; }

        /// <summary>
        ///     报错时的调用堆栈，只有当查询字符串中指定 stacktrace 参数时，返回值中才会有该值。
        /// </summary>
        [JsonProperty("stackTrace")]
        public string StackTrace { get; set; }
    }
}