/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.ComponentModel;
using System.Drawing;

namespace Xu
{
    /// <summary>
    /// The object stands for a single ribbon tab item.
    /// </summary>
    [DesignerCategory("Code")]
    public class RibbonTabItem : TabItem
    {
        #region Ctor
        public RibbonTabItem(string tabName) : base(tabName)
        {
            // Create the Panel and add it to the controls of this tab.
            // The panel will remain in this tab at this tab's entire life cycle.
            Panel = new RibbonTabPanel(this);
            Controls.Add(Panel);

            //BackColor = Main.Theme.Panel.FillBrush.Color;

            // We want the tab to have centered text only, without Icon.
            HasIcon = false;
            Btn_Pin.Enabled = false;
            Btn_Close.Enabled = false;

            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        #region Components

        /// <summary>
        /// The Ribbon which containcs this ribbon tab, or the bibbon this Ribbon belongs to.
        /// </summary>
        public Ribbon Ribbon { get; set; }

        public bool IsShrink => (Ribbon == null) || Ribbon.IsShrink;

        public override Color BackColor => Main.Theme.Panel.FillColor;
        /// <summary>
        /// Permanent link to the associated RibbonTabPanel
        /// </summary>
        public RibbonTabPanel Panel { get; protected set; }

        public void Add(RibbonPane pane, int order) => Panel.Add(pane, order);
        public void Add(RibbonPane pane) => Panel.Add(pane);
        public void Remove(RibbonPane pane) => Panel.Remove(pane);
        #endregion

        #region Coordinate
        protected override void CoordinateLayout()
        {
            if (!IsShrink)
            {
                Panel.Location = new Point(0, 0);
                Panel.Width = Width;
                Panel.Height = Ribbon.PanelHeight - 1;
            }
        }
        #endregion
    }
}
