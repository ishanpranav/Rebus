// Ishan Pranav's REBUS: Lens.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using SkiaSharp;

namespace Rebus.Client.Lenses
{
    public abstract class Lens
    {
        public virtual void SetZones(IEnumerable<ZoneInfo> zones) { }

        public abstract SKColor GetColor(ZoneInfo zone);
    }
}
