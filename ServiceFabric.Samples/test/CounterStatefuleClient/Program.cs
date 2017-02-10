// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : CounterStatefuleClient
// File             : Program.cs
// Created          : 2017-02-09  14:49
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CounterStatefuleService.Interface;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace CounterStatefuleClient
{
    internal class Program
    {
        [SuppressMessage("ReSharper", "FunctionNeverReturns")]
        private static void Main(string[] args)
        {
            ICounterStatefuleService counterStatefuleService = ServiceProxy.Create<ICounterStatefuleService>(
                new Uri("fabric:/SampleDemoApplication/CounterStatefuleService"), new ServicePartitionKey(-1));


            //counterStatefuleService.ResetAsync().Wait();

            while (true)
            {
                string result = counterStatefuleService.CountAsync().GetAwaiter().GetResult();

                Console.WriteLine(result);

                Task.Delay(TimeSpan.FromSeconds(3)).Wait();
            }
        }
    }
}