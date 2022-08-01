// Ishan Pranav's REBUS: ICommand.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebus.Server.Commands
{
    internal interface ICommand
    {
        CommandType Type { get; }

        Task ExecuteAsync(ExecutionContext context);

        IEnumerable<HexPoint> GetDestinations(ZoneInfo source, IReadOnlyDictionary<HexPoint, ZoneInfo> zones);
    }
}
