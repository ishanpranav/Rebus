// Ishan Pranav's REBUS: ZoneFunctions.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

namespace Rebus.Server.Functions
{
    internal sealed class ZoneFunctions : IFunctions<Zone>
    {
        private readonly int _playerId;
        private readonly IQueryable<Zone> _zones;
        private readonly Map _map;
        private readonly RuleSet _rules;

        public ZoneFunctions(int playerId, IQueryable<Zone> zones, Map map, RuleSet rules)
        {
            _playerId = playerId;
            _zones = zones;
            _map = map;
            _rules = rules;
        }

        public int Cost(Zone value, Zone neighbor)
        {
            return _rules.FuelCost;
        }

        public int Heuristic(Zone source, Zone destination)
        {
            return HexPoint.Distance(source.Location, destination.Location);
        }

        public IEnumerable<Zone> Neighbors(Zone value)
        {
            foreach (HexPoint neighbor in value.Location.Neighbors())
            {
                if (_map.TryGetLayers(neighbor, out IReadOnlyList<int>? layers) && layers.Count != Depths.Star)
                {
                    Zone? result = _zones.SingleOrDefault(x => x.Q == neighbor.Q && x.R == neighbor.R && x.PlayerId == _playerId);

                    if (result != null)
                    {
                        yield return result;
                    }
                }
            }
        }
    }
}
