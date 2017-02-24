// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.ServiceFabric.Seesion
// File             : SessionMetadata.cs
// Created          : 2017-02-15  19:02
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.AspNetCore.Http;

namespace Credit.Kolibre.Foundation.ServiceFabric.Seesion
{
    public class SessionMetadata
    {
        public string SessionId { get; set; }

        public string UserId { get; set; }

        public string DeviceId { get; set; }

        public string ClientIp { get; set; }

        public string ClientType { get; set; }

        public string Host { get; set; }

        public bool IsFirstRequest { get; set; }

        public DateTimeOffset InitTime { get; set; }

        public DateTimeOffset ExpiryTime { get; set; }

        public string SourceSessionId { get; set; }


        public static SessionMetadata InitSessionMetadata(string sessionId, HttpContext context, TimeSpan expiryTimeSpan)
        {
            return InitSessionMetadata(sessionId, null, context, expiryTimeSpan);
        }

        public static SessionMetadata InitSessionMetadata(string sessionId, string sourceSessionId, HttpContext context, TimeSpan expiryTimeSpan)
        {
            object userId = null;
            context?.Items.TryGetValue(Constants.X_KC_USERID, out userId);
            object deviceId = null;
            context?.Items.TryGetValue(Constants.X_KC_DEVICEID, out deviceId);
            object clientIp = null;
            context?.Items.TryGetValue(Constants.X_KC_CLIENTIP, out clientIp);
            object clientType = null;
            context?.Items.TryGetValue(Constants.X_KC_CLIENTTYPE, out clientType);
            object host = null;
            context?.Items.TryGetValue(Constants.X_KC_HOST, out host);

            return InitSessionMetadata(sessionId, sourceSessionId, userId?.ToString(), deviceId?.ToString(),
                clientIp?.ToString(), clientType?.ToString(), host?.ToString(), expiryTimeSpan);
        }

        public static SessionMetadata InitSessionMetadata(string sessionId, string sourceSessionId, string userId,
            string deviceId, string clientIp, string clientType, string host, TimeSpan expiryTimeSpan)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            SessionMetadata sessionMetadata = new SessionMetadata
            {
                ClientIp = clientIp ?? string.Empty,
                ClientType = clientType ?? string.Empty,
                DeviceId = deviceId ?? string.Empty,
                ExpiryTime = now.Add(expiryTimeSpan),
                Host = host ?? string.Empty,
                InitTime = now,
                IsFirstRequest = true,
                SessionId = sessionId,
                SourceSessionId = sourceSessionId ?? string.Empty,
                UserId = userId ?? string.Empty
            };

            return sessionMetadata;
        }
    }
}