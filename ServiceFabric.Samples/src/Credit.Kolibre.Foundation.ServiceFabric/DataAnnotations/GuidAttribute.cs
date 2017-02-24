// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : GuidAttribute.cs
// Created          : 2016-07-08  11:55 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using Credit.Kolibre.Foundation.Sys;

namespace Credit.Kolibre.Foundation.ServiceFabric.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class GuidAttribute : DataTypeAttribute
    {
        public GuidAttribute() : base(DataType.Text)
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
            return input.IsGuid();
        }
    }
}