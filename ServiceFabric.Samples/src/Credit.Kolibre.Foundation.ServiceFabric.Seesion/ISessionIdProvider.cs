// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Session
// File             : ISessionIdProvider.cs
// Created          : 2016-07-03  5:09 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Credit.Kolibre.Foundation.ServiceFabric.Seesion
{
    public interface ISessionIdProvider
    {
        string GetSessionId();

        string InitAndSetSessionId();

        string SetSessionId(string sessionId);
    }
}