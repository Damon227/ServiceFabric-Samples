// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.ServiceFabric.Logging
// File             : ETWLoggerOptions.cs
// Created          : 2017-02-15  16:38
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.ServiceFabric.Logging
{
    public class ETWLoggerOptions : IOptions<ETWLoggerOptions>
    {
        public LogLevel MinLevel { get; set; }

        #region IOptions<ETWLoggerOptions> Members

        ETWLoggerOptions IOptions<ETWLoggerOptions>.Value
        {
            get { return this; }
        }

        #endregion
    }
}