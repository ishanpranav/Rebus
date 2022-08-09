// Ishan Pranav's REBUS: BiomeLens.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using SkiaSharp;

namespace Rebus.Client.Lenses
{
    public class BiomeLens : Lens
    {
        public override SKColor GetColor(ZoneInfo zone)
        {
            return new SKColor(zone.Biome.Red, zone.Biome.Green, zone.Biome.Blue);
        }
    }
}
