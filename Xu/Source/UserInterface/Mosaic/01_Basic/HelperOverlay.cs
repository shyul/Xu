/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Xu.WindowsNativeMethods;

namespace Xu
{
    [DesignerCategory("Code")]
    public class HelperOverlay : Form
    {
        #region Ctor
        public HelperOverlay()
        {
            SuspendLayout();
            Visible = false;
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            FormBorderStyle = FormBorderStyle.None;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.Manual;
            TopMost = false; // Make sure the overlay does not steal focus when show up
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            BackColor = Color.Magenta;
            TransparencyKey = BackColor;
            AutoScaleMode = AutoScaleMode.Dpi;
            Name = "Overlay";
            Text = "Overlay";
            ResumeLayout(false);
        }
        /// <summary>
        /// Make sure the overlay does not steal focus when show up
        /// </summary>
        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }
        /// <summary>
        /// No click on this form
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams baseParams = base.CreateParams;
                baseParams.ExStyle |= (int)(WindowStyles.EX_NOACTIVATE | WindowStyles.EX_TOOLWINDOW);
                return baseParams;
            }
        }
        #endregion
    }
}
