// Ishan Pranav's REBUS: ZoneResult.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rebus
{
    [DataContract]
    public class ZoneResult
    {
        [DataMember(Order = 0)]
        public Zone Value { get; }

        [DataMember(Order = 1)]
        public Biome Biome { get; }

        [DataMember(Order = 2)]
        public string? Name { get; }

        [DataMember(Order = 3)]
        public int Constellation { get; }

        [DataMember(Order = 4)]
        public IReadOnlyCollection<HexPoint> Neighbors { get; }

        public ZoneResult(Zone value, Biome biome, string? name, int constellation) : this(value, biome, name, constellation, Array.Empty<HexPoint>()) { }

        public ZoneResult(Zone value, Biome biome, string? name, int constellation, IReadOnlyCollection<HexPoint> neighbors)
        {
            Value = value;
            Biome = biome;
            Name = name;
            Constellation = constellation;
            Neighbors = neighbors;
        }
    }
}
