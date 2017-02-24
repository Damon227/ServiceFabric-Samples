// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : InternalServerErrorExceptionHandlerMiddleware.cs
// Created          : 2016-08-23  10:04 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Credit.Kolibre.Foundation.Logging;
using Credit.Kolibre.Foundation.ServiceFabric.Model;
using Credit.Kolibre.Foundation.Static;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Credit.Kolibre.Foundation.ServiceFabric.Middleware
{
    public class InternalServerErrorExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InternalServerErrorExceptionHandlerMiddleware" /> class
        /// </summary>
        /// <param name="next"></param>
        public InternalServerErrorExceptionHandlerMiddleware(RequestDelegate next)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            _next = next;
        }

        /// <summary>
        ///     Process an individual request.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            InternalServerErrorResponse response = new InternalServerErrorResponse
            {
                Code = EventCode.CREDIT_KOLIBRE_FOUNDATION_ASPNETCORE_ERROR_UNEXPECTED,
                Data = new List<string>(),
                Message = string.Empty,
                StackTrace = string.Empty
            };

            try
            {
                await _next(httpContext);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                if (httpContext.Response.HasStarted)
                {
                    throw;
                }

                response.Data.Add("RequestId:" + GetRequestId(httpContext));

                response.Code = EventCode.CREDIT_KOLIBRE_FOUNDATION_ASPNETCORE_ERROR_UNEXPECTED;
                response.Message = dbUpdateConcurrencyException.Message;

                if (httpContext.Request.Query.ContainsKey("debug"))
                {
                    response.StackTrace = dbUpdateConcurrencyException.StackTrace;
                }

                httpContext.Response.Clear();
                httpContext.Response.StatusCode = 500;
                httpContext.Response.ContentType = "application/json; charset=utf-8";
                await httpContext.Response.WriteAsync(response.ToJson(SETTING.WEB_API_JSON_SETTINGS));
            }
            catch (Exception e)
            {
                if (httpContext.Response.HasStarted)
                {
                    throw;
                }

                response.Data.Add("RequestId:" + GetRequestId(httpContext));

                response.Message = e.Message;

                if (httpContext.Request.Query.ContainsKey("debug"))
                {
                    response.StackTrace = e.StackTrace;
                }

                httpContext.Response.Clear();
                httpContext.Response.StatusCode = 500;
                httpContext.Response.ContentType = "application/json; charset=utf-8";
                await httpContext.Response.WriteAsync(response.ToJson(SETTING.WEB_API_JSON_SETTINGS));
            }
        }

        private static string GetRequestId(HttpContext httpContext)
        {
            object requestIdItem;
            if (httpContext.Items.TryGetValue(Constants.X_KC_REQUESTID, out requestIdItem))
            {
                return requestIdItem.ToString();
            }

            return string.Empty;
        }
    }
}