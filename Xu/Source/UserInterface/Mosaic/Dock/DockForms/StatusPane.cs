/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Xu
{
    [DesignerCategory("Code")]
    public class StatusStrip : Control
    {
        public const int StatusStripHeight = 24;

        #region Ctor
        public StatusStrip()
        {
            SuspendLayout();
            Name = "PacMainStatus";
            components = new Container();
            DoubleBuffered = true;
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, false);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            Dock = DockStyle.Bottom;
            Height = StatusStripHeight;
            ForeColor = Main.Theme.Panel.ForeColor;
            BackColor = Main.Theme.Panel.FillColor;
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
        #endregion

        public override Font Font => Main.Theme.Font;
    }
}
