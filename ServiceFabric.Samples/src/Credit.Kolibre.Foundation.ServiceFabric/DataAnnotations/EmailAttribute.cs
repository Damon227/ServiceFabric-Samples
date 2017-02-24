// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : EmailAttribute.cs
// Created          : 2016-07-08  11:51 PM
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
    public class EmailAttribute : DataTypeAttribute
    {
        private static readonly Regex s_regex = REGEX.EMAIL_REGEX;

        public EmailAttribute() : base(DataType.EmailAddress)
        {
            ErrorMessage = "{0}的格式错误。";
        }


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