// Ishan Pranav's REBUS: Zone.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Rebus
{
    [DataContract]
    [Index(nameof(Q), nameof(R), nameof(PlayerId), IsUnique = true)]
    [Table(nameof(Zone))]
    public class Zone : IEquatable<Zone>
    {
        public int Id { get; set; }

        public int Q { get; set; }

        public int R { get; set; }

        [DataMember(Order = 0)]
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
