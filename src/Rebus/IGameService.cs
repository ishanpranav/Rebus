// Ishan Pranav's REBUS: IGameService.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rebus.Commands;
using Rebus.EventArgs;

namespace Rebus
{
    public interface IGameService : IDisposable
    {
        event EventHandler<ConflictEventArgs> ConflictResolved;

        Task<Configuration> ConfigureAsync();

        IAsyncEnumerable<ZoneResult> GetZonesAsync(int playerId);

        Task<int> GetWealthAsync(int playerId);

        Task<bool> ExecuteAsync(Command command);
    }
}
