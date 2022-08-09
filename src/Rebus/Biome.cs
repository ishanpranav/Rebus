// Ishan Pranav's REBUS: Biome.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using MessagePack;

namespace Rebus
{
    [MessagePackObject]
    public class Biome
    {
        [IgnoreMember]
        public string Key { get; }

        [Key(0)]
        public int Population { get; }

        [Key(1)]
        public byte Red { get; }

        [Key(2)]
        public byte Green { get; }

        [Key(3)]
        public byte Blue { get; }

        [JsonConstructor]
        public Biome(string key, int population, byte red, byte green, byte blue)
        {
            Key = key;
            Population = population;
            Red = red;
            Green = green;
            Blue = blue;
        }

        [SerializationConstructor]
        public Biome(int population, byte red, byte green, byte blue) : this(string.Empty, population, red, green, blue) { }
    }
}
