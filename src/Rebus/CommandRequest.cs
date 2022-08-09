// Ishan Pranav's REBUS: CommandRequest.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using MessagePack;

namespace Rebus
{
    [MessagePackObject]
    public class CommandRequest
    {
        [Key(0)]
        public int UserId { get; }

        [Key(1)]
        public Arguments Arguments { get; }

        public CommandRequest(int userId, Arguments arguments)
        {
            UserId = userId;
            Arguments = arguments;
        }
    }
}
