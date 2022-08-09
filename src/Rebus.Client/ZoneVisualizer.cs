// Ishan Pranav's REBUS: ZoneVisualizer.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Rebus.Client.Lenses;
using SkiaSharp;

namespace Rebus.Client
{
    public class ZoneVisualizer : SKDrawable
    {
        private readonly IEnumerable<ZoneInfo> _zones;
        private readonly Lens _lens;
        private readonly int _playerId;
        private readonly Layout _layout;

        public ZoneVisualizer(IEnumerable<ZoneInfo> zones, Lens lens, int playerId, Layout layout)
        {
            _zones = zones;
            _lens = lens;
            _playerId = playerId;
            _layout = layout;
        }

        protected override void OnDraw(SKCanvas canvas)
        {
            canvas.Clear(SKColors.Black);

            using (SKFont font = new SKFont(SKTypeface.Default, size: 11))
            using (SKPaint paint = new SKPaint(font)
            {
                StrokeJoin = SKStrokeJoin.Bevel,
                TextAlign = SKTextAlign.Center
            })
            {
                _lens.SetZones(_zones);

                foreach (ZoneInfo zone in _zones)
                {
                    paint.Color = SKColors.Black;
                    paint.Style = SKPaintStyle.Fill;

                    using (SKPath hexagon = _layout.GetHexagon(zone.Location, scale: 0.7f))
                    {
                        if (zone.Units.Count == 0)
                        {
                            paint.Color = SKColors.White;

                            canvas.DrawPath(hexagon, paint);

                            paint.Color = _lens.GetColor(zone);

                            if (paint.Color != SKColors.Black)
                            {
                                paint.Color = paint.Color.WithAlpha(alpha: 128);
                            }
                        }
                        else
                        {
                            paint.Color = _lens.GetColor(zone);
                        }

                        canvas.DrawPath(hexagon, paint);

                        paint.StrokeWidth = 3;
                        paint.Style = SKPaintStyle.Stroke;

                        if (zone.Layers.Count == Depths.Star)
                        {
                            if (_lens.GetColor(zone) == SKColors.White)
                            {
                                paint.Color = SKColors.Silver;
                            }
                            else
                            {
                                paint.Color = SKColors.White;
                            }

                            canvas.DrawPath(hexagon, paint);
                        }
                        else if (zone.Units.Count > 0)
                        {
                            paint.Color = SKColors.White;

                            canvas.DrawPath(hexagon, paint);

                            paint.Style = SKPaintStyle.Fill;

                            string text = zone.Units.Count.ToString();
                            SKPoint point = _layout.GetCenter(zone.Location);

                            point.Y += font.Size * 0.5f;

                            paint.Color = SKColors.White;
                            paint.StrokeWidth = 0;
                            paint.Style = SKPaintStyle.Stroke;

                            canvas.DrawText(text, point, paint);
                        }
                    }
                }
            }
        }
    }
}
