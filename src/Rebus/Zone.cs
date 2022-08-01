// Ishan Pranav's REBUS: Zone.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MessagePack;
using Microsoft.EntityFrameworkCore;

namespace Rebus
{
    [Index(nameof(Q), nameof(R), nameof(PlayerId), IsUnique = true)]
    [MessagePackObject]
    [Table(nameof(Zone))]
    public class Zone : IEquatable<Zone>
    {
        [IgnoreMember]
        public int Id { get; set; }

        [IgnoreMember]
        public int Q { get; set; }

        [IgnoreMember]
        public int R { get; set; }

        [Key(0)]
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

        public int PlayerId { get; set; }

#nullable disable
        public Player Player { get; set; }
#nullable enable

        public ICollection<Unit> Units { get; set; } = new HashSet<Unit>();

        public bool Equals(Zone? other)
        {
            return other != null && Location == other.Location;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Zone);
        }

        public override int GetHashCode()
        {
            return Location.GetHashCode();
        }
    }
}
