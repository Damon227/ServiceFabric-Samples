// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.AspNet
// File             : AvailableValuesAttribute.cs
// Created          : 2017-02-15  15:01
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace Credit.Kolibre.Foundation.ServiceFabric.DataAnnotations
{
    /// <summary>
    ///     Determines whether the specified value of the object is valid.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class AvailableValuesAttribute : ValidationAttribute
    {
        private readonly string[] _availableValues;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AvailableValuesAttribute" /> class.
        /// </summary>
        /// <param name="values">The values.</param>
        public AvailableValuesAttribute(params object[] values)
            : base(@"The {0} value is not available.")
        {
            _availableValues = values.Select(v => v.ToString()).ToArray();
        }

        /// <summary>
        ///     Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <returns>
        ///     An instance of the formatted error message.
        /// </returns>
        /// <param name="name">The name to include in the formatted message.</param>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
        }

        /// <summary>
        ///     Determines whether the specified value of the object is valid.
        /// </summary>
        /// <returns>
        ///     true if the specified value is valid; otherwise, false.
        /// </returns>
        /// <param name="value">The value of the object to validate. </param>
        public override bool IsValid(object value)
        {
            return value != null && _availableValues.Contains(value.ToString());
        }
    }
}