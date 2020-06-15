/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

namespace Xu.Chart
{
    public abstract class Series : IItem, IEquatable<Series>, IDependable
    {
        public virtual void Dispose() { }

        public string Name { get; set; } = "Default";

        public string Label { get; set; }

        public string Description { get; set; }

        // Should use protected, but the interface requirement
        public virtual ColorTheme Theme { get; } = new ColorTheme();

        // Should use protected, but the interface requirement
        public virtual ColorTheme TextTheme { get; } = new ColorTheme();

        public Importance Importance { get; set; } = Importance.Minor;

        public bool Enabled { get; set; } = true;

        public int Order { get; set; }

        public HashSet<string> Tags { get; set; } = new HashSet<string>();

        public void Remove(bool recursive) { }

        public virtual ICollection<IDependable> Children { get; } = new HashSet<IDependable>();

        public virtual ICollection<IDependable> Parents { get; } = new HashSet<IDependable>();

        public virtual Color Color
        {
            get
            {
                return Theme.ForeColor;
            }
            set
            {
                Theme.ForeColor = value;

                TextTheme.ForeColor = (value.GetBrightness() < 0.6) ? Color.White : Color.DimGray;
                TextTheme.FillColor = value.Opaque(255);
                TextTheme.EdgeColor = value.Opaque(255);
            }
        }

        public virtual Color ShadeColor
        {
            get
            {
                return Theme.EdgeColor;
            }
            set
            {
                Theme.EdgeColor = value;
            }
        }

        public ulong Uid { get; set; }

        /// <summary>
        /// Size of the graphics objects
        /// </summary>
        public virtual float Width { get { return m_Width; } set { m_Width = Theme.ForePen.Width = Theme.EdgePen.Width = value; } }

        protected float m_Width = 1f;

        /// <summary>
        /// Which side of the axis Y is this series using
        /// </summary>
        public AlignType Side { get; set; } = AlignType.Right;

        /// <summary>
        /// Coordinate when the graphics area and data point changed
        /// </summary>
        public virtual void RefreshAxis(IArea area, ITable table) { }

        /// <summary>
        /// Enable Antiasliasing for this series
        /// </summary>
        public bool IsAntialiasing { get; set; } = false;

        /// <summary>
        /// Legend Name, example: SMA, EMA
        /// </summary>
        public virtual string LegendName { get; set; } = string.Empty;

        /// <summary>
        /// The legend where this series belongs to
        /// </summary>
        public Legend Legend { get; set; }

        /// <summary>
        /// Draw Text Label for Legend
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public abstract List<(string text, Font font, Brush brush)> ValueLabels(ITable table, int pt);

        /// <summary>
        /// The format for legend labels
        /// </summary>
        public virtual string LegendLabelFormat { get; set; } = "G5";

        /// <summary>
        /// Chart settings will be here -- Color, Line Width, Line Style, Font
        /// </summary>
        public abstract void Draw(Graphics g, IArea area, ITable table);

        /// <summary>
        /// Enable the end / tail tag
        /// </summary>
        public bool HasTailTag { get; set; } = true;

        /// <summary>
        /// Draw Tag next to the last data point of the chart.
        /// </summary>
        /// <param name="g">Graphics handle for this drawing event</param>
        public virtual void DrawTailTag(Graphics g, IArea area, ITable table) { }

        /// <summary>
        /// Enable and disable the mouse cursor
        /// </summary>
        public bool HasCursor { get; set; } = true;

        /// <summary>
        /// Draw Cursor next to the cursor chart.
        /// </summary>
        /// <param name="g">Graphics handle for this drawing event</param>
        public virtual void DrawCursor(Graphics g, Area area, ITable table) { }

        #region Equality

        public override int GetHashCode() => Name.GetHashCode();

        public bool Equals(Series other) => GetHashCode() == other.GetHashCode();

        public override bool Equals(object obj)
        {
            /*
            if (object.ReferenceEquals(person1, null))
                return object.ReferenceEquals(person2, null);
                https://stackoverflow.com/questions/4219261/overriding-operator-how-to-compare-to-null
             */
            if (obj is Series ser)
                return Equals(ser);
            else
                return false;
        }

        public static bool operator !=(Series s1, Series s2) => !s1.Equals(s2);
        public static bool operator ==(Series s1, Series s2) => s1.Equals(s2);

        #endregion Equality

        public static (List<(int index, Point point)>, int, int, int) GetPixel(ITable table, NumericColumn column, IArea area, AlignType side)
        {
            List<(int index, Point point)> points = new List<(int index, Point point)>();
            int max_y = area.Bottom;
            int min_y = area.Top;
            int pt = 0;

            for (int i = area.StartPt; i < area.StopPt; i++)
            {
                if (i >= table.Count)
                    break;
                else if (i >= 0)
                {
                    double data = table[i, column];
                    if (!double.IsNaN(data))
                    {
                        int x = area.IndexToPixel(pt);
                        int data_pix = area.AxisY(side).ValueToPixel(data);

                        points.Add((i, new Point(x, data_pix)));
                        if (data_pix < max_y) max_y = data_pix;
                        if (data_pix > min_y) min_y = data_pix;
                    }
                }
                pt++;
            }

            return (points, pt, min_y, max_y);
        }

        public static (List<(int index, Point point, double gain)>, int, int, int) GetPixel(ITable table, NumericColumn data_Column, NumericColumn gain_Column, IArea area, AlignType side)
        {
            List<(int index, Point point, double gain)> points = new List<(int index, Point point, double gain)>();
            int max_y = area.Bottom;
            int min_y = area.Top;
            int pt = 0;

            for (int i = area.StartPt; i < area.StopPt; i++)
            {
                if (i >= table.Count)
                    break;
                else if (i >= 0)
                {
                    double data = table[i, data_Column];
                    if (!double.IsNaN(data))
                    {
                        int x = area.IndexToPixel(pt);
                        int data_pix = area.AxisY(side).ValueToPixel(data);

                        points.Add((i, new Point(x, data_pix), table[i, gain_Column]));

                        if (data_pix < max_y) max_y = data_pix;
                        if (data_pix > min_y) min_y = data_pix;
                    }
                }
                pt++;
            }
            return (points, pt, min_y, max_y);
        }

        public static IEnumerable<TagInfo> GetTags(ITable table, IEnumerable<TagColumn> columns, int index) 
            => columns.Select(n => table[index, n]).Where(n => n is TagInfo).Select(n => (TagInfo)n);

        public static void DrawTag(Graphics g, IEnumerable<TagInfo> list, int x, int middle) => DrawTag(g, list, x, middle, middle);

        public static void DrawTag(Graphics g, IEnumerable<TagInfo> list, int x, int high, int low)
        {
            int middle = (high + low) / 2;

            int x_left = x - 2;
            int x_right = x + 2;
            int y_high = high;
            int y_low = low;

            foreach (TagInfo tag in list)
            {
                string text = tag.Text;
                Size textSize = TextRenderer.MeasureText(text, Main.Theme.Font);
                Size labelSize = new Size(textSize.Width, textSize.Height - 2);
                Point c;

                switch (tag.Style)
                {
                    default:
                    case DockStyle.Top:

                        c = new Point(x, y_high - labelSize.Height);
                        using (GraphicsPath gp = ShapeTool.DownTag(c, labelSize, new Size(8, 4), 1))
                        {
                            g.FillPath(tag.Theme.FillBrush, gp);
                            g.DrawPath(tag.Theme.EdgePen, gp);
                        }
                        y_high -= labelSize.Height;
                        break;

                    case DockStyle.Bottom:
                        c = new Point(x, y_low + labelSize.Height + 2);
                        using (GraphicsPath gp = ShapeTool.UpTag(c, labelSize, new Size(8, 4), 1))
                        {
                            g.FillPath(tag.Theme.FillBrush, gp);
                            g.DrawPath(tag.Theme.EdgePen, gp);
                        }
                        y_high += labelSize.Height;
                        break;

                    case DockStyle.Left:
                        c = new Point(x_left, middle);
                        using (GraphicsPath gp = ShapeTool.LeftTag(c, labelSize, new Size(8, 4), 1))
                        {
                            g.FillPath(tag.Theme.FillBrush, gp);
                            g.DrawPath(tag.Theme.EdgePen, gp);
                        }
                        x_left -= labelSize.Width;
                        break;

                    case DockStyle.Right:
                        c = new Point(x_right, middle);
                        using (GraphicsPath gp = ShapeTool.RightTag(c, labelSize, new Size(8, 4), 1))
                        {
                            g.FillPath(tag.Theme.FillBrush, gp);
                            g.DrawPath(tag.Theme.EdgePen, gp);
                        }
                        x_right += labelSize.Width;
                        break;
                }

                g.DrawString(text, Main.Theme.Font, tag.Theme.ForeBrush, c, AppTheme.TextAlignCenter);
            }
        }
    }
}
