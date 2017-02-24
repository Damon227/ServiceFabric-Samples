// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : Extensions.cs
// Created          : 2016-06-30  8:20 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Credit.Kolibre.Foundation.ServiceFabric.Http
{
    /// <summary>
    ///     Set of extension methods for <see cref="Microsoft.AspNetCore.Http.HttpRequest" />.
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        ///     Gets http request Uri from request object
        /// </summary>
        /// <param name="request">request</param>
        /// <returns>A New Uri object representing request Uri</returns>
        public static Uri GetUri(this HttpRequest request)
        {
            if (null == request)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.Scheme))
            {
                throw new ArgumentException("Http request Scheme is not specified");
            }

            if (!request.Host.HasValue)
            {
                throw new ArgumentException("Http request Host is not specified");
            }

            StringBuilder builder = new StringBuilder();

            builder.Append(request.Scheme)
                .Append("://")
                .Append(request.Host);

            if (request.Path.HasValue)
            {
                builder.Append(request.Path.Value);
            }

            if (request.QueryString.HasValue)
            {
                builder.Append(request.QueryString);
            }

            return new Uri(builder.ToString());
        }
    }
}