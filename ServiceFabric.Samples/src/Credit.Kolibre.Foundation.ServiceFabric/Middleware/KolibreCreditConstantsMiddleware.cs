// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : KolibreCreditConstantsMiddleware.cs
// Created          : 2016-06-30  10:04 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Linq;
using System.Threading.Tasks;
using Credit.Kolibre.Foundation.ServiceFabric.Http.Headers;
using Credit.Kolibre.Foundation.Sys;
using Credit.Kolibre.Foundation.Utilities;
using Microsoft.AspNetCore.Http;

namespace Credit.Kolibre.Foundation.ServiceFabric.Middleware
{
    public class KolibreCreditConstantsMiddleware
    {
        private static readonly CookieOptions s_cookieOptions = new CookieOptions { Domain = ".kolibre.credit", Expires = DateTimeOffset.MaxValue, HttpOnly = false, Secure = false };
        private static readonly CookieOptions s_cookieRemoveOptions = new CookieOptions { Domain = ".kolibre.credit", Expires = DateTimeOffset.UtcNow.AddYears(-10), HttpOnly = false, Secure = false };
        private readonly RequestDelegate _next;

        public KolibreCreditConstantsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        ///     Invokes the logic of the middleware.
        /// </summary>
        /// <param name="httpContext">The <see cref="HttpContext" />.</param>
        /// <returns>A <see cref="Task" /> that completes when the middleware has completed processing.</returns>
        public async Task Invoke(HttpContext httpContext)
        {
            GetOrInitTrackingValue(httpContext, Constants.X_KC_CLIENTIP);
            GetOrInitTrackingValue(httpContext, Constants.X_KC_CLIENTTYPE, "unknown");
            GetOrInitTrackingValue(httpContext, Constants.X_KC_HOST);
            GetOrInitTrackingValue(httpContext, Constants.X_KC_PAGENAME, "unknown");
            GetOrInitTrackingValue(httpContext, Constants.X_KC_SOURCE);

            GetOrInitTrackingValue(httpContext, Constants.X_KC_DEVICEID, ID.NewSequentialGuid().ToGuidString());
            GetOrInitTrackingValue(httpContext, Constants.X_KC_REQUESTID, ID.NewSequentialGuid().ToGuidString(), true);
            // Session Id 会在Session中间件中初始化
            GetOrInitTrackingValue(httpContext, Constants.X_KC_SESSIONID);
            // User Id 会在认证过程中被更新
            GetOrInitTrackingValue(httpContext, Constants.X_KC_USERID, ID.NewSequentialGuid().ToGuidString());

            httpContext.Response.OnStarting(() =>
            {
                httpContext.Response.Headers.Append(HeaderNames.X_KC_TIME, DateTimeOffset.UtcNow.ToString("R"));
                UpdateTrackingValues(httpContext);
                return Task.FromResult(0);
            });

            await _next.Invoke(httpContext);
        }

        public static void UpdateTrackingValues(HttpContext httpContext)
        {
            UpdateTrackingValueToResponse(httpContext, Constants.X_KC_DEVICEID);
            UpdateTrackingValueToResponse(httpContext, Constants.X_KC_REQUESTID);
            UpdateTrackingValueToResponse(httpContext, Constants.X_KC_SESSIONID);
            UpdateTrackingValueToResponse(httpContext, Constants.X_KC_USERID);
        }

        private static string GetOrInitTrackingValue(HttpContext httpContext, string name, string initValue = null, bool forceInit = false)
        {
            string value = null;
            object item;
            if (httpContext.Items.TryGetValue(name, out item) && item is string)
            {
                value = item.ToString();
            }

            if (value.IsNullOrEmpty() && !forceInit)
            {
                value = httpContext.Request.Headers[name];
            }

            if (value.IsNullOrEmpty() && !forceInit)
            {
                if (httpContext.Request.Cookies != null && httpContext.Request.Cookies.ContainsKey(name))
                {
                    string cookie = httpContext.Request.Cookies[name];
                    if (cookie.IsNotNullOrEmpty())
                    {
                        string[] cookieParts = cookie.Split(new[] { ',', ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
                        value = cookieParts.LastOrDefault();
                    }
                }
            }

            if (value.IsNullOrEmpty())
            {
                value = initValue;
            }

            if (value.IsNotNullOrEmpty())
            {
                httpContext.Request.Headers[name] = value;
                httpContext.Items[name] = value;
            }

            return value;
        }

        private static void UpdateTrackingValueToResponse(HttpContext httpContext, string name)
        {
            string value = null;
            object item;
            if (httpContext.Items.TryGetValue(name, out item) && item is string)
            {
                value = item.ToString();
            }

            if (value.IsNullOrEmpty())
            {
                httpContext.Response.Headers[name] = "DELETE";
                httpContext.Response.Cookies.Append(name, "DELETE", s_cookieRemoveOptions);
            }
            else
            {
                httpContext.Response.Headers[name] = value;
                httpContext.Response.Cookies.Append(name, value, s_cookieOptions);
            }
        }
    }
}