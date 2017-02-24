// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.Logging
// File             : LoggerProvider.cs
// Created          : 2016-10-12  16:11
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Credit.Kolibre.Foundation.Logging
{
    public abstract class LoggerProvider : ILoggerProvider
    {
        protected readonly Func<string, LogLevel, bool> _filter;
        protected readonly Func<string> _operationIdAccessor;
        protected readonly LogLevelSettings _settings;

        protected LoggerProvider(Func<string, LogLevel, bool> filter, Func<string> operationIdAccessor)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (operationIdAccessor == null)
            {
                throw new ArgumentNullException(nameof(operationIdAccessor));
            }

            _filter = filter;
            _operationIdAccessor = operationIdAccessor;
        }

        protected LoggerProvider(IConfiguration configuration, Func<string> operationIdAccessor)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            //if (operationIdAccessor == null)
            //{
            //    throw new ArgumentNullException(nameof(operationIdAccessor));
            //}

            _settings = new LogLevelSettings(configuration, LogLevelSettionsSectionKey);
            _operationIdAccessor = operationIdAccessor;
        }

        protected virtual string LogLevelSettionsSectionKey
        {
            get { return "Logging: LogLevel"; }
        }

        public virtual Func<string> OperationIdAccessor
        {
            get { return _operationIdAccessor ?? (() => string.Empty); }
        }

        #region IDisposable Members

        public virtual void Dispose()
        {
        }

        #endregion

        #region ILoggerProvider Members

        public abstract ILogger CreateLogger(string name);

        #endregion

        protected Func<string, LogLevel, bool> GetFilter()
        {
            if (_filter != null)
            {
                return _filter;
            }

            if (_settings != null)
            {
                return (name, logLevel) =>
                {
                    foreach (string prefix in GetKeyPrefixes(name))
                    {
                        LogLevel level;
                        if (_settings.TryGetSwitch(prefix, out level))
                        {
                            return logLevel >= level;
                        }
                    }

                    return false;
                };
            }

            return (name, logLevel) => false;
        }

        private static IEnumerable<string> GetKeyPrefixes(string name)
        {
            while (!string.IsNullOrEmpty(name))
            {
                yield return name;
                int lastIndexOfDot = name.LastIndexOf('.');
                if (lastIndexOfDot == -1)
                {
                    yield return "Default";
                    break;
                }
                name = name.Substring(0, lastIndexOfDot);
            }
        }
    }
}