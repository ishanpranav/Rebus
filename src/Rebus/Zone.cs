// Ishan Pranav's REBUS: Zone.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Rebus
{
    [Index(nameof(PlayerId), nameof(Q), nameof(R))]
    [Table(nameof(Zone))]
    public class Zone : IEquatable<Zone>
    {
        public int Id { get; set; }

        public int PlayerId { get; set; }

        [NotNull]
        [Required]
        public Player? Player { get; set; }

        public int Q { get; set; }

        public int R { get; set; }

        [NotMapped]
        public HexPoint Location
        {
            get
            {
                return new HexPoint(Q, R);
            }
            set
            {
                Q = value.Q;
                R = value.R;
            }
        }

        public ICollection<Unit> Units { get; } = new HashSet<Unit>();

        public bool Equals(Zone? other)
        {
            return other != null && Location == other.Location && PlayerId == other.PlayerId;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Zone);
        }

        public override int GetHashCode()
        {
            HashCode result = new HashCode();

            result.Add(Location);
            result.Add(PlayerId);

            return result.ToHashCode();
        }
    }
}
