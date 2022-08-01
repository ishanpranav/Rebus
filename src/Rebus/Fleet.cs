// Ishan Pranav's REBUS: Fleet.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using MessagePack;

namespace Rebus
{
    [MessagePackObject]
    public class Fleet
    {
        [Key(0)]
        public string Username { get; }

        [Key(1)]
        public int Size { get; }

        public Fleet(string username, int size)
        {
            Username = username;
            Size = size;
        }
    }
}
