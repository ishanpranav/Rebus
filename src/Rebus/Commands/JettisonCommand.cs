// Ishan Pranav's REBUS: JettisonCommand.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using MessagePack;

namespace Rebus.Commands
{
    [MessagePackObject]
    public class JettisonCommand : Command
    {
        [IgnoreMember]
        protected override int Priority
        {
            get
            {
                return 5;
            }
        }

        public override Task<bool> ExecuteAsync(IExecutor executor)
        {
            return executor.JettisonAsync(UnitIds);
        }
    }
}
