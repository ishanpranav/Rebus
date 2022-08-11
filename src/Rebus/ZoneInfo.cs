// Ishan Pranav's REBUS: ZoneInfo.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using MessagePack;

namespace Rebus
{
    [MessagePackObject]
    public class ZoneInfo
    {
        [Key(0)]
        public HexPoint Location { get; }

        [Key(1)]
        public int PlayerId { get; }

        [Key(2)]
        public string? Name { get; }

        [Key(3)]
        public Biome Biome { get; }

        [Key(4)]
        public IReadOnlyList<int> Layers { get; }

        [Key(5)]
        public IReadOnlyCollection<Unit> Units { get; }

        [Key(6)]
        public IReadOnlyCollection<HexPoint> Neighbors { get; }

        [Key(7)]
        public ICollection<Arguments> Arguments { get; }

        public ZoneInfo(HexPoint location, int playerId, string? name, Biome biome, IReadOnlyList<int> layers, IReadOnlyCollection<Unit> units, IReadOnlyCollection<HexPoint> neighbors) : this(location, playerId, name, biome, layers, units, neighbors, new List<Arguments>()) { }

        public ZoneInfo(HexPoint location, int playerId, string? name, Biome biome, IReadOnlyList<int> layers, IReadOnlyCollection<Unit> units, IReadOnlyCollection<HexPoint> neighbors, ICollection<Arguments> arguments)
        {
            Location = location;
            Name = name;
            Biome = biome;
            Layers = layers;
            PlayerId = playerId;
            Units = units;
            Neighbors = neighbors;
            Arguments = arguments;
        }
    }
}
