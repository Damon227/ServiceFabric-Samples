// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Caching
// File             : ListRedisCacheModel.cs
// Created          : 2016-11-26  14:37
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Credit.Kolibre.Foundation.ServiceFabric.Caching
{
    public class ListRedisCacheModel
    {
        public DateTimeOffset? AbsoluteExpiration { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }

        public string Payload { get; set; }
    }
}