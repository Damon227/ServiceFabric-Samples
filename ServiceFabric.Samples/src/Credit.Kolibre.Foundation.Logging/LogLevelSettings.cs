// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.Logging
// File             : LogLevelSettings.cs
// Created          : 2016-09-22  8:47 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Credit.Kolibre.Foundation.Logging
{
    public class LogLevelSettings
    {
        private readonly IConfiguration _configuration;
        private readonly string _sectionKey;

        public LogLevelSettings(IConfiguration configuration)
            : this(configuration, "Logging:LogLevel")
        {
        }

        public LogLevelSettings(IConfiguration configuration, string sectionKey)
        {
            _configuration = configuration;
            _sectionKey = sectionKey;
        }

        public bool TryGetSwitch(string name, out LogLevel level)
        {
            IConfigurationSection switches = _configuration.GetSection(_sectionKey);
            if (switches == null)
            {
                level = LogLevel.None;
                return false;
            }

            string defaultLevel = switches["Default"];
            if (defaultLevel.IsNullOrEmpty())
            {
                defaultLevel = "Information";
            }

            string value = switches[name];
            if (value.IsNullOrEmpty())
            {
                value = defaultLevel;
            }

            if (Enum.TryParse(value, out level))
            {
                return true;
            }

            string message = $"Configuration value '{value}' for category '{name}' is not supported.";
            throw new InvalidOperationException(message);
        }
    }
}