// Ishan Pranav's REBUS: Commodity.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.ComponentModel.DataAnnotations.Schema;
using MessagePack;
using Microsoft.EntityFrameworkCore;

namespace Rebus
{
    [MessagePackObject]
    [Table(nameof(Commodity))]
    public class Commodity : IComparable, IComparable<Commodity>
    {
        [IgnoreMember]
        public int Q { get; set; }

        [IgnoreMember]
        public int R { get; set; }

        [Key(0)]
        public int Mass { get; set; }

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

        [Key(1)]
        public int Price { get; set; }

        [Key(2)]
        public int Quantity { get; set; }

        public int CompareTo(object? obj)
        {
            if (obj is HexPoint other)
            {
                return CompareTo(other);
            }
            else
            {
                throw new ArgumentException("Argument is not a Commodity instance.", nameof(obj));
            }
        }

        public int CompareTo(Commodity? other)
        {
            if (other == null)
            {
                return 1;
            }
            else
            {
                return Mass.CompareTo(other.Mass);
            }
        }
    }
}
