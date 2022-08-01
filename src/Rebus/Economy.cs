// Ishan Pranav's REBUS: Economy.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using MessagePack;

namespace Rebus
{
    [MessagePackObject]
    public class Economy
    {
        [Key(0)]
        public string Description { get; }

        [Key(1)]
        public IReadOnlyCollection<Commodity> Commodities { get; }

        public Economy(string description, IReadOnlyCollection<Commodity> commodities)
        {
            Description = description;
            Commodities = commodities;
        }
    }
}
