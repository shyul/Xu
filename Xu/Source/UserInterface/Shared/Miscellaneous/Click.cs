/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Xu
{
    public class Click : IOrdered, IStackable
    {
        public Click(Command cmd)
        {
            Command = cmd;
        }

        public Command Command { get; protected set; }

        public string Name { get => Command.Name; set { Command.Name = value; } }

        public string Label { get => Command.Label; set { Command.Label = value; } }

        public string Description { get => Command.Description; set { Command.Description = value; } }

        public Importance Importance { get => Command.Importance; set { Command.Importance = value; } }

        public bool Enabled { get => Command.Enabled; set { Command.Enabled = value; } }

        public int Order { get => Command.Order; set { Command.Order = value; } }

        public bool IsSectionEnd { get { return (m_IsSectionEnd || Importance > Importance.Minor || (IsLineEnd && StackedY >= 2)); } set { m_IsSectionEnd = value; } }
        protected bool m_IsSectionEnd = false;

        public bool IsLineEnd { get { return (m_IsLineEnd || Importance > Importance.Minor); } set { m_IsLineEnd = value; } }
        protected bool m_IsLineEnd = false;

        public int StackedY { get; set; } = 0;

        public int SectionIndex { get; set; } = 0;

        public bool Checked { get; set; } = false;

        public bool Toggle { get; set; } = false;

        #region Coordinate

        public Rectangle Bounds { get; set; }

        public Point Location { get => Bounds.Location; set { Bounds = new Rectangle(value, Bounds.Size); } }

        public Size Size { get => Bounds.Size; set { Bounds = new Rectangle(Bounds.Location, value); } }

        public int Top => Bounds.Top;

        public int Bottom => Bounds.Bottom;

        public int Left => Bounds.Left;

        public int Right => Bounds.Right;

        public Point Center => new Point((Left + Right) / 2, (Top + Bottom) / 2);

        public Point DropMenuOriginPoint => new Point(Bounds.Left, Bounds.Bottom);

        public Rectangle ShadowRect => new Rectangle(Bounds.Left + 1, Bounds.Top + 1, Bounds.Width - 2, Bounds.Height - 2);

        #endregion

        #region Paint
        public void PaintControl(Graphics g, Color color)
        {
            if (Enabled)
            {
                Rectangle shadowRect = ShadowRect;
                if (MouseState == MouseState.Down)
                {
                    g.FillRectangle(Main.Theme.Click.FillBrush, shadowRect);
                }
                else if (MouseState == MouseState.Hover)
                {
                    g.FillRectangle(Main.Theme.Hover.FillBrush, shadowRect);
                }
            }
            Command.DrawIcon(g, Bounds, color, MouseState, Checked, Enabled);
        }

        public void PaintControl(Graphics g)
        {
            if (Enabled)
            {
                Rectangle shadowRect = ShadowRect;
                if (MouseState == MouseState.Down)
                {
                    g.FillRectangle(Main.Theme.Click.FillBrush, shadowRect);
                }
                else if (MouseState == MouseState.Hover)
                {
                    g.FillRectangle(Main.Theme.Hover.FillBrush, shadowRect);
                }
            }
            Command.DrawIcon(g, Bounds, MouseState, Checked, Enabled);
        }

        #endregion

        #region Mouse

        public MouseState MouseState { get; protected set; } = MouseState.Out;

        public bool IsHot => (Enabled && MouseState == MouseState.Down);

        public virtual bool MouseMove(Point pt)
        {
            bool GotPt = Bounds.Contains(pt) & Enabled;
            MouseState = (GotPt) ? MouseState.Hover : MouseState.Out;
            return GotPt;
        }

        public virtual bool MouseDown(Point pt, MouseButtons mb)
        {
            bool GotPt = (mb == MouseButtons.Left) && Bounds.Contains(pt) & Enabled;
            MouseState = (GotPt) ? MouseState.Down : MouseState;
            return GotPt;
        }

        public virtual bool MouseUp(Point pt, MouseButtons mb)
        {
            bool isExec = (mb == MouseButtons.Left) && Bounds.Contains(pt) & Enabled;
            MouseState = (isExec) ? MouseState.Hover : MouseState;
            if (isExec)
            {
                Command.Start();
                if (Toggle) Checked = !Checked;
            }
            return isExec;
        }

        public virtual void MouseLeave() => MouseState = MouseState.Out;

        #endregion
    }
}
