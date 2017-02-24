// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Caching
// File             : JsonRedisCacheModel.cs
// Created          : 2016-07-26  11:54
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Credit.Kolibre.Foundation.ServiceFabric.Caching
{
    public class JsonRedisCacheModel
    {
        public DateTimeOffset? AbsoluteExpiration { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }

        public string Payload { get; set; }
    }
}