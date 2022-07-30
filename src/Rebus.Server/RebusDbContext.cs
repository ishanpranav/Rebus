﻿// Ishan Pranav's REBUS: RebusDbContext.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    public class RebusDbContext : DbContext
    {
        private const string IgnoreCaseCollation = "NOCASE";

#nullable disable
        public DbSet<Player> Players { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Zone> Zones { get; set; }
#nullable enable

        public RebusDbContext(DbContextOptions<RebusDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (Database.IsSqlite())
            {
                modelBuilder
                    .Entity<Player>()
                    .Property(x => x.Username)
                    .UseCollation(IgnoreCaseCollation);

                modelBuilder.Entity<Unit>(x =>
                {
                    x.HasOne(x => x.Zone)
                     .WithMany(x => x.Units)
                     .HasForeignKey(x => x.ZoneId);

                    x.HasOne(x => x.Sanctuary)
                     .WithMany();
                });
            }
        }
    }
}