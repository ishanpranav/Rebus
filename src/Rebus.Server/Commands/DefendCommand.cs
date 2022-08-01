// Ishan Pranav's REBUS: DefendCommand.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebus.Server.Commands
{
    internal sealed class DefendCommand : ICommand
    {
        public CommandType Type
        {
            get
            {
                return CommandType.Defend;
            }
        }

        public async Task ExecuteAsync(ExecutionContext context)
        {
            await foreach (Unit unit in context.GetUnitsAsync())
            {
                unit.Sanctuary = null;
            }
        }

        public IEnumerable<HexPoint> GetDestinations(ZoneInfo source, IReadOnlyDictionary<HexPoint, ZoneInfo> zones)
        {
            yield return source.Location;
        }
    }
}
