// Ishan Pranav's REBUS: CommerceCommand.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server.Commands
{
    internal abstract class CommerceCommand : ICommand
    {
        public abstract CommandType Type { get; }

        public abstract void Continue(ExecutionContext context, Unit unit, Commodity commodity);

        public async Task ExecuteAsync(ExecutionContext context)
        {
            if (context.CommodityMass > 0)
            {
                Zone destination = await context.GetDestinationAsync();

                await foreach (Unit unit in context.GetUnitsAsync())
                {
                    Commodity commodity = await context.Database.Commodities.SingleAsync(x => x.Q == destination.Q && x.R == destination.R && x.Mass == context.CommodityMass);

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

        public IEnumerable<HexPoint> GetDestinations(ZoneInfo source, IReadOnlyDictionary<HexPoint, ZoneInfo> zones)
        {
            yield return source.Location;
        }
    }
}
