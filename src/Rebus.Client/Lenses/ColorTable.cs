// Ishan Pranav's REBUS: ColorTable.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using SkiaSharp;

namespace Rebus.Client.Lenses
{
    internal static class ColorTable
    {
        private static readonly double[,] s_colors = new double[,]
        {
            { 1,   1,   0.5 },
            { 1,   1,   0   },
            { 1,   0.5, 1   },
            { 1,   0.5, 0.5 },
            { 1,   0.5, 0   },
            { 1,   0,   1   },
            { 1,   0,   0.5 },
            { 1,   0,   0   },
            { 0.5, 1,   1   },
            { 0.5, 1,   0.5 },
            { 0.5, 1,   0   },
            { 0.5, 0.5, 1   },
            { 0.5, 0,   1   },
            { 0,   1,   1   },
            { 0,   1,   0.5 },
            { 0,   1,   0   },
            { 0,   0.5, 1   },
            { 0,   0,   1   },
        };

        public static SKColor Get(int index)
        {
            int intensity = byte.MaxValue - (index % byte.MaxValue);

            index %= s_colors.GetLength(dimension: 0);

            return new SKColor((byte)(intensity * s_colors[index, 0]), (byte)(intensity * s_colors[index, 1]), (byte)(intensity * s_colors[index, 2]));
        }
    }
}
