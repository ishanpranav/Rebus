// Ishan Pranav's REBUS: IGameService.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rebus.EventArgs;

namespace Rebus
{
    public interface IGameService : IDisposable
    {
        event EventHandler<ConflictEventArgs> ConflictResolved;

        Task<Configuration> GetConfigurationAsync();
        Task<Player> GetPlayerAsync(int playerId);
        Task<Economy?> GetEconomyAsync(int playerId, HexPoint location);
        IAsyncEnumerable<ZoneInfo> GetZonesAsync(int playerId);
        IAsyncEnumerable<HexPoint> GetDestinationsAsync(int playerId, CommandType type, ZoneInfo source);

        Task<CommandResponse> ExecuteAsync(CommandRequest request);
    }
}
