// Ishan Pranav's REBUS: Commodity.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.ComponentModel.DataAnnotations.Schema;
using MessagePack;
using Microsoft.EntityFrameworkCore;

namespace Rebus
{
    [Index(nameof(Q), nameof(R), nameof(Mass), IsUnique = true)]
    [MessagePackObject]
    [Table(nameof(Commodity))]
    public class Commodity
    {
        [IgnoreMember]
        public int Id { get; set; }

        [Key(0)]
        [NotMapped]
        public string Name
        {
            get
            {
                return Mass.ToString();
            }
        }

        [Key(1)]
        public int Mass { get; set; }

        [Key(2)]
        public int Price { get; set; }

        [Key(3)]
        public int Quantity { get; set; }

        [IgnoreMember]
        public int Q { get; set; }

        [IgnoreMember]
        public int R { get; set; }

        [IgnoreMember]
        [NotMapped]
        public HexPoint Location
        {
            get
            {
                return new HexPoint(Q, R);
            }
            set
            {
                Q = value.Q;
                R = value.R;
            }
        }
    }
}
