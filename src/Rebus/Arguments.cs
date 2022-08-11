// Ishan Pranav's REBUS: Arguments.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using MessagePack;

namespace Rebus
{
    [MessagePackObject]
    public class Arguments
    {
        [Key(0)]
        public CommandType Type { get; }

        [Key(1)]
        public IReadOnlyCollection<int> UnitIds { get; }

        [Key(2)]
        public HexPoint Destination { get; }

        [Key(3)]
        public int Commodity { get; set; }

        public Arguments(CommandType type, IReadOnlyCollection<int> unitIds, HexPoint destination)
        {
            Type = type;
            UnitIds = unitIds;
            Destination = destination;
        }
    }
}
