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
using System.Windows.Forms;

namespace Xu.GridView
{
    public abstract class GridWidget : DockTab
    {
        protected GridWidget(string name) : base(name)
        {

        }

        public virtual string Label { get; set; }

        public virtual string Description { get; set; }

        #region Rows

        public abstract int DataCount { get; }

        public virtual int StartPt { get; set; } = 0;

        public virtual int StopPt => StartPt + IndexCount;

        public virtual int IndexCount { get; protected set; }

        /// <summary>
        /// Minimum Cell Height
        /// </summary>
        public virtual int CellHeight { get; set; } = 22;

        protected int ActualCellHeight { get; set; }

        #endregion Rows

        #region Columns

        public abstract IEnumerable<GridStripe> Columns { get; }

        protected virtual IEnumerable<GridStripe> EnabledColumns => Columns.Where(n => n.Enabled).OrderBy(n => n.Order);

        protected virtual IEnumerable<GridStripe> VisibleColumns => Columns.Where(n => n.Enabled && n.Visible);

        protected int TotalColumnWidth => VisibleColumns.Select(n => n.Width).Sum();

        #endregion Columns

        #region Coordinate

        public virtual Rectangle GridBounds { get; protected set; }

        public virtual bool ReadyToShow => IsActive;

        protected override void CoordinateLayout()
        {
            ResumeLayout(true);

            if (ReadyToShow && GridBounds.Width > 0)
                lock (GraphicsObjectLock)
                {
                    ActualCellHeight = Math.Max(CellHeight, Columns.Select(n => n.MinimumCellHeight).Max());
                    IndexCount = Math.Floor(GridBounds.Height * 1.0f / ActualCellHeight).ToInt32(1);
                    foreach (var column in Columns)
                    {
                        column.Visible = column.Enabled;
                        column.ActualWidth = column.Width;
                        column.ActualHeight = ActualCellHeight;
                    }

                    while (TotalColumnWidth > Width)
                        VisibleColumns.OrderByDescending(n => n.Importance).ThenBy(n => n.Order).Last().Visible = false;

                    var autoWidthColumns = VisibleColumns.Where(n => n.AutoWidth);
                    if (autoWidthColumns.Count() > 0) // Need to scale up?
                    {
                        int totalAutoWidth = autoWidthColumns.Select(n => n.Width).Sum();
                        if (totalAutoWidth > 0)
                        {
                            int totalFixedWidth = TotalColumnWidth - totalAutoWidth;
                            int availableTotalAutoWidth = GridBounds.Width - totalFixedWidth;
                            double scale = 1.0 * availableTotalAutoWidth / totalAutoWidth;
                            foreach (var column in autoWidthColumns)
                            {
                                column.ActualWidth = (column.Width * scale).ToInt32();
                            }
                        }
                    }

                    int X = GridBounds.Left;

                    foreach (var column in VisibleColumns)
                    {
                        column.Actual_X = X;
                        X += column.ActualWidth;

                        if (X > GridBounds.Right)
                        {
                            column.ActualWidth = GridBounds.Right - column.Actual_X;
                            break;
                        }
                    }
                }

            PerformLayout();
        }

        #endregion Coordinate

        #region Paint

        public virtual ColorTheme Theme { get; } = new ColorTheme();

        #endregion
    }
}
