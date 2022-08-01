// Ishan Pranav's REBUS: NullCommand.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebus.Server.Commands
{
    internal sealed class NullCommand : ICommand
    {
        public CommandType Type
        {
            get
            {
                return CommandType.None;
            }
        }

        public Task ExecuteAsync(ExecutionContext context)
        {
            return Task.CompletedTask;
        }

        public IEnumerable<HexPoint> GetDestinations(ZoneInfo source, IReadOnlyDictionary<HexPoint, ZoneInfo> zones)
        {
            yield return source.Location;
        }
    }
}
