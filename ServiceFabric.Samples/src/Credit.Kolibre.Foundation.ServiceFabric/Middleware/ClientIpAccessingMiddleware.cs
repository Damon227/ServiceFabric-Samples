// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : ClientIpAccessingMiddleware.cs
// Created          : 2016-07-20  12:25 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Credit.Kolibre.Foundation.Sys.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;

namespace Credit.Kolibre.Foundation.ServiceFabric.Middleware
{
    public class ClientIpAccessingMiddleware
    {
        private readonly char[] _headerValuesSeparatorDefault = { ',' };
        private readonly char[] _headerValueSeparators;
        private readonly RequestDelegate _next;

        public ClientIpAccessingMiddleware(RequestDelegate next)
        {
            _next = next;
            HeaderNames = new List<string> { Http.Headers.HeaderNames.X_Forwarded_For, Http.Headers.HeaderNames.X_KC_CLIENTIP };
            _headerValueSeparators = _headerValuesSeparatorDefault;
        }

        /// <summary>
        ///     Gets or sets comma separated list of request header names that is used to check client id.
        /// </summary>
        public ICollection<string> HeaderNames { get; }

        public async Task Invoke(HttpContext httpContext)
        {
            string clientIp = GetClientIp(httpContext);
            if (clientIp.IsNotNullOrEmpty())
            {
                httpContext.Items[Constants.X_KC_CLIENTIP] = clientIp;
            }

            await _next.Invoke(httpContext);
        }

        private static string CutPort(string address)
        {
            // For Web sites in Azure header contains ip address with port e.g. 50.47.87.223:54464
            int portSeparatorIndex = address.IndexOf(":", StringComparison.OrdinalIgnoreCase);

            if (portSeparatorIndex > 0)
            {
                return address.Substring(0, portSeparatorIndex);
            }

            return address;
        }

        private string GetClientIp(HttpContext context)
        {
            string resultIp = null;
            foreach (string name in HeaderNames)
            {
                StringValues headerValue = context.Request.Headers[name];
                if (headerValue.IsNotNullOrEmpty())
                {
                    string ip = GetIpFromHeader(headerValue);
                    ip = CutPort(ip);
                    if (IsCorrectIpAddress(ip))
                    {
                        resultIp = ip;
                        break;
                    }
                }
            }

            if (resultIp.IsNullOrEmpty())
            {
                IHttpConnectionFeature connectionFeature = context.Features.Get<IHttpConnectionFeature>();

                if (connectionFeature != null)
                {
                    resultIp = connectionFeature.RemoteIpAddress.ToString();
                }
            }

            if (resultIp.IsNullOrEmpty())
            {
                resultIp = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }

            if (resultIp.IsNullOrEmpty())
            {
                resultIp = context.Connection.RemoteIpAddress.ToString();
            }

            return MapToIPv4(resultIp);
        }

        private string GetIpFromHeader(string clientIpsFromHeader)
        {
            string[] ips = clientIpsFromHeader.Split(_headerValueSeparators, StringSplitOptions.RemoveEmptyEntries);
            return ips[0].Trim();
        }

        private static bool IsCorrectIpAddress(string address)
        {
            IPAddress outParameter;
            address = address.Trim();

            // Core SDK does not support setting Location.Ip to malformed ip address
            if (IPAddress.TryParse(address, out outParameter))
            {
                // Also SDK supports only ipv4!
                if (outParameter.AddressFamily == AddressFamily.InterNetwork)
                {
                    return true;
                }
            }

            return false;
        }


        private static string MapToIPv4(string address)
        {
            if (address.Equals("::1") || address.Equals("localhost", StringComparison.OrdinalIgnoreCase))
            {
                address = "127.0.0.1";
            }

            address = CutPort(address);
            return IsCorrectIpAddress(address) ? address : string.Empty;
        }
    }
}