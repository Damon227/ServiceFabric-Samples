// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : GodLog.Foundation.Logging.ServiceFabric
// File             : ETWLoggerOptions.cs
// Created          : 2017-02-14  16:18
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GodLog.Foundation.Logging.ServiceFabric
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