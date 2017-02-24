// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation
// File             : CoreAppSettings.cs
// Created          : 2017-02-15  14:00
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Microsoft.Extensions.Configuration;

namespace Credit.Kolibre.Foundation.Configuration
{
    /// <summary>
    ///     ASP.NET CORE 项目的配置处理类，继承此类自定义配置的处理。
    /// </summary>
    public sealed class CoreAppSettings : AppSettings
    {
        private readonly IConfiguration _configuration;

        public CoreAppSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override string GetSetting(string key)
        {
            return _configuration[key];
        }
    }
}