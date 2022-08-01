// Ishan Pranav's REBUS: CommandResponse.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using MessagePack;

namespace Rebus
{
    [MessagePackObject]
    public class CommandResponse
    {
        [Key(0)]
        public bool Modified { get; }

        [Key(1)]
        public Player Player { get; }

        public CommandResponse(bool modified, Player player)
        {
            Modified = modified;
            Player = player;
        }
    }
}
