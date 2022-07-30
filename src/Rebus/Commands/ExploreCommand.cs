// Ishan Pranav's REBUS: ExploreCommand.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagePack;

namespace Rebus.Commands
{
    public class ExploreCommand : MoveCommand
    {
        [IgnoreMember]
        protected override int Priority
        {
            get
            {
                return 1;
            }
        }

        public override IEnumerable<HexPoint> Filter(ZoneResult source, IReadOnlyDictionary<HexPoint, ZoneResult> domain)
        {
            return source.Neighbors.Where(x => !domain.ContainsKey(x));
        }
    }
}
