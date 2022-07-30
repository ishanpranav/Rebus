// Ishan Pranav's REBUS: Defend.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using MessagePack;

namespace Rebus.Commands
{
    [MessagePackObject]
    public class DefendCommand : Command
    {
        [IgnoreMember]
        protected override int Priority
        {
            get
            {
                return 3;
            }
        }

        public override Task<bool> ExecuteAsync(IExecutor executor)
        {
            return executor.DefendAsync(UnitIds);
        }
    }
}
