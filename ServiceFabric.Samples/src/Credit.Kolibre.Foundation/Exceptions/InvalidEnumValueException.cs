// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : InvalidEnumValueException.cs
// Created          : 2016-08-20  6:44 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Runtime.Serialization;
using Credit.Kolibre.Foundation.Static;

namespace Credit.Kolibre.Foundation.Exceptions
{
    public class InvalidEnumValueException : InvalidOperationException
    {
        public InvalidEnumValueException(string initValue, Type enumType) : base(SR.InvalidOperation_InvalidEnumValue)
        {
            InitValue = initValue;
            EnumType = enumType;
        }

        public InvalidEnumValueException(string s, string initValue, Type enumType) : base(s)
        {
            InitValue = initValue;
            EnumType = enumType;
        }

        public InvalidEnumValueException(string s, string initValue, Type enumType, Exception e) : base(s, e)
        {
            InitValue = initValue;
            EnumType = enumType;
        }

        public InvalidEnumValueException(SerializationInfo info, StreamingContext cxt) : base(info, cxt)
        {
        }

        public string InitValue { get; }

        public Type EnumType { get; }
    }
}