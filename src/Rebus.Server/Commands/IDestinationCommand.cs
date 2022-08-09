// Ishan Pranav's REBUS: IDestinationCommand.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus.Server.Commands
{
    internal interface IDestinationProvider
    {
        IEnumerable<HexPoint> GetDestinations(ZoneInfo source, IReadOnlyDictionary<HexPoint, ZoneInfo> zones);
    }
}
