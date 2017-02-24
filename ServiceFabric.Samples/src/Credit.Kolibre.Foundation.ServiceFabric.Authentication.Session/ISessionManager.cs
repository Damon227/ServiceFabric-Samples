// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Authentication.Session
// File             : ISessionManager.cs
// Created          : 2016-07-05  1:34 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Microsoft.AspNetCore.Http;

namespace Credit.Kolibre.Foundation.AspNetCore.Authentication.Session
{
    public interface ISessionManager
    {
        ISession GetRequestSession(HttpContext context);
    }
}