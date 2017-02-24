// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : Field.cs
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
    public static class FieldInfoExtensions
    {
        public static IProperty ToIProperty(this FieldInfo info)
        {
            return new Field { FieldInfo = info };
        }
    }

    public class Field : IProperty
    {
        internal FieldInfo FieldInfo { get; set; }

        #region IProperty Members

        public string Name
        {
            get { return FieldInfo.Name; }
        }

        public Type PropertyType
        {
            get { return FieldInfo.FieldType; }
        }

        public object GetValue(object obj, object[] index)
        {
            return FieldInfo.GetValue(obj);
        }

        public void SetValue(object obj, object val, object[] index)
        {
            FieldInfo.SetValue(obj, val);
        }

        #endregion IProperty Members
    }
}