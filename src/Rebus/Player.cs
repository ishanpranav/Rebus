// Ishan Pranav's REBUS: Player.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations.Schema;
using MessagePack;
using Microsoft.EntityFrameworkCore;

namespace Rebus
{
    [Index(nameof(Username), IsUnique = true)]
    [MessagePackObject]
    [Table(nameof(Player))]
    public class Player
    {
        [IgnoreMember]
        public int Id { get; set; }

        [IgnoreMember]
        public string Username { get; set; } = string.Empty;

        [IgnoreMember]
        public string Password { get; set; } = string.Empty;

        [Key(0)]
        public int Credits { get; set; }

        [IgnoreMember]
        public int Q { get; set; }

        [IgnoreMember]
        public int R { get; set; }

        [Key(1)]
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
