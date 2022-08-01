// Ishan Pranav's REBUS: JettisonCommand.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebus.Server.Commands
{
    internal sealed class JettisonCommand : ICommand
    {
        public CommandType Type
        {
            get
            {
                return CommandType.Jettison;
            }
        }

        public async Task ExecuteAsync(ExecutionContext context)
        {
            await foreach (Unit unit in context.GetUnitsAsync())
            {
                unit.CargoMass = 0;
            }
        }

        public IEnumerable<HexPoint> GetDestinations(ZoneInfo source, IReadOnlyDictionary<HexPoint, ZoneInfo> zones)
        {
            yield return source.Location;
        }
    }
}
