// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : GodLog.Foundation.Logging
// File             : ConsoleLoggerOptions.cs
// Created          : 2017-02-14  15:29
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GodLog.Foundation.Logging
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