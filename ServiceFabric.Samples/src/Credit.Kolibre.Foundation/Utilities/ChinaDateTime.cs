// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : ChinaDateTime.cs
// Created          : 2016-07-15  1:18 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Credit.Kolibre.Foundation.Sys;

namespace Credit.Kolibre.Foundation.Utilities
{
    public class ChinaDateTime
    {
        public static DateTimeOffset NowOffset => DateTimeOffset.UtcNow.ToChinaStandardTime();

        public static DateTime Now => DateTime.UtcNow.ToChinaStandardTime();

        public static string NowOffsetString => DateTimeOffset.UtcNow.ToChinaStandardTime().ToReadableString();

        public static string NowString => DateTime.UtcNow.ToChinaStandardTime().ToReadableString();
    }
}