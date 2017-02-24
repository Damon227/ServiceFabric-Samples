// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : ReflectionUsingDynamicExtensions.cs
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
    public static class ReflectionUsingDynamicExtensions
    {
        public static dynamic AsDynamic(this object o)
        {
            // Don't wrap primitive types, which don't have many interesting internal APIs
            if (o == null || o.GetType().IsPrimitive || o is string || o is ReflectionDynamicObjectBase)
                return o;

            return new ReflectionDynamicInstanceObject(o);
        }

        public static dynamic AsDynamicType(this Type type)
        {
            return new ReflectionDynamicStaticObject(type);
        }
    }
}