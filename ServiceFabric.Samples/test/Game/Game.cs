// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : Game
// File             : Game.cs
// Created          : 2017-01-18  14:30
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Data;

namespace Game
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
    internal class Game : Actor, IGame
    {
        private readonly string s_stateKey = "MyGame";

        /// <summary>
        ///     Initializes a new instance of Game
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public Game(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        private ActorState ActorState { get; set; }

        #region IGame Members

        public Task<bool> JoinGameAsync(long playerId, string playerName)
        {
            if (ActorState.Players.Count >= 2 || ActorState.Players.FirstOrDefault(p => p.Item2 == playerName) != null)
            {
                return Task.FromResult(false);
            }

            ActorState.Players.Add(new Tuple<long, string>(playerId, playerName));

            StateManager.SetStateAsync(s_stateKey, ActorState);

            return Task.FromResult(true);
        }

        public Task<int[]> GetGameBoardAsync()
        {
            return Task.FromResult(ActorState.Board);
        }

        public Task<string> GetWinnerAsync()
        {
            return Task.FromResult(ActorState.Winner);
        }

        public Task<List<Tuple<long, string>>> GetPlayersAsync()
        {
            return Task.FromResult(ActorState.Players);
        }

        public Task<bool> MakeMoveAsync(long playerId, int x, int y)
        {
            if (x < 0 || x > 2 || y < 0 || y > 2
                || ActorState.Players.Count != 2
                || ActorState.NumberOfMoves >= 9
                || ActorState.Winner != "")
            {
                return Task.FromResult(false);
            }

            int index = ActorState.Players.FindIndex(p => p.Item1 == playerId);
            if (index == ActorState.NextPlayerIndex)
            {
                if (ActorState.Board[y * 3 + x] == 0)
                {
                    int piece = index * 2 - 1;
                    ActorState.Board[y * 3 + x] = piece;
                    ActorState.NumberOfMoves++;

                    if (HasWon(piece * 3))
                        ActorState.Winner = ActorState.Players[index].Item2 + " (" + (piece == -1 ? "X" : "O") + ")";

                    else if (ActorState.Winner == "" && ActorState.NumberOfMoves >= 9)
                        ActorState.Winner = "TIE";

                    ActorState.NextPlayerIndex = (ActorState.NextPlayerIndex + 1) % 2;

                    StateManager.SetStateAsync(s_stateKey, ActorState);

                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
            return Task.FromResult(false);
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

            ConditionalValue<ActorState> state = StateManager.TryGetStateAsync<ActorState>(s_stateKey).GetAwaiter().GetResult();
            if (!state.HasValue)
            {
                ActorState = new ActorState
                {
                    Board = new int[9],
                    Winner = null,
                    Players = new List<Tuple<long, string>>(),
                    NextPlayerIndex = 0,
                    NumberOfMoves = 0
                };

                StateManager.TryAddStateAsync(s_stateKey, ActorState);
            }
            else
            {
                ActorState = state.Value;
            }

            return Task.FromResult(true);
        }


        private bool HasWon(int sum)
        {
            return ActorState.Board[0] + ActorState.Board[1] + ActorState.Board[2] == sum
                   || ActorState.Board[3] + ActorState.Board[4] + ActorState.Board[5] == sum
                   || ActorState.Board[6] + ActorState.Board[7] + ActorState.Board[8] == sum
                   || ActorState.Board[0] + ActorState.Board[3] + ActorState.Board[6] == sum
                   || ActorState.Board[1] + ActorState.Board[4] + ActorState.Board[7] == sum
                   || ActorState.Board[2] + ActorState.Board[5] + ActorState.Board[8] == sum
                   || ActorState.Board[0] + ActorState.Board[4] + ActorState.Board[8] == sum
                   || ActorState.Board[2] + ActorState.Board[4] + ActorState.Board[6] == sum;
        }
    }
}