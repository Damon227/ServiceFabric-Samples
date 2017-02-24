// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.ServiceFabric.Actors
// File             : StateNameAttribute.cs
// Created          : 2017-01-17  6:03 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Credit.Kolibre.Foundation.ServiceFabric.Actors
{
    [AttributeUsage(AttributeTargets.Property)]
    public class StateNameAttribute : Attribute
    {
        public StateNameAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"Argument {nameof(name)} can not be null or whitespace.", nameof(name));
            }

            Name = name;
        }

        public string Name { get; }
    }
}