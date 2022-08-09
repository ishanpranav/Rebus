// Ishan Pranav's REBUS: NavigationCommand.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.EventArgs;
using Rebus.Server.ExecutionContexts;
using Rebus.Server.Functions;

namespace Rebus.Server.Commands
{
    internal abstract class NavigationCommand : ICommand, IDestinationProvider
    {
        private readonly Map _map;
        private readonly RuleSet _rules;

        public abstract CommandType Type { get; }

        public NavigationCommand(Map map, RuleSet rules)
        {
            _map = map;
            _rules = rules;
        }

        protected async Task<bool> ContinueAsync(ExecutionContext context, IList<Unit> invaders, Zone source, Zone destination, ZoneFunctions functions)
        {
            List<Unit> occupants = await context.Database.Units
                .Where(x => x.Zone.Q == destination.Q && x.Zone.R == destination.R && x.Zone.Player != context.Player)
                .OrderByDescending(x => x.Commodity)
                .Include(x => x.Zone)
                .ThenInclude(x => x.Player)
                .Include(x => x.Sanctuary)
                .ToListAsync();
            List<Unit> defenders = new List<Unit>();

            int deltaCredits = -invaders.Count * functions.Cost(source, destination);

            context.Player.Credits += deltaCredits;

            if (occupants.Count == 0)
            {
                invade();

                return true;
            }
            else
            {
                Player adversary = occupants[0].Zone.Player;

                foreach (Unit unit in occupants)
                {
                    if (unit.Sanctuary == null || await context.Database.Units.AnyAsync(x => x.Zone.Q == unit.Sanctuary.Q && x.Zone.R == unit.Sanctuary.R && x.Zone.PlayerId != context.Player.Id))
                    {
                        defenders.Add(unit);
                    }
                    else
                    {
                        adversary.Credits -= functions.Cost(unit.Zone, unit.Sanctuary);
                        unit.Zone = unit.Sanctuary;
                    }

                    unit.Sanctuary = null;
                }

                Fleet invader = new Fleet(context.Player.Name, invaders.Count);
                Fleet occupant = new Fleet(adversary.Name, occupants.Count);

                if (defenders.Count == 0)
                {
                    invade();

                    context.OnConflictResolved(new ConflictEventArgs(destination.Location, invasionSucceeded: true, invader, occupant, occupants.Count));
                }
                else if (invaders.Count == defenders.Count)
                {
                    context.OnConflictResolved(new ConflictEventArgs(destination.Location, invasionSucceeded: false, invader, occupant, invaders.Count));
                }
                else
                {
                    bool invasionSucceeded = invaders.Count > defenders.Count;
                    IList<Unit> largerFleet;
                    IList<Unit> smallerFleet;
                    Zone largerZone;

                    if (invasionSucceeded)
                    {
                        invade();

                        largerFleet = invaders;
                        largerZone = destination;
                        smallerFleet = defenders;
                    }
                    else
                    {
                        largerFleet = defenders;
                        largerZone = defenders[0].Zone;
                        smallerFleet = invaders;
                    }

                    int unitsCaptured = Math.Max(_rules.MinUnitsCaptured, (int)(smallerFleet.Count * _rules.UnitCaptureRate));
                    int unitsDestroyed = smallerFleet.Count - unitsCaptured;

                    for (int i = 0; i < unitsCaptured; i++)
                    {
                        smallerFleet[i].Zone = largerZone;
                        smallerFleet[i].Sanctuary = null;
                    }

                    for (int i = unitsCaptured; i < smallerFleet.Count; i++)
                    {
                        context.Database.Units.Remove(smallerFleet[i]);
                    }

                    for (int i = 0; i < unitsDestroyed; i++)
                    {
                        context.Database.Units.Remove(largerFleet[i]);
                    }

                    context.OnConflictResolved(new ConflictEventArgs(destination.Location, invasionSucceeded, invader, occupant, occupants.Count - defenders.Count, unitsDestroyed, unitsCaptured));
                }

                return false;
            }

            void invade()
            {
                context.SetLocation(destination.Location);

                foreach (Unit invader in invaders)
                {
                    invader.Zone = destination;
                    invader.Sanctuary = null;
                }
            }
        }

        protected abstract Task ExecuteAsync(ExecutionContext context, IList<Unit> invaders, Zone source, Zone? destination, ZoneFunctions functions);

        public async Task ExecuteAsync(ExecutionContext context)
        {
            List<Unit> invaders = await context.Database.Units
                .Where(x => context.UnitIds.Contains(x.Id))
                .OrderByDescending(x => x.Commodity)
                .Include(x => x.Sanctuary)
                .Include(x => x.Zone)
                .ToListAsync();
            ZoneFunctions functions = new ZoneFunctions(context.Player.Id, context.Database.Zones, _map, _rules);

            if (invaders.Count > 0)
            {
                await ExecuteAsync(context, invaders, invaders[0].Zone, await context.Database.Zones.SingleOrDefaultAsync(x => x.Q == context.Destination.Q && x.R == context.Destination.R && x.PlayerId == context.Player.Id), functions);
            }
        }

        public abstract IEnumerable<HexPoint> GetDestinations(ZoneInfo source, IReadOnlyDictionary<HexPoint, ZoneInfo> zones);
    }
}
