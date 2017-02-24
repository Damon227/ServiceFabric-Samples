// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Caching
// File             : IDistributedRedisCache.cs
// Created          : 2016-08-01  20:58
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Microsoft.Extensions.Caching.Distributed;

namespace Credit.Kolibre.Foundation.ServiceFabric.Caching
{
    public interface IDistributedRedisCache : IDistributedCache
    {
    }
}