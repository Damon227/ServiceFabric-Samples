// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : Player.Interfaces
// File             : IPlayer.cs
// Created          : 2017-01-13  20:29
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace Player.Interfaces
{
    /// <summary>
    ///     This interface defines the methods exposed by an actor.
    ///     Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IPlayer : IActor
    {
        Task<bool> JoinGameAsync(ActorId gameId, string playerName);

        Task<bool> MakeMoveAsync(ActorId gameId, int x, int y);

        Task<string> GetCurrentInstanceAsync(int x, int y);
    }
}