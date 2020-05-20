/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;

namespace Xu
{
    [DesignerCategory("Code"), Serializable, DataContract]
    public class ToggleWidget : ButtonWidget
    {
        public ToggleWidget(Command cmd, Command cmdChecked, bool isChecked = false, bool hasIconColor = false,
            bool hasSmooth = false, bool hasEdge = true, int edgeWidth = 1,
            int order = 0, Importance importance = Importance.Minor)
            : base(cmd, hasIconColor, hasSmooth, hasEdge, edgeWidth, order, importance)
        {
            Checked = isChecked;
            CommandChecked = cmdChecked;
            CheckedHoverFillBrush = new SolidBrush(Color.FromArgb(70, CommandChecked.Theme.FillColor));
            CheckedClickFillBrush = new SolidBrush(Color.FromArgb(200, CommandChecked.Theme.FillColor));
            CheckedHoverEdgePen = new Pen(Color.FromArgb(70, CommandChecked.Theme.EdgeColor), edgeWidth);
            CheckedClickEdgePen = new Pen(Color.FromArgb(200, CommandChecked.Theme.EdgeColor), edgeWidth);

            Coordinate();
            ResumeLayout(false);
            PerformLayout();
        }

        public bool Checked { get { return m_Checked; } set { m_Checked = value; Invalidate(true); } }
        protected bool m_Checked;
        protected Command CommandChecked { get; set; }

        public virtual Brush CheckedHoverFillBrush { get; }
        public virtual Brush CheckedClickFillBrush { get; }
        public virtual Pen CheckedHoverEdgePen { get; }
        public virtual Pen CheckedClickEdgePen { get; }

        public override void Execute(IItem sender = null, string[] args = null)
        {
            if (Checked) CommandChecked.Start(sender, args);
            else Command.Start(sender, args);
            Checked = !Checked;
        }

        public override void PaintControl(Graphics g)
        {
            if (!Checked)
            {
                base.PaintControl(g);
            }
            else
            {
                if (CommandChecked.Enabled && Enabled)
                    if (MouseState == MouseState.Hover)
                    {
                        g.FillRectangle(CheckedHoverFillBrush, ControlRect);
                        if (HasEdge) g.DrawRectangle(CheckedHoverEdgePen, ControlRect);
                    }
                    else if (MouseState == MouseState.Down)
                    {
                        g.FillRectangle(CheckedClickFillBrush, ControlRect);
                        if (HasEdge) g.DrawRectangle(CheckedClickEdgePen, ControlRect);
                    }

                if (!HasIconColor)
                    CommandChecked.DrawIconCenter(g, new Size(16, 16), ClientRectangle, MouseState, Checked, Enabled);
                else
                    CommandChecked.DrawIconCenter(g, new Size(16, 16), ClientRectangle, ForeColor, MouseState, Checked, Enabled);
            }

        }
    }
}
