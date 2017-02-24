// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Session
// File             : SessionOptions.cs
// Created          : 2016-07-02  11:04 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.Extensions.Options;

namespace Credit.Kolibre.Foundation.ServiceFabric.Seesion
{
    /// <summary>
    ///     Represents the session state options for the application.
    /// </summary>
    public class SessionOptions : IOptions<SessionOptions>
    {
        public TimeSpan IdleTimeout { get; set; } = SessionDefaults.IDLE_TIMEOUT;

        public string SessionIdName { get; set; } = SessionDefaults.SESSION_ID_NAME;

        #region IOptions<SessionOptions> Members

        SessionOptions IOptions<SessionOptions>.Value
        {
            get { return this; }
        }

        #endregion
    }
}