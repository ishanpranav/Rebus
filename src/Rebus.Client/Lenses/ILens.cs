// Ishan Pranav's REBUS: 
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using SkiaSharp;

namespace Rebus.Client.Lenses
{
    public interface ILens
    {
        SKColor GetColor(ZoneResult zone);
    }
}
