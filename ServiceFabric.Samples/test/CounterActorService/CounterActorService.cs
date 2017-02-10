// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : CounterActorService
// File             : CounterActorService.cs
// Created          : 2017-02-09  19:50
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;
using CounterActorService.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Data;
using Newtonsoft.Json;

namespace CounterActorService
{
    /// <remarks>
    ///     This class represents an actor.
    ///     Every ActorID maps to an instance of this class.
    ///     The StatePersistence attribute determines persistence and replication of actor state:
    ///     - Persisted: State is written to disk and replicated.
    ///     - Volatile: State is kept in memory only and replicated.
    ///     - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class CounterActorService : Actor, ICounterActorService
    {
        /// <summary>
        ///     Initializes a new instance of CounterActorService
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public CounterActorService(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        #region ICounterActorService Members

        public async Task<string> GetCountAsync(CancellationToken cancellationToken)
        {
            ConditionalValue<CounterState> result = await StateManager.TryGetStateAsync<CounterState>("Counter", cancellationToken);

            CounterState counterState = result.HasValue ? result.Value : null;

            string state = JsonConvert.SerializeObject(counterState);

            return $"Current state is {state}, from partition {ActorService.Context.PartitionId} and replica {ActorService.Context.ReplicaId}";
        }

        public async Task<string> CountAsync(CancellationToken cancellationToken)
        {
            ConditionalValue<CounterState> result = await StateManager.TryGetStateAsync<CounterState>("Counter", cancellationToken);

            CounterState counterState = result.HasValue ? result.Value : new CounterState();

            counterState.CurrentCount ++;
            counterState.CurrentTime = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss");
            await StateManager.SetStateAsync("Counter", counterState, cancellationToken);

            string state = JsonConvert.SerializeObject(counterState);

            return $"Current state is {state}, from partition {ActorService.Context.PartitionId} and replica {ActorService.Context.ReplicaId}";
        }

        public async Task ResetAsync(CancellationToken cancellationToken)
        {
            ConditionalValue<CounterState> result = await StateManager.TryGetStateAsync<CounterState>("Counter", cancellationToken);

            CounterState counterState = result.HasValue ? result.Value : new CounterState();

            counterState.CurrentCount = 0;
            counterState.CurrentTime = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss");
            await StateManager.SetStateAsync("Counter", counterState, cancellationToken);
        }

        #endregion

        /// <summary>
        ///     This method is called whenever an actor is activated.
        ///     An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            // The StateManager is this actor's private state store.
            // Data stored in the StateManager will be replicated for high-availability for actors that use volatile or persisted state storage.
            // Any serializable object can be saved in the StateManager.
            // For more information, see https://aka.ms/servicefabricactorsstateserialization

            return StateManager.TryAddStateAsync("count", 0);
        }
    }
}