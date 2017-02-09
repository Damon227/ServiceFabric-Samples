// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : CountStatelessClient
// File             : Program.cs
// Created          : 2017-02-08  11:13
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using CounterStatelessService.Interface;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace CountStatelessClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ICounterStatelessService counterStatelessService = ServiceProxy.Create<ICounterStatelessService>(
                new Uri("fabric:/SampleDemoApplication/CounterStatelessService"));

            counterStatelessService.ResetAsync().Wait();

            while (true)
            {
                try
                {
                    Console.WriteLine(counterStatelessService.CountAsync().GetAwaiter().GetResult());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}