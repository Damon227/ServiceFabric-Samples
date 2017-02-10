// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : CounterStatefuleService.Interface
// File             : ICounterStatefuleService.cs
// Created          : 2017-02-09  11:45
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace CounterStatefuleService.Interface
{
    public interface ICounterStatefuleService : IService
    {
        Task<string> CountAsync();

        Task ResetAsync();

        Task<string> GetCountAsync();
    }
}