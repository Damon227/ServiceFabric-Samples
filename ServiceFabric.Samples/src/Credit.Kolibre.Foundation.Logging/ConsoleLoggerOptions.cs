// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.Logging
// File             : ConsoleLoggerOptions.cs
// Created          : 2016-09-26  3:53 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.Logging
{
    public class ConsoleLoggerOptions : IOptions<ConsoleLoggerOptions>
    {
        public bool Colored { get; set; }

        public LogLevel MinLevel { get; set; }

        #region IOptions<ConsoleLoggerOptions> Members

        ConsoleLoggerOptions IOptions<ConsoleLoggerOptions>.Value
        {
            get { return this; }
        }

        #endregion
    }
}