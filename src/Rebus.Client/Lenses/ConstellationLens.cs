// Ishan Pranav's REBUS: ConstellationLens.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using SkiaSharp;

namespace Rebus.Client.Lenses
{
    public class ConstellationLens : ILens
    {
        public SKColor GetColor(ZoneResult zone)
        {
            int constellation = Depths.Layer(zone.Layers, Depths.Constellation);  
            byte intensity = (byte)(200 - constellation);

            switch (constellation % 6)
            {
                case 0:
                    return new SKColor(intensity, green: 0, blue: 0);

                case 1:
                    return new SKColor(intensity, intensity, blue: 0);

                case 2:
                    return new SKColor(red: 0, intensity, blue: 0);

                case 3:
                    return new SKColor(red: 0, intensity, intensity);

                case 4:
                    return new SKColor(red: 0, green: 0, blue: intensity);

                default:
                    return new SKColor(intensity, green: 0, blue: intensity);
            }
        }
    }
}
