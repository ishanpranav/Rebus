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
        public User User { get; }

        [Key(2)]
        public Arguments Advice { get; set; }

        public CommandResponse(bool modified, User user)
        {
            Modified = modified;
            User = user;
        }
    }
}
