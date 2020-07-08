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

namespace Xu.Chart
{
    /// <summary>
    /// Handing:
    /// 1. XAxis Cursor
    /// 2. YAxis Cursor
    /// 3. Labels following the cursor
    /// 4. Detect Mouse movement and Invalidate itelf only
    /// </summary>
    public class ChartOverlay : Widget
    {
        public ChartOverlay(ChartWidget chart)
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            Chart = chart;
            //BackColor = Color.Transparent;
            Dock = DockStyle.Fill;
            ResumeLayout(false);
            PerformLayout();
        }

        public override Color BackColor => Color.Transparent;

        protected ChartWidget Chart { get; }

        public ITable Table => Chart.Table;

        public ICollection<Area> Areas => Chart.Areas;

        public virtual Rectangle ChartBounds => Chart.ChartBounds;

        public bool ReadyToShow => Table is ITable && Chart.DataCount > 0 && Chart.ReadyToShow && ChartBounds.Width > 0;

        public override void Coordinate() => Chart.CoordinateOverlay();

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (ReadyToShow)
                lock (Chart.GraphicsLockObject)
                {
                    Graphics g = pe.Graphics;
                    g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                    // Draw Legends
                    // ==================================
                    foreach (Area ca in Areas)
                    {
                        if (ca.Enabled && ca.Visible)
                        {
                            using SolidBrush backBrush = new SolidBrush(Chart.Theme.FillColor.Opaque(220));
                            g.FillPath(backBrush, ca.LegendBackgroundPath);

                            foreach (var lg in ca.Legends.Values)
                                lg.Draw(g, Chart.Table);
                        }
                    }

                    if (Chart.HoverIndex > -1)
                    {
                        // Draw cursor on X Axis
                        int x = Chart.SelectedIndexPixel;
                        g.DrawLine(Main.Theme.ActiveCursor.EdgePen, new Point(x, ChartBounds.Top), new Point(x, Areas.Last().Bottom));

                        Font tagFont = Chart.Style[Importance.Major].Font;
                        for (int i = 0; i < Areas.Count; i++)
                        {
                            Area ca = Areas.ElementAt(i); // Areas[i];
                            if (ca.Enabled && ca.Visible)
                            {
                                if (ca.HasXAxisBar)
                                {
                                    Point tagLocation = new Point(x, ca.TimeLabelY);
                                    if (i < Areas.Count - 1)
                                    {
                                        using GraphicsPath gp = ShapeTool.UpDownTag(tagLocation, new Size(85, Chart.AxisXLabelHeight - 2), new Size(8, 4), 1);
                                        g.FillPath(Main.Theme.ActiveCursor.FillBrush, gp);
                                        g.DrawPath(Main.Theme.ActiveCursor.EdgePen, gp);
                                    }
                                    else
                                    {
                                        using GraphicsPath gp = ShapeTool.UpTag(tagLocation, new Size(85, Chart.AxisXLabelHeight - 2), new Size(8, 4), 1);
                                        g.FillPath(Main.Theme.ActiveCursor.FillBrush, gp);
                                        g.DrawPath(Main.Theme.ActiveCursor.EdgePen, gp);
                                    }

                                    g.DrawString(Chart[Chart.HoverIndex], tagFont, Main.Theme.ActiveCursor.EdgeBrush, tagLocation, AppTheme.TextAlignCenter);
                                }

                                // Draw cursor belongs to each series
                                ca.DrawCursor(g, Chart.Table);

                                // Draw cursor on Y Axis
                                if (ca.Bounds.Contains(MousePoint))
                                {
                                    Point rightCursorPt = new Point(ca.RightCursorX, MousePoint.Y);
                                    g.DrawLine(Main.Theme.ActiveCursor.EdgePen, new Point(ChartBounds.Left, MousePoint.Y), rightCursorPt);

                                    ContinuousAxis axis = ca.AxisY(AlignType.Right);
                                    if (axis.Pixel_Far > MousePoint.Y && MousePoint.Y > axis.Pixel_Near && axis.Delta > 0)
                                    {
                                        g.DrawLeftCursor(axis.PixelToString(MousePoint.Y), tagFont, Main.Theme.ActiveCursor, rightCursorPt, 16, Chart.RightYAxisLabelWidth);
                                    }

                                    axis = ca.AxisY(AlignType.Left);
                                    if (axis.Pixel_Far > MousePoint.Y && MousePoint.Y > axis.Pixel_Near && axis.Delta > 0)
                                    {
                                        Point leftCursorPt = new Point(ca.LeftCursorX, MousePoint.Y);
                                        g.DrawRightCursor(axis.PixelToString(MousePoint.Y), tagFont, Main.Theme.ActiveCursor, leftCursorPt, 16, Chart.LeftYAxisLabelWidth);
                                    }
                                }
                            }
                        }
                    }
                }
        }

        #region Mouse

        public virtual Point MousePoint { get; protected set; }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Chart.ReadyToShow)
            {
                MousePoint = new Point(e.X, e.Y);
                if (ClientRectangle.Contains(MousePoint) & Enabled)
                    MouseState = MouseState.Hover;
                else
                    MouseState = MouseState.Out;
                //int x = e.X - Chart.ChartBounds.Left;

                Chart.HoverIndex = Chart.PixelToIndex(e.X);

                foreach (Area ca in Areas) ca.UpdateLegend();

                Invalidate();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (ReadyToShow)
            {
                Chart.GetFocus();
                //Invalidate();
            }

            base.OnMouseClick(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (Chart.ReadyToShow)
            {
                int num = -e.Delta * SystemInformation.MouseWheelScrollLines / 120;

                if (num != 0)
                {
                    if (ModifierKeys.HasFlag(Keys.Control))
                    {
                        Chart.ScaleStartPt(num);
                    }
                    else
                    {
                        double limit = 0.3 * Chart.IndexCount;
                        Chart.ShiftPt(num, limit.ToInt32());
                    }

                    Chart.HoverIndex = Chart.PixelToIndex(e.X);

                    Coordinate();
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Chart.HoverIndex = -1;

            if (Chart.ReadyToShow)
                foreach (Area ca in Areas)
                    ca.UpdateLegend();

            base.OnMouseLeave(e);
        }

        #endregion Mouse
    }
}
