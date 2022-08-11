// Ishan Pranav's REBUS: Arguments.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

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
        public IReadOnlyCollection<int> Units { get; }

        [Key(2)]
        public HexPoint Destination { get; }

        [Key(3)]
        public int Commodity { get; }

        [Key(4)]
        public double Evaluation { get; set; }

        public Arguments(CommandType type, IReadOnlyCollection<int> units, HexPoint destination)
        {
            Type = type;
            Units = units;
            Destination = destination;
        }

        public Arguments(CommandType type, IReadOnlyCollection<int> units, HexPoint destination, int commodity) : this(type, units, destination)
        {
            Commodity = commodity;
        }
    }
}
