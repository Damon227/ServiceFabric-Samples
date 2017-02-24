// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : IProperty.cs
// Created          : 2016-06-24  1:56 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

#pragma warning disable 1591

namespace Credit.Kolibre.Foundation.Reflection
{
    /// <summary>
    ///     Simple abstraction to make field and property access consistent
    /// </summary>
    public interface IProperty
    {
        string Name { get; }

        Type PropertyType { get; }

        object GetValue(object obj, object[] index);

        void SetValue(object obj, object val, object[] index);
    }
}