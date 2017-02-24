// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : IHttpTelemetryClientAccessor.cs
// Created          : 2016-07-09  2:42 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Microsoft.ApplicationInsights;

namespace Credit.Kolibre.Foundation.ServiceFabric.Insights
{
    public interface IHttpTelemetryClientAccessor
    {
        TelemetryClient GetTelemetryClient();
    }
}