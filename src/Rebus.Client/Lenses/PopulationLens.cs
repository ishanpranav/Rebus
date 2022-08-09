// Ishan Pranav's REBUS: PopulationLens.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SkiaSharp;

namespace Rebus.Client.Lenses
{
    public class PopulationLens : Lens
    {
        private int _maxPopulation;

        public override void SetZones(IEnumerable<ZoneInfo> zones)
        {
            _maxPopulation = zones
                .Where(x => x.Biome.Population < int.MaxValue && x.Biome.Population >= 0)
                .Max(x => x.Biome.Population);

            base.SetZones(zones);
        }

        public override SKColor GetColor(ZoneInfo zone)
        {
            if (zone.Biome.Population >= 0)
            {
                if (zone.Biome.Population < int.MaxValue)
                {
                    Vector3 lerp = Vector3.Lerp(new Vector3(x: 0, y: 100, z: 49.8f), new Vector3(x: 120, y: 100, z: 49.8f), amount: (float)zone.Biome.Population / _maxPopulation);

                    return SKColor.FromHsv(lerp.X, lerp.Y, lerp.Z);
                }
                else
                {
                    return SKColors.White;
                }
            }
            else
            {
                return SKColors.Black;
            }
        }
    }
}
