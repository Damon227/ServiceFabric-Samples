// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : CounterActorService.Interfaces
// File             : ICounterActorService.cs
// Created          : 2017-02-09  19:50
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace CounterActorService.Interfaces
{
    /// <summary>
    ///     This interface defines the methods exposed by an actor.
    ///     Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface ICounterActorService : IActor
    {
        Task<string> GetCountAsync(CancellationToken cancellationToken);

        Task<string> CountAsync(CancellationToken cancellationToken);

        Task ResetAsync(CancellationToken cancellationToken);
    }
}