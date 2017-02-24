// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Authentication.Session
// File             : SessionManager.cs
// Created          : 2016-07-10  4:24 AM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Credit.Kolibre.Foundation.AspNetCore.Authentication.Session
{
    public class SessionManager : ISessionManager
    {
        #region ISessionManager Members

        public ISession GetRequestSession(HttpContext context)
        {
            ISession session = context.Session;
            if (session == null)
            {
                ISessionFeature feature = context.Features.Get<ISessionFeature>();
                if (feature != null)
                {
                    session = feature.Session;
                }
            }

            return session;
        }

        #endregion
    }
}