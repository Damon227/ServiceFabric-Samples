// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation
// File             : SETTING.CS
// Created          : 2017-02-15  14:00
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

#pragma warning disable 1591

namespace Credit.Kolibre.Foundation.Static
{
    public static class SETTING
    {
        public static readonly JsonSerializerSettings EXCEPTION_JSON_SETTINGS = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            //DefaultValueHandling = DefaultValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
            MaxDepth = 100,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static readonly JsonSerializerSettings LOG_JSON_SETTINGS = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            //DefaultValueHandling = DefaultValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
            MaxDepth = 100,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static readonly JsonSerializerSettings DATA_JSON_SETTINGS = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            //DefaultValueHandling = DefaultValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
            MaxDepth = 100,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static readonly JsonSerializerSettings REDIS_CACHE_JSON_SETTINGS = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            //DefaultValueHandling = DefaultValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Include,
            MaxDepth = 100,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static readonly JsonSerializerSettings WEB_API_JSON_SETTINGS = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            //DefaultValueHandling = DefaultValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = "yyyy-MM-dd hh:mm:ss",
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Include
        };

        public static readonly JsonSerializerSettings DATA_IGNORE_DEFAULT_VALUE_JSON_SETTINGS = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
            MaxDepth = 100,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
    }
}