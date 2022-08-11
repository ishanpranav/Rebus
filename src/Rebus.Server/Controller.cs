// Ishan Pranav's REBUS: Controller.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.EventArgs;
using Rebus.Server.ArgumentProviders;
using Rebus.Server.Commands;
using Rebus.Server.Considerations;
using Rebus.Server.ExecutionContexts;
using Tracery;
using Tracery.ContentSelectors;

namespace Rebus.Server
{
    internal sealed class Controller : IGameService, ILoginService
    {
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;
        private readonly Grammar _grammar;
        private readonly IReadOnlyDictionary<CommandType, ICommand> _commands;
        private readonly BehaviorCollection _decisionMaker;
        private readonly Map _map;
        private readonly Namer _namer;

        public event EventHandler<ConflictEventArgs>? ConflictResolved;

        public Controller(IDbContextFactory<RebusDbContext> contextFactory, IReadOnlyDictionary<CommandType, ICommand> commands, BehaviorCollection decisionMaker, Grammar grammar, Map map, Namer namer)
        {
            grammar.AddTracery();

            _contextFactory = contextFactory;
            _grammar = grammar;
            _commands = commands;
            _decisionMaker = decisionMaker;
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

                    int seed = CantorPairing.Pair(location.Q, location.R);

                    Random random = new Random(seed);
                    NegativeExponentialRandom exponentialDistribution = new NegativeExponentialRandom(seed, lambda: 2.5);
                    RandomContentSelector selector = new RandomContentSelector(random);
                    string description = _grammar.Flatten(biome.Key, selector);

                    if (layers.Count == Depths.Planet)
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
            ZoneCache cache = await ZoneCache.CreateAsync(playerId, _contextFactory, _map, _namer, new ArgumentProvider(_commands));

            foreach (ZoneInfo zone in cache.Values)
            {
                yield return zone;
            }
        }

        public async Task<CommandResponse> ExecuteAsync(CommandRequest request)
        {
            User user;
            bool modified;

            await using (RebusDbContext dbContext = await _contextFactory.CreateDbContextAsync())
            {
                user = await dbContext.Users
                   .Include(x => x.Player)
                   .Include(x => x.Twin)
                   .SingleAsync(x => x.Id == request.UserId);

                await using (UserExecutionContext context = new UserExecutionContext(user, controller: this, dbContext, request.Arguments.UnitIds, request.Arguments.Commodity)
                {
                    Destination = request.Arguments.Destination
                })
                {
                    await _commands[request.Arguments.Type].ExecuteAsync(context);

                    modified = await dbContext.SaveChangesAsync() > 0;
                }
            }

            if (modified)
            {
                await using (RebusDbContext dbContext = await _contextFactory.CreateDbContextAsync())
                {
                    Arguments decision = _decisionMaker.Select(new SelectionContext(), (await ZoneCache.CreateAsync(user.Twin.Id, _contextFactory, _map, _namer, new AIArgumentProvider(_commands))).Values.SelectMany(x => x.Arguments), out _);

                    await using (AIExecutionContext context = new AIExecutionContext(user, controller: this, dbContext, decision.UnitIds, decision.Commodity)
                    {
                        Destination = decision.Destination
                    })
                    {
                        await _commands[decision.Type].ExecuteAsync(context);

                        await dbContext.SaveChangesAsync();
                    }
                }
            }

            return new CommandResponse(modified, user);
        }

        public async Task<int> LoginAsync(string name, string password)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                return (await context.Users
                    .Include(x => x.Player)
                    .SingleOrDefaultAsync(x => x.Password == password && x.Player.Name == name))?.Id ?? default;
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
                        Location = await getLocationAsync()
                    };

                    playerZone.Units.Add(new Unit()
                    {
                        Name = Guid
                            .NewGuid()
                            .ToString()
                    });


                    range.Remove(playerZone.Location);
                    range.Reverse();

                    Zone twinZone = new Zone()
                    {
                        Player = user.Twin,
                        Location = await getLocationAsync()
                    };

                    twinZone.Units.Add(new Unit()
                    {
                        Name = Guid
                            .NewGuid()
                            .ToString()
                    });

                    await context.Users.AddAsync(user);
                    await context.Zones.AddAsync(playerZone);
                    await context.Zones.AddAsync(twinZone);

                    await context.SaveChangesAsync();

                    async Task<HexPoint> getLocationAsync()
                    {
                        if (range.Count == 0)
                        {
                            throw new InvalidOperationException();
                        }
                        else
                        {
                            HexPoint minLocation = HexPoint.Empty;
                            int minPriority = int.MaxValue;

                            foreach (HexPoint location in range)
                            {
                                if (_map.TryGetLayers(location, out IReadOnlyList<int>? layers) && layers.Count != Depths.Star)
                                {
                                    int priority = await zones.CountAsync(x => x.Q == location.Q && x.R == location.R);

                                    if (priority == 0)
                                    {
                                        return location;
                                    }
                                    else if (priority < minPriority)
                                    {
                                        minLocation = location;
                                        minPriority = priority;
                                    }
                                }
                            }

                            return minLocation;
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
