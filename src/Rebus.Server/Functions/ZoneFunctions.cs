// Ishan Pranav's REBUS: ZoneHeuristics.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

namespace Rebus.Server.Functions
{
    /// <summary>
    /// Provides functions used to search graphs of <see cref="Zone"/> instances.
    /// </summary>
    public class ZoneFunctions : IFunctions<Zone>
    {
        private readonly int _playerId;
        private readonly IQueryable<Zone> _zones;
        private readonly Map _map;
        private readonly RuleSet _rules;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneFunctions"/> class.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="zones">The zones visible to the player.</param>
        /// <param name="map">The map.</param>
        /// <param name="rules">The set of game rules.</param>
        public ZoneFunctions(int playerId, IQueryable<Zone> zones, Map map, RuleSet rules)
        {
            _playerId = playerId;
            _zones = zones;
            _map = map;
            _rules = rules;
        }

        /// <inheritdoc/>
        public int Cost(Zone value, Zone neighbor)
        {
            return _rules.FuelCost;
        }

        /// <inheritdoc/>
        public int Heuristic(Zone source, Zone destination)
        {
            return HexPoint.Distance(source.Location, destination.Location);
        }

        /// <inheritdoc/>
        public IEnumerable<Zone> Neighbors(Zone value)
        {
            foreach (HexPoint neighbor in value.Location.Neighbors())
            {
                if (!_map.IsStar(neighbor) && _map.Contains(neighbor))
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
