// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : ReflectionDynamicStaticObject.cs
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
    public class ReflectionDynamicStaticObject : ReflectionDynamicObjectBase
    {
        private static readonly IDictionary<Type, IDictionary<string, IProperty>> s_propertiesOnType = new ConcurrentDictionary<Type, IDictionary<string, IProperty>>();

        public ReflectionDynamicStaticObject(Type type)
        {
            TargetType = type;
        }

        public override object RealObject
        {
            get { return TargetType; }
        }

        internal override IDictionary<Type, IDictionary<string, IProperty>> PropertiesOnType
        {
            get { return s_propertiesOnType; }
        }

        protected override BindingFlags BindingFlags
        {
            get { return BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic; }
        }

        protected override object Instance
        {
            get { return null; }
        }

        // For static calls, we have the type and the instance is always null
        protected override Type TargetType { get; }

        public dynamic New(params object[] args)
        {
            return Activator.CreateInstance(TargetType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, args, null).AsDynamic();
        }
    }
}