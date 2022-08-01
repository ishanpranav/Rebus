// Ishan Pranav's REBUS: Controller.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.EventArgs;
using Rebus.Server.Commands;
using Tracery;
using Tracery.ContentSelectors;

namespace Rebus.Server
{
    internal sealed class Controller : IGameService, ILoginService
    {
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;
        private readonly CantorPairing _pairing;
        private readonly Grammar _grammar;
        private readonly Dictionary<CommandType, ICommand> _commands;
        private readonly Map _map;
        private readonly Namer _namer;

        public event EventHandler<ConflictEventArgs>? ConflictResolved;

        public Controller(IDbContextFactory<RebusDbContext> contextFactory, IEnumerable<ICommand> commands, CantorPairing pairing, Grammar grammar, Map map, Namer namer)
        {
            _contextFactory = contextFactory;
            _pairing = pairing;
            _grammar = grammar;
            _commands = commands.ToDictionary(x => x.Type);
            _map = map;
            _namer = namer;
        }

        public Task<Configuration> GetConfigurationAsync()
        {
            return Task.FromResult(new Configuration(_map.Radius));
        }

        public async Task<Player> GetPlayerAsync(int playerId)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                return await context.Players.SingleAsync(x => x.Id == playerId);
            }
        }

        public async Task<Economy?> GetEconomyAsync(int playerId, HexPoint location)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                if (await context.Zones.AnyAsync(x => x.Q == location.Q && x.R == location.R && x.PlayerId == playerId) && _map.TryGetLayers(location, out IReadOnlyList<int>? layers) && layers.Count == Depths.Planet)
                {
                    Biome biome = _map.GetBiome(location, layers);
                    string? name = _namer.Name(layers);
                    Random random = new Random(_pairing.Pair(location.Q, location.R));

                    if (name == null)
                    {
                        _grammar[nameof(name)] = Array.Empty<string>();
                    }
                    else
                    {
                        _grammar[nameof(name)] = new string[]
                        {
                            name
                        };
                    }

                    List<Commodity> commodities;

                    commodities = await context.Commodities
                       .Where(x => x.Q == location.Q && x.R == location.R)
                       .ToListAsync();

                    if (commodities.Count == 0)
                    {
                        for (int i = 1; i < random.Next(maxValue: 10); i++)
                        {
                            int sign;

                            if (random.NextDouble() < 0.5)
                            {
                                sign = -1;
                            }
                            else
                            {
                                sign = 1;
                            }

                            Commodity commodity = new Commodity()
                            {
                                Location = location,
                                Mass = i,
                                Price = random.Next(minValue: 1, maxValue: 1000),
                                Quantity = sign * random.Next(minValue: 1, maxValue: 1000)
                            };

                            commodities.Add(commodity);

                            await context.Commodities.AddAsync(commodity);
                        }

                        await context.SaveChangesAsync();
                    }

                    return new Economy(_grammar.Flatten(biome.Key, new RandomContentSelector(random)), commodities);
                }
                else
                {
                    return null;
                }
            }
        }

        public async IAsyncEnumerable<ZoneInfo> GetZonesAsync(int playerId)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                HashSet<HexPoint> locations = new HashSet<HexPoint>();

                foreach (Zone zone in context.Zones
                    .Where(x => x.PlayerId == playerId)
                    .Include(x => x.Units)
                    .ThenInclude(x => x.Sanctuary))
                //foreach (Zone zone in _map.Origin.Range(_map.Radius).Select(x => new Zone()
                //{
                //    Q = x.Q,
                //    R = x.R,
                //    PlayerId = playerId
                //}))
                {
                    if (locations.Add(zone.Location))
                    {
                        HashSet<HexPoint> neighbors = new HashSet<HexPoint>();

                        string? name;
                        Biome biome;

                        if (_map.TryGetLayers(zone.Location, out IReadOnlyList<int>? layers))
                        {
                            name = _namer.Name(layers);
                            biome = _map.GetBiome(zone.Location, layers);

                            foreach (HexPoint neighbor in zone.Location.Neighbors())
                            {
                                if (_map.TryGetLayers(neighbor, out IReadOnlyList<int>? neighborLayers) && neighborLayers.Count == Depths.Star && locations.Add(neighbor))
                                {
                                    yield return new ZoneInfo(neighbor, playerId: 0, _namer.Name(neighborLayers), _map.GetBiome(neighbor, neighborLayers), neighborLayers, Array.Empty<Unit>(), Array.Empty<HexPoint>());
                                }
                                else if (_map.Contains(neighbor))
                                {
                                    neighbors.Add(neighbor);
                                }
                            }

                            yield return new ZoneInfo(zone.Location, zone.PlayerId, name, biome, layers, new List<Unit>(zone.Units), neighbors);
                        }
                    }
                }
            }
        }

        public async IAsyncEnumerable<HexPoint> GetDestinationsAsync(int playerId, CommandType type, ZoneInfo source)
        {
            ICommand command = _commands[type];

            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                Dictionary<HexPoint, ZoneInfo> dictionary = new Dictionary<HexPoint, ZoneInfo>();

                await foreach (ZoneInfo zone in GetZonesAsync(playerId))
                {
                    dictionary.Add(zone.Location, zone);
                }

                foreach (HexPoint result in command.GetDestinations(source, dictionary))
                {
                    yield return result;
                }
            }
        }

        public async Task<CommandResponse> ExecuteAsync(CommandRequest request)
        {
            await using (RebusDbContext dbContext = await _contextFactory.CreateDbContextAsync())
            {
                Player player = await dbContext.Players.SingleAsync(x => x.Id == request.PlayerId);

                await using (ExecutionContext context = new ExecutionContext(controller: this, dbContext, player, request.UnitIds, request.CommodityMass)
                {
                    Destination = request.Destination
                })
                {
                    await _commands[request.Type].ExecuteAsync(context);

                    return new CommandResponse(await context.Database.SaveChangesAsync() > 0, player);
                }
            }
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
                    var zones = context.Zones
                        .Include(x => x.Units)
                        .Where(x => x.Units.Count == 0);
                    PriorityQueue<HexPoint, int> locations = new PriorityQueue<HexPoint, int>();

                    foreach (HexPoint location in _map.Origin.Spiral(_map.Radius))
                    {
                        if (_map.TryGetLayers(location, out IReadOnlyList<int>? layers) && layers.Count != Depths.Star)
                        {
                            int priority = await zones.CountAsync(x => x.Q == location.Q && x.R == location.R);

                            if (priority == 0)
                            {
                                await addAsync(location);

                                locations.Clear();

                                break;
                            }
                            else
                            {
                                locations.Enqueue(location, priority);
                            }
                        }
                    }

                    if (locations.Count > 0)
                    {
                        await addAsync(locations.Dequeue());
                    }

                    async Task addAsync(HexPoint location)
                    {
                        await context.Zones.AddAsync(new Zone()
                        {
                            Location = location,
                            Player = new Player()
                            {
                                Username = username,
                                Password = password
                            },
                            Units = new Unit[]
                            {
                                new Unit()
                                {
                                    Name = Guid
                                        .NewGuid()
                                        .ToString()
                                }
                            }
                        });

                        await context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateException) { }
            }

            return await LoginAsync(username, password);
        }

        public void OnConflictResolved(ConflictEventArgs e)
        {
            ConflictResolved?.Invoke(sender: this, e);
        }

        public void Dispose() { }
    }
}
