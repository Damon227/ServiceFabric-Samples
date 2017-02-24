// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Identity
// File             : KolibreIdentityDbContext.cs
// Created          : 2016-07-02  4:09 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using Microsoft.EntityFrameworkCore;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    public class KolibreIdentityDbContext : IdentityDbContext<KolibreUser>
    {
        public KolibreIdentityDbContext(DbContextOptions<KolibreIdentityDbContext> options) : base(options)
        {
        }
    }
}