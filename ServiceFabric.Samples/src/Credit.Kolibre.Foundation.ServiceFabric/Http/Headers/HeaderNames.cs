// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : HeaderNames.cs
// Created          : 2016-07-20  12:25 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Diagnostics.CodeAnalysis;

namespace Credit.Kolibre.Foundation.ServiceFabric.Http.Headers
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class HeaderNames
    {
        public const string Accept = "Accept";
        public const string AcceptCharset = "Accept-Charset";
        public const string AcceptEncoding = "Accept-Encoding";
        public const string AcceptLanguage = "Accept-Language";
        public const string AcceptRanges = "Accept-Ranges";
        public const string Age = "Age";
        public const string Allow = "Allow";
        public const string Authorization = "Authorization";
        public const string CacheControl = "Cache-Control";
        public const string Connection = "Connection";
        public const string ContentDisposition = "Content-Disposition";
        public const string ContentEncoding = "Content-Encoding";
        public const string ContentLanguage = "Content-Language";
        public const string ContentLength = "Content-Length";
        public const string ContentLocation = "Content-Location";
        public const string ContentMD5 = "ContentMD5";
        public const string ContentRange = "Content-Range";
        public const string ContentType = "Content-Type";
        public const string Cookie = "Cookie";
        public const string Date = "Date";
        public const string ETag = "ETag";
        public const string Expect = "Expect";
        public const string Expires = "Expires";
        public const string From = "From";
        public const string Host = "Host";
        public const string IfMatch = "If-Match";
        public const string IfModifiedSince = "If-Modified-Since";
        public const string IfNoneMatch = "If-None-Match";
        public const string IfRange = "If-Range";
        public const string IfUnmodifiedSince = "If-Unmodified-Since";
        public const string LastModified = "Last-Modified";
        public const string Location = "Location";
        public const string MaxForwards = "Max-Forwards";
        public const string Pragma = "Pragma";
        public const string ProxyAuthenticate = "Proxy-Authenticate";
        public const string ProxyAuthorization = "Proxy-Authorization";
        public const string Range = "Range";
        public const string Referer = "Referer";
        public const string RetryAfter = "Retry-After";
        public const string Server = "Server";
        public const string SetCookie = "Set-Cookie";
        public const string TE = "TE";
        public const string Trailer = "Trailer";
        public const string TransferEncoding = "Transfer-Encoding";
        public const string Upgrade = "Upgrade";
        public const string UserAgent = "User-Agent";
        public const string Vary = "Vary";
        public const string Via = "Via";
        public const string Warning = "Warning";
        public const string WebSocketSubProtocols = "Sec-WebSocket-Protocol";
        public const string WWWAuthenticate = "WWW-Authenticate";

        public const string X_Forwarded_For = "X-Forwarded-For";

        // Kolibre Credit Headers
        public const string X_KC_APIKEY = Constants.X_KC_APIKEY;
        public const string X_KC_CLIENTIP = Constants.X_KC_CLIENTIP;
        public const string X_KC_CLIENTTYPE = Constants.X_KC_CLIENTTYPE;
        public const string X_KC_DEVICEID = Constants.X_KC_DEVICEID;
        public const string X_KC_HOST = Constants.X_KC_HOST;
        public const string X_KC_MD5 = Constants.X_KC_MD5;
        public const string X_KC_PAGENAME = Constants.X_KC_PAGENAME;
        public const string X_KC_REQUESTID = Constants.X_KC_REQUESTID;
        public const string X_KC_SESSIONID = Constants.X_KC_SESSIONID;
        public const string X_KC_SOURCE = Constants.X_KC_SOURCE;
        public const string X_KC_TIME = "X-KC-TIME";
        public const string X_KC_USERID = Constants.X_KC_USERID;
    }
}