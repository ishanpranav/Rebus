// Ishan Pranav's REBUS: CommerceCommand.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Server.ExecutionContexts;

namespace Rebus.Server.Commands
{
    internal abstract class CommerceCommand : ICommand
    {
        public abstract CommandType Type { get; }

        public abstract void Continue(ExecutionContext context, Unit unit, Commodity commodity);

        public async Task ExecuteAsync(ExecutionContext context)
        {
            if (context.Commodity > 0)
            {
                Zone destination = await context.GetDestinationAsync();

                await foreach (Unit unit in context.GetUnitsAsync())
                {
                    Commodity commodity = await context.Database.Commodities.SingleAsync(x => x.Q == destination.Q && x.R == destination.R && x.Mass == context.Commodity);

                    if (commodity.Quantity == 0)
                    {
                        context.Database.Remove(commodity);

                        break;
                    }
                    else
                    {
                        Continue(context, unit, commodity);
                    }
                }
            }
        }
    }
}
