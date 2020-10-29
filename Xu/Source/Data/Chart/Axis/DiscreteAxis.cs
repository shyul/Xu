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
    public class DiscreteAxis
    {
        public DiscreteAxis(ChartWidget chart)
        {
            Chart = chart;
        }

        protected ChartWidget Chart { get; }

        /*
        public readonly Dictionary<Importance, AxisTickStyle> Style = new Dictionary<Importance, AxisTickStyle>()
        {
            { Importance.Tiny,  new AxisTickStyle() },
            { Importance.Micro, new AxisTickStyle() },
            { Importance.Minor, new AxisTickStyle() },
            { Importance.Major, new AxisTickStyle() },
        };*/

        // int mean the relative location, StartPt as zero
        public readonly Dictionary<int, (Importance Importance, string Label)> TickList = new Dictionary<int, (Importance Importance, string Label)>();

        public virtual int IndexCount { get; set; }

        public int IndexToPixel(int i)
        {
            lock (Chart.GraphicsLockObject)
            {
                if (indexToPixel is int[])
                {
                    if (i >= indexToPixel.Length)
                        return indexToPixel.Last();
                    else if (i < 0)
                        return indexToPixel.First();
                    else
                        return indexToPixel[i];
                }
                else
                    return -1;
            }
        }

        protected int[] indexToPixel; // { get; protected set; }

        public int PixelToIndex(int x)
        {
            if (pixelToIndex is int[] && x < pixelToIndex.Length && x >= 0)
            {
                int i = pixelToIndex[x];
                return (i > -1) ? i : -1;
            }
            else
            {
                return -1;
            }
        }

        protected int[] pixelToIndex; // { get; protected set; }

        public int TickWidth { get; protected set; }

        public int HalfTickWidth { get; protected set; }

        public virtual void Coordinate(int size)
        {
            int tickNum = IndexCount;
            TickWidth = size / tickNum;

            indexToPixel = new int[tickNum];
            pixelToIndex = new int[size];

            double half = size / (tickNum * 2.0);
            HalfTickWidth = half.ToInt32();

            for (int i = 0; i < tickNum; i++)
            {
                double pix = size * ((0.5D + i) / tickNum);
                indexToPixel[i] = pix.ToInt32();

                int pix_left = Math.Floor(pix - half).ToInt32();
                int pix_right = Math.Ceiling(pix + half).ToInt32();

                if (pix_left < 0) pix_left = 0;

                for (int j = pix_left; j < pix_right; j++)
                {
                    if (j < size) pixelToIndex[j] = i;
                }
            }
        }

        public virtual void Draw(Graphics g, Area area, int labelOffset, bool hasXAxisBar)
        {
            Rectangle bounds = area.Bounds;
            bool[] textPixel = new bool[bounds.Right];

            foreach (var tk in TickList.OrderByDescending(n => n.Value.Importance))
            {
                AxisTickStyle style = Chart.Style[tk.Value.Importance];

                //int location = indexToPixel[tk.Key] + bounds.Left;
                int location = IndexToPixel(tk.Key) + bounds.Left;

                if (style.HasLine)
                {
                    g.DrawLine(style.Theme.EdgePen, location, bounds.Bottom, location, bounds.Top);
                }

                if (hasXAxisBar && style.HasLabel && labelOffset > 0)
                {
                    int width = TextRenderer.MeasureText(tk.Value.Label, style.Font).Width / 2;

                    int t_left = location - width;
                    if (t_left < 0) t_left = 0;

                    int t_right = location + width;
                    if (t_right >= bounds.Right) t_right = bounds.Right - 1;

                    bool drawString = true;

                    for (int i = t_left; i <= t_right; i++)
                    {
                        if (textPixel[i]) drawString = false;
                    }

                    if (drawString)
                    {
                        for (int i = t_left; i <= t_right; i++) textPixel[i] = true;
                        if (tk.Value.Importance > Importance.Minor)
                        {
                            using GraphicsPath gp = new GraphicsPath();
                            Point[] tri = new Point[4]
                                {
                                    new Point(location, bounds.Bottom + 4),
                                    new Point(location - 2, bounds.Bottom + 1),
                                    new Point(location + 2, bounds.Bottom + 1),
                                    new Point(location, bounds.Bottom + 4)
                                };
                            gp.AddLines(tri);
                            g.FillPath(area.Theme.EdgeBrush, gp);
                            g.DrawPath(area.Theme.EdgePen, gp);
                        }
                        g.DrawString(tk.Value.Label, style.Font, style.Theme.ForeBrush,
                        new Point(location, labelOffset), AppTheme.TextAlignCenter);
                    }
                }
            }
        }
    }
}
