// Ishan Pranav's REBUS: StarLens.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using SkiaSharp;

namespace Rebus.Client.Lenses
{
    public class StarLens : Lens
    {
        private readonly CantorPairing _pairing;

        public StarLens(CantorPairing pairing)
        {
            _pairing = pairing;
        }

        public override SKColor GetColor(ZoneInfo zone)
        {
            if (zone.Layers.Count >= Depths.Star)
            {
                return ColorTable.Get(_pairing.Pair(zone.Layers[Depths.Constellation - 1], Depths.Layer(zone.Layers, Depths.Star)));
            }
            else
            {
                return SKColors.Black;
            }
        }
    }
}
