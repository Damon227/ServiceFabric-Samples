// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.ServiceFabric.Actors
// File             : KolibreActor.cs
// Created          : 2017-01-18  7:13 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace Credit.Kolibre.Foundation.ServiceFabric.Actors
{
    public abstract class KolibreActor : Actor
    {
        protected KolibreActor(ActorService actorService, ActorId actorId) : base(actorService, actorId)
        {
        }

        protected static Task Done
        {
            get { return Task.FromResult<object>(null); }
        }
    }
}