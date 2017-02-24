// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : Property.cs
// Created          : 2016-06-24  1:56 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Reflection;

#pragma warning disable 1591

namespace Credit.Kolibre.Foundation.Reflection
{
    public static class PropertyInfoExtensions
    {
        public static IProperty ToIProperty(this PropertyInfo info)
        {
            return new Property { PropertyInfo = info };
        }
    }

    public class Property : IProperty
    {
        internal PropertyInfo PropertyInfo { get; set; }

        #region IProperty Members

        public string Name
        {
            get { return PropertyInfo.Name; }
        }

        public Type PropertyType
        {
            get { return PropertyInfo.PropertyType; }
        }

        public object GetValue(object obj, object[] index)
        {
            return PropertyInfo.GetValue(obj, index);
        }

        public void SetValue(object obj, object val, object[] index)
        {
            PropertyInfo.SetValue(obj, val, index);
        }

        #endregion IProperty Members
    }
}