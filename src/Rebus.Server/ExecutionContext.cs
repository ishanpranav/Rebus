// Ishan Pranav's REBUS: ExecutionContext.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.EventArgs;

namespace Rebus.Server
{
    internal sealed class ExecutionContext : IAsyncDisposable, IDisposable
    {
        private readonly Controller _controller;

        private RebusDbContext? _dbContext;

        public RebusDbContext Database
        {
            get
            {
                return _dbContext ?? throw new ObjectDisposedException(nameof(Database));
            }
        }

        public User User { get; }
        public IReadOnlyCollection<int> UnitIds { get; }
        public HexPoint Destination { get; set; }
        public int CommodityMass { get; }

        public ExecutionContext(Controller controller, RebusDbContext dbContext, User user, IReadOnlyCollection<int> unitIds, int commodityMass)
        {
            _controller = controller;
            _dbContext = dbContext;
            User = user;
            UnitIds = unitIds;
            CommodityMass = commodityMass;
        }

        public async IAsyncEnumerable<Unit> GetUnitsAsync()
        {
            foreach (int unitId in UnitIds)
            {
                yield return await Database.Units.SingleAsync(x => x.Id == unitId);
            }
        }

        public Task<Zone> GetDestinationAsync()
        {
            return Database.Zones.SingleAsync(x => x.Q == Destination.Q && x.R == Destination.R && x.PlayerId == User.PlayerId);
        }

        public void OnConflictResolved(ConflictEventArgs e)
        {
            _controller.OnConflictResolved(e);
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
            _dbContext = null;

            GC.SuppressFinalize(obj: this);
        }

        public async ValueTask DisposeAsync()
        {
            if (_dbContext != null)
            {
                await _dbContext
                    .DisposeAsync()
                    .ConfigureAwait(continueOnCapturedContext: false);

                _dbContext = null;
            }

            GC.SuppressFinalize(obj: this);
        }
    }
}
