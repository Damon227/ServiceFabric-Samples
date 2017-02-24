// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : CounterStatefuleService
// File             : CounterStatefuleService.cs
// Created          : 2017-02-09  11:41
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using CounterStatefuleService.Interface;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace CounterStatefuleService
{
    /// <summary>
    ///     An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class CounterStatefuleService : StatefulService, ICounterStatefuleService
    {
        public CounterStatefuleService(StatefulServiceContext context)
            : base(context)
        {
        }

        #region ICounterStatefuleService Members

        public async Task<string> CountAsync()
        {
            IReliableDictionary<string, int> states = await StateManager.GetOrAddAsync<IReliableDictionary<string, int>>("states");
            IReliableQueue<string> events = await StateManager.GetOrAddAsync<IReliableQueue<string>>("events");

            int number;

            using (ITransaction tx = StateManager.CreateTransaction())
            {
                ConditionalValue<int> counter = await states.TryGetValueAsync(tx, "Counter");
                number = counter.HasValue ? counter.Value : 0;
                number++;

                await states.SetAsync(tx, "Counter", number);
                await events.EnqueueAsync(tx, $"{DateTimeOffset.UtcNow:O} The Counter is {number}.");

                await tx.CommitAsync();
            }

            return $"Current number is {number}, from partition {Context.PartitionId} and replica {Context.ReplicaId}.";
        }

        public async Task ResetAsync()
        {
            IReliableDictionary<string, int> states = await StateManager.GetOrAddAsync<IReliableDictionary<string, int>>("states");
            IReliableQueue<string> events = await StateManager.GetOrAddAsync<IReliableQueue<string>>("events");

            using (ITransaction tx = StateManager.CreateTransaction())
            {
                await states.SetAsync(tx, "Counter", 0);
                await events.EnqueueAsync(tx, $"{DateTimeOffset.UtcNow:O} The Counter is reset.");

                await tx.CommitAsync();
            }
        }

        public async Task<string> GetCountAsync()
        {
            IReliableDictionary<string, int> states = await StateManager.GetOrAddAsync<IReliableDictionary<string, int>>("states");

            int number;

            using (ITransaction tx = StateManager.CreateTransaction())
            {
                ConditionalValue<int> result = await states.TryGetValueAsync(tx, "Counter");

                number = result.HasValue ? result.Value : -1;

                await tx.CommitAsync();
            }

            return $"Current number is {number}, from partition {Context.PartitionId} and replica {Context.ReplicaId}.";
        }

        #endregion

        /// <summary>
        ///     Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        ///     For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(context =>
                    new FabricTransportServiceRemotingListener(context, this))
            };
        }

        /// <summary>
        ///     This is the main entry point for your service replica.
        ///     This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        [SuppressMessage("ReSharper", "FunctionNeverReturns")]
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ServiceEventSource.Current.ServiceMessage(Context, "Working-{0}", ++iterations);


                //ILogger<CounterStatefuleService> logger = loggerFactory.CreateLogger<CounterStatefuleService>();
                //logger.LogInformation(1, DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}