/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Xu
{
    /// <summary>
    /// The object stands for the tab containers for all ribbon menu items
    /// </summary>
    [DesignerCategory("Code")]
    public class RibbonTabContainer : Tab
    {
        #region Ctor
        public RibbonTabContainer(Ribbon ribbon) : base()
        {
            Ribbon = ribbon;
            // Control Settings
            Dock = DockStyle.None;
            //Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left;
            //Size = new Size(300, MoTheme.RibbTabHeight);
            //BackColor = Main.Theme.Panel.FillColor;
            //Capture = true;
            ShowPin = false; // Do not show pin or close button on the tabs
            ShowClose = false;
            ShowIcon = false;
            m_showTab = false;
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        #region Components
        public MosaicForm MoForm { get; protected set; }
        public Ribbon Ribbon { get; protected set; }
        protected bool Unlocked => DockCanvas.Unlocked;
        protected bool IsShrink => MoForm.IsRibbonShrink;
        public override Color BackColor => Main.Theme.Panel.FillColor;
        public void AddRibbonTab(RibbonTabItem rt)
        {
            if (rt.Ribbon == null) rt.Ribbon = Ribbon;
            lock (Tabs)
            {
                if (!Controls.Contains(rt))
                {
                    foreach (Control cs in Controls)
                    {
                        cs.Visible = false;
                    }
                    rt.Visible = true;
                    Controls.Add(rt);
                }
                else
                {
                    foreach (Control cs in Controls)
                    {
                        cs.Visible = (cs == rt);
                    }
                }
                if (!Tabs.Contains(rt))
                {
                    rt.Order = Count;
                    rt.MouseState = MouseState.Out;
                    Tabs.Add(rt);
                }
                Sort();
            }
            if (IsShrink) DeActivate();
            else ActivateTab(0);
            UpdateGraphics();
            rt.HostContainer = this;
            if (!Visible) Visible = true;
            rt.Bounds = new Rectangle(0, 0, Width - 2, Height - 2);
        }
        public override void ActivateTab(int index)
        {
            lock (Tabs)
            {
                if (index >= 0 && index < Count)
                {
                    RibbonTabItem rt_active = (RibbonTabItem)Tabs[index];
                    foreach (RibbonTabItem rt in Tabs)
                    {
                        if (rt != rt_active)
                        {
                            rt.Visible = false;
                            rt.SuspendLayout();
                        }
                        else
                        {
                            rt.ResumeLayout(false);
                            rt.PerformLayout();
                            ActiveTab = rt_active;
                            rt.Visible = true;
                            if (IsShrink)
                            {
                                Rectangle tabRect = rt.TabRect;
                                // RibbonTabContextHost must be recreated 
                                MosaicForm.ContextPane.Show(MoForm, new RibbonTabContextHost(rt), new Point(Ribbon.Location.X + tabRect.X, Ribbon.Bounds.Bottom));
                            }
                            else
                                rt.Focus();
                        }
                    }
                    ActiveTabIndex = index;
                }
            }
            UpdateGraphics();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent != null)
            {
                //Log.Debug("RibbonTabContainer Parent is: " + Parent.GetType().ToString());
                if ((typeof(MosaicForm)).IsAssignableFrom(Parent.GetType()))
                {
                    MoForm = (MosaicForm)Parent;
                }
                else
                    throw new Exception("RibbonContainer can only be exsiting in Ribbon Parent: " + Parent.GetType().ToString());
            }
            else MoForm = null;
        }
        #endregion

        #region Coordinate
        public override void Coordinate()
        {
            SuspendLayout();
            m_edgeRect = new Rectangle(0, 0, Width - 1, Height - 1);

            lock (Tabs)
                foreach (RibbonTabItem rt in Tabs)
                {
                    if (!IsShrink)
                    {
                        if (!Controls.Contains(rt)) Controls.Add(rt);
                        if (!rt.Controls.Contains(rt.Panel)) rt.Controls.Add(rt.Panel);
                        rt.Panel.Size = new Size(Width, 93);
                        rt.Location = new Point(0, 1);
                        rt.Size = new Size(Width, Height - 1);
                    }
                    rt.Panel.Coordinate();
                }

            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        #region Paint
        protected override void OnPaint(PaintEventArgs pe)
        {
            if(!(ActiveTab is null)) 
            {
                Graphics g = pe.Graphics;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                Rectangle tabRect = ActiveTab.TabRect;
                g.DrawLine(Main.Theme.Panel.EdgePen, new Point(0, 0), new Point(MosaicForm.RibbonToLeftWindowEdgeMargin + tabRect.Left, 0));
                g.DrawLine(Main.Theme.Panel.EdgePen, new Point(MosaicForm.RibbonToLeftWindowEdgeMargin + tabRect.Right, 0), new Point(Width, 0));
            }
        }
        #endregion

        #region Mouse
        protected override void OnMouseMove(MouseEventArgs e) { }
        protected override void OnMouseDown(MouseEventArgs e) { }
        protected override void OnMouseUp(MouseEventArgs e) { }
        protected override void OnMouseLeave(EventArgs e) { }
        #endregion
    }
}
