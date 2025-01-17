﻿/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
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
    public abstract class GridWidget<T> : DockForm, IDataRenderer
    {
        protected GridWidget(string name) : base(name, true)
        {
            HasIcon = false;
            Btn_Pin.Enabled = true;
            Btn_Close.Enabled = true;

            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                if (pi.GetAttribute<BrowsableAttribute>() is BrowsableAttribute bra && bra.Browsable)
                {
                    ColumnConfigurations.Add(pi, new GridColumnConfiguration(pi));
                }
            }

            //SortableProperties = ColumnConfigurations.Where(n => typeof(IComparable).IsAssignableFrom(n.Key.PropertyType) && n.Value.SortPriority < int.MaxValue).OrderBy(n => n.Value.SortPriority).Select(n => n.Key);
            //SortableProperties = ColumnConfigurations.Where(n => n.Value.SortPriority < int.MaxValue).OrderBy(n => n.Value.SortPriority).Select(n => n.Key);
            SortableProperties = ColumnConfigurations.Where(n => n.Key.PropertyType is IComparable && n.Value.SortPriority < int.MaxValue).OrderBy(n => n.Value.SortPriority).Select(n => n.Key);
        }

        #region Rows

        public object DataLockObject { get; } = new object();

        protected IEnumerable<T> SourceRows { get; set; }

        public override void DataIsUpdated(IDataProvider provider)
        {
            if (SortableProperties.Count() > 0)
            {
                var orderedList = SourceRows.OrderBy(n => SortableProperties.First().GetValue(n, null));

                if (SortableProperties.Count() > 1)
                {
                    foreach (PropertyInfo p in SortableProperties.Skip(1))
                    {
                        orderedList = orderedList.ThenBy(n => p.GetValue(n, null));
                    }
                }

                lock (DataLockObject)
                    lock (GraphicsLockObject)
                    {
                        Rows = orderedList.ToArray();
                    }
            }
            else
            {
                lock (DataLockObject)
                    lock (GraphicsLockObject)
                    {
                        Rows = SourceRows.ToArray();
                    }
            }

            base.DataIsUpdated(provider);
            ReadyToShow = true;
        }

        protected IEnumerable<T> Rows { get; set; }

        public virtual int DataCount
        {
            get
            {
                lock (DataLockObject)
                    return (Rows is T[] rows) ? rows.Length : 0;
            }
        }

        public virtual int StartPt { get; set; } = 0;

        public virtual int StopPt { get => StartPt + IndexCount; set { } }

        public virtual int IndexCount { get; protected set; }

        public virtual int LastIndexMax { get; private set; }  // m_BarTable.Count - 1;

        public virtual void ShiftPt(int num)
        {
            if ((StartPt + num >= 0) && (StopPt + num < DataCount))
            {
                StartPt += num;

                lock (GraphicsLockObject)
                    CoordinateRows();

                m_AsyncUpdateUI = true; // real time update
            }
        }

        public virtual void PointerSnapToEnd()
        {
            lock (DataLockObject)
                if (Rows is T[] rows)
                {
                    LastIndexMax = rows.Count() - 1;
                    StopPt = rows.Count();

                    lock (GraphicsLockObject)
                        CoordinateRows();
                }
                else
                {
                    LastIndexMax = -1;
                    StopPt = 0;
                }

            m_AsyncUpdateUI = true; // async update
        }

        public virtual void PointerSnapToNextTick()
        {
            lock (DataLockObject)
                if (Rows is T[] rows && StopPt > rows.Count() - 3)
                {
                    LastIndexMax = rows.Count() - 1;
                    StopPt = rows.Count();

                    lock (GraphicsLockObject)
                        CoordinateRows();
                }
                else
                {
                    LastIndexMax = -1;
                    StopPt = 0;
                }

            m_AsyncUpdateUI = true;
        }

        /// <summary>
        /// Minimum Cell Height
        /// </summary>
        public virtual int CellHeight { get; set; } = 22;

        protected int ActualCellHeight { get; set; }

        #endregion Rows

        #region Stripes / Columns

        public Dictionary<PropertyInfo, GridColumnConfiguration> ColumnConfigurations { get; } = new Dictionary<PropertyInfo, GridColumnConfiguration>();

        public GridColumnConfiguration GetColumnConfiguration(string name) //nameof(Label)
        {
            PropertyInfo result = typeof(T).GetProperty(name);

            if (ColumnConfigurations.ContainsKey(result))
                return ColumnConfigurations[result];
            else
                return null;
        }

        public virtual IEnumerable<GridColumnConfiguration> Columns => ColumnConfigurations.Select(n => n.Value).Where(n => n.Enabled);

        //protected virtual IEnumerable<GridColumnConfiguration> EnabledColumns => Columns.OrderBy(n => n.DisplayOrder);

        protected virtual IEnumerable<GridColumnConfiguration> VisibleColumns => Columns.Where(n => n.Visible).OrderBy(n => n.DisplayOrder);

        protected int TotalStripesWidth => VisibleColumns.Select(n => n.DataCellRenderer.Width).Sum();

        protected IEnumerable<PropertyInfo> SortableProperties { get; }

        #endregion Stripes / Columns

        #region Coordinate

        public virtual Rectangle GridBounds { get; }

        public virtual int StripeTitleHeight { get; set; } = 21;

        public virtual Dictionary<int, (int Y, int Height)> RowBounds { get; } = new Dictionary<int, (int Y, int Height)>();

        protected override void CoordinateLayout()
        {
            ResumeLayout(true);

            if (ReadyToShow && GridBounds.Width > 0)
                lock (GraphicsLockObject)
                {
                    ActualCellHeight = Math.Max(CellHeight, Columns.Select(n => n.DataCellRenderer.MinimumHeight).Max());
                    IndexCount = Math.Ceiling((GridBounds.Height - StripeTitleHeight) * 1.0f / ActualCellHeight).ToInt32(1);

                    foreach (var col in Columns)
                    {
                        col.Visible = col.Enabled;
                        col.ActualWidth = col.DataCellRenderer.Width;
                    }

                    while (TotalStripesWidth > Width)
                        VisibleColumns.OrderBy(n => n.DisplayPriority).ThenBy(n => n.DisplayOrder).Last().Visible = false;

                    var autoWidthStripes = VisibleColumns.Where(n => n.DataCellRenderer.AutoWidth);
                    if (autoWidthStripes.Count() > 0) // Need to scale up?
                    {
                        int totalAutoWidth = autoWidthStripes.Select(n => n.DataCellRenderer.Width).Sum();
                        if (totalAutoWidth > 0)
                        {
                            int totalFixedWidth = TotalStripesWidth - totalAutoWidth;
                            int availableTotalAutoWidth = GridBounds.Width - totalFixedWidth;
                            double scale = 1.0 * availableTotalAutoWidth / totalAutoWidth;
                            foreach (var col in autoWidthStripes)
                            {
                                col.ActualWidth = (col.DataCellRenderer.Width * scale).ToInt32();
                            }
                        }
                    }

                    int X = GridBounds.Left;

                    foreach (var stripe in VisibleColumns)
                    {
                        stripe.Actual_X = X;
                        X += stripe.ActualWidth;

                        if (X > GridBounds.Right)
                        {
                            stripe.ActualWidth = GridBounds.Right - stripe.Actual_X;
                            break;
                        }
                    }

                    CoordinateRows();
                }

            PerformLayout();
        }

        protected virtual void CoordinateRows()
        {
            RowBounds.Clear();
            int y = GridBounds.Top + StripeTitleHeight;
            for (int i = StartPt; i < StopPt; i++)
            {
                int height = ActualCellHeight;
                RowBounds[i] = (y, height);
                y += height;
            }
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

            lock (DataLockObject)
                if (ReadyToShow && GridBounds.Width > 0 && Rows is T[] rows)
                    lock (GraphicsLockObject)
                    {
                        int top = GridBounds.Top;
                        int bottom = GridBounds.Bottom;
                        int left = GridBounds.Left;
                        int right = GridBounds.Right;

                        int i = 0;

                        for (i = StartPt; i < StopPt; i++)
                        {
                            var (y, height) = RowBounds[i];
                            if (i == HoverIndex)
                            {
                                g.FillRectangle(new SolidBrush(Color.Red), new Rectangle(Left, y, GridBounds.Width, height));
                            }
                            else if (i == SelectedIndex)
                            {

                                g.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(Left, y, GridBounds.Width, height));
                            }
                        }

                        g.DrawRectangle(Theme.EdgePen, GridBounds);

                        // Draw Vertical Grid Lines
                        i = 0;
                        foreach (var stripe in VisibleColumns)
                        {
                            int x = stripe.Actual_X;

                            if (i > 0)
                            {
                                g.DrawLine(Theme.EdgePen, new Point(x, top), new Point(x, bottom));
                            }

                            Rectangle titleBox = new(x, top, stripe.ActualWidth, StripeTitleHeight);
                            g.DrawString(stripe.Name, Main.Theme.FontBold, Theme.ForeBrush, titleBox);

                            i++;
                        }

                        //y = top + StripeTitleHeight;

                        for (i = StartPt; i < StopPt; i++)
                        {
                            var (y, height) = RowBounds[i];

                            g.DrawLine(Theme.EdgePen, new Point(Left, y), new Point(Right, y));

                            foreach (GridColumnConfiguration col in VisibleColumns)
                            {
                                int x = col.Actual_X;
                                Rectangle cellBox = new(x, y, col.ActualWidth, height);

                                if (i < DataCount)
                                {
                                    col.DataCellRenderer.Draw(g, cellBox, col.PropertyInfo.GetValue(rows[i]));
                                }
                            }
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

                lock (DataLockObject)
                    Console.WriteLine("Selected Index = " + SelectedIndex + ((SelectedIndex >= 0 && SelectedIndex < DataCount) ? Rows.ElementAt(SelectedIndex).ToString() : string.Empty));

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
                    //Invalidate();
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
