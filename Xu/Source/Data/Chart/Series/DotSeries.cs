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
    public class DotSeries : Series, ITagSeries
    {
        public DotSeries(NumericColumn data_column, float width = 3)
        {
            Data_Column = data_column;
            Width = width;
            Color = EdgeColor = Color.Red;
        }

        public DotSeries(NumericColumn data_column, Color color, float width = 3) : this(data_column, color, color, width) { }

        public DotSeries(NumericColumn data_column, Color color, Color shadeColor, float width = 3)
        {
            Data_Column = data_column;
            Width = width;
            EdgeColor = shadeColor;
            Color = color;
        }

        protected DotSeries() { }

        public virtual NumericColumn Data_Column { get; protected set; }

        /// <summary>
        /// Series Tags
        /// </summary>
        public virtual List<DatumColumn> TagColumns { get; } = new List<DatumColumn>();

        public override void RefreshAxis(IIndexArea area, ITable table)
        {
            for (int i = area.StartPt; i < area.StopPt; i++)
            {
                if (i >= table.Count)
                    break;

                double data = table[i, Data_Column];

                if (i >= 0 && !double.IsNaN(data))
                    area.AxisY(Side).Range.Insert(data);
            }
        }

        public override List<(string text, Font font, Brush brush)> ValueLabels(ITable table, int pt)
        {
            List<(string text, Font font, Brush brush)> labels = new();

            string text = Label;
            if (text is null) text = Data_Column.Label;

            double data = table[pt, Data_Column];
            if (!double.IsNaN(data)) text += ": " + data.ToString(LegendLabelFormat) + " ";

            if (text.Length > 0) labels.Add((text, Main.Theme.Font, Legend.LabelBrush(Theme)));

            return labels;
        }

        public override void Draw(Graphics g, IIndexArea area, ITable table)
        {
            var (pointList, pt, _, _) = GetPixel(table, Data_Column, area, Side);

            float width = Width;
            int tickWidth = area.AxisX.TickWidth;
            if (tickWidth > (Width + 3)) width = (Width - 1) + tickWidth / 2;

            // Don't even bother if the data column has no data.
            if (pointList.Count > 0 && pt > 0)
            {
                g.SmoothingMode = (IsAntialiasing) ? SmoothingMode.HighQuality : SmoothingMode.Default;

                foreach (var (_, p) in pointList)
                    DrawDot(g, Theme, p, width);

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
                int y = area.AxisY(Side).ValueToPixel(data);
                if (y >= area.Top && y <= area.Bottom)
                    g.DrawLeftCursor(data.ToSINumberString("G5").String, Main.Theme.Font,
                        TextTheme, new Point(area.RightCursorX, y), 11, 32);
            }
        }

        public override void DrawCursor(Graphics g, Area area, ITable table)
        {
            int pt = area.SelectedDataPoint;

            double data = table[pt, Data_Column];
            if (!double.IsNaN(data))
            {
                int y = area.AxisY(Side).ValueToPixel(data);
                if (y >= area.Top && y <= area.Bottom)
                    g.DrawLeftCursor(data.ToSINumberString("G5").String, Main.Theme.Font,
                        TextTheme, new Point(area.SelectedIndexPixel, y), 11, 32);
            }
        }

        public static void DrawDot(Graphics g, ColorTheme theme, PointF pt, float size)
        {
            RectangleF center = new(pt.X - size / 2f, pt.Y - size / 2f, size, size);
            g.FillEllipse(theme.ForeBrush, center);
            g.DrawEllipse(theme.EdgePen, center);
        }
    }
}
