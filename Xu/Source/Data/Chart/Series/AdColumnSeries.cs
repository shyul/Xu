/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Xu.Chart
{
    public class AdColumnSeries : ColumnSeries, IAdvanceDeclineSeries
    {
        public AdColumnSeries(NumericColumn data_column, int width, int reference = 0, double gain_threshold = 0) 
            : this(data_column, data_column, width, reference, gain_threshold) { }

        public AdColumnSeries(NumericColumn data_column, NumericColumn gain_Column, int width, int reference = 0, double gain_threshold = 0)
        {
            Data_Column = data_column;
            Gain_Column = gain_Column;
            Reference = reference;
            Gain_Threshold = gain_threshold;

            Width = width;

            EdgeColor = Color.White.Brightness(-0.8f).Opaque(70);
            Color = Color.White.Brightness(-0.3f).Opaque(70);

            LowerEdgeColor = Color.FromArgb(208, 16, 48).Brightness(-0.8f).Opaque(70);
            LowerColor = Color.FromArgb(208, 16, 48).Opaque(70);
        }

        protected AdColumnSeries() { }

        public double Gain_Threshold { get; } = 0;

        public NumericColumn Gain_Column { get; protected set; }

        public ColorTheme LowerTheme { get; } = new ColorTheme();

        public ColorTheme LowerTextTheme { get; } = new ColorTheme();

        public override float Width { get { return m_Width; } set { m_Width = Theme.ForePen.Width = Theme.EdgePen.Width = LowerTheme.ForePen.Width = LowerTheme.EdgePen.Width = value; } }

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

        public override void Draw(Graphics g, IIndexArea area, ITable table)
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
                    LowerTheme.EdgePen.Width = 2;
                }
                else
                {
                    Theme.EdgePen.Width = 1;
                    LowerTheme.EdgePen.Width = 1;
                }

                foreach (var (_, p, gain) in pointList)
                {
                    SolidBrush brush = (gain < Gain_Threshold) ? LowerTheme.FillBrush : Theme.FillBrush;
                    Pen pen = (gain < Gain_Threshold) ? LowerTheme.EdgePen : Theme.EdgePen;
                    //DrawColumn(g, pen, brush, p, new Point(p.X, ref_pix), tickWidth);
                    DrawColumn(g, pen, brush, p.X, p.Y, ref_pix, tickWidth);
                }

                if (table is IDatumTable itag)
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

        public override void DrawTailTag(Graphics g, IIndexArea area, ITable table)
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
                        (gain < 0) ? LowerTextTheme : TextTheme, new Point(area.RightCursorX, y), 11, 32);
            }
        }
    }
}
