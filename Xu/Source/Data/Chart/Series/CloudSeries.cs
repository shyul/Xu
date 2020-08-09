/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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
    public class CloudSeries : BandSeries
    {
        public CloudSeries(NumericColumn high_column, NumericColumn low_column,
            LineType type = LineType.Default, int width = 1, float tension = 0)
        {
            High_Column = high_column;
            Low_Column = low_column;
            Width = width;
            Tension = tension;
            LineType = type;

            Color = EdgeColor = Color.Teal;
            FillColor = Color.Teal.Opaque(64);

            LowColor = LowShadeColor = Color.Orange;
            LowFillColor = Color.Peru.Opaque(64);
        }

        public virtual Color LowFillColor
        {
            get
            {
                return LowTheme.FillColor;
            }
            set
            {
                LowTheme.FillColor = value;
                LowTextTheme.FillColor = value.Opaque(255);
            }
        }

        public override List<(string text, Font font, Brush brush)> ValueLabels(ITable table, int pt)
        {
            List<(string text, Font font, Brush brush)> labels = new List<(string text, Font font, Brush brush)>();

            double high = table[pt, High_Column];
            string text_h = !double.IsNaN(high) ? High_Column.Label + ": " + high.ToSINumberString(LegendLabelFormat).String + " " : string.Empty;

            double low = table[pt, Low_Column];
            string text_l = !double.IsNaN(low) ? Low_Column.Label + ": " + low.ToSINumberString(LegendLabelFormat).String + " " : string.Empty;

            if (text_h.Length > 0) labels.Add((text_h, Main.Theme.Font, Legend.LabelBrush(Theme)));
            if (text_l.Length > 0) labels.Add((text_l, Main.Theme.Font, Legend.LabelBrush(LowTheme)));

            return labels;
        }

        public override void Draw(Graphics g, IArea area, ITable table)
        {
            var (h_pointsList, h_pt, h_min_y, h_max_y) = GetPixel(table, High_Column, area, Side);
            var (l_pointsList, l_pt, l_min_y, l_max_y) = GetPixel(table, Low_Column, area, Side);

            // Don't even bother if the data column has no data.
            if (h_pointsList.Count > 0 && l_pointsList.Count > 0 && h_pt > 0 && l_pt > 0)
            {
                // Turn on the antialiasing if it is required and always turn it off in the end.
                g.SmoothingMode = (IsAntialiasing || Tension > 0.5f) ? SmoothingMode.HighQuality : SmoothingMode.Default;

                var h_points = h_pointsList.Select(n => n.point);
                var l_points = l_pointsList.Select(n => n.point);

                int max_y = Math.Min(h_max_y, l_max_y);
                int min_y = Math.Max(h_min_y, l_min_y);

                int first_x = h_points.First().X;
                int last_x = h_points.Last().X + 1;
                int last_h_y = h_points.Last().Y;
                int last_l_y = l_points.Last().Y;

                // Get the line path.
                using GraphicsPath h_line = LineSeries.GetLinePath(h_points, LineType, Tension, area.AxisX.HalfTickWidth);
                using GraphicsPath l_line = LineSeries.GetLinePath(l_points, LineType, Tension, area.AxisX.HalfTickWidth);

                using (GraphicsPath l_gp = new GraphicsPath())
                using (GraphicsPath h_gp_clip = new GraphicsPath())
                {
                    l_gp.AddPath(l_line, true);
                    l_gp.AddLines(new Point[] { new Point(last_x, last_l_y), new Point(last_x, max_y), new Point(first_x, max_y) });

                    h_gp_clip.AddPath(h_line, true);
                    h_gp_clip.AddLines(new Point[] { new Point(last_x, last_h_y), new Point(last_x, min_y), new Point(first_x, min_y) });

                    g.SetClip(h_gp_clip);
                    g.FillPath(Theme.FillBrush, l_gp);
                    g.ResetClip();
                }

                using (GraphicsPath h_gp = new GraphicsPath())
                using (GraphicsPath l_gp_clip = new GraphicsPath())
                {
                    h_gp.AddPath(h_line, true);
                    h_gp.AddLines(new Point[] { new Point(last_x, last_h_y), new Point(last_x, max_y), new Point(first_x, max_y) });

                    l_gp_clip.AddPath(l_line, true);
                    l_gp_clip.AddLines(new Point[] { new Point(last_x, last_l_y), new Point(last_x, min_y), new Point(first_x, min_y) });

                    g.SetClip(l_gp_clip);
                    g.FillPath(LowTheme.FillBrush, h_gp);
                    g.ResetClip();
                }

                // Draw the line itself.
                LineSeries.DrawLine(g, Theme, h_line, h_points, Width, LineType);
                LineSeries.DrawLine(g, LowTheme, l_line, l_points, Width, LineType);


            }

            // Reset antialiasing.
            g.SmoothingMode = SmoothingMode.Default;
        }

        public override void DrawTailTag(Graphics g, IArea area, ITable table)
        {
            int pt = area.StopPt - 1;

            if (pt >= table.Count)
                pt = table.Count - 1;
            else if (pt < 0)
                pt = 0;

            double high = table[pt, High_Column];
            double low = table[pt, Low_Column];

            if (!double.IsNaN(high) && !double.IsNaN(low))
            {
                ContinuousAxis axisY = area.AxisY(Side);
                int x = area.RightCursorX;

                int h_y = axisY.ValueToPixel(high);
                if (h_y >= area.Top && h_y <= area.Bottom)
                    g.DrawLeftCursor(high.ToSINumberString("G5").String, Main.Theme.Font, TextTheme, new Point(x, h_y), 11, 32);

                int l_y = axisY.ValueToPixel(low);
                if (l_y >= area.Top && l_y <= area.Bottom)
                    g.DrawLeftCursor(low.ToSINumberString("G5").String, Main.Theme.Font, LowTextTheme, new Point(x, l_y), 11, 32);
            }
        }
    }
}