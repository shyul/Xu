/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Xu.Chart
{
    public abstract class ChartWidget : DockTab, IDataView
    {
        protected ChartWidget(string name) : base(name, true)
        {
            AxisX = new DiscreteAxis(this);
            Overlay = new ChartOverlay(this);
            Controls.Add(Overlay);
        }

        public readonly Dictionary<Importance, AxisTickStyle> Style = new Dictionary<Importance, AxisTickStyle>()
        {
            { Importance.Tiny,  new AxisTickStyle() },
            { Importance.Micro, new AxisTickStyle() },
            { Importance.Minor, new AxisTickStyle() },
            { Importance.Major, new AxisTickStyle() },
        };

        public virtual ColorTheme Theme { get; } = new ColorTheme();

        public virtual string Label { get; set; }

        public virtual string Description { get; set; }

        public abstract ITable Table { get; }

        public virtual int DataCount => Table.Count;

        public virtual int StartPt { get => StopPt - IndexCount; set { } }

        public virtual int StopPt { get; set; } // = 295;

        public virtual int IndexCount { get; set; } = 295;

        public virtual void ShiftPt(int num, int limit)
        {
            if ((StartPt + num > -limit) && (StopPt + num - DataCount < limit))
            {
                StopPt += num;
                SetAsyncUpdateUI(); //UpdateUI(); // TODO: Test Live BarChart. The Tick does not seems to autoscroll here.
            }
        }

        public virtual void ScaleStartPt(int num)
        {
            if (StopPt - (StartPt + num) > 1)
            {
                IndexCount += num;
                SetAsyncUpdateUI(); //UpdateUI();
            }
        }

        public virtual void ScaleStopPt(int num, int limit)
        {
            if ((StartPt + num > -limit) && (StopPt + num - DataCount < limit))
            {
                StopPt += num;
                IndexCount += num;
                SetAsyncUpdateUI(); //UpdateUI(); // real time update
            }
        }

        public virtual void PointerToEnd()
        {
            if (Table is ITable t)
            {
                StopPt = t.Count;
            }
            else
            {
                StopPt = 0;
            }

            SetAsyncUpdateUI(); // async update
        }

        public virtual void PointerToNextTick()
        {
            if (Table is ITable t && StopPt > t.Count - 2 && StopPt < t.Count + 2)
            {
                StopPt = t.Count;
            }

            SetAsyncUpdateUI();
        }

        protected ChartOverlay Overlay { get; }

        public virtual List<Area> Areas { get; } = new List<Area>(); // Scan Maze m and yield this list

        public virtual int TotalAreaHeightRatio => Areas.Where(n => n.Visible && n.Enabled).Select(n => n.HeightRatio).Sum();

        public virtual T AddArea<T>(T area, int order) where T : Area
        {
            for (int i = 0; i < Areas.Count; i++)
            {
                Area a = Areas[i];
                a.Order = i;
                if (a.Order == order) a.Order++;
            }

            if (!Areas.Contains(area))
            {
                Areas.Add(area);
                //Console.WriteLine("Adding area: " + area.Name);
            }

            T ca = (T)Areas.Where(n => n == area).First();
            ca.Order = order;
            Areas.Sort((t1, t2) => t1.Order.CompareTo(t2.Order));

            return ca;
        }

        public virtual T AddArea<T>(T area) where T : Area => AddArea(area, Areas.Count);

        public Area this[string areaName]
        {
            get
            {
                var list = Areas.Where(n => n.Name == areaName);
                if (list.Count() > 0)
                    return list.First();
                else
                    return null;
            }
        }

        #region Coordinate

        public DiscreteAxis AxisX { get; }

        public int HoverIndex { get; set; } = -1;

        public int SelectedIndexPixel => IndexToPixel(HoverIndex);

        /// <summary>
        /// Will give -1 if the selected data point is out of range
        /// </summary>
        public int SelectedDataPointUnregulated
        {
            get
            {
                int pt = (HoverIndex > 0) ? HoverIndex + StartPt : -1;
                if (pt >= DataCount) pt = -1;
                return pt;
            }
        }

        /// <summary>
        /// Range: From 1 to DataCount
        /// </summary>
        public int SelectedDataPoint
        {
            get
            {
                int pt = (HoverIndex > 0) ? HoverIndex + StartPt : StopPt - 1;
                if (pt >= DataCount) pt = DataCount - 1;
                else if (pt < 0) pt = 0;
                return pt;
            }
        }

        public int PixelToIndex(int x) => AxisX.PixelToIndex(x - ChartBounds.Left);

        public int IndexToPixel(int index) => AxisX.IndexToPixel(index) + ChartBounds.Left;

        public abstract string this[int i] { get; }

        public virtual int AxisXLabelHeight { get; set; } = 20;

        public virtual int RightBlankAreaWidth { get; set; } = 16;

        public virtual int LeftYAxisLabelWidth { get; set; } = 50;

        public virtual int RightYAxisLabelWidth { get; set; } = 50;

        public virtual bool AutoScaleFit { get; set; } = true;

        public virtual Rectangle ChartBounds { get; protected set; }

        protected override void CoordinateLayout()
        {
            ResumeLayout(true);
            if (ReadyToShow && Table is ITable)
                lock (GraphicsLockObject)
                {
                    ChartBounds = new Rectangle(
                            LeftYAxisLabelWidth + Margin.Left,
                            Margin.Top,
                            ClientRectangle.Width - LeftYAxisLabelWidth - Margin.Left - RightYAxisLabelWidth - Margin.Right,
                            ClientRectangle.Height - Margin.Top - Margin.Bottom
                            );

                    if (ChartBounds.Width > RightBlankAreaWidth)
                    {
                        AxisX.IndexCount = IndexCount;
                        AxisX.Coordinate(ChartBounds.Width - RightBlankAreaWidth);

                        int ptY = ChartBounds.Top, totalY = TotalAreaHeightRatio;

                        if (AutoScaleFit)
                        {
                            foreach (Area ca in Areas)
                            {
                                if (ca.Visible && ca.Enabled)
                                {
                                    if (ca.HasXAxisBar)
                                    {
                                        ca.Bounds = new Rectangle(ChartBounds.X, ptY, ChartBounds.Width, ChartBounds.Height * ca.HeightRatio / totalY - AxisXLabelHeight);
                                        ptY += ca.Bounds.Height + AxisXLabelHeight;
                                        ca.TimeLabelY = ca.Bounds.Bottom + AxisXLabelHeight / 2 + 1;
                                    }
                                    else
                                    {
                                        ca.Bounds = new Rectangle(ChartBounds.X, ptY, ChartBounds.Width, ChartBounds.Height * ca.HeightRatio / totalY);
                                        ptY += ca.Bounds.Height;
                                    }
                                    ca.Coordinate();
                                }
                            }
                        }
                        else
                        {



                        }
                    }

                }
            PerformLayout();
        }

        #endregion Coordinate

        #region Paint

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            if (Table is null || DataCount < 1)
            {
                g.DrawString("No Data", Main.Theme.FontBold, Main.Theme.GrayTextBrush, new Point(Bounds.Width / 2, Bounds.Height / 2), AppTheme.TextAlignCenter);
            }
            else if (IsActive && !ReadyToShow)
            {
                g.DrawString("Preparing Data... Stand By.", Main.Theme.FontBold, Main.Theme.GrayTextBrush, new Point(Bounds.Width / 2, Bounds.Height / 2), AppTheme.TextAlignCenter);
            }
            else if (ReadyToShow && ChartBounds.Width > 0 && Table is ITable t)
            {
                lock (t.DataLockObject)
                    lock (GraphicsLockObject)
                    {
                        for (int i = 0; i < Areas.Count; i++)
                        {
                            Area ca = Areas[i];
                            if (ca.Visible && ca.Enabled)
                            {
                                ca.Draw(g);
                                if (ca.HasXAxisBar)
                                {
                                    for (int j = 0; j < IndexCount; j++)
                                    {
                                        int x = IndexToPixel(j);
                                        int y = ca.Bottom;
                                        g.DrawLine(ca.Theme.EdgePen, x, y, x, y + 1);

                                        if (i < Areas.Count - 1)
                                        {
                                            y = Areas[i + 1].Top;
                                            g.DrawLine(ca.Theme.EdgePen, x, y, x, y - 1);
                                        }
                                    }
                                }
                            }
                        }
                    }
            }
        }

        #endregion

        protected override void AsyncUpdateUIWorker()
        {
            while (AsyncUpdateUITask_Cts.Continue())
            {
                if (m_AsyncUpdateUI)
                {
                    this?.Invoke(() => {
                        CoordinateLayout();
                        Invalidate(true);
                    });
                    m_AsyncUpdateUI = false;
                }
                Thread.Sleep(5);
            }
        }
    }
}
