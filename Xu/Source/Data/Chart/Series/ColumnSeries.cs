/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Xu.Chart
{
    public class ColumnSeries : DotSeries
    {
        public ColumnSeries(NumericColumn data_column, int maxWidth, int reference = 0)
        {
            Data_Column = data_column;
            Reference = reference;
            Width = maxWidth;
            Color = Color.FromArgb(88, 168, 208); // Edge Color
            EdgeColor = Color.FromArgb(32, 104, 136); // Fill Color
        }

        public ColumnSeries(NumericColumn data_column, Color color, int maxWidth, int reference = 0) : this(data_column, color, color, maxWidth, reference) { }

        public ColumnSeries(NumericColumn data_column, Color color, Color edgeColor, int maxWidth, int reference = 0)
        {
            Data_Column = data_column;
            Reference = reference;
            Width = maxWidth;
            Color = color;
            EdgeColor = edgeColor;
        }

        protected ColumnSeries() { }

        // Fill color for the column
        public override Color Color
        {
            get
            {
                return Theme.FillColor;
            }
            set
            {
                Theme.ForeColor = value;
                Theme.FillColor = value;

                TextTheme.ForeColor = Color.White;
                TextTheme.FillColor = value.Opaque(255);
            }
        }

        // Edge color for the column
        public override Color EdgeColor
        {
            get
            {
                return Theme.EdgeColor;
            }
            set
            {
                Theme.EdgeColor = value;
                TextTheme.EdgeColor = value.Opaque(255);
            }
        }

        public override float Width { get; set; }

        public double Reference { get; set; } = 0;

        public override void RefreshAxis(IIndexArea area, ITable table)
        {
            base.RefreshAxis(area, table);
            area.AxisY(Side).Range.Insert(Reference);
        }

        public override void Draw(Graphics g, IIndexArea area, ITable table)
        {
            g.SmoothingMode = SmoothingMode.Default; // Reset antialiasing.

            var (pointList, pt, _, _) = GetPixel(table, Data_Column, area, Side);

            // Don't even bother if the data column has no data.
            if (pointList.Count > 0 && pt > 0)
            {
                int ref_pix = area.AxisY(Side).ValueToPixel(Reference);

                int tickWidth = area.AxisX.TickWidth;
                if (tickWidth > 6) tickWidth = (tickWidth * 0.8f).ToInt32();
                if (tickWidth > Width) tickWidth = Width.ToInt32();

                SolidBrush brush = Theme.FillBrush;
                Pen pen = Theme.EdgePen;
                pen.Width = (tickWidth > 30) ? 2 : 1;

                foreach (var (_, p) in pointList)
                    DrawColumn(g, pen, brush, p.X, p.Y, ref_pix, tickWidth);

                if (table is ITagTable itag)
                    foreach (var (index, p) in pointList)
                    {
                        var tagList = GetTags(itag, TagColumns, index);

                        if (tagList.Count() > 0)
                            DrawTag(g, tagList, area.IndexToPixel(index), p.Y);
                    }
            }
        }

        public static void DrawColumn(Graphics g, Pen edgePen, SolidBrush fillBrush, int x, int upper_pix, int lower_pix, int tickWidth)
        {
            int height = Math.Abs(upper_pix - lower_pix);
            int half_tickWidth = (tickWidth / 2.0f).ToInt32();

            if (tickWidth > 1)
            {
                if (upper_pix < lower_pix)
                {
                    g.FillRectangle(fillBrush, x - half_tickWidth, upper_pix, tickWidth, height);
                    g.DrawRectangle(edgePen, x - half_tickWidth, upper_pix, tickWidth, height);
                }
                else if (upper_pix > lower_pix)
                {
                    g.FillRectangle(fillBrush, x - half_tickWidth, lower_pix, tickWidth, height);
                    g.DrawRectangle(edgePen, x - half_tickWidth, lower_pix, tickWidth, height);
                }
                else
                    g.DrawLine(edgePen, x - half_tickWidth, upper_pix, x + half_tickWidth, upper_pix);
            }
            else
            {
                g.DrawLine(edgePen, x, upper_pix, x, lower_pix);
            }
        }
    }
}
