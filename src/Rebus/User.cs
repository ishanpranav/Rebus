// Ishan Pranav's REBUS: User.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using MessagePack;
using Microsoft.EntityFrameworkCore;

namespace Rebus
{
    [MessagePackObject]
    [Index(nameof(Password))]
    [Table(nameof(User))]
    public class User
    {
        [IgnoreMember]
        public int Id { get; set; }

        [IgnoreMember]
        public string Password { get; set; } = string.Empty;

        [IgnoreMember]
        public int Q { get; set; }

        [IgnoreMember]
        public int R { get; set; }

        [MessagePack.Key(0)]
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

        [MessagePack.Key(1)]
        [NotNull]
        [Required]
        public Player? Player { get; set; }

        [IgnoreMember]
        [NotNull]
        [Required]
        public Player? Twin { get; set; }
    }
}
