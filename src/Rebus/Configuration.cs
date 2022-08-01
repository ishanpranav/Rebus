// Ishan Pranav's REBUS: Configuration.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using MessagePack;

namespace Rebus
{
    [MessagePackObject]
    public class Configuration
    {
        [Key(0)]
        public int Radius { get; }

        public Configuration(int radius)
        {
            Radius = radius;
        }
    }
}
