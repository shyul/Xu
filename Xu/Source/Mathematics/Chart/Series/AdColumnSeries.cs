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
using System.Drawing.Text;
using System.Linq;

namespace Xu.Chart
{
    public class AdColumnSeries : ColumnSeries, IAdvanceDeclineSeries
    {
        public AdColumnSeries(NumericColumn data_column, NumericColumn gain_Column, int width, int reference = 0)
        {
            Data_Column = data_column;
            Gain_Column = gain_Column;
            Reference = reference;
            Width = width;

            ShadeColor = Color.White.Brightness(-0.8f).Opaque(70);
            Color = Color.White.Brightness(-0.3f).Opaque(70);

            DownShadeColor = Color.FromArgb(208, 16, 48).Brightness(-0.8f).Opaque(70);
            DownColor = Color.FromArgb(208, 16, 48).Opaque(70);
        }

        protected AdColumnSeries() { }

        public NumericColumn Gain_Column { get; protected set; }

        public ColorTheme DownTheme { get; } = new ColorTheme();

        public ColorTheme DownTextTheme { get; } = new ColorTheme();

        public override float Width { get { return m_Width; } set { m_Width = Theme.ForePen.Width = Theme.EdgePen.Width = DownTheme.ForePen.Width = DownTheme.EdgePen.Width = value; } }

        // Fill color for the column
        public Color DownColor
        {
            get
            {
                return DownTheme.FillColor;
            }
            set
            {
                DownTheme.ForeColor = value;
                DownTheme.FillColor = value;

                DownTextTheme.ForeColor = Color.White;
                DownTextTheme.FillColor = value.Opaque(255);
            }
        }

        // Edge color for the column
        public Color DownShadeColor
        {
            get
            {
                return DownTheme.EdgeColor;
            }
            set
            {
                DownTheme.EdgeColor = value;
                DownTextTheme.EdgeColor = value.Opaque(255);
            }
        }

        public override void Draw(Graphics g, IArea area, ITable table)
        {
            var (pointList, pt, _, _) = GetPixel(table, Data_Column, Gain_Column, area, Side);

            // Don't even bother if the data column has no data.
            if (pointList.Count > 0 && pt > 0)
            {
                int ref_pix = area.AxisY(Side).ValueToPixel(Reference);

                int tickWidth = area.AxisX.TickWidth;
                if (tickWidth > 6) tickWidth = (tickWidth * 0.8f).ToInt32();
                if (tickWidth > Width) tickWidth = Width.ToInt32();

                if (tickWidth > 30)
                {
                    Theme.EdgePen.Width = 2;
                    DownTheme.EdgePen.Width = 2;
                }
                else
                {
                    Theme.EdgePen.Width = 1;
                    DownTheme.EdgePen.Width = 1;
                }

                foreach (var (_, p, gain) in pointList)
                {
                    SolidBrush brush = (gain < 0) ? DownTheme.FillBrush : Theme.FillBrush;
                    Pen pen = (gain < 0) ? DownTheme.EdgePen : Theme.EdgePen;
                    //DrawColumn(g, pen, brush, p, new Point(p.X, ref_pix), tickWidth);
                    DrawColumn(g, pen, brush, p.X, p.Y, ref_pix, tickWidth);
                }

                if (table is ITagTable itag)
                    foreach (var (index, p, _) in pointList)
                    {
                        var tagList = GetTags(itag, TagColumns, index);

                        if (tagList.Count() > 0)
                            DrawTag(g, tagList, area.IndexToPixel(index), p.Y);
                    }
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

            double data = table[pt, Data_Column];
            double gain = table[pt, Gain_Column];

            if (!double.IsNaN(data) && !double.IsNaN(gain))
            {
                int y = area.AxisY(Side).ValueToPixel(data);
                if (y >= area.Top && y <= area.Bottom)
                    g.DrawLeftCursor(data.ToSINumberString("G4").String, Main.Theme.Font,
                        (gain < 0) ? DownTextTheme : TextTheme, new Point(area.RightCursorX, y), 11, 32);
            }
        }
    }
}
