// Ishan Pranav's REBUS: SellCommand.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using Rebus.Server.ExecutionContexts;

namespace Rebus.Server.Commands
{
    internal sealed class SellCommand : CommerceCommand
    {
        public override CommandType Type
        {
            get
            {
                return CommandType.Sell;
            }
        }

        public override void Continue(ExecutionContext context, Unit unit, Commodity commodity)
        {
            if (commodity.Quantity < 0 && unit.Commodity == commodity.Mass)
            {
                context.Player.Credits += commodity.Price;

                commodity.Quantity++;
                commodity.Price--;

                unit.Commodity = 0;
            }
        }
    }
}
