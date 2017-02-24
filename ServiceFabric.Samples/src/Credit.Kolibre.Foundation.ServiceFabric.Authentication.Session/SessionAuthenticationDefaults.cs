// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Authentication.Session
// File             : SessionAuthenticationDefaults.cs
// Created          : 2016-07-05  1:36 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Credit.Kolibre.Foundation.Sys;

namespace Credit.Kolibre.Foundation.AspNetCore.Authentication.Session
{
    public static class SessionAuthenticationDefaults
    {
        public static readonly string AUTHENTICATION_SCHEME = "KC-Identity-Application";

        public static readonly bool AUTOMATIC_AUTHENTICATE = true;

        public static readonly bool AUTOMATIC_CHALLENGE = true;

        public static readonly string DEFAULT_SESSION_TICKET_NAME = "X-KC-AUTHTICKET";

        public static readonly TimeSpan DEFAULT_EXPIRE_TIME_SPAN = 1.Days();

    }
}