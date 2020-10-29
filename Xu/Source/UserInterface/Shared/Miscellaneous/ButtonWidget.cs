/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Xu
{
    [DesignerCategory("Code")]
    public class ButtonWidget : Widget
    {
        public ButtonWidget(Command cmd, bool hasIconColor = false, bool hasSmooth = false, bool hasEdge = true, int edgeWidth = 1, int order = 0, Importance importance = Importance.Minor) : base(order, importance)
        {
            Command = cmd;

            HasIconColor = hasIconColor;
            HasSmooth = hasSmooth;
            HasEdge = hasEdge;

            HoverFillBrush = new SolidBrush(Color.FromArgb(70, Command.Theme.FillColor));
            ClickFillBrush = new SolidBrush(Color.FromArgb(200, Command.Theme.FillColor));
            HoverEdgePen = new Pen(Color.FromArgb(70, Command.Theme.EdgeColor), edgeWidth);
            ClickEdgePen = new Pen(Color.FromArgb(200, Command.Theme.EdgeColor), edgeWidth);

            Coordinate();
            ResumeLayout(false);
            PerformLayout();
        }

        protected Command Command { get; set; }

        public virtual void Execute(IObject sender = null, string[] args = null)
        {
            Command.Start(sender, args);
        }

        protected virtual bool HasIconColor { get; set; }

        protected virtual bool HasSmooth { get; set; }

        protected virtual bool HasEdge { get; set; }

        public override Color BackColor => Color.Transparent; // Command.Theme.FillColor;

        public override Color ForeColor => (Parent != null) ? Parent.ForeColor : Command.Theme.ForeColor;

        public override void Coordinate() { }

        public override Brush HoverFillBrush { get; protected set; }

        public override Brush ClickFillBrush { get; protected set; }

        public override Pen HoverEdgePen { get; protected set; }

        public override Pen ClickEdgePen { get; protected set; }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;

            if (HasSmooth)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.SmoothingMode = SmoothingMode.AntiAlias;
            }
            else
            {
                g.SmoothingMode = SmoothingMode.HighSpeed;
                g.SmoothingMode = SmoothingMode.Default;
            }

            //g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(BackColor);

            PaintControl(g);
        }

        public virtual void PaintControl(Graphics g)
        {
            if (Command.Enabled && Enabled)
                if (MouseState == MouseState.Hover)
                {
                    g.FillRectangle(HoverFillBrush, ControlRect);
                    if (HasEdge) g.DrawRectangle(HoverEdgePen, ControlRect);
                }
                else if (MouseState == MouseState.Down)
                {
                    g.FillRectangle(ClickFillBrush, ControlRect);
                    if (HasEdge) g.DrawRectangle(ClickEdgePen, ControlRect);
                }

            if (!HasIconColor)
                Command.DrawIconCenter(g, new Size(16, 16), ClientRectangle, MouseState, false, Enabled);
            else
                Command.DrawIconCenter(g, new Size(16, 16), ClientRectangle, ForeColor, MouseState, false, Enabled);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            if (e.Button == MouseButtons.Left && ClientRectangle.Contains(pt) && Command.Enabled && Enabled)
                Execute(this, new string[0] { });
            Invalidate(true);
        }
    }
}