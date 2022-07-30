// Ishan Pranav's REBUS: MoveCommand.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus.Commands
{
    public abstract class MoveCommand : Command
    {
        public override Task<bool> ExecuteAsync(IExecutor executor)
        {
            return executor.MoveAsync(UnitIds, PlayerId, Destination);
        }
    }
}
