// Ishan Pranav's REBUS: Map.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Rebus.Server
{
    public class Map
    {
        public const int ConstellationDepth = 1;
        public const int StarDepth = 2;
        public const int PlanetDepth = 3;

        private readonly int _size;
        private readonly int _stars;
        private readonly int _planets;
        private readonly Dictionary<HexPoint, int[]> _zones = new Dictionary<HexPoint, int[]>();

        public JuliaSet JuliaSet { get; }
        public int Radius { get; }
        public int Depth { get; }
        public double Zoom { get; }

        public HexPoint Origin { get; }

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
                        case StarDepth:
                            _stars++;
                            break;

                        case PlanetDepth:
                            _planets++;
                            break;
                    }
                }
            }

            Array.Fill(buffer, value: 1);

            foreach (HexPoint location in Origin.Spiral(Radius))
            {
                if (recurse(location))
                {
                    move(ConstellationDepth);
                }
            }

            bool recurse(HexPoint location)
            {
                if (_zones.TryGetValue(location, out int[]? zone) && zone.Length > ConstellationDepth && zone[ConstellationDepth - 1] == default)
                {
                    for (int i = 0; i < zone.Length; i++)
                    {
                        zone[i] = buffer[i];
                    }

                    move(zone.Length);

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
            return _zones.TryGetValue(location, out int[]? layers) && layers.Length == StarDepth;
        }

        public void GetDetails(HexPoint location, Namer namer, out Biome biome, out string? name, out int constellation)
        {
            if (_zones.TryGetValue(location, out int[]? layers))
            {
                biome = getBiome();
                name = namer.Name(_stars, _planets, layers);
                constellation = layers[ConstellationDepth - 1];

                Biome getBiome()
                {
                    switch (layers.Length)
                    {
                        case StarDepth:
                            return Biome.Stellar;

                        case PlanetDepth:
                            const Biome min = Biome.Urban;

                            return (int)((StarDepth - JuliaDepth(location)) * (Biome.Tundra - min)) + min;

                        default:
                            return Biome.None;
                    }
                }
            }
            else
            {
                constellation = 0;
                biome = Biome.None;
                name = null;
            }
        }
    }
}
