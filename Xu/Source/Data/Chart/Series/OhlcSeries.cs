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
using System.Windows.Forms;

namespace Xu.Chart
{
    public enum OhlcType : int
    {
        Candlestick = 0,
        HollowCandles = 1,
        Bar = 2,
        Line = 3,
        Area = 4
    }

    public class OhlcSeries : Series, IAdvanceDeclineSeries, ITagSeries
    {
        public OhlcSeries(NumericColumn open_Column, NumericColumn high_Column, NumericColumn low_Column, NumericColumn close_Column,
            NumericColumn percent_Column, OhlcType type = OhlcType.Candlestick)
        {
            Open_Column = open_Column;
            High_Column = high_Column;
            Low_Column = low_Column;
            Close_Column = close_Column;
            Percent_Column = percent_Column;

            Type = type;

            Theme.ForeColor = Theme.EdgeColor = Color.FromArgb(96, 96, 96); // Color.Black;
            Theme.FillColor = Color.FromArgb(196, 236, 236, 236); // Color.Transparent;

            TextTheme.ForeColor = TextTheme.EdgeColor = Color.Green;
            TextTheme.FillColor = Color.YellowGreen;
        }

        public NumericColumn Open_Column { get; }

        public NumericColumn High_Column { get; }

        public NumericColumn Low_Column { get; }

        public NumericColumn Close_Column { get; }

        public NumericColumn Percent_Column { get; }

        public NumericColumn Gain_Column => Percent_Column;

        #region Color Theme

        // Down side color theme
        // =======================

        public ColorTheme LowerTheme { get; } = new ColorTheme(Color.FromArgb(208, 16, 48), Color.FromArgb(208, 16, 48));

        public ColorTheme LowerTextTheme { get; } = new ColorTheme(Color.FromArgb(255, 255, 253, 245), Color.HotPink, Color.DarkOrchid);

        // Fill color for the column
        public Color LowerColor
        {
            get
            {
                return LowerTheme.FillColor;
            }
            set
            {
                LowerTheme.ForeColor = value;
                LowerTheme.FillColor = value;

                LowerTextTheme.ForeColor = Color.White;
                LowerTextTheme.FillColor = value.Opaque(255);
            }
        }

        // Edge color for the column
        public Color LowerEdgeColor
        {
            get
            {
                return LowerTheme.EdgeColor;
            }
            set
            {
                LowerTheme.EdgeColor = value;
                LowerTextTheme.EdgeColor = value.Opaque(255);
            }
        }

        #endregion Color Theme

        public OhlcType Type { get; set; }

        public override float Width { get { return m_Width; } set { m_Width = Theme.ForePen.Width = Theme.EdgePen.Width = LowerTheme.ForePen.Width = LowerTheme.EdgePen.Width = value; } }

        /// <summary>
        /// Series Tags
        /// </summary>
        public virtual List<DatumColumn> TagColumns { get; } = new List<DatumColumn>();

        /// <summary>
        /// Only inflate the axis 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="startPt"></param>
        /// <param name="stopPt"></param>
        public override void RefreshAxis(IIndexArea area, ITable table)
        {
            ContinuousAxis axisY = area.AxisY(Side);

            for (int i = area.StartPt; i < area.StopPt; i++)
            {
                if (i >= table.Count)
                    break;

                if (i >= 0)
                    axisY.Range.Insert(new double[] { table[i, Open_Column], table[i, High_Column], table[i, Low_Column], table[i, Close_Column] });
            }
        }

        public override List<(string text, Font font, Brush brush)> ValueLabels(ITable table, int pt)
        {
            List<(string text, Font font, Brush brush)> labels = new();

            string text = string.Empty;
            double open = table[pt, Open_Column];
            text += !double.IsNaN(open) ? Open_Column.Label + ": " + open.ToString(LegendLabelFormat) + "  " : string.Empty;

            double high = table[pt, High_Column];
            text += !double.IsNaN(high) ? High_Column.Label + ": " + high.ToString(LegendLabelFormat) + "  " : string.Empty;

            double low = table[pt, Low_Column];
            text += !double.IsNaN(low) ? Low_Column.Label + ": " + low.ToString(LegendLabelFormat) + "    " : string.Empty;

            if (text.Length > 0) labels.Add((text, Main.Theme.Font, Legend.LabelBrush(Theme)));

            double close = table[pt, Close_Column];
            double percent = table[pt, Percent_Column];

            text = (!double.IsNaN(close) && !double.IsNaN(percent)) ? Close_Column.Label + ": " + close.ToString(LegendLabelFormat) + " ( " + percent.ToString("0.##") + "% )" : string.Empty;

            if (text.Length > 0) labels.Add((text, Main.Theme.Font, (percent < 0) ? Legend.LabelBrush(LowerTheme) : Legend.LabelBrush(Theme)));

            return labels;
        }

        public override void Draw(Graphics g, IIndexArea area, ITable table)
        {
            (List<(int index, Point point, double gain)> points, int pt, _, int max_close_y) = GetPixel(table, Close_Column, Percent_Column, area, Side);

            // Don't even bother if the data column has no data.
            if (points.Count > 0 && pt > 0)
            {
                float orignal_edge_width = m_Width;

                int tickWidth = area.AxisX.TickWidth;

                if ((Type == OhlcType.Candlestick || Type == OhlcType.HollowCandles || Type == OhlcType.Bar) && tickWidth > 2)
                {
                    var (o_points, _, _, _) = GetPixel(table, Open_Column, area, Side);
                    var (h_points, _, _, _) = GetPixel(table, High_Column, area, Side);
                    var (l_points, _, _, _) = GetPixel(table, Low_Column, area, Side);

                    if (tickWidth > 6)
                    {
                        tickWidth = (tickWidth * 0.8f).ToInt32();
                    }

                    if (tickWidth > 6 || (tickWidth > 3 && Type == OhlcType.Bar))
                    {
                        Theme.ForePen.Width = Theme.EdgePen.Width = LowerTheme.ForePen.Width = LowerTheme.EdgePen.Width = 2 * m_Width;
                    }

                    for (int i = 0; i < points.Count; i++)
                    {
                        var (index, p, gain) = points[i];
                        int high_pix = h_points[i].point.Y;
                        int low_pix = l_points[i].point.Y;

                        int open_pix = o_points[i].point.Y;
                        int close_pix = p.Y;

                        Pen edgePen = (gain < 0) ? LowerTheme.EdgePen : Theme.EdgePen;

                        SolidBrush fillBrush = (gain < 0) ?
                            ((close_pix > open_pix) ? LowerTheme.FillBrush : Theme.FillBrush) :
                            ((close_pix > open_pix) ? Theme.EdgeBrush : Theme.FillBrush);

                        if (Type == OhlcType.Candlestick || Type == OhlcType.HollowCandles)
                            DrawCandlestick(g, edgePen, fillBrush, p.X, open_pix, high_pix, low_pix, close_pix, tickWidth);
                        else if (Type == OhlcType.Bar)
                            DrawBar(g, edgePen, p.X, open_pix, high_pix, low_pix, close_pix, tickWidth);

                        if (table is IDatumTable itag)
                        {
                            var tagList = GetTags(itag, TagColumns, points[i].index);

                            if (tagList.Count() > 0)
                                DrawTag(g, tagList, points[i].point.X, high_pix, low_pix);
                        }
                    }
                }
                else if (Type == OhlcType.Line || Type == OhlcType.Area || tickWidth < 3)
                {
                    // Turn on the antialiasing if it is required and always turn it off in the end.
                    g.SmoothingMode = (IsAntialiasing) ? SmoothingMode.HighQuality : SmoothingMode.Default;

                    if (tickWidth > 1)
                    {
                        Theme.ForePen.Width = Theme.EdgePen.Width = LowerTheme.ForePen.Width = LowerTheme.EdgePen.Width = 2 * m_Width;
                    }

                    if (pt > 1)
                    {
                        if (Type == OhlcType.Area)
                        {
                            var c_points = points.Skip(1).Select(n => n.point).ToList();

                            int first_x = c_points[0].X;
                            int last_x = c_points[c_points.Count - 1].X + 1;
                            int last_y = c_points[c_points.Count - 1].Y;
                            using GraphicsPath fillpath = LineSeries.GetLinePath(c_points, LineType.Default, 0, tickWidth);
                            fillpath.AddLines(new Point[] { new Point(last_x, last_y), new Point(last_x, area.Bottom), new Point(first_x, area.Bottom) });

                            int height = Math.Abs(area.Bottom - max_close_y);
                            if (height > 0)
                            {
                                using Brush gbrush = new LinearGradientBrush(
                                    new Rectangle(first_x, max_close_y, area.Bounds.Width, height),
                                    Theme.FillColor.Opaque(255),
                                    Theme.FillColor.Opaque(0),
                                    LinearGradientMode.Vertical);
                                g.FillPath(gbrush, fillpath);
                            }
                        }

                        for (int i = 1; i < points.Count; i++)
                        {
                            var (index, p, gain) = points[i];
                            var (_, p_1, _) = points[i - 1];
                            Pen edgePen = (gain < 0 && Type != OhlcType.Area) ? LowerTheme.EdgePen : Theme.EdgePen;
                            g.DrawLine(edgePen, p_1, p);

                            if (table is IDatumTable itag)
                            {
                                var tagList = GetTags(itag, TagColumns, index); // TagColumns.Select(n => table[index, n]).Where(n => n is TagInfo).Select(n => (TagInfo)n);

                                if (tagList.Count() > 0)
                                    DrawTag(g, tagList, points[i].point.X, p.Y);
                            }
                        }
                    }
                    else
                    {
                        DotSeries.DrawDot(g, Theme, points[0].point, 3);

                        if (table is IDatumTable itag)
                        {
                            var tagList = GetTags(itag, TagColumns, 0); // .Select(n => table[0, n]).Where(n => n is TagInfo).Select(n => (TagInfo)n);

                            if (tagList.Count() > 0)
                                DrawTag(g, tagList, points[0].point.X, points[0].index, points[0].point.Y);
                        }
                    }
                }
                Width = orignal_edge_width;
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

            double data = table[pt, Close_Column];
            double percent = table[pt, Percent_Column];

            if (!double.IsNaN(data) && !double.IsNaN(percent))
            {
                ContinuousAxis axisY = area.AxisY(Side);
                Point location = new(area.RightCursorX, axisY.ValueToPixel(data));

                if (percent < 0)
                    DrawLeftPercentCursor(g, data.ToSINumberString("G5").String, percent.ToString("G3") + "%", LowerTextTheme, location);
                else
                    DrawLeftPercentCursor(g, data.ToSINumberString("G5").String, percent.ToString("G3") + "%", TextTheme, location);

                /*
                if (percent < 0)
                    DrawLeftPercentCursor(g, data.ToSINumberString("G5").String, percent.ToString("P"), DownTextTheme, location);
                else
                    DrawLeftPercentCursor(g, data.ToSINumberString("G5").String, percent.ToString("P"), TextTheme, location);*/
            }
        }

        public static void DrawCandlestick(Graphics g, Pen edgePen, SolidBrush fillBrush, int x, int open_pix, int high_pix, int low_pix, int close_pix, int tickWidth)
        {
            int height = Math.Abs(close_pix - open_pix);
            int half_tickWidth = (tickWidth / 2.0f).ToInt32();

            if (tickWidth > 1)
            {
                if (open_pix < close_pix)
                {
                    g.DrawLine(edgePen, x, high_pix, x, open_pix);
                    g.DrawLine(edgePen, x, close_pix, x, low_pix);
                    g.FillRectangle(fillBrush, x - half_tickWidth, open_pix, tickWidth, height);
                    g.DrawRectangle(edgePen, x - half_tickWidth, open_pix, tickWidth, height);
                }
                else if (open_pix > close_pix)
                {
                    g.DrawLine(edgePen, x, high_pix, x, close_pix);
                    g.DrawLine(edgePen, x, open_pix, x, low_pix);
                    g.FillRectangle(fillBrush, x - half_tickWidth, close_pix, tickWidth, height);
                    g.DrawRectangle(edgePen, x - half_tickWidth, close_pix, tickWidth, height);
                }
                else
                {
                    g.DrawLine(edgePen, x, high_pix, x, low_pix);
                    g.DrawLine(edgePen, x - half_tickWidth, close_pix, x + half_tickWidth, close_pix);
                }
            }
            else
            {
                g.DrawLine(edgePen, x, high_pix, x, low_pix);
            }
        }

        public static void DrawBar(Graphics g, Pen edgePen, int x, int open_pix, int high_pix, int low_pix, int close_pix, int tickWidth)
        {
            int half_tickWidth = (tickWidth / 2.0f).ToInt32();
            g.DrawLine(edgePen, x, high_pix, x, low_pix);

            if (tickWidth > 1)
            {
                g.DrawLine(edgePen, x, open_pix, x - half_tickWidth, open_pix);
                g.DrawLine(edgePen, x, close_pix, x + half_tickWidth, close_pix);
            }
        }

        public static void DrawLeftPercentCursor(Graphics g, string price, string percent, ColorTheme theme, Point location)
        {
            Size priceTextSize = TextRenderer.MeasureText(price, Main.Theme.Font);
            Size percentTextSize = TextRenderer.MeasureText(percent, Main.Theme.TinyFont);

            int height = priceTextSize.Height + percentTextSize.Height;
            int width = Math.Max(priceTextSize.Width, percentTextSize.Width) + 1;
            location.X += 1;

            Size size = new(width, height);
            using (GraphicsPath gp = ShapeTool.LeftTag(location, size, new Size(8, 4), 1))
            {
                g.FillPath(theme.FillBrush, gp);
                g.DrawPath(theme.EdgePen, gp);
            }

            location.X += 5;
            location.Y -= priceTextSize.Height / 2 - 1;

            g.DrawString(price, Main.Theme.Font, theme.ForeBrush, location, AppTheme.TextAlignLeft);
            location.Y += percentTextSize.Height + 1;
            g.DrawString(percent, Main.Theme.TinyFont, theme.ForeBrush, location, AppTheme.TextAlignLeft);
        }
    }
}
