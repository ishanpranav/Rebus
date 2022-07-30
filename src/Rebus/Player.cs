// Ishan Pranav's REBUS: Player.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Rebus
{
    [Table(nameof(Player))]
    [Index(nameof(Username), IsUnique = true)]
    public class Player
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Credits { get; set; }
    }
}
