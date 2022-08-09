// Ishan Pranav's REBUS: AutopilotCommand.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rebus.Server.ExecutionContexts;
using Rebus.Server.Functions;

namespace Rebus.Server.Commands
{
    internal sealed class AutopilotCommand : NavigationCommand
    {
        public override CommandType Type
        {
            get
            {
                return CommandType.Autopilot;
            }
        }

        public AutopilotCommand(Map map, RuleSet rules) : base(map, rules) { }

        protected override async Task ExecuteAsync(ExecutionContext context, IList<Unit> invaders, Zone source, Zone? destination, ZoneFunctions functions)
        {
            if (destination != null)
            {
                Stack<Zone> steps = AStarSearch.Search(source, destination, functions);

                if (steps.Count > 0)
                {
                    Zone previous = steps.Pop();

                    while (steps.TryPop(out Zone? current))
                    {
                        if (await ContinueAsync(context, invaders, previous, current, functions))
                        {
                            previous = current;
                        }
                        else
                        {
                            return;
                        }
                    }

                    context.SetLocation(context.Destination);
                }
            }
        }

        public override IEnumerable<HexPoint> GetDestinations(ZoneInfo source, IReadOnlyDictionary<HexPoint, ZoneInfo> zones)
        {
            return zones.Values
                .Where(x => x.Layers.Count != Depths.Star && x.Location != source.Location)
                .Select(x => x.Location);
        }
    }
}
