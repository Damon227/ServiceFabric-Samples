// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : LogTestClient
// File             : Program.cs
// Created          : 2017-02-15  10:59
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using GodLog.Foundation.Logging.ServiceFabric;
using Microsoft.Extensions.Logging;

namespace LogTestClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new ETWLoggerProvider(serviceContext, filter, () => null, options));
        }
    }
}