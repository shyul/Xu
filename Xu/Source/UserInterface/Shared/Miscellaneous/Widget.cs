/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Xu
{
    [DesignerCategory("Code")]
    public abstract class Widget : UserControl, IItem
    {
        protected Widget(int order = 0, Importance importance = Importance.Minor)
        {
            SuspendLayout();
            Order = order;
            m_Importance = importance;

            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.Selectable, false);
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //Dock = DockStyle.None;
        }

        #region IItem

        public virtual string Label { get; set; }

        public virtual string Description { get; set; }

        public virtual ColorTheme Theme { get; protected set; } = new ColorTheme();

        public virtual Importance Importance
        {
            get { return m_Importance; }
            set { m_Importance = value; Coordinate(); }
        }
        protected Importance m_Importance;

        public virtual int Order { get; set; }

        public HashSet<string> Tags { get; set; }

        public virtual ulong Uid { get; set; }

        #endregion

        #region Layout / Coordinate

        public virtual Brush HoverFillBrush { get => Main.Theme.Hover.FillBrush; protected set { } }
        public virtual Brush ClickFillBrush { get => Main.Theme.Click.FillBrush; protected set { } }
        public virtual Pen HoverEdgePen { get => Main.Theme.Hover.EdgePen; protected set { } }
        public virtual Pen ClickEdgePen { get => Main.Theme.Click.EdgePen; protected set { } }

        public Rectangle ControlRect { get { return new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1); } }

        public Point Center => new Point((Left + Right) / 2, (Top + Bottom) / 2);

        public Point DropMenuOriginPoint => new Point(Bounds.Left, Bounds.Bottom);

        /// <summary>
        /// Is this chart on the front? add your code here to prove it is inactive 
        /// </summary>
        public virtual bool IsActive
        {
            get
            {
                if (!(Parent is null) && Parent is TabItem parentTab && !(parentTab.HostContainer is null))
                {
                    if (parentTab.HostContainer.ActiveTab != parentTab) return false;
                }
                return true;
            }
        }

        public abstract void Coordinate();

        protected override void OnResize(EventArgs e)
        {
            if (IsActive)
            {
                Coordinate();
                base.OnResize(e);
            }
        }
        protected override void OnClientSizeChanged(EventArgs e)
        {
            if (IsActive)
            {
                Coordinate();
                base.OnClientSizeChanged(e);
            }
        }

        //protected override void OnAc

        #endregion

        #region Mouse Events

        public virtual MouseState MouseState { get; protected set; }
        public virtual bool IsHot => (Enabled && MouseState == MouseState.Down);
        public virtual void Reset() => MouseState = MouseState.Out;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            if (ClientRectangle.Contains(pt) & Enabled)
                MouseState = MouseState.Hover;
            else
                MouseState = MouseState.Out;
            Invalidate(true);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (Parent != null)
            {
                Parent.Invalidate(false);
                Parent.Focus();
            }

            Point pt = new Point(e.X, e.Y);
            if (e.Button == MouseButtons.Left && ClientRectangle.Contains(pt) & Enabled)
                MouseState = MouseState.Down;
            //else
            //MouseState = MouseStateType.Out;
            Invalidate(true);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            MouseState = MouseState.Out;
            Invalidate(true);
        }

        #endregion
    }
}
