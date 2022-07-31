// Ishan Pranav's REBUS: AutopilotCommand.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using MessagePack;

namespace Rebus.Commands
{
    public class AutopilotCommand : MoveCommand
    {
        [IgnoreMember]
        protected override int Priority
        {
            get
            {
                return 2;
            }
        }

        public override IEnumerable<HexPoint> Filter(ZoneResult source, IReadOnlyDictionary<HexPoint, ZoneResult> domain)
        {
            return domain.Values
                .Where(x => x.Biome != Biome.Stellar && x.Location != source.Location)
                .Select(x => x.Location);
        }
    }
}
