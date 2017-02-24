// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore.Authentication.Session
// File             : SessionAuthenticationHandler.cs
// Created          : 2016-07-05  2:13 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Credit.Kolibre.Foundation.ServiceFabric;
using Credit.Kolibre.Foundation.ServiceFabric.Session;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;

namespace Credit.Kolibre.Foundation.AspNetCore.Authentication.Session
{
    public class SessionAuthenticationHandler : AuthenticationHandler<SessionAuthenticationOptions>
    {
        private readonly ISessionManager _sessionManager;
        private AuthenticateResult _sessionTicket;


        public SessionAuthenticationHandler(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            AuthenticateResult result = EnsureSessionTicket();

            if (result.Succeeded && result.Ticket.Principal.Identity.IsAuthenticated
                && result.Ticket.Properties.ExpiresUtc.GetValueOrDefault(DateTimeOffset.UtcNow) >= DateTimeOffset.UtcNow)
            {
                Context.Items[Constants.X_KC_USERID] = result.Ticket.Principal.Identity.Name;
            }

            return Task.FromResult(result);
        }


        protected override Task HandleSignInAsync(SignInContext context)
        {
            AuthenticationTicket ticket = new AuthenticationTicket(context.Principal,
                new AuthenticationProperties(context.Properties), context.AuthenticationScheme);

            DateTimeOffset issuedUtc;
            if (ticket.Properties.IssuedUtc.HasValue)
            {
                issuedUtc = ticket.Properties.IssuedUtc.Value;
            }
            else
            {
                issuedUtc = DateTimeOffset.UtcNow;
                ticket.Properties.IssuedUtc = issuedUtc;
            }

            if (!ticket.Properties.ExpiresUtc.HasValue)
            {
                ticket.Properties.ExpiresUtc = issuedUtc.Add(Options.ExpireTimeSpan);
            }

            ISession session = _sessionManager.GetRequestSession(Context);
            if (session != null && session.IsAvailable)
            {
                // 暂时以后登录进的 Pricipal 和 Identity 为准
                //SessionTicket sessionTicket = session.GetObject<SessionTicket>(Options.SessionTicketName);
                //if (sessionTicket != null)
                //{
                //    sessionTicket.AddAuthenticationTicket(ticket);
                //}
                //else
                //{
                //    sessionTicket = new SessionTicket(ticket);
                //}

                session.SetObject(Options.SessionTicketName, new SessionTicket(ticket));
            }

            if (context.Principal.Identity != null && context.Principal.Identity.IsAuthenticated && context.Principal.Identity.Name.IsNotNullOrEmpty())
            {
                Context.Items[Constants.X_KC_USERID] = context.Principal.Identity.Name;
            }

            return base.HandleSignInAsync(context);
        }

        protected override Task HandleSignOutAsync(SignOutContext context)
        {
            ISession session = _sessionManager.GetRequestSession(Context);
            if (session != null && session.IsAvailable)
            {
                SessionTicket sessionTicket = session.GetObject<SessionTicket>(Options.SessionTicketName);
                if (sessionTicket != null)
                {
                    if (sessionTicket.AuthenticationScheme != context.AuthenticationScheme)
                    {
                        return Task.FromResult(0);
                    }
                }

                session.Remove(Options.SessionTicketName);
            }

            Context.Items[Constants.X_KC_USERID] = string.Empty;
            Context.Items[Constants.X_KC_SESSIONID] = string.Empty;

            return base.HandleSignOutAsync(context);
        }

        private AuthenticateResult EnsureSessionTicket()
        {
            // We only need to read the ticket once
            return _sessionTicket ?? (_sessionTicket = ReadSessionTicket());
        }

        private AuthenticateResult ReadSessionTicket()
        {
            ISession session = _sessionManager.GetRequestSession(Context);
            if (session == null || !session.IsAvailable)
            {
                return AuthenticateResult.Skip();
            }

            SessionTicket sessionTicket = session.GetObject<SessionTicket>(Options.SessionTicketName);
            if (sessionTicket == null)
            {
                return AuthenticateResult.Skip();
            }

            AuthenticationTicket ticket = sessionTicket.ToAuthenticationTicket();

            DateTimeOffset currentUtc = DateTimeOffset.UtcNow;
            DateTimeOffset? expiresUtc = ticket.Properties.ExpiresUtc;

            if (expiresUtc != null && expiresUtc.Value < currentUtc)
            {
                return AuthenticateResult.Fail("Ticket expired");
            }

            // Finally we have a valid ticket
            return AuthenticateResult.Success(ticket);
        }
    }
}