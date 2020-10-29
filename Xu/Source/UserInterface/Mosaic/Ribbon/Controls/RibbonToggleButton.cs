/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.ComponentModel;

namespace Xu
{
    [DesignerCategory("Code")]
    public class RibbonToggleButton : RibbonButton
    {
        public RibbonToggleButton(Command cmd, Command cmdChecked, bool isChecked = false, int order = 0, Importance importance = Importance.Minor)
            : base(cmd, order, importance)
        {
            Checked = isChecked;
            CommandChecked = cmdChecked;
            Coordinate();
            ResumeLayout(false);
            PerformLayout();
        }

        public bool Checked { get { return m_Checked; } set { m_Checked = value; Coordinate(); Invalidate(true); } }
        protected bool m_Checked;
        protected Command CommandChecked { get; set; }

        public override string Label => (Checked) ? CommandChecked.Label : Command.Label;
        public override string Description => (Checked) ? CommandChecked.Description : Command.Description;

        #region Coordinate






        #endregion

    }
}
