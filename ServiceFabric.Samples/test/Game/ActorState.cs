// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : Game
// File             : ActorState.cs
// Created          : 2017-01-18  15:00
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Game
{
    [DataContract]
    public class ActorState
    {
        [DataMember]
        public int[] Board { get; set; }

        [DataMember]
        public string Winner { get; set; }

        [DataMember]
        public List<Tuple<long, string>> Players { get; set; }

        [DataMember]
        public int NextPlayerIndex { get; set; }

        [DataMember]
        public int NumberOfMoves { get; set; }
    }
}