// Ishan Pranav's REBUS: IGameService.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebus
{
    public interface IGameService : IDisposable
    {
        event EventHandler<ConflictEventArgs> ConflictResolved;

        Task<Configuration> GetConfigurationAsync();
        Task<User> GetUserAsync(int userId);
        Task<Economy?> GetEconomyAsync(int playerId, HexPoint location);
        IAsyncEnumerable<ZoneInfo> GetZonesAsync(int playerId);

        Task<CommandResponse> ExecuteAsync(CommandRequest request);
    }
}
