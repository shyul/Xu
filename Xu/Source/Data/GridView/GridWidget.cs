/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;


using System.Windows.Forms;

namespace Xu.GridView
{
    /// <summary>
    /// TODO: Fix mouse scroll
    /// TODO: Select Row
    /// TODO: Default color theme
    /// TODO: Group By
    /// TODO: Sort By
    /// TODO: Format
    /// </summary>
    public abstract class GridWidget : DockForm, IDataView, IDisposable
    {
        protected GridWidget(string name) : base(name, true)
        {
            HasIcon = true;
            Btn_Pin.Enabled = true;
            Btn_Close.Enabled = true;
        }

        public virtual string Label { get; set; }

        public virtual string Description { get; set; }

        public abstract ITable Table { get; }

        #region Rows

        public virtual int DataCount => Table.Count;

        public virtual int StartPt { get; set; } = 0;

        public virtual int StopPt { get => StartPt + IndexCount; set { } }

        public virtual int IndexCount { get; protected set; }

        public virtual void ShiftPt(int num)
        {
            if ((StartPt + num >= 0) && (StopPt + num < DataCount))
            {
                StartPt += num;
                SetAsyncUpdateUI(); // UpdateUI(); // real time update
            }
        }

        public virtual void PointerToEnd()
        {
            if (Table is ITable t)
            {
                StopPt = t.Count;
                SetAsyncUpdateUI(); // async update
            }
            else
            {
                StopPt = 0;
            }
        }

        public virtual void PointerToNextTick()
        {
            if (Table is ITable t && StopPt > t.Count - 3)
            {
                StopPt = t.Count;
                SetAsyncUpdateUI();
            }
            else
            {
                StopPt = 0;
            }
        }

        /// <summary>
        /// Minimum Cell Height
        /// </summary>
        public virtual int CellHeight { get; set; } = 22;

        protected int ActualCellHeight { get; set; }

        #endregion Rows

        #region Stripes / Columns

        public virtual GridStripe SortStripe { get; set; }

        public abstract IEnumerable<GridStripe> Stripes { get; }

        protected virtual IEnumerable<GridStripe> EnabledStripes => Stripes.Where(n => n.Enabled).OrderBy(n => n.Order);

        protected virtual IEnumerable<GridStripe> VisibleStripes => Stripes.Where(n => n.Enabled && n.Visible);

        protected int TotalStripesWidth => VisibleStripes.Select(n => n.Width).Sum();

        #endregion Stripes / Columns

        #region Coordinate

        public virtual Rectangle GridBounds { get; }

        public virtual int StripeTitleHeight { get; set; } = 21;

        protected override void CoordinateLayout()
        {
            ResumeLayout(true);

            if (ReadyToShow && GridBounds.Width > 0)
                lock (GraphicsLockObject)
                {
                    ActualCellHeight = Math.Max(CellHeight, Stripes.Select(n => n.MinimumCellHeight).Max());
                    IndexCount = Math.Ceiling((GridBounds.Height - StripeTitleHeight) * 1.0f / ActualCellHeight).ToInt32(1);
                    foreach (var stripe in Stripes)
                    {
                        stripe.Visible = stripe.Enabled;
                        stripe.ActualWidth = stripe.Width;
                    }

                    while (TotalStripesWidth > Width)
                        VisibleStripes.OrderByDescending(n => n.Importance).ThenBy(n => n.Order).Last().Visible = false;

                    var autoWidthStripes = VisibleStripes.Where(n => n.AutoWidth);
                    if (autoWidthStripes.Count() > 0) // Need to scale up?
                    {
                        int totalAutoWidth = autoWidthStripes.Select(n => n.Width).Sum();
                        if (totalAutoWidth > 0)
                        {
                            int totalFixedWidth = TotalStripesWidth - totalAutoWidth;
                            int availableTotalAutoWidth = GridBounds.Width - totalFixedWidth;
                            double scale = 1.0 * availableTotalAutoWidth / totalAutoWidth;
                            foreach (var stripe in autoWidthStripes)
                            {
                                stripe.ActualWidth = (stripe.Width * scale).ToInt32();
                            }
                        }
                    }

                    int X = GridBounds.Left;

                    foreach (var stripe in VisibleStripes)
                    {
                        stripe.Actual_X = X;
                        X += stripe.ActualWidth;

                        if (X > GridBounds.Right)
                        {
                            stripe.ActualWidth = GridBounds.Right - stripe.Actual_X;
                            break;
                        }
                    }
                }

            PerformLayout();
        }

        #endregion Coordinate

        #region Paint

        public virtual ColorTheme Theme { get; } = new ColorTheme();

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            if (DataCount < 1)
            {
                g.DrawString("No Data", Main.Theme.FontBold, Main.Theme.GrayTextBrush, new Point(Bounds.Width / 2, Bounds.Height / 2), AppTheme.TextAlignCenter);
            }
            else if (IsActive && !ReadyToShow)
            {
                g.DrawString("Preparing Data... Stand By.", Main.Theme.FontBold, Main.Theme.GrayTextBrush, new Point(Bounds.Width / 2, Bounds.Height / 2), AppTheme.TextAlignCenter);
            }

            if (ReadyToShow && GridBounds.Width > 0)
                lock (GraphicsLockObject)
                {
                    int top = GridBounds.Top;
                    int bottom = GridBounds.Bottom;
                    int left = GridBounds.Left;
                    int right = GridBounds.Right;

                    int y = top + StripeTitleHeight;

                    int i = 0;

                    for (i = StartPt; i < StopPt; i++)
                    {
                        if (i == HoverIndex)
                        {
                            g.FillRectangle(new SolidBrush(Color.Red), new Rectangle(Left, y, GridBounds.Width, CellHeight));
                        }
                        else if (i == SelectedIndex)
                        {

                            g.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(Left, y, GridBounds.Width, CellHeight));
                        }
                        y += ActualCellHeight;
                    }

                    g.DrawRectangle(Theme.EdgePen, GridBounds);

                    // Draw Vertical Grid Lines
                    i = 0;
                    foreach (var stripe in VisibleStripes)
                    {
                        int x = stripe.Actual_X;

                        if (i > 0)
                        {
                            g.DrawLine(Theme.EdgePen, new Point(x, top), new Point(x, bottom));
                        }

                        Rectangle titleBox = new Rectangle(x, top, stripe.ActualWidth, StripeTitleHeight);
                        g.DrawString(stripe.Name, Main.Theme.FontBold, Theme.ForeBrush, titleBox);

                        i++;
                    }

                    y = top + StripeTitleHeight;

                    for (i = StartPt; i < StopPt; i++)
                    {
                        g.DrawLine(Theme.EdgePen, new Point(Left, y), new Point(Right, y));

                        foreach (var stripe in VisibleStripes)
                        {
                            int x = stripe.Actual_X;
                            Rectangle cellBox = new Rectangle(x, y, stripe.ActualWidth, ActualCellHeight);

                            if (i < DataCount)
                                stripe.Draw(g, cellBox, Table, i);
                        }
                        y += ActualCellHeight;
                    }
                }
        }

        #endregion

        #region Mouse

        public virtual int HoverIndex { get; set; } = -1;
        public virtual int SelectedIndex { get; set; } = -1;
        public virtual Point MousePoint { get; protected set; }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (ReadyToShow)
            {
                MousePoint = new Point(e.X, e.Y);

                HoverIndex = Math.Floor(1.0 * (e.Y - Top) / CellHeight).ToInt32(0) + StartPt;

                //Console.WriteLine(">>>>>>>>>>>>>>>>>>> HoverIndex = " + HoverIndex);

                Invalidate();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (ReadyToShow)
            {
                MousePoint = new Point(e.X, e.Y);
                SelectedIndex = HoverIndex;
                Invalidate();
            }

            base.OnMouseClick(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (ReadyToShow)
            {
                int num = -e.Delta * SystemInformation.MouseWheelScrollLines / 120;

                if (num != 0)
                {
                    ShiftPt(num);
                    Invalidate();
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            HoverIndex = -1;
            Invalidate();


            base.OnMouseLeave(e);
        }

        #endregion Mouse
    }
}
