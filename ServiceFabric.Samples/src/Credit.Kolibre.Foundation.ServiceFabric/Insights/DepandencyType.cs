// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : DepandencyType.cs
// Created          : 2016-07-09  7:39 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Credit.Kolibre.Foundation.ServiceFabric.Insights
{
    public static class DepandencyType
    {
        public static readonly string Redis = "Redis";

        public static readonly string Sql = "SQL";

        public static readonly string DataWarehouse = "DataWarehouse";

        public static readonly string Http = "HTTP";
    }
}