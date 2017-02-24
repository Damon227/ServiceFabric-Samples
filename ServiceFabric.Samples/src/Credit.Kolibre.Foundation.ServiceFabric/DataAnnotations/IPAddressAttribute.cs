// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : IPAddressAttribute.cs
// Created          : 2016-07-01  12:00 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Credit.Kolibre.Foundation.Static;

namespace Credit.Kolibre.Foundation.ServiceFabric.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class IPAddressAttribute : DataTypeAttribute
    {
        private static readonly Regex s_regex = REGEX.IP_ADDRESS_REGEX;

        public IPAddressAttribute() : base(DataType.Custom)
        {
        }


        /// <summary>Checks that the value of the data field is valid.</summary>
        /// <returns>true always.</returns>
        /// <param name="value">The data field value to validate.</param>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string input = value as string;
            return s_regex != null && input != null && s_regex.Match(input).Length > 0;
        }
    }
}