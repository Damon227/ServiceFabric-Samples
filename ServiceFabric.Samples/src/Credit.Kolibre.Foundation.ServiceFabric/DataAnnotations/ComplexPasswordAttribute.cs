// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : ComplexPasswordAttribute.cs
// Created          : 2016-07-01  11:58 AM
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
    public sealed class ComplexPasswordAttribute : DataTypeAttribute
    {
        private static readonly Regex s_regex = REGEX.COMPLEX_PASSWORD_REGEX;

        public ComplexPasswordAttribute() : base(DataType.Password)
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