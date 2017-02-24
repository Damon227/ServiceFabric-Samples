// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.ServiceFabric.Seesion
// File             : SessionDefaults.cs
// Created          : 2017-02-15  19:02
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Credit.Kolibre.Foundation.Sys;

namespace Credit.Kolibre.Foundation.ServiceFabric.Seesion
{
    /// <summary>
    ///     Represents defaults for the Session.
    /// </summary>
    public static class SessionDefaults
    {
        public static readonly TimeSpan IDLE_TIMEOUT = 20.Minutes();

        public static readonly string SESSION_ID_NAME = Constants.X_KC_SESSIONID;
    }
}