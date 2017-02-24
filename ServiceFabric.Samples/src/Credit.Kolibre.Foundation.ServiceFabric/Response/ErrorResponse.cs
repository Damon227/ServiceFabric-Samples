// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : ErrorResponse.cs
// Created          : 2016-07-07  9:27 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Credit.Kolibre.Foundation.ServiceFabric.Response
{
    public class ErrorResponse
    {
        /// <summary>
        ///     错误码
        /// </summary>
        [Required]
        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }

        /// <summary>
        ///     错误信息
        /// </summary>
        [Required]
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        ///     错误的附加信息
        /// </summary>
        [Required]
        [JsonProperty("data")]
        public IDictionary<string, string> Data { get; set; }

        /// <summary>
        ///     错误的调用堆栈信息
        /// </summary>
        [Required]
        [JsonProperty("stack")]
        public string Stack { get; set; }
    }
}