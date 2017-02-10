// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : CounterActorService
// File             : CounterState.cs
// Created          : 2017-02-10  10:38
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Runtime.Serialization;

namespace CounterActorService
{
    [DataContract]
    public class CounterState
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int CurrentCount { get; set; }

        [DataMember]
        public string CurrentTime { get; set; }
    }
}