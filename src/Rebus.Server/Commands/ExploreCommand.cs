// Ishan Pranav's REBUS: ExploreCommand.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rebus.Server.Functions;

namespace Rebus.Server.Commands
{
    internal sealed class ExploreCommand : NavigationCommand
    {
        public override CommandType Type
        {
            get
            {
                return CommandType.Explore;
            }
        }

        public ExploreCommand(Map map, RuleSet rules) : base(map, rules) { }

        protected override async Task ExecuteAsync(ExecutionContext context, IList<Unit> invaders, Zone source, Zone? destination, ZoneFunctions functions)
        {
            if (HexPoint.Distance(source.Location, context.Destination) == 1)
            {
                if (destination == null)
                {
                    destination = new Zone()
                    {
                        Player = context.Player,
                        Location = context.Destination
                    };
                }

                await ContinueAsync(context, invaders, source, destination, functions);
            }
        }

        public override IEnumerable<HexPoint> GetDestinations(ZoneInfo source, IReadOnlyDictionary<HexPoint, ZoneInfo> zones)
        {
            IEnumerable<HexPoint> results = source.Neighbors.Where(x => !zones.ContainsKey(x));

            if (results.Any())
            {
                foreach (HexPoint result in results)
                {
                    yield return result;
                }
            }
            else
            {
                yield return source.Location;
            }
        }
    }
}
