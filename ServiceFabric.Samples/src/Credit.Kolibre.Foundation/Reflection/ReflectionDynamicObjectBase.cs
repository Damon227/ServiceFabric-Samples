﻿// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : ReflectionDynamicObjectBase.cs
// Created          : 2016-06-27  2:07 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Credit.Kolibre.Foundation.Static;
using Credit.Kolibre.Foundation.Sys;

#pragma warning disable 1591

namespace Credit.Kolibre.Foundation.Reflection
{
    public abstract class ReflectionDynamicObjectBase : DynamicObject
    {
        public abstract object RealObject { get; }

        // We need to virtualize this so we use a different cache for instance and static props
        internal abstract IDictionary<Type, IDictionary<string, IProperty>> PropertiesOnType { get; }

        protected abstract BindingFlags BindingFlags { get; }

        protected abstract object Instance { get; }

        protected abstract Type TargetType { get; }

        public override string ToString()
        {
            return Instance.ToString();
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder == null)
            {
                throw new ArgumentNullException(nameof(binder));
            }

            result = binder.Type.IsInstanceOfType(RealObject) ? RealObject : Convert.ChangeType(RealObject, binder.Type);
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes == null)
            {
                throw new ArgumentNullException(nameof(indexes));
            }
            if (indexes.Length > 0)
            {
                throw new ArgumentException(SR.Argument_ArrayZeroError);
            }

            IProperty prop = GetIndexProperty(indexes.FirstOrDefault()?.ToString());
            result = prop.GetValue(Instance, indexes);

            // Wrap the sub object if necessary. This allows nested anonymous objects to work.
            result = result.AsDynamic();

            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder == null)
            {
                throw new ArgumentNullException(nameof(binder));
            }

            IProperty prop = GetProperty(binder.Name);

            // Get the property value
            result = prop.GetValue(Instance, null);

            // Wrap the sub object if necessary. This allows nested anonymous objects to work.
            result = result.AsDynamic();

            return true;
        }

        // Called when a method is called
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (binder == null)
            {
                throw new ArgumentNullException(nameof(binder));
            }
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            for (int i = 0; i < args.Length; i++)
            {
                args[i] = Unwrap(args[i]);
            }

            Type csharpBinder = binder.GetType().GetInterface("Microsoft.CSharp.RuntimeBinder.ICSharpInvokeOrInvokeMemberBinder");
            IList<Type> typeArgs = csharpBinder.GetProperty("TypeArguments").GetValue(binder, null) as IList<Type>;
            result = InvokeMethodOnType(TargetType, Instance, binder.Name, args, typeArgs);

            // Wrap the sub object if necessary. This allows nested anonymous objects to work.
            result = result.AsDynamic();

            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (indexes == null)
            {
                throw new ArgumentNullException(nameof(indexes));
            }

            IProperty prop = GetIndexProperty(indexes.FirstOrDefault()?.ToString());
            prop.SetValue(Instance, Unwrap(value), indexes);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (binder == null)
            {
                throw new ArgumentNullException(nameof(binder));
            }

            IProperty prop = GetProperty(binder.Name);

            // Set the property value.  Make sure to unwrap it first if it's one of our dynamic objects
            prop.SetValue(Instance, Unwrap(value), null);

            return true;
        }

        private static object InvokeMethodOnType(Type type, object target, string name, object[] args, ICollection<Type> typeArgs)
        {
            MethodInfo method = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                .FirstOrDefault(a => a.Name == name && ParametersCompatible(a, args, typeArgs));

            if (method == null)
            {
                return InvokeMethodOnType(type.BaseType, target, name, args, typeArgs);
            }

            if (typeArgs.Count > 0)
            {
                method = method.MakeGenericMethod(typeArgs.ToArray());
            }

            return method.Invoke(target, args);
        }

        private static bool ParametersCompatible(MethodInfo method, IReadOnlyList<object> @params, ICollection<Type> typeArgs)
        {
            if (typeArgs != null && typeArgs.Count > 0)
            {
                method = method.MakeGenericMethod(typeArgs.ToArray());
            }

            ParameterInfo[] parameterInfos = method.GetParameters();
            if (parameterInfos.Length != @params.Count)
            {
                return false;
            }
            return !parameterInfos.Where((t, i) => !((@params[i] == null && !t.ParameterType.IsValueType) || t.ParameterType.IsInstanceOfType(@params[i]))).Any();
        }

        private static object Unwrap(object o)
        {
            ReflectionDynamicObjectBase dynObject = o as ReflectionDynamicObjectBase;

            // If it's a wrap object, unwrap it and return the real thing
            // Otherwise, return it unchanged
            return dynObject != null ? dynObject.RealObject : o;
        }

        private IProperty GetIndexProperty(string propertyName)
        {
            // The index property is always named "Item" in C#
            if (propertyName.IsNullOrEmpty())
            {
                propertyName = "Item";
            }

            return GetProperty(propertyName);
        }

        private IProperty GetProperty(string propertyName)
        {
            // Get the list of properties and fields for this type
            IDictionary<string, IProperty> typeProperties = GetTypeProperties(TargetType);

            // Look for the one we want
            IProperty property;
            if (typeProperties.TryGetValue(propertyName, out property))
            {
                return property;
            }

            // The property doesn't exist

            // Get a list of supported properties and fields and show them as part of the exception message
            // For fields, skip the auto property backing fields (which name start with <)
            IOrderedEnumerable<string> propNames = typeProperties.Keys.Where(name => name[0] != '<').OrderBy(name => name);
            throw new ArgumentException($"The property {propertyName} doesn't exist on type {TargetType}. Supported properties are: {string.Join(", ", propNames)}");
        }

        private IDictionary<string, IProperty> GetTypeProperties(Type type)
        {
            // First, check if we already have it cached
            IDictionary<string, IProperty> typeProperties;
            if (PropertiesOnType.TryGetValue(type, out typeProperties))
            {
                return typeProperties;
            }

            // Not cached, so we need to build it
            typeProperties = new ConcurrentDictionary<string, IProperty>();

            // First, recurse on the base class to add its fields
            if (type.BaseType != null)
            {
                foreach (IProperty prop in GetTypeProperties(type.BaseType).Values)
                {
                    typeProperties[prop.Name] = prop;
                }
            }

            // Then, add all the properties from the current type
            foreach (PropertyInfo prop in type.GetProperties(BindingFlags).Where(p => p.DeclaringType == type))
            {
                typeProperties[prop.Name] = new Property { PropertyInfo = prop };
            }

            // Finally, add all the fields from the current type
            foreach (FieldInfo field in type.GetFields(BindingFlags).Where(p => p.DeclaringType == type))
            {
                typeProperties[field.Name] = new Field { FieldInfo = field };
            }

            // Cache it for next time
            PropertiesOnType[type] = typeProperties;

            return typeProperties;
        }
    }
}