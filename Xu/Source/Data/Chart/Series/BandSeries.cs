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
    public class BandSeries : Series, IRangeSeries
    {
        public BandSeries(NumericColumn high_column, NumericColumn low_column,
            Color color, LineType type = LineType.Default, int width = 1, float tension = 0)
        {
            High_Column = high_column;
            Low_Column = low_column;
            Width = width;
            Tension = tension;
            LineType = type;

            Color = EdgeColor = LowerColor = LowerEdgeColor = color;
            FillColor = color.Opaque(64);
        }

        protected BandSeries() { }

        public NumericColumn High_Column { get; protected set; }

        public NumericColumn Low_Column { get; protected set; }

        public ColorTheme LowTheme { get; set; } = new ColorTheme();

        public ColorTheme LowTextTheme { get; set; } = new ColorTheme();

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

        public virtual Color LowerColor
        {
            get
            {
                return LowTheme.ForeColor;
            }
            set
            {
                LowTheme.ForeColor = value;
                LowTextTheme.FillColor = LowTextTheme.EdgeColor = value.Opaque(255);
                LowTextTheme.ForeColor = (value.GetBrightness() < 0.6) ? Color.White : Color.DimGray;
            }
        }

        public virtual Color LowerEdgeColor
        {
            get
            {
                return LowTheme.EdgeColor;
            }
            set
            {
                LowTheme.EdgeColor = value;
            }
        }

        public LineType LineType { get; set; }

        public float Tension { get; set; } = 0.5f;

        public override float Width
        {
            get
            {
                return Theme.ForePen.Width;
            }
            set
            {
                Theme.ForePen.Width = Theme.EdgePen.Width = value;
                LowTheme.ForePen.Width = LowTheme.EdgePen.Width = value;
            }
        }

        public bool IsGradient { get; set; } = true;

        public override void RefreshAxis(IIndexArea area, ITable table)
        {
            ContinuousAxis axisY = area.AxisY(Side);

            for (int i = area.StartPt; i < area.StopPt; i++)
            {
                if (i >= table.Count)
                    break;

                if (i >= 0)
                    axisY.Range.Insert(new double[] { table[i, High_Column], table[i, Low_Column] });
            }
        }

        public override List<(string text, Font font, Brush brush)> ValueLabels(ITable table, int pt)
        {
            List<(string text, Font font, Brush brush)> labels = new List<(string text, Font font, Brush brush)>();

            string text = " ";

            double high = table[pt, High_Column];
            text += !double.IsNaN(high) ? High_Column.Label + ": " + high.ToSINumberString(LegendLabelFormat).String + "  " : string.Empty;

            double low = table[pt, Low_Column];
            text += !double.IsNaN(low) ? Low_Column.Label + ": " + low.ToSINumberString(LegendLabelFormat).String + "    " : string.Empty;

            if (text.Length > 0) labels.Add((text, Main.Theme.Font, Legend.LabelBrush(Theme)));

            return labels;
        }

        public override void Draw(Graphics g, IIndexArea area, ITable table)
        {
            var (h_pointsList, h_pt, _, _) = GetPixel(table, High_Column, area, Side);
            var (l_pointsList, l_pt, _, _) = GetPixel(table, Low_Column, area, Side);

            // Don't even bother if the data column has no data.
            if (h_pointsList.Count > 0 && l_pointsList.Count > 0 && h_pt > 0 && l_pt > 0)
            {
                // Turn on the antialiasing if it is required and always turn it off in the end.
                g.SmoothingMode = (IsAntialiasing || Tension > 0.5f) ? SmoothingMode.HighQuality : SmoothingMode.Default;

                var h_points = h_pointsList.Select(n => n.point);
                var l_points = l_pointsList.Select(n => n.point);

                int last_x = h_points.Last().X + 1;
                int last_h_y = h_points.Last().Y;
                int last_l_y = l_points.Last().Y;

                // Get the line path.
                using GraphicsPath h_line = LineSeries.GetLinePath(h_points, LineType, Tension, area.AxisX.HalfTickWidth);
                using GraphicsPath l_line = LineSeries.GetLinePath(l_points, LineType, Tension, area.AxisX.HalfTickWidth);
                using (GraphicsPath gp = new GraphicsPath())
                {
                    l_line.Reverse();
                    gp.AddPath(h_line, true);
                    gp.AddLines(new Point[] { new Point(last_x, last_h_y), new Point(last_x, last_l_y) });
                    gp.AddPath(l_line, true);
                    g.FillPath(Theme.FillBrush, gp);
                }

                // Draw the line itself.
                LineSeries.DrawLine(g, Theme, h_line, h_points, Width, LineType);
                LineSeries.DrawLine(g, LowTheme, l_line, l_points, Width, LineType);
            }

            // Reset antialiasing.
            g.SmoothingMode = SmoothingMode.Default;
        }

        public override void DrawTailTag(Graphics g, IIndexArea area, ITable table)
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
