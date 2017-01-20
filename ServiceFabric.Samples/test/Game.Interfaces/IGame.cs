// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : Game.Interfaces
// File             : IGame.cs
// Created          : 2017-01-18  14:30
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace Game.Interfaces
{
    /// <summary>
    ///     This interface defines the methods exposed by an actor.
    ///     Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IGame : IActor
    {
        Task<bool> JoinGameAsync(long playerId, string playerName);

        Task<int[]> GetGameBoardAsync();

        Task<string> GetWinnerAsync();

        Task<List<Tuple<long, string>>> GetPlayersAsync();

        Task<bool> MakeMoveAsync(long playerId, int x, int y);
    }
}