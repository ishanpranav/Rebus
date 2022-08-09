// Ishan Pranav's REBUS: User.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations.Schema;
using MessagePack;

namespace Rebus
{
    [MessagePackObject]
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

        [Key(0)]
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

        [IgnoreMember]
        public int PlayerId { get; set; }

        [IgnoreMember]
        public int TwinId { get; set; }

#nullable disable
        [Key(1)]
        public Player Player { get; set; }

        [IgnoreMember]
        public Player Twin { get; set; }
#nullable enable
    }
}
