/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Xu.Chart
{
    public class AreaSeries : LineSeries
    {
        public AreaSeries(NumericColumn data_column, bool isGradient,
            LineType type = LineType.Default, int width = 1, float tension = 0)
        {
            Data_Column = data_column;
            IsGradient = isGradient;
            Width = width;
            Tension = tension;
            LineType = type;
            Color = EdgeColor = FillColor = Color.Gray;
        }

        public AreaSeries(NumericColumn data_column, Color color, bool isGradient,
            LineType type = LineType.Default, int width = 1, float tension = 0)
            : this(data_column, color, color, isGradient, type, width, tension) { }

        public AreaSeries(NumericColumn data_column, Color color, Color fillColor, bool isGradient,
            LineType type = LineType.Default, int width = 1, float tension = 0)
        {
            Data_Column = data_column;
            IsGradient = isGradient;
            Width = width;
            Tension = tension;
            LineType = type;
            FillColor = fillColor;
            Color = EdgeColor = color;
        }

        public AreaSeries(NumericColumn data_column,
            Color color, Color shadeColor, Color fillColor, bool isGradient,
            LineType type = LineType.Default, int width = 1, float tension = 0)
        {
            Data_Column = data_column;
            IsGradient = isGradient;
            Width = width;
            Tension = tension;
            LineType = type;
            EdgeColor = shadeColor;
            FillColor = fillColor;
            Color = color;
        }

        protected AreaSeries() { }

        public virtual Color FillColor
        {
            get
            {
                return Theme.FillColor;
            }
            set
            {
                Theme.FillColor = value;
                TextTheme.FillColor = value.Opaque(255);
            }
        }

        public bool IsGradient { get; set; } = true;

        public override void Draw(Graphics g, IArea area, ITable table)
        {
            var (pointList, pt, _, max_y) = GetPixel(table, Data_Column, area, Side);

            // Don't even bother if the data column has no data.
            if (pointList.Count > 0 && pt > 0)
            {
                // Turn on the antialiasing if it is required and always turn it off in the end.
                g.SmoothingMode = (IsAntialiasing || Tension > 0.5f) ? SmoothingMode.HighQuality : SmoothingMode.Default;

                var points = pointList.Select(n => n.point);

                DrawArea(g, area, Theme, points, area.AxisX.TickWidth, max_y, IsGradient, Width, Tension, LineType);
            }

            // Reset antialiasing.
            g.SmoothingMode = SmoothingMode.Default;
        }

        public static void DrawArea(Graphics g, IArea area, ColorTheme theme, IEnumerable<Point> points, int tickWidth, int max_y,
            bool isGradient, float width, float tension, LineType type = LineType.Default)
        {
            using GraphicsPath line = GetLinePath(points, type, tension, tickWidth);

            int first_x = points.First().X;
            Point lastPoint = points.Last();
            int last_x = lastPoint.X + 1;
            int last_y = lastPoint.Y;

            // Fill the area
            using GraphicsPath fillpath = new GraphicsPath();
            fillpath.AddPath(line, true);
            fillpath.AddLines(new Point[] { new Point(last_x, last_y), new Point(last_x, area.Bottom), new Point(first_x, area.Bottom) });

            if (isGradient)
            {
                int height = Math.Abs(area.Bottom - max_y);
                if (height > 0)
                {
                    using Brush gbrush = new LinearGradientBrush(
                        new Rectangle(first_x, max_y, area.Bounds.Width, height),
                        theme.FillColor.Opaque(255),
                        theme.FillColor.Opaque(0),
                        LinearGradientMode.Vertical);
                    g.FillPath(gbrush, fillpath);
                }
            }
            else
            {
                g.FillPath(theme.FillBrush, fillpath);
            }

            // Draw the line itself.
            DrawLine(g, theme, line, points, width, type);
        }
    }
}
