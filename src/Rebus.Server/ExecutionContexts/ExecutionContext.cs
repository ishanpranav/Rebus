// Ishan Pranav's REBUS: ExecutionContext.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server.ExecutionContexts
{
    internal abstract class ExecutionContext : IAsyncDisposable, IDisposable
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

        public abstract Player Player { get; }

        public IReadOnlyCollection<int> Units { get; }
        public HexPoint Destination { get; set; }
        public int Commodity { get; }

        public ExecutionContext(Controller controller, RebusDbContext dbContext, IReadOnlyCollection<int> units, int commodity)
        {
            _controller = controller;
            _dbContext = dbContext;
            Units = units;
            Commodity = commodity;
        }

        public virtual void SetLocation(HexPoint value) { }

        public async IAsyncEnumerable<Unit> GetUnitsAsync()
        {
            foreach (int unit in Units)
            {
                yield return await Database.Units.SingleAsync(x => x.Id == unit);
            }
        }

        public void OnConflictResolved(ConflictEventArgs e)
        {
            _controller.OnConflictResolved(e);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);

            Dispose(disposing: false);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext?.Dispose();
                _dbContext = null;
            }
        }

        [SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "DisposeAsyncCore")]
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_dbContext != null)
            {
                await _dbContext
                    .DisposeAsync()
                    .ConfigureAwait(continueOnCapturedContext: false);

                _dbContext = null;
            }
        }
    }
}
