// Ishan Pranav's REBUS: Unit.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Rebus
{
    [DataContract]
    [Table(nameof(Unit))]
    public class Unit : IEquatable<Unit>
    {
        [DataMember(Order = 0)]
        public int Id { get; set; }

        public int ZoneId { get; set; }

#nullable disable
        public Zone Zone { get; set; }
#nullable enable

        [DataMember(Order = 1)]
        public Zone? Sanctuary { get; set; }

        [DataMember(Order = 2)]
        public int? CargoMass { get; set; }

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
