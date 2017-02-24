// ***********************************************************************
// Solution         : GodLog
// Project          : GodLog.Foundation.Logging
// File             : LogLevelSettings.cs
// Created          : 2017-02-14  14:50
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GodLog.Foundation.Logging
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
            if (string.IsNullOrEmpty(defaultLevel))
            {
                defaultLevel = "Information";
            }

            string value = switches[name];
            if (string.IsNullOrEmpty(value))
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