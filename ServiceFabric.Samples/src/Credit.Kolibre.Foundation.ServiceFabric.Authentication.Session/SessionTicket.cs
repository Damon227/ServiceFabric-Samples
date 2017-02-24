// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.ServiceFabric.Authentication.Session
// File             : SessionTicket.cs
// Created          : 2017-02-15  17:00
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using ClaimTypes = Credit.Kolibre.Foundation.ServiceFabric.Identity.ClaimTypes;

namespace Credit.Kolibre.Foundation.AspNetCore.Authentication.Session
{
    public class SessionTicket
    {
        public SessionTicket()
        {
            Claims = new List<KeyValuePair<string, string>>();
            Properties = new Dictionary<string, string>();
        }

        public SessionTicket(AuthenticationTicket authenticationTicket) : this()
        {
            UserId = authenticationTicket.Principal.Identity.Name;
            AuthenticationScheme = authenticationTicket.AuthenticationScheme;
            AuthenticationType = authenticationTicket.Principal.Identity.AuthenticationType;
            IssueTime = authenticationTicket.Properties.IssuedUtc;
            ExpiryTime = authenticationTicket.Properties.ExpiresUtc;
            Claims = authenticationTicket.Principal.Claims.Select(c => new KeyValuePair<string, string>(c.Type, c.Value)).ToList();
            Properties = authenticationTicket.Properties.Items.ToDictionary(i => i.Key, i => i.Value);
        }

        public string UserId { get; set; }

        public string AuthenticationScheme { get; set; }

        public string AuthenticationType { get; set; }

        public DateTimeOffset? IssueTime { get; set; }

        public DateTimeOffset? ExpiryTime { get; set; }

        public List<KeyValuePair<string, string>> Claims { get; set; }

        public Dictionary<string, string> Properties { get; set; }

        public List<Claim> BuildClaims()
        {
            return Claims.Select(pair => new Claim(pair.Key, pair.Value)).ToList();
        }

        public AuthenticationTicket ToAuthenticationTicket()
        {
            ClaimsIdentity identity = new ClaimsIdentity(BuildClaims(), AuthenticationType, ClaimTypes.NameIdentifier, ClaimTypes.Role);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            AuthenticationTicket ticket = new AuthenticationTicket(principal, new AuthenticationProperties(Properties), AuthenticationScheme);
            return ticket;
        }
    }
}