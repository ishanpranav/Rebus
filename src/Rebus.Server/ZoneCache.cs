// Ishan Pranav's REBUS: ZoneCache.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    internal sealed class ZoneCache
    {
        private readonly int _playerId;
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;
        private readonly Map _map;
        private readonly Namer _namer;
        private readonly Dictionary<HexPoint, ZoneInfo> _sources = new Dictionary<HexPoint, ZoneInfo>();

        public IReadOnlyDictionary<HexPoint, ZoneInfo> Zones
        {
            get
            {
                return _sources;
            }
        }

        private ZoneCache(int playerId, IDbContextFactory<RebusDbContext> contextFactory, Map map, Namer namer)
        {
            _playerId = playerId;
            _contextFactory = contextFactory;
            _map = map;
            _namer = namer;
        }

        private async Task InitializeAsync()
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                foreach (Zone zone in getZones())
                {
                    if (!_sources.ContainsKey(zone.Location) && _map.TryGetLayers(zone.Location, out IReadOnlyList<int>? layers))
                    {
                        HashSet<HexPoint> neighbors = new HashSet<HexPoint>();
                        string? name;
                        Biome biome;

                        name = _namer.Name(layers);
                        biome = _map.GetBiome(zone.Location, layers);

                        foreach (HexPoint neighbor in zone.Location.Neighbors())
                        {
                            if (!_sources.ContainsKey(neighbor) && _map.TryGetLayers(neighbor, out IReadOnlyList<int>? neighborLayers) && neighborLayers.Count == Depths.Star)
                            {
                                ZoneInfo neighborResult = new ZoneInfo(neighbor, playerId: 0, _namer.Name(neighborLayers), _map.GetBiome(neighbor, neighborLayers), neighborLayers, Array.Empty<Unit>(), Array.Empty<HexPoint>());

                                _sources.Add(neighbor, neighborResult);
                            }
                            else if (_map.Contains(neighbor))
                            {
                                neighbors.Add(neighbor);
                            }
                        }

                        ZoneInfo result = new ZoneInfo(zone.Location, zone.PlayerId, name, biome, layers, new List<Unit>(zone.Units), neighbors);

                        _sources.Add(zone.Location, result);
                    }
                }

                IEnumerable<Zone> getZones()
                {
                    if (_playerId == -1)
                    {
                        return _map.Origin
                            .Range(_map.Radius)
                            .Select(x => new Zone()
                            {
                                Q = x.Q,
                                R = x.R,
                                PlayerId = _playerId
                            });
                    }
                    else
                    {
                        return context.Zones
                            .Where(x => x.PlayerId == _playerId)
                            .Include(x => x.Units)
                            .ThenInclude(x => x.Sanctuary);
                    }
                }
            }
        }

        public static async Task<ZoneCache> CreateAsync(int playerId, IDbContextFactory<RebusDbContext> contextFactory, Map map, Namer namer)
        {
            ZoneCache result = new ZoneCache(playerId, contextFactory, map, namer);

            await result.InitializeAsync();

            return result;
        }
    }
}
