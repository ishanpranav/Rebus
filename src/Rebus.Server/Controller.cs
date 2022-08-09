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
            grammar.AddTracery();

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

        public async Task<User> GetUserAsync(int userId)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                return await context.Users
                    .Include(x => x.Player)
                    .SingleAsync(x => x.Id == userId);
            }
        }

        public async Task<Economy?> GetEconomyAsync(int playerId, HexPoint location)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                if (_map.TryGetLayers(location, out IReadOnlyList<int>? layers))
                {
                    string? name = _namer.Name(layers);

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

                    Biome biome = _map.GetBiome(location, layers);
                    Random random = new Random(_pairing.Pair(location.Q, location.R));
                    ExponentialDistribution exponentialDistribution = new ExponentialDistribution(random, lambda: 2.5);
                    RandomContentSelector selector = new RandomContentSelector(random);
                    string description = _grammar.Flatten(biome.Key, selector);

                    if (await context.Zones.AnyAsync(x => x.Q == location.Q && x.R == location.R && x.PlayerId == playerId) && layers.Count == Depths.Planet)
                    {
                        SortedDictionary<int, Commodity> commodities = new SortedDictionary<int, Commodity>();

                        foreach (Commodity commodity in context.Commodities.Where(x => x.Q == location.Q && x.R == location.R))
                        {
                            commodities.Add(commodity.Mass, commodity);
                        }

                        if (commodities.Count == 0)
                        {
                            int max = biome.Population * random.Next(minValue: 1, maxValue: 3);

                            while (commodities.Count < max)
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

                                int mass = exponentialDistribution.Next(minValue: 1, maxValue: 118);

                                Commodity commodity = new Commodity()
                                {
                                    Location = location,
                                    Mass = mass,
                                    Price = random.Next(minValue: 1, maxValue: 101),
                                    Quantity = sign * random.Next(minValue: 1, maxValue: 101)
                                };

                                if (commodities.TryAdd(mass, commodity))
                                {
                                    await context.Commodities.AddAsync(commodity);
                                }
                            }

                            await context.SaveChangesAsync();
                        }

                        return new Economy(description, commodities.Values);
                    }
                    else
                    {
                        return new Economy(description, Array.Empty<Commodity>());
                    }
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

                foreach (Zone zone in getZones())
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

                IEnumerable<Zone> getZones()
                {
                    if (playerId == -1)
                    {
                        return _map.Origin
                            .Range(_map.Radius)
                            .Select(x => new Zone()
                            {
                                Q = x.Q,
                                R = x.R,
                                PlayerId = playerId
                            });
                    }
                    else
                    {
                        return context.Zones
                            .Where(x => x.PlayerId == playerId)
                            .Include(x => x.Units)
                            .ThenInclude(x => x.Sanctuary);
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

                foreach (HexPoint result in command
                    .GetDestinations(source, dictionary)
                    .OrderByDescending(x =>
                    {
                        if (dictionary.TryGetValue(x, out ZoneInfo? zone))
                        {
                            return zone.Layers.Count;
                        }
                        else
                        {
                            return 0;
                        }
                    }))
                {
                    yield return result;
                }
            }
        }

        public async Task<CommandResponse> ExecuteAsync(CommandRequest request)
        {
            await using (RebusDbContext dbContext = await _contextFactory.CreateDbContextAsync())
            {
                User user = await dbContext.Users
                    .Include(x => x.Player)
                    .SingleAsync(x => x.Id == request.UserId);

                await using (ExecutionContext context = new ExecutionContext(controller: this, dbContext, user, request.UnitIds, request.CommodityMass)
                {
                    Destination = request.Destination
                })
                {
                    await _commands[request.Type].ExecuteAsync(context);

                    return new CommandResponse(await context.Database.SaveChangesAsync() > 0, user);
                }
            }
        }

        public async Task<int> LoginAsync(string name, string password)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                return (await context.Users
                    .Include(x => x.Player)
                    .SingleOrDefaultAsync(x => x.Player.Name == name && x.Password == password))?.Id ?? default;
            }
        }

        public async Task<int> RegisterAsync(string name, string password)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                try
                {
                    IQueryable<Zone> zones = context.Zones
                        .Include(x => x.Units)
                        .Where(x => x.Units.Count == 0);
                    List<HexPoint> range = _map.Origin
                        .Spiral(_map.Radius)
                        .ToList();
                    User user = new User()
                    {
                        Password = password,
                        Player = new Player()
                        {
                            Name = name
                        },
                        Twin = new Player()
                        {
                            Name = '~' + name
                        }
                    };
                    Zone playerZone = new Zone()
                    {
                        Player = user.Player,
                        Location = await getLocationAsync(),
                        Units = new Unit[]
                        {
                            new Unit()
                            {
                                Name = Guid
                                    .NewGuid()
                                    .ToString()
                            }
                        }
                    };

                    range.Remove(playerZone.Location);
                    range.Reverse();

                    Zone twinZone = new Zone()
                    {
                        Player = user.Twin,
                        Location = await getLocationAsync(),
                        Units = new Unit[]
                        {
                            new Unit()
                            {
                                Name = Guid
                                    .NewGuid()
                                    .ToString()
                            }
                        }
                    };

                    await context.Users.AddAsync(user);
                    await context.Zones.AddAsync(playerZone);
                    await context.Zones.AddAsync(twinZone);

                    await context.SaveChangesAsync();

                    async Task<HexPoint> getLocationAsync()
                    {
                        PriorityQueue<HexPoint, int> locations = new PriorityQueue<HexPoint, int>();

                        foreach (HexPoint location in range)
                        {
                            if (_map.TryGetLayers(location, out IReadOnlyList<int>? layers) && layers.Count != Depths.Star)
                            {
                                int priority = await zones.CountAsync(x => x.Q == location.Q && x.R == location.R);

                                if (priority == 0)
                                {
                                    return location;
                                }
                                else
                                {
                                    locations.Enqueue(location, priority);
                                }
                            }
                        }

                        if (locations.Count > 0)
                        {
                            return locations.Dequeue();
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }
                }
                catch (DbUpdateException) { }
            }

            return await LoginAsync(name, password);
        }

        public void OnConflictResolved(ConflictEventArgs e)
        {
            ConflictResolved?.Invoke(sender: this, e);
        }

        public void Dispose() { }
    }
}
