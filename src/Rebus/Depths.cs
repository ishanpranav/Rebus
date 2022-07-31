// Ishan Pranav's REBUS: Depths.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus
{
    public static class Depths
    {
        public const int EmptyValue = -1;
        public const int FirstValue = 1;

        public const int Constellation = 1;
        public const int Star = 2;
        public const int Planet = 3;

        public static int Layer(IReadOnlyList<int> layers, int depth)
        {
            return layers[depth - 1] + EmptyValue;
        }
    }
}
