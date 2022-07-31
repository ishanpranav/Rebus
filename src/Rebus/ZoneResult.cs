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
        public HexPoint Location { get; }

        [DataMember(Order = 1)]
        public int PlayerId { get; }

        [DataMember(Order = 2)]
        public Biome Biome { get; }

        [DataMember(Order = 3)]
        public string? Name { get; }

        [DataMember(Order = 4)]
        public IReadOnlyList<int> Layers { get; }

        [DataMember(Order = 5)]
        public IReadOnlyCollection<Unit> Units { get; }

        [DataMember(Order = 6)]
        public IReadOnlyCollection<HexPoint> Neighbors { get; }

        public ZoneResult(HexPoint location, string? name, IReadOnlyList<int> layers) : this(location, playerId: 0, Biome.Stellar, name, layers, Array.Empty<Unit>())
        {
            Location = location;
            Name = name;
            Layers = layers;
        }

        public ZoneResult(HexPoint location, int playerId, Biome biome, string? name, IReadOnlyList<int> layers, IReadOnlyCollection<Unit> units) : this(location, playerId, biome, name, layers, units, Array.Empty<HexPoint>()) { }

        public ZoneResult(HexPoint location, int playerId, Biome biome, string? name, IReadOnlyList<int> layers, IReadOnlyCollection<Unit> units, IReadOnlyCollection<HexPoint> neighbors)
        {
            Location = location;
            Name = name;
            Layers = layers;
            PlayerId = playerId;
            Biome = biome;
            Units = units;
            Neighbors = neighbors;
        }
    }
}
