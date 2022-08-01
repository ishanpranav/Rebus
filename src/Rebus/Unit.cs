// Ishan Pranav's REBUS: Unit.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.ComponentModel.DataAnnotations.Schema;
using MessagePack;
using Microsoft.EntityFrameworkCore;

namespace Rebus
{
    [Index(nameof(Name), IsUnique = true)]
    [MessagePackObject]
    [Table(nameof(Unit))]
    public class Unit : IEquatable<Unit>
    {
        [Key(0)]
        public int Id { get; set; }

        [Key(1)]
        public string Name { get; set; } = string.Empty;

        [IgnoreMember]
        public int ZoneId { get; set; }

#nullable disable
        [IgnoreMember]
        public Zone Zone { get; set; }
#nullable enable

        [IgnoreMember]
        public Zone? Sanctuary { get; set; }

        [Key(2)]
        public HexPoint? SanctuaryLocation
        {
            get
            {
                return Sanctuary?.Location;
            }
        }

        [Key(3)]
        public int CargoMass { get; set; }

        public bool Equals(Unit? other)
        {
            return other != null && Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Unit);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
