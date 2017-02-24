// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : REGEX.cs
// Created          : 2016-06-27  2:07 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Text.RegularExpressions;

#pragma warning disable 1591

namespace Credit.Kolibre.Foundation.Static
{
    public static class REGEX
    {
        public static readonly Regex CELLPHONE_REGEX = new Regex(CONST.CELLPHONE_REGEX_STRING, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, TimeSpan.FromMinutes(2));

        public static readonly Regex COMPLEX_PASSWORD_REGEX = new Regex(CONST.COMPLEX_PASSWORD_REGEX_STRING, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, TimeSpan.FromMinutes(2));

        public static readonly Regex DATETIME_STRING_REGEX = new Regex(CONST.DATETIME_STRING_REGEX_STRING, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, TimeSpan.FromMinutes(2));

        public static readonly Regex EMAIL_REGEX = new Regex(CONST.EMAIL_REGEX_STRING, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, TimeSpan.FromMinutes(2));

        public static readonly Regex ID_CARD_REGEX = new Regex(CONST.ID_CARD_REGEX_STRING, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, TimeSpan.FromMinutes(2));

        public static readonly Regex IP_ADDRESS_REGEX = new Regex(CONST.IP_ADDRESS_REGEX_STRING, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, TimeSpan.FromMinutes(2));

        public static readonly Regex NUMERIC_PASSWORD_REGEX = new Regex(CONST.NUMERIC_PASSWORD_REGEX_STRING, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, TimeSpan.FromMinutes(2));

        public static readonly Regex SIMPLE_PASSWORD_REGEX = new Regex(CONST.SIMPLE_PASSWORD_REGEX_STRING, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, TimeSpan.FromMinutes(2));

        public static readonly Regex URL_REGEX = new Regex(CONST.IP_ADDRESS_REGEX_STRING, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, TimeSpan.FromMinutes(2));
    }
}