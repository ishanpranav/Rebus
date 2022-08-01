// Ishan Pranav's REBUS: ConstellationLens.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using SkiaSharp;

namespace Rebus.Client.Lenses
{
    public class ConstellationLens : ILens
    {
        public SKColor GetColor(ZoneInfo zone)
        {
            if (zone.Layers.Count >= Depths.Constellation)
            {
                return Colors.Get(zone.Layers[Depths.Constellation - 1]);
            }
            else
            {
                return SKColors.Black;
            }
        }
    }
}
