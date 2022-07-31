// Ishan Pranav's REBUS: RpcService.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Commands;
using Rebus.EventArgs;
using Rebus.Server.Functions;

namespace Rebus.Server
{
    public class RpcService : IExecutor, IGameService, ILoginService
    {
        private readonly AStarSearch _search;
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;
        private readonly Map _map;
        private readonly Namer _namer;
        private readonly RuleSet _rules;

        public event EventHandler<ConflictEventArgs>? ConflictResolved;

        public RpcService(AStarSearch search, IDbContextFactory<RebusDbContext> contextFactory, Map map, Namer namer, RuleSet rules)
        {
            _search = search;
            _contextFactory = contextFactory;
            _map = map;
            _namer = namer;
            _rules = rules;
        }

        public async Task<bool> RetreatAsync(ICollection<int> unitIds, int playerId, HexPoint destination)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                Zone zone = await context.Zones.SingleAsync(x => x.Q == destination.Q && x.R == destination.R && x.PlayerId == playerId);

                foreach (int unitId in unitIds)
                {
                    Unit unit = await context.Units.SingleAsync(x => x.Id == unitId);

                    unit.Sanctuary = zone;
                }

                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> DefendAsync(ICollection<int> unitIds)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                foreach (int unitId in unitIds)
                {
                    Unit unit = await context.Units.SingleAsync(x => x.Id == unitId);

                    unit.Sanctuary = null;
                }

                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> JettisonAsync(ICollection<int> unitIds)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                foreach (int unitId in unitIds)
                {
                    Unit unit = await context.Units.SingleAsync(x => x.Id == unitId);

                    unit.CargoMass = null;
                }

                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> MoveAsync(ICollection<int> unitIds, int playerId, HexPoint destination)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                Player player = await context.Players.SingleAsync(x => x.Id == playerId);
                List<Unit> invaders = await context.Units
                    .Where(x => unitIds.Contains(x.Id))
                    .OrderByDescending(x => x.CargoMass)
                    .ToListAsync();
                Player adversary = await context.Zones
                    .Where(x => x.Id == invaders[0].ZoneId)
                    .Select(x => x.Player)
                    .SingleAsync();
                ZoneFunctions functions = new ZoneFunctions(playerId, context.Zones, _map, _rules);

                if (invaders.Count > 0)
                {
                    Zone start = await context.Zones.SingleAsync(x => x.Id == invaders[0].ZoneId);

                    if (HexPoint.Distance(start.Location, destination) == 1)
                    {
                        Zone? end = await context.Zones.SingleOrDefaultAsync(x => x.Q == destination.Q && x.R == destination.R && x.PlayerId == playerId);

                        if (end == default)
                        {
                            end = new Zone()
                            {
                                Player = player,
                                Location = destination
                            };
                        }

                        await moveAsync(start, end);
                    }
                    else
                    {
                        Stack<Zone> steps = _search.Search(start, await context.Zones.SingleAsync(x => x.Q == destination.Q && x.R == destination.R && x.PlayerId == playerId), functions);

                        start = steps.Pop();

                        while (steps.TryPop(out Zone? step))
                        {
                            if (await moveAsync(start, step))
                            {
                                start = step;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    async Task<bool> moveAsync(Zone previous, Zone current)
                    {
                        List<Unit> occupants = await context.Units
                            .Where(x => x.Zone.Q == current.Q && current.R == current.R && x.Zone.PlayerId != playerId)
                            .OrderByDescending(x => x.CargoMass)
                            .Include(x => x.Sanctuary)
                            .ToListAsync();
                        List<Unit> defenders = new List<Unit>();

                        foreach (Unit occupant in occupants)
                        {
                            if (occupant.Sanctuary == null || await context.Units.AnyAsync(x => x.Zone.Q == occupant.Sanctuary.Q && x.Zone.R == occupant.Sanctuary.R && x.Zone.PlayerId != playerId))
                            {
                                defenders.Add(occupant);
                            }
                            else
                            {
                                adversary.Credits -= functions.Cost(occupant.Zone, occupant.Sanctuary);
                                occupant.Zone = occupant.Sanctuary;
                            }

                            occupant.Sanctuary = null;
                        }

                        player.Credits -= invaders.Count * functions.Cost(previous, current);

                        if (occupants.Count == 0)
                        {
                            invade();

                            return true;
                        }
                        else
                        {
                            Fleet invader = new Fleet(player.Username, invaders.Count);
                            Fleet occupant = new Fleet(adversary.Username, occupants.Count);

                            if (defenders.Count == 0)
                            {
                                invade();
                                OnConflictResolved(new ConflictEventArgs(current.Location, invasionSucceeded: true, invader, occupant, occupants.Count));
                            }
                            else if (invaders.Count == defenders.Count)
                            {
                                OnConflictResolved(new ConflictEventArgs(current.Location, invasionSucceeded: false, invader, occupant, invaders.Count));
                            }
                            else
                            {
                                bool invasionSucceeded = invaders.Count > defenders.Count;
                                List<Unit> largerFleet;
                                List<Unit> smallerFleet;
                                Zone largerZone;

                                if (invasionSucceeded)
                                {
                                    invade();

                                    largerFleet = invaders;
                                    largerZone = current;
                                    smallerFleet = defenders;
                                }
                                else
                                {
                                    largerFleet = defenders;
                                    largerZone = defenders[0].Zone;
                                    smallerFleet = invaders;
                                }

                                int unitsCaptured = Math.Max(_rules.MinUnitsCaptured, smallerFleet.Count / 4);
                                int unitsDestroyed = smallerFleet.Count - unitsCaptured;

                                for (int i = 0; i < unitsCaptured; i++)
                                {
                                    smallerFleet[i].Zone = largerZone;
                                }

                                for (int i = unitsCaptured; i < smallerFleet.Count; i++)
                                {
                                    context.Units.Remove(smallerFleet[i]);
                                }

                                for (int i = 0; i < unitsDestroyed; i++)
                                {
                                    context.Units.Remove(largerFleet[i]);
                                }

                                OnConflictResolved(new ConflictEventArgs(current.Location, invasionSucceeded, invader, occupant, occupants.Count - defenders.Count, unitsDestroyed, unitsCaptured));
                            }

                            return false;
                        }

                        void invade()
                        {
                            foreach (Unit invader in invaders)
                            {
                                invader.Zone = current;
                            }
                        }
                    }
                }

                return await context.SaveChangesAsync() > 0;
            }
        }

        public Task<Configuration> ConfigureAsync()
        {
            return Task.FromResult(new Configuration(_map.Radius));
        }

        public async IAsyncEnumerable<ZoneResult> GetZonesAsync(int playerId)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                HashSet<HexPoint> locations = new HashSet<HexPoint>();

                foreach (Zone zone in context.Zones
                    .Where(x => x.PlayerId == playerId)
                    .Include(x => x.Units))
                {
                    if (locations.Add(zone.Location))
                    {
                        List<HexPoint> neighbors = new List<HexPoint>();

                        string? name;
                        Biome biome;

                        if (_map.TryGetLayers(zone.Location, out IReadOnlyList<int>? layers))
                        {
                            name = _namer.Name(_map.Stars, _map.Planets, layers);
                            biome = _map.GetBiome(zone.Location, layers);

                            foreach (HexPoint neighbor in zone.Location.Neighbors())
                            {
                                if (_map.Contains(neighbor))
                                {
                                    if (_map.TryGetLayers(neighbor, out IReadOnlyList<int>? neighborLayers) && _map.GetBiome(neighbor, neighborLayers) == Biome.Stellar && locations.Add(neighbor))
                                    {
                                        yield return new ZoneResult(neighbor, _namer.Name(_map.Stars, _map.Planets, neighborLayers), neighborLayers);
                                    }
                                    else
                                    {
                                        neighbors.Add(neighbor);
                                    }
                                }
                            }

                            yield return new ZoneResult(zone.Location, zone.PlayerId, biome, name, layers, new List<Unit>(zone.Units), neighbors);
                        }
                    }
                }
            }
        }

        public async Task<int> GetCreditsAsync(int playerId)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                return await context.Players
                    .Where(x => x.Id == playerId)
                    .Select(x => x.Credits)
                    .SingleAsync();
            }
        }

        public Task<bool> ExecuteAsync(Command command)
        {
            return command.ExecuteAsync(executor: this);
        }

        public async Task<int> LoginAsync(string username, string password)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                return (await context.Players.SingleOrDefaultAsync(x => x.Username == username && x.Password == password))?.Id ?? default;
            }
        }

        public async Task<int> RegisterAsync(string username, string password)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                try
                {
                    Player player = new Player()
                    {
                        Username = username,
                        Password = password
                    };

                    await context.Players.AddAsync(player);

                    await context.Zones.AddAsync(new Zone()
                    {
                        Player = player,
                        Units = new Unit[]
                        {
                            new Unit()
                        }
                    });

                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    return default;
                }
            }

            return await LoginAsync(username, password);
        }

        protected virtual void OnConflictResolved(ConflictEventArgs e)
        {
            ConflictResolved?.Invoke(sender: this, e);
        }

        protected virtual void Dispose(bool disposing) { }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
