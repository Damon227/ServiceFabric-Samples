// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : Player
// File             : Player.cs
// Created          : 2017-01-13  20:29
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Game.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Runtime;
using Player.Interfaces;

namespace Player
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
    internal class Player : Actor, IPlayer
    {
        /// <summary>
        ///     Initializes a new instance of Player
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public Player(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        #region IPlayer Members

        public Task<bool> JoinGameAsync(ActorId gameId, string playerName)
        {
            IGame game = ActorProxy.Create<IGame>(gameId, new Uri("fabric:/ActorTicTacToeApplication/GameActorService"));

            return game.JoinGameAsync(Id.GetLongId(), playerName);
        }

        public Task<bool> MakeMoveAsync(ActorId gameId, int x, int y)
        {
            IGame game = ActorProxy.Create<IGame>(gameId, new Uri("fabric:/ActorTicTacToeApplication/GameActorService"));

            return game.MakeMoveAsync(Id.GetLongId(), x, y);
        }

        public Task<string> GetCurrentInstanceAsync(int x, int y)
        {
            string s = $"Excuted at {Id.GetLongId()}. Result is : " + (x + y);
            return Task.FromResult(s);
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