// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.Logging
// File             : TraceLoggerOptions.cs
// Created          : 2016-09-26  10:24 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.Logging
{
    public class TraceLoggerOptions : IOptions<TraceLoggerOptions>
    {
        public LogLevel MinLevel { get; set; }

        #region IOptions<TraceLoggerOptions> Members

        TraceLoggerOptions IOptions<TraceLoggerOptions>.Value
        {
            get { return this; }
        }

        #endregion
    }
}