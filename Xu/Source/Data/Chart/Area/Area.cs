/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Xu.Chart
{
    public class Area : IArea, IEquatable<Area>
    {
        public Area(ChartWidget chart, string name, int heightRatio)
        {
            Name = name;
            HeightRatio = heightRatio;
            Chart = chart;

            AxisLeft = new ContinuousAxis(this, AlignType.Left, AlignType.Right, 1.0);
            AxisLeft.Style[Importance.Minor].Font = Main.Theme.TinyFont;
            AxisLeft.Style[Importance.Minor].HasLabel = true;
            AxisLeft.Style[Importance.Minor].HasLine = false;
            // AxisLeft.Style[Importance.Minor].Theme.EdgePen.DashPattern = new float[] { 4, 3 };

            AxisRight = new ContinuousAxis(this, AlignType.Right, AlignType.Right, 1.0);
            AxisRight.Style[Importance.Minor].Font = Main.Theme.TinyFont;
            AxisRight.Style[Importance.Minor].HasLabel = true;
            AxisRight.Style[Importance.Minor].HasLine = true;
            AxisRight.Style[Importance.Minor].Theme.EdgePen.DashPattern = new float[] { 1, 2 };

            AxisCenter = new ContinuousAxis(this, AlignType.Center, AlignType.Center, 1.0);
        }

        protected Area()
        {
            AxisLeft = new ContinuousAxis(this, AlignType.Left, AlignType.Right, 1.0);
            AxisRight = new ContinuousAxis(this, AlignType.Right, AlignType.Right, 1.0);
        }

        public override int GetHashCode() => Name.GetHashCode();

        public bool Equals(Area other) => other is Area a && Name == a.Name;

        public override bool Equals(object other) => other is Area a && Equals(a);

        public static bool operator !=(Area s1, Area s2) => !s1.Equals(s2);
        public static bool operator ==(Area s1, Area s2) => s1.Equals(s2);

        public string Name { get; set; }

        public string Label { get; set; }

        public Importance Importance { get; set; } = Importance.Minor;

        public bool Enabled { get; set; } = true;

        public bool Visible { get; set; } = true;

        public int Order { get; set; }

        public virtual ColorTheme Theme { get; } = new ColorTheme(Color.FromArgb(112, 112, 112), Color.Transparent, Color.FromArgb(192, 192, 192));

        public bool HasXAxisBar { get; set; } = false;

        public int HeightRatio { get; set; }

        public ChartWidget Chart { get; protected set; }

        public ITable Table => Chart.Table;

        public int StopPt => Chart.StopPt;

        public int StartPt => Chart.StartPt;

        public DiscreteAxis AxisX => Chart.AxisX;

        public virtual int IndexCount => Chart.IndexCount;

        public int IndexToPixel(int index) => Chart.IndexToPixel(index);

        public int PointToPixel(int index)
        {
            int pt = index - StartPt;
            if (pt < 0) pt = 0;
            else if (pt >= Chart.DataCount)
            {
                pt = Chart.DataCount - 1;
            }
            return Chart.IndexToPixel(pt);
        }
        public int SelectedIndexPixel => Chart.SelectedIndexPixel;

        public int SelectedIndex => Chart.HoverIndex;

        public int SelectedDataPoint => Chart.SelectedDataPoint;

        public ContinuousAxis AxisY(AlignType side)
        {
            return side switch
            {
                AlignType.Left => AxisLeft,
                AlignType.Right => AxisRight,
                _ => AxisCenter,
            };
        }

        protected readonly ContinuousAxis AxisLeft;

        protected readonly ContinuousAxis AxisCenter;

        protected readonly ContinuousAxis AxisRight;

        protected readonly List<Series> Series = new List<Series>();

        public readonly Dictionary<string, Legend> Legends = new Dictionary<string, Legend>();

        /// <summary>
        /// Order issues here
        /// </summary>
        /// <param name="ser"></param>
        public void AddSeries(Series ser)
        {
            if (!Series.Contains(ser))
            {
                Series.Add(ser);
                Series.Sort((t1, t2) => t1.Order.CompareTo(t2.Order));
                if (ser.LegendName.Length > 0)
                {
                    int legend_key_index = 0;
                    string legend_key = ser.LegendName + "_" + ser.Side + "_" + legend_key_index;

                    while (true)
                    {
                        if (!Legends.ContainsKey(legend_key))
                        {
                            switch (ser.Importance)
                            {
                                case Importance.Huge:
                                    Legends.Add(legend_key, new Legend(this, ser.LegendName, ser.Side, ser.Importance, new Size(32, 32)));
                                    break;
                                default:
                                    Legends.Add(legend_key, new Legend(this, ser.LegendName, ser.Side, ser.Importance, new Size(20, 16)));
                                    break;
                            }
                            break;
                        }
                        else if (Legends[legend_key].Series.Count < 7) // 6 is the maximum number of series per legends
                        {
                            break;
                        }

                        legend_key_index++;
                        legend_key = ser.LegendName + "_" + ser.Side + "_" + legend_key_index;
                    }

                    Legend lg = Legends[legend_key];
                    lg.Label = ser.LegendName;

                    lg.Series.Add(ser);
                    ser.Legend = lg;
                }
            }
            else
            {
                Console.WriteLine("\nWow, we already have this series: " + ser.Name);
            }
        }

        public void RemoveSeries(Series ser)
        {
            lock (Series)
            {
                // Remove legend
                ser.Legend.Series.Remove(ser);

                if (ser.Legend.Series.Count == 0)
                {
                    Legends.Remove(ser.Legend.Name);
                    ser.Legend = null;
                }

                Series.CheckRemove(ser);
            }
        }

        #region Coordinate

        public Rectangle Bounds { get; set; } = new Rectangle();
        public Point Location { get => Bounds.Location; set { Bounds = new Rectangle(value, Bounds.Size); } }
        public Size Size { get => Bounds.Size; set { Bounds = new Rectangle(Bounds.Location, value); } }
        public int Top => Bounds.Top;
        public int Bottom => Bounds.Bottom;
        public int Left => Bounds.Left;
        public int Right => Bounds.Right;
        public int Height => Bounds.Height;
        public int Width => Bounds.Width;
        public Point Center => new Point((Left + Right) / 2, (Top + Bottom) / 2);
        public int LeftCursorX { get; protected set; }
        public int RightCursorX { get; protected set; }

        public int TimeLabelY { get; set; }

        public virtual void Coordinate()
        {
            RightCursorX = Right - 4; // AxisX.HalfTickWidth;
            LeftCursorX = Left + 4; // AxisX.HalfTickWidth;

            /// Reset and inflate axis' range 
            AxisLeft.Reset();
            AxisRight.Reset();

            lock (Series)
                foreach (Series ser in Series)
                {
                    ser.RefreshAxis(this, Table);
                }

            AxisRight.Coordinate(Height, Top);
            GenerateTicks(AxisRight);

            if (AxisLeft.HeightRatio < 1)
            {
                // if the height ratio is less than 1
                // Align Left Axis Range with Right Axis tick
                int left_axis_pix_count = Bottom - (AxisLeft.HeightRatio * Height).ToInt32();
                foreach (var tk in AxisRight.TickList.OrderBy(n => n.Key))
                {
                    int pix = AxisRight.ValueToPixel(tk.Key);
                    if (pix < left_axis_pix_count)
                    {
                        left_axis_pix_count = pix;
                        break;
                    }
                }
                AxisLeft.Coordinate(Bottom - left_axis_pix_count, left_axis_pix_count);
            }
            else
                AxisLeft.Coordinate(Height, Top);

            GenerateTicks(AxisLeft);

            // *****************************
            // Update Legend
            // *****************************

            UpdateLegend();
        }

        public readonly GraphicsPath LegendBackgroundPath = new GraphicsPath();

        public virtual void UpdateLegend()
        {
            lock (Chart.GraphicsLockObject)
            {
                Size LegendMargin = new Size(5, 3);

                int x = Bounds.Left + LegendMargin.Width;
                int y = Bounds.Top + LegendMargin.Width;

                LegendBackgroundPath.Reset();

                int pt = SelectedDataPoint;
                int i = 0, label_area_buttom = 0;

                // Stack up Legends and get their coordinates
                foreach (var lg in Legends.Values.OrderByDescending(n => n.Importance).OrderBy(n => n.Side))
                {
                    lg.Bounds = new Rectangle(x, y, lg.Width, lg.Height);

                    int y1 = y;
                    y += lg.Height + LegendMargin.Height;

                    lg.StopPt = pt + 1;
                    lg.StartPt = lg.StopPt - lg.IndexCount;

                    lg.Coordinate(Table);

                    if (LegendBackgroundPath.PointCount == 0 && lg.LabelBound.Count > 0) // the first legend
                    {
                        LegendBackgroundPath.AddLines(new Point[] { new Point(Bounds.Left + 1, Bounds.Top + 1), new Point(lg.LabelBound[0].Right, Bounds.Top + 1) });
                    }

                    foreach (Rectangle lbb in lg.LabelBound)
                    {
                        if (LegendBackgroundPath.PointCount > 0)
                        {
                            PointF lp = LegendBackgroundPath.GetLastPoint();
                            LegendBackgroundPath.AddLines(new Point[] { new Point(lbb.Right, lp.Y.ToInt32() + 1), new Point(lbb.Right, lbb.Bottom + 1) });
                        }
                    }

                    if (label_area_buttom < lg.Bottom) label_area_buttom = lg.Bottom;

                    i++;
                }

                // Draw the last buttom line
                label_area_buttom += LegendMargin.Width + 1;
                if (LegendBackgroundPath.PointCount > 0)
                {
                    PointF last_point = LegendBackgroundPath.GetLastPoint();
                    LegendBackgroundPath.AddLines(new Point[] { new Point(last_point.X.ToInt32(), label_area_buttom), new Point(Bounds.Left + 1, label_area_buttom) });
                }
            }
        }

        public virtual void GenerateTicks(ContinuousAxis axis)
        {
            int actual_size = (axis.HeightRatio * Height).ToInt32();

            int tickCount = (1.0 * actual_size / axis.MinimumTickHeight).ToInt32(); // It needs at least 10 pixel for a tick

            if (tickCount > 0)
            {
                double tickStep = axis.Delta / tickCount;

                if (tickStep > 0)
                {
                    tickStep = tickStep.FitDacades(axis.TickDacades);

                    axis.Range.Insert(axis.Range.Minimum - axis.Range.Minimum % tickStep);

                    double max_remainder = axis.Range.Maximum % tickStep;

                    if (max_remainder > 0)
                        axis.Range.Insert(axis.Range.Maximum - max_remainder + tickStep); // * 1.0001); // Fix the last tick
                    else if (max_remainder < -0)
                        axis.Range.Insert(axis.Range.Maximum + max_remainder - tickStep); // * 1.0001); // Fix the last tick

                    double tickVal = axis.Range.Minimum;

                    while (tickVal <= axis.Range.Maximum)
                    {
                        axis.TickList.CheckAdd(tickVal, (Importance.Minor, tickVal.ToSINumberString("0.##").String));
                        tickVal += tickStep;
                    }
                }
            }
        }

        #endregion

        #region Paint

        public virtual void DrawAxis(Graphics g)
        {
            // Draw Y Axis
            AxisLeft.Draw(g, Bounds, 3);
            AxisRight.Draw(g, Bounds, 3);

            // Draw X Axis
            AxisX.Draw(g, this, TimeLabelY, HasXAxisBar);

            // Draw and Fill Outline
            if (Theme.FillColor != Color.Transparent)
                g.FillRectangle(Theme.FillBrush, Bounds);

            g.DrawRectangle(Theme.EdgePen, Bounds);
        }

        public virtual void DrawCustomBackground(Graphics g) { }

        public virtual void DrawCustomOverlay(Graphics g) { }

        public virtual void Draw(Graphics g)
        {
            DrawAxis(g);

            DrawCustomBackground(g);

            Array importanceArray = Enum.GetValues(typeof(Importance));
            //List<Importance> importanceList = new List<Importance>();
            g.SetClip(Bounds);

            lock (Series)
            {
                // Draw Patterns here
                // ==================================
                foreach (Importance imp in importanceArray)
                {
                    /*
                    //importanceList.Add(imp);
                    foreach (Pattern pattern in Patterns)
                    {
                        if (pattern.Importance == imp)
                            pattern.Draw(g, this);
                    }*/
                }

                // Draw All Series
                // ==================================
                foreach (Importance imp in importanceArray)
                {
                    foreach (Series ser in Series)
                    {
                        if (ser.Importance == imp)
                            ser.Draw(g, this, Table);
                    }
                }

                g.ResetClip();

                // Draw Last Cursors
                // ==================================
                foreach (Importance imp in importanceArray)
                {
                    foreach (Series ser in Series)
                    {
                        if (ser.Importance == imp && ser.HasTailTag)
                            ser.DrawTailTag(g, this, Table);
                    }
                }

                DrawCustomOverlay(g);
            }
        }

        public virtual void DrawCursor(Graphics g, ITable table)
        {
            if (Chart.SelectedDataPointUnregulated >= 0)
            {
                Array importanceArray = Enum.GetValues(typeof(Importance));

                // Draw Lower Priority Cursors First
                // ==================================
                lock (Series)
                {
                    foreach (Importance imp in importanceArray)
                    {
                        foreach (Series ser in Series)
                        {
                            if (ser.HasCursor)
                                ser.DrawCursor(g, this, table);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
