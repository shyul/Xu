/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Xu
{
    /// <summary>
    /// This object will always belong to designated RibbonTab
    /// </summary>
    [DesignerCategory("Code")]
    public class RibbonTabPanel : UserControl
    {
        #region Ctor
        public RibbonTabPanel(RibbonTabItem rt)
        {
            RibbonTab = rt;
            components = new Container();
            //m_Panes 
            //Enabled = true;
            SuspendLayout();
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.Selectable, false);
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //BackColor = Color.Transparent;
            //Dock = DockStyle.Top;
            Dock = DockStyle.None;
            //Anchor = AnchorStyles.None; // AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left;
            Location = new Point(0, 0);
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        #region Components
        private IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Permanent link to the associated RibbonTab
        /// </summary>
        public RibbonTabItem RibbonTab { get; protected set; }

        public List<RibbonPane> Panes { get; } = new List<RibbonPane>();
        public virtual int Count => Panes.Count;
        public virtual void Sort()
        {
            if (Count > 1)
            {
                Panes.Sort((f1, f2) => f1.Order.CompareTo(f2.Order));
                for (int i = 0; i < Count; i++) Panes[i].Order = i;
            }
        }

        public void Add(RibbonPane pane, int order)
        {
            pane.Order = order;
            Add(pane);
        }
        public void Add(RibbonPane pane)
        {
            lock (Panes)
            {
                if (!Panes.Contains(pane)) Panes.Add(pane);
                if (!Controls.Contains(pane)) Controls.Add(pane);
                Sort();
            }
            Coordinate();
        }
        public void Remove(RibbonPane pane)
        {
            lock (Panes)
            {
                if (Panes.Contains(pane)) Panes.Remove(pane);
                if (Controls.Contains(pane)) Controls.Remove(pane);
                Sort();
            }
            Coordinate();
        }
        #endregion

        #region Coordinate
        public bool IsShrink => RibbonTab.IsShrink;

        protected Rectangle TabRect => RibbonTab.TabRect;

        public const int MaximumWidth = 100;


        public virtual void Coordinate()
        {
            SuspendLayout();
            // Squeeze each pane first






            // Then set the coordinates
            int x = 0, y = 0;
            int w = 0, h = 0;
            int margin = 3;
            if (IsShrink)
            {
                // margin = 3;
                x = y = margin = 3;
                lock (Panes)
                    foreach (RibbonPane pane in Panes)
                    {
                        pane.Location = new Point(x, y);
                        x = pane.Bounds.Right;
                        h = pane.Bounds.Bottom;
                        if (w < x) w = x;
                        if (x > MaximumWidth)
                        {
                            x = margin;
                            y = pane.Bounds.Bottom + margin + 2;
                        }
                    }

                w += margin;
                h += margin + 2;
                if (w < TabRect.Width) w = TabRect.Width;
                if (h < 30) h = 30;
                Size = new Size(w, h);
            }
            else
            {
                x = 1;
                y = 0;
                lock (Panes)
                    foreach (RibbonPane pane in Panes)
                    {
                        pane.Location = new Point(x, y);
                        x = pane.Bounds.Right + 1;
                    }
            }
            ResumeLayout(false);
            PerformLayout();
        }
        protected override void OnResize(EventArgs e)
        {
            Coordinate();
            base.OnResize(e);
        }
        protected override void OnClientSizeChanged(EventArgs e)
        {
            Coordinate();
            base.OnClientSizeChanged(e);
        }

        #endregion

        #region Paint
        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;

            if (IsShrink)
            {
                g.DrawLine(Main.Theme.Panel.EdgePen, new Point(TabRect.Width, 0), new Point(Width, 0));
                g.DrawLine(Main.Theme.Panel.EdgePen, new Point(0, 0), new Point(0, Height));
                g.DrawLine(Main.Theme.Panel.EdgePen, new Point(0, Height - 1), new Point(Width, Height - 1));
                g.DrawLine(Main.Theme.Panel.EdgePen, new Point(Width - 1, 0), new Point(Width - 1, Height));
            }

            int last_y = 0;

            lock (Panes)
                foreach (RibbonPane pane in Panes)
                {
                    if (pane.Bounds.Right < Width - 30)
                    {
                        if (IsShrink)
                            g.DrawLine(Main.Theme.Panel.EdgePen, new Point(pane.Bounds.Right, pane.Bounds.Top + 3), new Point(pane.Bounds.Right, pane.Bounds.Bottom - 2));
                        else
                            g.DrawLine(Main.Theme.Panel.EdgePen, new Point(pane.Bounds.Right, pane.Bounds.Top + 4), new Point(pane.Bounds.Right, pane.Bounds.Bottom - 5));
                    }

                    if (IsShrink && pane.Bounds.Bottom < Height - 30 && pane.Bounds.Bottom != last_y)
                    {
                        last_y = pane.Bounds.Bottom;
                        g.DrawLine(Main.Theme.Panel.EdgePen, new Point(4, last_y + 4), new Point(Width - 5, last_y + 4));
                    }
                }
        }
        #endregion
    }
}
