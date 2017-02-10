// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : CounterActorClient
// File             : Program.cs
// Created          : 2017-02-10  11:01
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using CounterActorService.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace CounterActorClient
{
    internal class Program
    {
        [SuppressMessage("ReSharper", "FunctionNeverReturns")]
        private static void Main(string[] args)
        {
            ActorId actor1 = new ActorId(1);
            ActorId actor2 = new ActorId("-1");

            ICounterActorService counterActorService = ActorProxy.Create<ICounterActorService>(actor2,
                new Uri("fabric:/SampleDemoApplication/CounterActorServiceActorService"));

            while (true)
            {
                try
                {
                    string result = counterActorService.CountAsync(CancellationToken.None).GetAwaiter().GetResult();
                    Console.WriteLine(result);
                    Task.Delay(TimeSpan.FromSeconds(3)).Wait();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}