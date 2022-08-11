// Ishan Pranav's REBUS: RebusDbContext.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    internal sealed class RebusDbContext : DbContext
    {
        private const string IgnoreCaseCollation = "NOCASE";

        [NotNull]
        public DbSet<Commodity>? Commodities { get; set; }

        [NotNull]
        public DbSet<Player>? Players { get; set; }

        [NotNull]
        public DbSet<Unit>? Units { get; set; }

        [NotNull]
        public DbSet<User>? Users { get; set; }

        [NotNull]
        public DbSet<Zone>? Zones { get; set; }

        public RebusDbContext(DbContextOptions<RebusDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Commodity>()
                .HasKey(x => new
                {
                    x.Q,
                    x.R,
                    x.Mass
                });

            modelBuilder.Entity<Unit>(x =>
            {
                x.HasOne(x => x.Sanctuary)
                 .WithMany();

                x.HasOne(x => x.Zone)
                 .WithMany(x => x.Units);
            });

            if (Database.IsSqlite())
            {
                modelBuilder
                    .Entity<Player>()
                    .Property(x => x.Name)
                    .UseCollation(IgnoreCaseCollation);

                modelBuilder
                    .Entity<Unit>()
                    .Property(x => x.Name)
                    .UseCollation(IgnoreCaseCollation);
            }
        }
    }
}
