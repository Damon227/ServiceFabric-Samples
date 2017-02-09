// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : CounterStatelessService.Interface
// File             : ICounterStatelessService.cs
// Created          : 2017-02-08  10:39
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace CounterStatelessService.Interface
{
    public interface ICounterStatelessService : IService
    {
        Task<string> CountAsync();

        Task ResetAsync();
    }
}