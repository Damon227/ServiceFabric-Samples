// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : ReflectionDynamicInstanceObject.cs
// Created          : 2016-06-24  1:56 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

#pragma warning disable 1591

namespace Credit.Kolibre.Foundation.Reflection
{
    public class ReflectionDynamicInstanceObject : ReflectionDynamicObjectBase
    {
        private static readonly IDictionary<Type, IDictionary<string, IProperty>> s_propertiesOnType = new ConcurrentDictionary<Type, IDictionary<string, IProperty>>();
        private readonly object _instance;

        public ReflectionDynamicInstanceObject(object instance)
        {
            _instance = instance;
        }

        public override object RealObject
        {
            get { return Instance; }
        }

        internal override IDictionary<Type, IDictionary<string, IProperty>> PropertiesOnType
        {
            get { return s_propertiesOnType; }
        }

        protected override BindingFlags BindingFlags
        {
            get { return BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic; }
        }

        protected override object Instance
        {
            get { return _instance; }
        }

        // For instance calls, we get the type from the instance
        protected override Type TargetType
        {
            get { return _instance.GetType(); }
        }
    }
}