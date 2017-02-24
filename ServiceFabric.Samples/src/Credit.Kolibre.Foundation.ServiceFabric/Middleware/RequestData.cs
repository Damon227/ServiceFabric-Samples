// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : RequestData.cs
// Created          : 2016-06-27  11:10 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Credit.Kolibre.Foundation.ServiceFabric.Middleware
{
    public class RequestData
    {
        public string App { get; set; }

        public string AppVersion { get; set; }

        public string ClientIPAddress { get; set; }

        public string ClientType { get; set; }

        public string DeviceModel { get; set; }

        public string DeviceType { get; set; }

        public string Duration { get; set; }

        public string Env { get; set; }

        public string EventTime { get; set; }

        public string HttpMethod { get; set; }

        public string PageName { get; set; }

        public string RequestId { get; set; }

        public string RequestName { get; set; }

        public string RequestTime { get; set; }

        public string RequestUrl { get; set; }

        public string ResponseCode { get; set; }

        public string ResponseTime { get; set; }

        public string RoleInstance { get; set; }

        public string RoleName { get; set; }

        public string Success { get; set; }
    }
}