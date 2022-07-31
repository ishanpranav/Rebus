// Ishan Pranav's REBUS: ZoneVisualizer.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using Rebus.Client.Lenses;
using SkiaSharp;

namespace Rebus.Client
{
    public class ZoneVisualizer : SKDrawable
    {
        private readonly IEnumerable<ZoneResult> _zones;
        private readonly ILens _lens;
        private readonly int _playerId;
        private readonly Layout _layout;
        private readonly Dictionary<HexPoint, SKPath> _hexagons = new Dictionary<HexPoint, SKPath>();

        public ZoneVisualizer(IEnumerable<ZoneResult> zones, ILens lens, int playerId, Layout layout)
        {
            _zones = zones;
            _lens = lens;
            _playerId = playerId;
            _layout = layout;
        }

        protected override void OnDraw(SKCanvas canvas)
        {
            canvas.Clear(new SKColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue));

            using (SKFont font = new SKFont(SKTypeface.Default, size: 11))
            using (SKPaint paint = new SKPaint(font)
            {
                StrokeJoin = SKStrokeJoin.Bevel,
                Style = SKPaintStyle.Fill,
                TextAlign = SKTextAlign.Center
            })
            {
                foreach (ZoneResult zone in _zones)
                {
                    bool found;

                    if (_hexagons.TryGetValue(zone.Location, out SKPath? path))
                    {
                        found = true;
                    }
                    else
                    {
                        found = false;
                        path = _layout.GetHexagon(zone.Location);
                    }

                    paint.Color = _lens.GetColor(zone);

                    if (zone.Units.Count == 0)
                    {
                        paint.Color = paint.Color.WithAlpha(alpha: 128);

                        if (!found && zone.Biome == Biome.Stellar)
                        {
                            _hexagons.TryAdd(zone.Location, path);
                        }
                    }
                    else if (!found)
                    {
                        _hexagons.TryAdd(zone.Location, path);
                    }

                    canvas.DrawPath(path, paint);
                }

                foreach (ZoneResult zone in _zones)
                {
                    if (_hexagons.TryGetValue(zone.Location, out SKPath? hexagon))
                    {
                        paint.StrokeWidth = 4;
                        paint.Style = SKPaintStyle.Stroke;

                        if (zone.Biome == Biome.Stellar)
                        {
                            paint.Color = SKColors.Silver;
                        }
                        else if (zone.PlayerId == _playerId)
                        {
                            paint.Color = SKColors.DodgerBlue;
                        }
                        else
                        {
                            paint.Color = SKColors.Red;
                        }

                        canvas.DrawPath(hexagon, paint);

                        if (zone.Units.Count > 0)
                        {
                            paint.Style = SKPaintStyle.Fill;

                            string text = zone.Units.Count.ToString();
                            SKPoint point = _layout.GetCenter(zone.Location);

                            point.Y += font.Size * 0.5f;

                            canvas.DrawText(text, point, paint);

                            paint.Color = SKColors.White;
                            paint.StrokeWidth = 0;
                            paint.Style = SKPaintStyle.Stroke;

                            canvas.DrawText(text, point, paint);
                        }
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (SKPath hexagon in _hexagons.Values)
                {
                    hexagon.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
