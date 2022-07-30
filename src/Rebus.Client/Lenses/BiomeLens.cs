// Ishan Pranav's REBUS: BiomeLens.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using SkiaSharp;

namespace Rebus.Client.Lenses
{
    public class BiomeLens : ILens
    {
        public SKColor GetColor(ZoneResult zone)
        {
            switch (zone.Biome)
            {
                case Biome.Stellar:
                    return SKColors.White;

                case Biome.Desert:
                    return SKColors.Sienna;

                case Biome.Urban:
                    return SKColors.DarkMagenta;

                case Biome.Marine:
                    return SKColors.Blue;

                case Biome.Forested:
                    return SKColors.ForestGreen;

                case Biome.Tundra:
                    return SKColors.Turquoise;

                case Biome.Gaseous:
                    return SKColors.OrangeRed;

                case Biome.Montane:
                    return SKColors.Brown;

                default:
                    return SKColors.Black;
            }
        }
    }
}
