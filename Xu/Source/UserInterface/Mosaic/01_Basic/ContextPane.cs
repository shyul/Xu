/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Xu
{
    [DesignerCategory("Code")]
    public class ContextPane : ToolStripDropDown
    {
        #region Ctor
        public ContextPane()
        {
            // Components
            components = new Container();
            // Control Settings
            Padding = Margin = Padding.Empty;
        }
        #endregion
        #region Components
        private IContainer components = null;
        private ContextHost m_host;
        //private ToolStripControlHost m_host;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                if (m_host != null) Items.Remove(m_host);
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
        public void Show(Form f, ContextHost host, Point p)
        {
            if (host != null)
            {
                if (m_host != host)
                {
                    if (m_host != null)
                        Items.Remove(m_host);
                    Items.Add(host);
                    m_host = host;
                }
            }
            else
                throw new ArgumentNullException("content");
            /*
            m_panel.Disposed += delegate (object sender, EventArgs e)
            {
                //m_pane = null;
                Dispose(true);// this popup container will be disposed immediately after disposion of the contained control
            };
            */
            this.Show(f, new Rectangle(p, new Size(0, 0)));

        }
        private void Show(Control control, Rectangle area)
        {
            if (control == null)
                throw new ArgumentNullException("control");


            Point location = control.PointToScreen(new Point(area.Left, area.Top + area.Height));

            Rectangle screen = Screen.FromControl(control).WorkingArea;

            if (location.X + Size.Width > (screen.Left + screen.Width))
                location.X = (screen.Left + screen.Width) - Size.Width;

            if (location.Y + Size.Height > (screen.Top + screen.Height))
                location.Y -= Size.Height + area.Height;

            location = control.PointToClient(location);

            Show(control, location, ToolStripDropDownDirection.BelowRight);
        }
        private bool IsFade => DockCanvas.IsMenuFadeEnabled;
        private const int c_frames = 5;
        private const int c_totalduration = 100;
        private const int c_frameduration = c_totalduration / c_frames;
        protected override void SetVisibleCore(bool visible)
        {
            double opacity = Opacity;
            if (visible && IsFade) Opacity = 0;
            base.SetVisibleCore(visible);
            if (!visible || !IsFade)
            {
                return;
            }

            for (int i = 1; i <= c_frames; i++)
            {
                if (i > 1)
                {
                    System.Threading.Thread.Sleep(c_frameduration);
                }
                Opacity = opacity * (double)i / (double)c_frames;
            }
            Opacity = opacity;
        }
        protected override void OnOpening(CancelEventArgs e)
        {
            //if (m_pane.IsDisposed || m_pane.Disposing)
            if (m_host.IsDisposed)
            {
                e.Cancel = true;
                return;
            }
            base.OnOpening(e);
        }
        protected override void OnOpened(EventArgs e)
        {
            //m_pane.Focus();
            base.OnOpened(e);
        }
        protected override void OnClosing(ToolStripDropDownClosingEventArgs e)
        {
            m_host.Close();
        }
        //prevent alt from closing it and allow alt+menumonic to work
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if ((keyData & Keys.Alt) == Keys.Alt)
                return false;

            return base.ProcessDialogKey(keyData);
        }
    }

    [DesignerCategory("Code")]
    public abstract class ContextHost : ToolStripControlHost
    {
        public ContextHost(UserControl c) : base(c)
        {
            BackColor = Main.Theme.Panel.FillColor;
            Padding = Margin = Padding.Empty;
            AutoSize = false;
        }
        public abstract void Close();

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;

            g.DrawLine(Main.Theme.Panel.EdgePen, new Point(0, 0), new Point(0, Height));
            g.DrawLine(Main.Theme.Panel.EdgePen, new Point(0, Height - 1), new Point(Width, Height - 1));
            //g.DrawLine(MoTheme.Panel.EdgePen, new Point(Width - 1, 0), new Point(Width - 1, Height - 1));
        }
    }
}
