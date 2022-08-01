// Ishan Pranav's REBUS: BiomeLens.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using SkiaSharp;

namespace Rebus.Client.Lenses
{
    public class BiomeLens : ILens
    {
        public SKColor GetColor(ZoneInfo zone)
        {
            return new SKColor(zone.Biome.Red, zone.Biome.Green, zone.Biome.Blue);

            //switch (zone.Biome)
            //{
            //case Depths.Star:
            //    return SKColors.White;

            //case Depths.Planet + 0:
            //    return SKColors.DarkMagenta;

            //case Depths.Planet + 1:
            //    return SKColors.Blue;

            //case Depths.Planet + 2:
            //    return SKColors.ForestGreen;

            //case Depths.Planet + 3:
            //    return SKColors.Brown;

            //case Depths.Planet + 4:
            //    return SKColors.OrangeRed;

            //case Depths.Planet + 5: 
            //    return SKColors.Tan;

            //case Depths.Planet + 6:
            //    return SKColors.Turquoise;

            //default:
            //    return SKColors.Black;
            //}
        }
    }
}
