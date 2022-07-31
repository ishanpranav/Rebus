// Ishan Pranav's REBUS: Map.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Rebus.Server
{
    public class Map
    {
        private readonly int _size;
        private readonly Dictionary<HexPoint, int[]> _zones = new Dictionary<HexPoint, int[]>();

        public JuliaSet JuliaSet { get; }
        public int Radius { get; }
        public int Depth { get; }
        public double Zoom { get; }

        public HexPoint Origin { get; }

        public int Stars { get; }
        public int Planets { get; }

        public Map(JuliaSet juliaSet, HexPoint origin, int radius, int depth, double zoom)
        {
            JuliaSet = juliaSet;
            Origin = origin;
            _size = radius * 2 + 1;
            Radius = radius;
            Depth = depth;
            Zoom = zoom;

            int[] buffer = new int[depth];

            foreach (HexPoint location in Origin.Range(Radius))
            {
                int index = (int)JuliaDepth(location);

                if (index > 0)
                {
                    int layers = depth - index + 1;

                    _zones.Add(location, new int[layers]);

                    switch (layers)
                    {
                        case Depths.Star:
                            Stars++;
                            break;

                        case Depths.Planet:
                            Planets++;
                            break;
                    }
                }
            }

            Array.Fill(buffer, Depths.FirstValue);

            foreach (HexPoint location in Origin.Spiral(Radius))
            {
                if (recurse(location))
                {
                    move(Depths.Constellation);
                }
            }

            bool recurse(HexPoint location)
            {
                if (_zones.TryGetValue(location, out int[]? layers) && layers.Length > Depths.Constellation && Depths.Layer(layers, Depths.Constellation) == Depths.EmptyValue)
                {
                    for (int i = 0; i < layers.Length; i++)
                    {
                        layers[i] = buffer[i];
                    }

                    move(layers.Length);

                    foreach (HexPoint neighbor in location.Neighbors())
                    {
                        recurse(neighbor);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }

            void move(int depth)
            {
                buffer[depth - 1]++;

                Array.Fill(buffer, value: 1, depth, buffer.Length - depth);
            }
        }

        public bool Contains(HexPoint location)
        {
            return HexPoint.Distance(location, Origin) <= Radius;
        }

        private double JuliaDepth(HexPoint location)
        {
            return JuliaSet.Julia(location.Q, location.R, _size, _size, Zoom) * Depth;
        }

        public bool IsStar(HexPoint location)
        {
            return _zones.TryGetValue(location, out int[]? layers) && layers.Length == Depths.Star;
        }

        public bool TryGetLayers(HexPoint location, [MaybeNullWhen(false)] out IReadOnlyList<int> layers)
        {
            if (_zones.TryGetValue(location, out int[]? results))
            {
                layers = results;

                return true;
            }
            else
            {
                layers = null;

                return false;
            }
        }

        public Biome GetBiome(HexPoint location, IReadOnlyList<int> layers)
        {
            switch (layers.Count)
            {
                case Depths.Star:
                    return Biome.Stellar;

                case Depths.Planet:
                    const Biome min = Biome.Urban;

                    return (int)((Depths.Star - JuliaDepth(location)) * (Biome.Tundra - min)) + min;

                default:
                    return Biome.None;
            }
        }
    }
}
