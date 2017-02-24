// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : SdkVersionUtils.cs
// Created          : 2016-06-29  12:57 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Linq;
using System.Reflection;

namespace Credit.Kolibre.Foundation.ServiceFabric.Utilities
{
    public class SdkVersionUtils
    {
        internal static string GetAssemblyVersion()
        {
            return typeof(SdkVersionUtils).GetTypeInfo().Assembly.GetCustomAttributes<AssemblyInformationalVersionAttribute>()
                .First()
                .InformationalVersion;
        }
    }
}