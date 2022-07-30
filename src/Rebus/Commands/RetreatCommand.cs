// Ishan Pranav's REBUS: RetreatCommand.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagePack;

namespace Rebus.Commands
{
    public class RetreatCommand : Command
    {
        [IgnoreMember]
        protected override int Priority
        {
            get
            {
                return 4;
            }
        }

        public override Task<bool> ExecuteAsync(IExecutor executor)
        {
            return executor.RetreatAsync(UnitIds, PlayerId, Destination);
        }

        public override IEnumerable<HexPoint> Filter(ZoneResult source, IReadOnlyDictionary<HexPoint, ZoneResult> domain)
        {
            return source.Neighbors.Where(x => domain.ContainsKey(x));
        }
    }
}
