// Ishan Pranav's REBUS: CommandRequest.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using MessagePack;

namespace Rebus
{
    [MessagePackObject]
    public class CommandRequest
    {
        [Key(0)]
        public CommandType Type { get; }

        [Key(1)]
        public int PlayerId { get; }

        [Key(2)]
        public IReadOnlyCollection<int> UnitIds { get; }

        [Key(3)]
        public HexPoint Destination { get; }

        [Key(4)]
        public int CommodityMass { get; }

        public CommandRequest(CommandType type, int playerId, IReadOnlyCollection<int> unitIds, HexPoint destination)
        {
            Type = type;
            PlayerId = playerId;
            UnitIds = unitIds;
            Destination = destination;
        }

        public CommandRequest(CommandType type, int playerId, IReadOnlyCollection<int> unitIds, HexPoint destination, int commodityMass) : this(type, playerId, unitIds, destination)
        {
            CommodityMass = commodityMass;
        }
    }
}
