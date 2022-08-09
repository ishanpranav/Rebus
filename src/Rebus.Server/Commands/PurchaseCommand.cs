// Ishan Pranav's REBUS: PurchaseCommand.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using Rebus.Server.ExecutionContexts;

namespace Rebus.Server.Commands
{
    internal sealed class PurchaseCommand : CommerceCommand
    {
        public override CommandType Type
        {
            get
            {
                return CommandType.Purchase;
            }
        }

        public override void Continue(ExecutionContext context, Unit unit, Commodity commodity)
        {
            if (commodity.Quantity > 0 && unit.Commodity == 0)
            {
                context.Player.Credits -= commodity.Price;

                commodity.Quantity--;
                commodity.Price++;

                unit.Commodity = commodity.Mass;
            }
        }
    }
}
