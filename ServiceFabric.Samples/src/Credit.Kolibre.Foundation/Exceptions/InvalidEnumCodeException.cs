// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : InvalidEnumCodeException.cs
// Created          : 2016-08-20  6:04 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Runtime.Serialization;
using Credit.Kolibre.Foundation.Static;

namespace Credit.Kolibre.Foundation
{
    [Serializable]
    public class InvalidEnumCodeException : InvalidOperationException
    {
        public InvalidEnumCodeException(int initCode, Type enumType) : base(SR.InvalidOperation_InvalidEnumCode)
        {
            InitCode = initCode;
            EnumType = enumType;
        }

        public InvalidEnumCodeException(string s, int initCode, Type enumType) : base(s)
        {
            InitCode = initCode;
            EnumType = enumType;
        }

        public InvalidEnumCodeException(string s, int initCode, Type enumType, Exception e) : base(s, e)
        {
            InitCode = initCode;
            EnumType = enumType;
        }

        public InvalidEnumCodeException(SerializationInfo info, StreamingContext cxt) : base(info, cxt)
        {
        }

        public int InitCode { get; }

        public Type EnumType { get; }
    }
}