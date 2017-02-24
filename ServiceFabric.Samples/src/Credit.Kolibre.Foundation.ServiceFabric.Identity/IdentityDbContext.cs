// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.ServiceFabric.Identity
// File             : IdentityDbContext.cs
// Created          : 2017-02-15  18:46
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Credit.Kolibre.Foundation.ServiceFabric.Identity
{
    /// <summary>
    ///     Base class for the Entity Framework database context used for identity.
    /// </summary>
    public class IdentityDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="IdentityDbContext" />.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext" />.</param>
        public IdentityDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="IdentityDbContext" /> class.
        /// </summary>
        protected IdentityDbContext()
        {
        }
    }

    /// <summary>
    ///     Base class for the Entity Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of the user objects.</typeparam>
    public class IdentityDbContext<TUser> : IdentityDbContext<TUser, IdentityRole, string> where TUser : IdentityUser
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="IdentityDbContext" />.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext" />.</param>
        public IdentityDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="IdentityDbContext" /> class.
        /// </summary>
        protected IdentityDbContext()
        {
        }
    }

    /// <summary>
    ///     Base class for the Entity Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of user objects.</typeparam>
    /// <typeparam name="TRole">The type of role objects.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
    public class IdentityDbContext<TUser, TRole, TKey> : IdentityDbContext<TUser, TRole, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityRoleClaim<TKey>>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="IdentityDbContext" />.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext" />.</param>
        public IdentityDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="IdentityDbContext" /> class.
        /// </summary>
        protected IdentityDbContext()
        {
        }
    }

    /// <summary>
    ///     Base class for the Entity Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of user objects.</typeparam>
    /// <typeparam name="TRole">The type of role objects.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
    /// <typeparam name="TUserClaim">The type of the user claim object.</typeparam>
    /// <typeparam name="TUserRole">The type of the user role object.</typeparam>
    /// <typeparam name="TRoleClaim">The type of the role claim object.</typeparam>
    public abstract class IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TRoleClaim> : DbContext
        where TUser : IdentityUser<TKey, TUserClaim, TUserRole>
        where TRole : IdentityRole<TKey, TUserRole, TRoleClaim>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="IdentityDbContext" />.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext" />.</param>
        protected IdentityDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="IdentityDbContext" /> class.
        /// </summary>
        protected IdentityDbContext()
        {
        }

        /// <summary>
        ///     Gets or sets the <see cref="DbSet{TEntity}" /> of Users.
        /// </summary>
        public DbSet<TUser> Users { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="DbSet{TEntity}" /> of User claims.
        /// </summary>
        public DbSet<TUserClaim> UserClaims { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="DbSet{TEntity}" /> of User roles.
        /// </summary>
        public DbSet<TUserRole> UserRoles { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="DbSet{TEntity}" /> of roles.
        /// </summary>
        public DbSet<TRole> Roles { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="DbSet{TEntity}" /> of role claims.
        /// </summary>
        public DbSet<TRoleClaim> RoleClaims { get; set; }

        /// <summary>
        ///     <para>
        ///         Override this method to configure the database (and other options) to be used for this context.
        ///         This method is called for each instance of the context that is created.
        ///     </para>
        ///     <para>
        ///         In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" /> may or may not have been passed
        ///         to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" /> to determine if
        ///         the options have already been set, and skip some or all of the logic in
        ///         <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
        ///     </para>
        /// </summary>
        /// <param name="optionsBuilder">
        ///     A builder used to create or modify options for this context. Databases (and other extensions)
        ///     typically define extension methods on this object that allow you to configure the context.
        /// </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(b => b.Ignore(RelationalEventId.QueryClientEvaluationWarning));
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        ///     Configures the schema needed for the identity framework.
        /// </summary>
        /// <param name="builder">
        ///     The builder being used to construct the model for this context.
        /// </param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TUser>(b =>
            {
                b.ToTable("Credit.Kolibre.Identity.Users", "dbo");

                b.HasKey(i => i.UserId);

                b.HasIndex(i => i.UserId).HasName("UQ_dbo.Credit.Kolibre.Identity.Users_UserId").IsUnique();
                b.HasIndex(i => i.NormalizedUserName).HasName("UQ_dbo.Credit.Kolibre.Identity.Users_NormalizedUserName").IsUnique();
                b.HasIndex(i => i.Email).HasName("IN_dbo.Credit.Kolibre.Identity.Users_Email");
                b.HasIndex(i => i.NormalizedEmail).HasName("IN_dbo.Credit.Kolibre.Identity.Users_NormalizedEmail");
                b.HasIndex(i => i.Cellphone).HasName("IN_dbo.Credit.Kolibre.Identity.Users_Cellphone");

                b.Property(i => i.ConcurrencyStamp).HasMaxLength(200).IsRequired().IsConcurrencyToken();

                b.Property(i => i.UserId).HasMaxLength(32).IsRequired();
                b.Property(i => i.UserName).HasMaxLength(200).IsRequired();
                b.Property(i => i.NormalizedUserName).HasMaxLength(200).IsRequired();
                b.Property(i => i.Email).HasMaxLength(200);
                b.Property(i => i.NormalizedEmail).HasMaxLength(200);
                b.Property(i => i.EmailConfirmed).IsRequired();
                b.Property(i => i.PasswordHash).IsRequired();
                b.Property(i => i.Cellphone).HasMaxLength(200);
                b.Property(i => i.CellphoneConfirmed).IsRequired();
                b.Property(i => i.LockoutEnabled).IsRequired();
                b.Property(i => i.ConcurrencyStamp).IsRequired();

                b.HasMany(i => i.Claims).WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
                b.HasMany(i => i.Roles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            });

            builder.Entity<TRole>(b =>
            {
                b.HasKey(i => i.RoleId);

                b.ToTable("Credit.Kolibre.Identity.Roles", "dbo");

                b.HasIndex(i => i.RoleId).HasName("UQ_dbo.Credit.Kolibre.Identity.Roles_RoleId").IsUnique();
                b.HasIndex(i => i.NormalizedName).HasName("UQ_dbo.Credit.Kolibre.Identity.Roles_NormalizedName").IsUnique();
                b.HasIndex(i => i.Name).HasName("IN_dbo.Credit.Kolibre.Identity.Roles_Name");

                b.Property(i => i.ConcurrencyStamp).HasMaxLength(200).IsRequired().IsConcurrencyToken();

                b.Property(i => i.RoleId).HasMaxLength(32).IsRequired();
                b.Property(i => i.Name).HasMaxLength(200).IsRequired();
                b.Property(i => i.NormalizedName).HasMaxLength(200).IsRequired();

                b.HasMany(i => i.Users).WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
                b.HasMany(i => i.Claims).WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
            });

            builder.Entity<TUserClaim>(b =>
            {
                b.HasKey(i => i.Id);

                b.ToTable("Credit.Kolibre.Identity.UserClaims", "dbo");

                b.HasIndex(i => i.UserId).HasName("IN_dbo.Credit.Kolibre.Identity.UserClaims_UserId");

                b.Property(i => i.UserId).HasMaxLength(32).IsRequired();
            });

            builder.Entity<TRoleClaim>(b =>
            {
                b.HasKey(i => i.Id);

                b.ToTable("Credit.Kolibre.Identity.RoleClaims", "dbo");

                b.HasIndex(i => i.RoleId).HasName("IN_dbo.Credit.Kolibre.Identity.RoleClaims_RoleId");

                b.Property(i => i.RoleId).HasMaxLength(32).IsRequired();
            });

            builder.Entity<TUserRole>(b =>
            {
                b.HasKey(i => new { i.UserId, i.RoleId });

                b.ToTable("Credit.Kolibre.Identity.UserRoles", "dbo");

                b.HasIndex(i => i.RoleId).HasName("IN_dbo.Credit.Kolibre.Identity.UserRoles_UserId_RoleId");
                b.HasIndex(i => i.RoleId).HasName("IN_dbo.Credit.Kolibre.Identity.UserRoles_UserId");
                b.HasIndex(i => i.RoleId).HasName("IN_dbo.Credit.Kolibre.Identity.UserRoles_RoleId");

                b.Property(i => i.UserId).HasMaxLength(32).IsRequired();
                b.Property(i => i.RoleId).HasMaxLength(32).IsRequired();
            });
        }
    }
}