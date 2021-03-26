/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Xu
{
    [DesignerCategory("Code")]
    public class RibbonPane : UserControl
    {
        public const int MinimumWidth = 20;

        #region Ctor
        public RibbonPane(string name)
        {
            Order = 0;
            Name = name;
            InitializeComponent();
        }
        public RibbonPane(string name, int order)
        {
            Order = order;
            Name = name;
            InitializeComponent();
        }
        protected void InitializeComponent()
        {
            components = new Container();
            SuspendLayout();
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.Selectable, false);
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            //Dock = DockStyle.Top;
            Dock = DockStyle.None;
            Height = Ribbon.PanelHeight - 1;
            //Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left;
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
        public RibbonTabPanel HostPanel { get; protected set; }
        public bool IsShrink => (HostPanel == null) || HostPanel.IsShrink;

        /// <summary>
        /// To be fixed with showing internal customization organize window
        /// </summary>
        public Command CornerButtonCommand { get; set; }

        public int Order { get; set; } = 0;
        public string Description { get; set; } = string.Empty;

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent != null)
            {
                if ((typeof(RibbonTabPanel)).IsAssignableFrom(Parent.GetType()))
                {
                    HostPanel = (RibbonTabPanel)Parent;
                }
                else
                    throw new Exception("RibbonPane can only be exsiting in RibbonTabPanel as Parent: " + Parent.GetType().ToString());
            }
        }
        protected override void OnControlAdded(ControlEventArgs e)
        {
            if (e.Control.GetType().IsAssignableFrom(typeof(Widget)))
                throw new Exception("RibbonPane can only contain RibbonControl: " +
                    e.Control.GetType().ToString());
            base.OnControlAdded(e);
        }

        public List<IStackable> RibbonControls { get; } = new List<IStackable>();

        public virtual int Count => RibbonControls.Count;

        public virtual void Sort()
        {
            if (Count > 1)
            {
                RibbonControls.Sort((f1, f2) => f1.Order.CompareTo(f2.Order));
                for (int i = 0; i < Count; i++) RibbonControls[i].Order = i;
            }
        }
        public void Add(IStackable c, int order)
        {
            if (c is Control)
            {
                c.Order = order;
                Add(c);
            }
        }
        public void Add(object c)
        {
            if (c is Control && c is IStackable)
            {
                lock (RibbonControls)
                {
                    if (!RibbonControls.Contains((IStackable)c)) RibbonControls.Add((IStackable)c);
                    if (!Controls.Contains((Control)c)) Controls.Add((Control)c);
                    Sort();
                }
                Coordinate();
            }
        }
        public void Remove(object c)
        {
            if (c is Control && c is IStackable)
            {
                lock (RibbonControls)
                {
                    if (RibbonControls.Contains((IStackable)c)) RibbonControls.Remove((IStackable)c);
                    if (Controls.Contains((Control)c)) Controls.Remove((Control)c);
                    Sort();
                }
                Coordinate();
            }
        }
        #endregion

        #region Coordinate
        protected Point Origin { get; set; } = new Point(3, 3);
        protected Rectangle LabelBound { get; set; } = Rectangle.Empty;
        protected Rectangle CornerButton { get; set; } = Rectangle.Empty;
        protected Rectangle ControlRect { get { return new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1); } }

        protected void Coordinate()
        {
            SuspendLayout();
            int section_idx = 0;
            int stacked_y = 0;
            Dictionary<int, int> SectionMaxY = new();

            lock (RibbonControls)
            {
                foreach (IStackable rbc in RibbonControls)
                {
                    rbc.SectionIndex = section_idx;
                    rbc.StackedY = stacked_y;

                    if (SectionMaxY.ContainsKey(section_idx))
                    {
                        SectionMaxY[section_idx] = Math.Max(SectionMaxY[section_idx], stacked_y);
                    }
                    else
                    {
                        SectionMaxY.Add(section_idx, stacked_y);
                    }

                    if (rbc.IsSectionEnd || (rbc.IsLineEnd && stacked_y == 2))
                    {
                        stacked_y = 0;
                        section_idx++;
                    }
                    else if (rbc.IsLineEnd)
                    {
                        stacked_y++;
                    }
                }

                int x = Origin.X;
                int y = Origin.Y;
                int x_base = 0;
                int w = TextRenderer.MeasureText(Name, Font).Width;

                foreach (IStackable rbc in RibbonControls)
                {
                    if (rbc.Importance > Importance.Minor)
                    {
                        rbc.Location = new Point(x, Origin.Y);
                        x = rbc.Bounds.Right;
                        x_base = x;
                        if (w < x) w = x;
                    }
                    else
                    {
                        y = 66 * (1 + (rbc.StackedY * 2)) / ((SectionMaxY[rbc.SectionIndex] + 1) * 2) - 11 + Origin.Y;
                        rbc.Location = new Point(x, y);
                        x = rbc.Bounds.Right;
                        if (w < x) w = x;

                        if (rbc.IsLineEnd)
                        {
                            x = x_base;
                        }

                        if (rbc.IsSectionEnd)
                        {
                            x = w;
                            x_base = w;
                        }
                    }
                }

                Width = w + Origin.X;
                int titleHeight = Height - 66 - Origin.Y - 4;
                int titleY = 66 + Origin.Y + 2;

                if (CornerButtonCommand == null)
                {
                    CornerButton = Rectangle.Empty;
                }
                else
                {
                    CornerButton = new Rectangle(Width - 16, titleY, 15, titleHeight + 1);
                }

                LabelBound = new Rectangle(4, titleY, Width - 9 - CornerButton.Width, titleHeight);

                // m_LabelRect = new Rectangle(4, 66 + m_Origin.Y + 2, Width - 9, Height - 66 - m_Origin.Y - 4);
                // m_CornerBtn = new Rectangle(Width - 10, m_LabelRect.Y, 13, Height - 66 - m_Origin.Y - 4);
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

        public virtual Brush HoverFillBrush => Main.Theme.Hover.FillBrush;
        public virtual Brush ClickFillBrush => Main.Theme.Click.FillBrush;
        public virtual Pen HoverEdgePen => Main.Theme.Hover.EdgePen;
        public virtual Pen ClickEdgePen => Main.Theme.Click.EdgePen;
        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            //g.Clear(Color.Transparent);
            if (true & AppTheme.DesignMode) g.DrawRectangle(Main.Theme.Highlight.EdgePen, ControlRect);
            if (true & AppTheme.DesignMode) g.DrawRectangle(Main.Theme.Highlight.EdgePen, LabelBound);

            if (IsShrink)
            {
                using (GraphicsPath tabLine = ShapeTool.ToRoundRect(
                        new Rectangle(LabelBound.X, LabelBound.Y + 1, LabelBound.Width, LabelBound.Height - 2),
                        5))
                {
                    g.FillPath(Main.Theme.Click.EdgeBrush, tabLine);
                }
                g.DrawString(Name, Font, AppTheme.WhiteBrush, LabelBound, AppTheme.TextAlignCenter);
            }
            else
                g.DrawString(Name, Font, Main.Theme.GrayTextBrush, LabelBound, AppTheme.TextAlignCenter);

            if (CornerButtonCommand != null)
            {
                if (CornerButtonCommand.Enabled)
                    if (MouseState == MouseState.Hover)
                    {
                        g.FillRectangle(HoverFillBrush, CornerButton);
                        g.DrawRectangle(HoverEdgePen, CornerButton);
                    }
                    else if (MouseState == MouseState.Down)
                    {
                        g.FillRectangle(ClickFillBrush, CornerButton);
                        g.DrawRectangle(ClickEdgePen, CornerButton);
                    }

                g.DrawIcon(Xu.Properties.Resources.PaneCornerButton, CornerButton,
                    (CornerButtonCommand.Enabled) ? Color.Gray : Main.Theme.Panel.EdgeColor);
            }
        }
        #endregion

        #region Mouse Events
        public virtual MouseState MouseState { get; protected set; } = MouseState.Out;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            Point pt = new(e.X, e.Y);
            if (CornerButton.Contains(pt) && CornerButtonCommand.Enabled)
                MouseState = MouseState.Hover;
            else
                MouseState = MouseState.Out;
            Invalidate(true);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Point pt = new(e.X, e.Y);
            if (e.Button == MouseButtons.Left && CornerButton.Contains(pt) && CornerButtonCommand.Enabled)
                MouseState = MouseState.Down;
            //else
            //MouseState = MouseStateType.Out;

            Invalidate(true);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            Point pt = new(e.X, e.Y);
            if (e.Button == MouseButtons.Left && CornerButton.Contains(pt) && CornerButtonCommand.Enabled)
                CornerButtonCommand.Start();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            MouseState = MouseState.Out;
            Invalidate(true);
        }
        #endregion
    }
}
