// Ishan Pranav's REBUS: RetreatCommand.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rebus.Server.ExecutionContexts;

namespace Rebus.Server.Commands
{
    internal sealed class RetreatCommand : ICommand, IDestinationProvider
    {
        public CommandType Type
        {
            get
            {
                return CommandType.Retreat;
            }
        }

        public async Task ExecuteAsync(ExecutionContext context)
        {
            Zone zone = await context.GetDestinationAsync();

            await foreach (Unit unit in context.GetUnitsAsync())
            {
                if (zone.Location != unit.Zone.Location)
                {
                    unit.Sanctuary = zone;
                }
            }
        }

        public IEnumerable<HexPoint> GetDestinations(ZoneInfo source, IReadOnlyDictionary<HexPoint, ZoneInfo> zones)
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
