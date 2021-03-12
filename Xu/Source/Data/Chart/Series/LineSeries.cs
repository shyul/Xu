/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Xu.Chart
{
    public enum LineType
    {
        Default,
        Dash,
        ColorDash,
        Step,
        Dot // Line mixed with dots
    }

    public class LineSeries : DotSeries
    {
        public LineSeries(NumericColumn data_column,
            LineType type = LineType.Default, float width = 1, float tension = 0)
        {
            Data_Column = data_column;
            Width = width;
            Tension = tension;
            LineType = type;
            Color = EdgeColor = Color.Magenta;
        }

        public LineSeries(NumericColumn data_column, Color color,
            LineType type = LineType.Default, float width = 1, float tension = 0)
            : this(data_column, color, color, type, width, tension) { }

        public LineSeries(NumericColumn data_column, Color color, Color shadeColor,
            LineType type = LineType.Default, float width = 1, float tension = 0)
        {
            Data_Column = data_column;
            Width = width;
            Tension = tension;
            LineType = type;
            EdgeColor = shadeColor;
            Color = color;
        }

        protected LineSeries() { }

        public LineType LineType { get; set; }

        public float Tension { get; set; } = 0.5f;

        public bool DrawLimitShade { get; set; } = true;

        public override void Draw(Graphics g, IIndexArea area, ITable table)
        {
            var (pointList, pt, _, _) = GetPixel(table, Data_Column, area, Side);

            // Don't even bother if the data column has no data.
            if (pointList.Count > 0 && pt > 0)
            {
                // Turn on the antialiasing if it is required and always turn it off in the end.
                g.SmoothingMode = (IsAntialiasing || Tension > 0.5f) ? SmoothingMode.HighQuality : SmoothingMode.Default;

                var points = pointList.Select(n => n.point);

                // Get the line path.
                using GraphicsPath line = GetLinePath(points, LineType, Tension, area.AxisX.HalfTickWidth);

                // If the area is oscillator, here is how we draw the limit shades
                if (area is OscillatorArea oa && DrawLimitShade)
                    oa.DrawLimitShade(g, line);

                // Draw the line itself.
                DrawLine(g, Theme, line, points, Width, LineType);

                if (table is IDatumTable itag)
                    foreach (var (index, p) in pointList)
                    {
                        var tagList = GetTags(itag, TagColumns, index);

                        if (tagList.Count() > 0)
                            DrawTag(g, tagList, p.X, p.Y);
                    }
            }

            // Reset antialiasing.
            g.SmoothingMode = SmoothingMode.Default;
        }

        public static GraphicsPath GetLinePath(IEnumerable<Point> pts, LineType type, float tension, int half_tickWidth)
        {
            GraphicsPath line = new GraphicsPath();

            if (pts.Count() > 0)
                if (type == LineType.Step)
                {
                    //int half_tickWidth = 0;
                    //int half_tickWidth = (tickWidth / 2.0f).ToInt32();
                    List<Point> stepPt = new List<Point>();

                    Point p0 = pts.First();
                    stepPt.Add(new Point(p0.X - half_tickWidth, p0.Y));

                    int next_X = p0.X + half_tickWidth;
                    stepPt.Add(new Point(next_X, p0.Y));

                    for (int i = 1; i < pts.Count(); i++)
                    {
                        Point p = pts.ElementAt(i);
                        stepPt.Add(new Point(next_X, p.Y));
                        next_X = p.X + half_tickWidth;
                        stepPt.Add(new Point(next_X, p.Y));
                    }

                    line.AddLines(stepPt.ToArray());
                }
                else if (tension > 0)
                    line.AddCurve(pts.ToArray(), tension);
                else
                    line.AddLines(pts.ToArray());

            return line;
        }

        public static void DrawLine(Graphics g, ColorTheme theme, GraphicsPath line, IEnumerable<Point> points, float width = 1, LineType type = LineType.Default)
        {
            // Drawing the solid background of the path.
            if (type == LineType.ColorDash)
                g.DrawPath(theme.EdgePen, line);

            // Draw the line itself. the line can be dashed.
            g.DrawPath(theme.ForePen, line);

            // Draw the data point dots on top
            if (type == LineType.Dot)
            {
                float dotSize = width + 2;
                foreach (PointF pt in points)
                    DrawDot(g, theme, pt, dotSize);
            }
        }

        public override void DrawTailTag(Graphics g, IIndexArea area, ITable table)
        {
            int pt = area.StopPt - 1;

            if (pt >= table.Count)
                pt = table.Count - 1;
            else if (pt < 0)
                pt = 0;

            double data = table[pt, Data_Column];
            if (!double.IsNaN(data))
            {
                ColorTheme tagTheme = TextTheme;
                if (area is OscillatorArea oa && DrawLimitShade)
                {
                    if (!double.IsNaN(oa.UpperLimit) && data >= oa.UpperLimit)
                    {
                        tagTheme = oa.UpperTextTheme;
                    }

                    if (!double.IsNaN(oa.LowerLimit) && data <= oa.LowerLimit)
                    {
                        tagTheme = oa.LowerTextTheme;
                    }
                }

                int y = area.AxisY(Side).ValueToPixel(data);
                if (y >= area.Top && y <= area.Bottom)
                    g.DrawLeftCursor(data.ToSINumberString("G5").String, Main.Theme.Font,
                        tagTheme, new Point(area.RightCursorX, y), 11, 32);
            }
        }
    }
}
