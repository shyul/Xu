/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Xu
{
    [DesignerCategory("Code")]
    public class CaptionBar : UserControl
    {
        public CaptionBar(MosaicForm fm)
        {
            MoForm = fm;

            if (MoForm != null)
            {
                DoubleClick += (object sender, EventArgs e) =>
                {
                    MouseState = MouseState.Out;
                    MoForm.Btn_Maximize.Execute();
                };
            }

            // Control Settings
            SuspendLayout();

            Controls.AddRange(new ButtonWidget[] {
                new ButtonWidget(Main.Command_Nav_Back),
                new ButtonWidget(Main.Command_Nav_Next),
                new ButtonWidget(Main.Command_File_Open),
                new ButtonWidget(Main.Command_File_Save)
            });

            DoubleBuffered = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;

            SearchBox = new TextBox()
            {
                BorderStyle = BorderStyle.FixedSingle,
                Font = Main.Theme.FontBold,
                BackColor = Color.Wheat,
                Width = 300,
                Height = 13
            };
            //Controls.Add(SearchBox);
            // Coordinate();

            ResumeLayout(false);
            PerformLayout();
        }
        public MosaicForm MoForm { get; protected set; }

        public TextBox SearchBox { get; }

        public bool IsActivated => MoForm.IsActivated;

        public Rectangle ControlRect { get { return new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1); } }

        private Point CaptionTitleLocation { get; set; } = Point.Empty;

        protected void Coordinate()
        {
            Width = MoForm.Btn_Minimize.Left - MoForm.Ribbon.Right - 20; // Add margin to the right bound
            SearchBox.Location = new Point(Width - SearchBox.Width, 0);//, 300, Height - 120);
            SearchBox.Height = 133;
            int btnX = 10, btnY = (MoForm.WindowState == FormWindowState.Maximized) ? 2 : 0;
            SuspendLayout();

            foreach (Control bt in Controls)
            {
                switch (bt)
                {
                    case ButtonWidget bw:
                        bw.Bounds = new Rectangle(btnX, btnY, 20, 20);
                        btnX = bw.Right + 3;
                        break;

                    case TextBox bw when bw.Enabled == false:

                        break;
                }
            }

            CaptionTitleLocation = new Point(btnX + 7, btnY + 1); // 3 or zero

            ResumeLayout(false);
            PerformLayout();
            Invalidate(true);
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

        public static readonly Pen SepPen = new(new SolidBrush(Color.FromArgb(180, ControlPaint.Dark(MosaicForm.ActiveColor, 0.1f))));
        public static readonly Pen SepPenShade1 = new(new SolidBrush(Color.FromArgb(120, ControlPaint.Light(MosaicForm.ActiveColor, 1f))));
        public void DrawSeparator(Graphics g, int x, int y1, int y2)
        {
            if (IsActivated)
            {
                g.DrawLine(SepPenShade1, new Point(x - 1, y1 + 1), new Point(x - 1, y2 - 1));
                g.DrawLine(SepPen, new Point(x, y1 + 1), new Point(x, y2 - 1));
                g.DrawLine(SepPenShade1, new Point(x + 1, y1 + 1), new Point(x + 1, y2 - 1));
                g.DrawLine(SepPenShade1, new Point(x, y1 + 1), new Point(x, y1));
                g.DrawLine(SepPenShade1, new Point(x, y2 - 1), new Point(x, y2));
            }
            else
            {
                g.DrawLine(SepPen, new Point(x, y1), new Point(x, y2));
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.Clear(Color.Transparent);

            int sepY1 = (MoForm.WindowState == FormWindowState.Maximized) ? 3 : 0;
            int sepY2 = Height - 4;

            DrawSeparator(g, 5, sepY1, sepY2);
            DrawSeparator(g, CaptionTitleLocation.X - 6, sepY1, sepY2);

            using SolidBrush textBrush = new(ForeColor);
            g.DrawString(MoForm.Text,
                Main.Theme.FontBold,
                textBrush,
                CaptionTitleLocation);
        }

        public virtual MouseState MouseState { get; protected set; }

        protected Point MouseOrigin { get; set; } = Point.Empty;
        protected Point FormOrigin { get; set; }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Point pt = new(e.X, e.Y);

            if (MouseState != MouseState.Drag)
            {
                if (ClientRectangle.Contains(pt))
                {
                    if (MouseState == MouseState.Down)
                        MouseState = MouseState.Drag;
                    else
                    {
                        MouseState = MouseState.Hover;
                    }
                }
                else
                    MouseState = MouseState.Out;
            }
            else
            {
                if (MoForm != null)
                {
                    Point newPt = Control.MousePosition;
                    MoForm.Location = new Point(FormOrigin.X + (newPt.X - MouseOrigin.X), FormOrigin.Y + (newPt.Y - MouseOrigin.Y));
                }
            }
            Invalidate(true);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Point pt = new(e.X, e.Y);
            if (ClientRectangle.Contains(pt))
            {
                MouseOrigin = Control.MousePosition;
                if (MoForm != null) FormOrigin = MoForm.Location;
                MouseState = MouseState.Down;
            }
            else
                MouseState = MouseState.Out;

            Invalidate(true);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            Point pt = new(e.X, e.Y);
            if (ClientRectangle.Contains(pt))
            {
                MouseState = MouseState.Hover;
                //Point newPt = Control.MousePosition;
                //Form.Location = new Point(FormOrigin.X + (newPt.X - MouseOrigin.X), FormOrigin.Y + (newPt.Y - MouseOrigin.Y));
            }
            else
                MouseState = MouseState.Out;
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            MouseState = MouseState.Out;
            Invalidate(true);
        }
    }
}
