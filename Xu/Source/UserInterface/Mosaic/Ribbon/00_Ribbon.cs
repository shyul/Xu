/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Xu
{
    [DesignerCategory("Code")]
    public class Ribbon : ContainerControl
    {
        #region Ctor
        public Ribbon()
        {
            // Components
            components = new Container();
            RibbonContainer = new RibbonTabContainer(this);

            // Control Settings
            SuspendLayout();
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            Dock = DockStyle.None;
            Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left;
            BackColor = Color.Transparent;
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        #region Components
        private IContainer components = null;
        public MosaicForm MoForm { get; protected set; }
        public RibbonTabContainer RibbonContainer { get; protected set; }
        public List<TabItem> Tabs => RibbonContainer.Tabs;
        public RibbonTabItem ActiveTab => (RibbonTabItem)RibbonContainer.ActiveTab;
        public int Count => RibbonContainer.Count;

        public void ActivateTab(int index) => RibbonContainer.ActivateTab(index);
        public void AddRibbonTab(RibbonTabItem rt)
        {
            RibbonContainer.AddRibbonTab(rt);
            rt.Ribbon = this;
            Coordinate();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent != null)
            {
                if ((typeof(MosaicForm)).IsAssignableFrom(Parent.GetType()))
                {
                    MoForm = (MosaicForm)Parent;

                }
                else throw new Exception("Ribbon can only be exsiting in RibbonForm / Parent: " + Parent.GetType().ToString());
            }
            else MoForm = null;
        }
        protected virtual void UpdateGraphics()
        {
            if (Parent != null) Parent.Invalidate(true);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Control
        protected bool Unlocked
        {
            get { return DockCanvas.Unlocked; }
            set
            {
                if (MoForm != null)
                {
                    MoForm.SuspendLayout();
                }
                DockCanvas.Unlocked = value;
                if (MoForm != null)
                {
                    MoForm.ResumeLayout(false);
                    MoForm.PerformLayout();
                }
            }
        }

        protected Click Btn_TabMenu { get; set; } = new Click(new Command()
        {
            Description = "Ribbon Menu",
            Enabled = true,
            IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>() {

                    { IconType.Normal, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Caption_Bars }, } },

                },
            //Action = (IItem sender, string[] args, Progress<Event> progress, CancellationTokenSource cts) => { ShowMenu(); },
        });//= new Click() { Description = "Ribbon Menu", Enabled = true };

        public bool IsShrink => MoForm.IsRibbonShrink;

        protected Click Btn_Shrink { get; set; } = new Click(new Command()
        {
            Description = "Shrink",
            Enabled = true,
            IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>() {

                    { IconType.Normal, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Caption_ArrowUp }, } },

                    { IconType.Checked, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Caption_ArrowDown }, } },

                    { IconType.CheckedHover, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Caption_ArrowDown }, } },

                },
        });// = new Click() { Description = "Shrink", Enabled = true };

        protected MouseState OrbButtonMouseState { get; set; } = MouseState.Out;
        #endregion

        #region Ribbon Geometry
        public const int TabHeight = 23;
        public const int PanelHeight = 92;
        public const int OrbWidth = 55;
        public static Color OrbDarkColor = Color.FromArgb(255, 32, 70, 80);
        public static Color OrbLightColor = Color.FromArgb(255, 80, 120, 140);
        public static Color OrbHoverDarkColor = Color.FromArgb(255, 60, 110, 120);
        public static Color OrbHoverLightColor = Color.FromArgb(255, 100, 140, 160);
        #endregion

        #region Coordinate
        private const int m_TabMargin = 3;
        private const int m_MinimumTabWdith = 55;
        private const int m_TextHeight = 17; // TabSize - (TabMargin * 2);
        private const int m_IconSize = 16;
        private const int m_IconMargin = 3;
        private Rectangle m_EdgeRect;
        private Rectangle m_OrbRect;

        private void Coordinate()
        {
            m_OrbRect = new Rectangle(0, 0, OrbWidth - 2, Height);
            Btn_Shrink.Bounds = new Rectangle(Width - (m_IconSize + m_IconMargin) * 2 - m_IconMargin, m_IconMargin, m_IconSize, m_IconSize);
            Btn_TabMenu.Bounds = new Rectangle(Width - (m_IconSize + m_IconMargin), m_IconMargin, m_IconSize, m_IconSize);

            int offset = OrbWidth;
            lock (RibbonContainer.Tabs)
                for (int i = 0; i < Count; i++)
                {
                    int baseOffset = offset;
                    RibbonTabItem rt = (RibbonTabItem)RibbonContainer.Tabs[i];

                    int textWidth = rt.TabNameWidth + m_TabMargin;
                    if (textWidth < m_MinimumTabWdith) textWidth = m_MinimumTabWdith;
                    rt.TabNameRect = new Rectangle(offset, m_TabMargin, textWidth, m_TextHeight);
                    offset += textWidth;
                    rt.TabRect = new Rectangle(baseOffset, m_EdgeRect.Top, offset - baseOffset, TabHeight);
                }
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
        #endregion

        #region Paint
        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            g.Clear(Color.Transparent);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            g.FillRectangle(AppTheme.WhiteBrush, new Rectangle(0, 0, Width, Height));

            switch (OrbButtonMouseState)
            {
                case MouseState.Hover:
                    g.FillRectangleStyle2010(m_OrbRect, 0, OrbHoverDarkColor, OrbHoverLightColor);
                    break;
                case MouseState.Down:
                    g.FillRectangleStyle2010(m_OrbRect, 0, OrbHoverLightColor, OrbHoverDarkColor);
                    break;
                default:
                    g.FillRectangleStyle2010(m_OrbRect, 0, OrbDarkColor, OrbLightColor);
                    break;
            }

            g.DrawString("File", Main.Theme.FontBold, AppTheme.WhiteBrush, m_OrbRect, AppTheme.TextAlignCenter);

            lock (Tabs)
                foreach (RibbonTabItem rt in Tabs)
                {
                    Rectangle tabRect = rt.TabRect;
                    if (rt == ActiveTab)
                    {
                        using (GraphicsPath tabOutline = new GraphicsPath())
                        {
                            tabOutline.AddLine(new Point(tabRect.Left, Height), new Point(tabRect.Left, 0));
                            tabOutline.AddLine(new Point(tabRect.Right, 0), new Point(tabRect.Right, Height));
                            g.FillPath(Main.Theme.Panel.FillBrush, tabOutline);
                            g.DrawPath(Main.Theme.Panel.EdgePen, tabOutline);
                        }
                    }

                    if (rt.MouseState != MouseState.Out)
                    {
                        Rectangle hoverRect = tabRect.Shrink(3);
                        if (rt == ActiveTab && rt.MouseState == MouseState.Down)
                        {
                            g.FillRectangle(Main.Theme.InactiveCursor.FillBrush, hoverRect);
                            g.DrawRectangle(Main.Theme.InactiveCursor.EdgePen, hoverRect);
                            g.DrawString(rt.TabName, Font, Main.Theme.DarkTextBrush, rt.TabNameRect, AppTheme.TextAlignCenter);
                        }
                        else if (rt != ActiveTab && rt.MouseState == MouseState.Hover && MouseState != MouseState.Out)
                        {
                            g.FillRectangle(Main.Theme.ActiveCursor.FillBrush, hoverRect);
                            g.DrawRectangle(Main.Theme.ActiveCursor.EdgePen, hoverRect);
                            g.DrawString(rt.TabName, Font, Main.Theme.ActiveCursor.EdgeBrush, rt.TabNameRect, AppTheme.TextAlignCenter);
                        }
                        else
                            g.DrawString(rt.TabName, Font, Main.Theme.DimTextBrush, rt.TabNameRect, AppTheme.TextAlignCenter);
                    }
                    else
                        g.DrawString(rt.TabName, Font, Main.Theme.DimTextBrush, rt.TabNameRect, AppTheme.TextAlignCenter);
                }

            //Btn_TabMenu.PaintControl(g, Xu.Properties.Resources.Caption_Bars, Main.Theme.DimTextBrush.Color);
            Btn_TabMenu.PaintControl(g, Main.Theme.DimTextBrush.Color);

            Btn_Shrink.Checked = (ActiveTab == null || IsShrink);
            Btn_Shrink.PaintControl(g, Main.Theme.DimTextBrush.Color);
            /*
            if (ActiveTab == null || IsShrink)
                Btn_Shrink.PaintControl(g, Xu.Properties.Resources.Caption_ArrowDown, Main.Theme.DimTextBrush.Color);
            else
                Btn_Shrink.PaintControl(g, Xu.Properties.Resources.Caption_ArrowUp, Main.Theme.DimTextBrush.Color);*/
        }
        #endregion

        #region Mouse
        public virtual MouseState MouseState { get; set; } = MouseState.Out;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Construct a Point type representing the mouse location.
            Point pt = new Point(e.X, e.Y);

            lock (Tabs)
            {
                foreach (RibbonTabItem rt in Tabs)
                {
                    Rectangle tabRect = rt.TabRect;
                    if (rt.MouseState != MouseState.Drag && rt.MouseState != MouseState.Down && rt.ShowTab)
                    {
                        rt.MouseState = (tabRect.Contains(pt)) ? MouseState.Hover : MouseState.Out;


                    }
                }
            }

            OrbButtonMouseState = (m_OrbRect.Contains(pt)) ? MouseState.Hover : MouseState.Out;

            MouseState = MouseState.Hover;
            Invalidate();
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            // Construct a Point type representing the mouse location.
            Point pt = new Point(e.X, e.Y);

            if (m_OrbRect.Contains(pt))
            {
                OrbButtonMouseState = MouseState.Out;
                //m_OrbButtonMouseState = MouseStateType.Down;
                MoForm.SetActivateOrbMenu(true);
            }
            else
            {
                OrbButtonMouseState = MouseState.Out;
            }

            bool FoundTab = false;
            lock (Tabs)
            {
                for (int i = 0; i < Tabs.Count; i++)
                {
                    RibbonTabItem rt = (RibbonTabItem)Tabs[i];
                    Rectangle tabRect = rt.TabRect;
                    if (tabRect.Contains(pt) && rt.ShowTab)
                    {
                        rt.MouseState = MouseState.Down;
                        ActivateTab(i);

                        FoundTab = true;
                    }
                    else
                        rt.MouseState = MouseState.Out;
                }
            }

            if (!FoundTab && ActiveTab != null)
            {
                ActiveTab.Focus();
                UpdateGraphics();
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            // Construct a Point type representing the mouse location.
            Point pt = new Point(e.X, e.Y);

            if (OrbButtonMouseState == MouseState.Down && m_OrbRect.Contains(pt))
            {
                OrbButtonMouseState = MouseState.Hover;
                Invalidate();

                //MessageBox.Show("Orb Button is clicked!");

                return;
            }
            else if (Btn_TabMenu.MouseUp(pt, e.Button))
            {
                Invalidate();




                return;
            }
            else if (Btn_Shrink.MouseUp(pt, e.Button))
            {
                Invalidate();
                MoForm.IsRibbonShrink = !MoForm.IsRibbonShrink;
                return;
            }
            else
            {
                TabItem tp_close = null;
                lock (Tabs)
                {
                    foreach (TabItem tp in Tabs)
                    {
                        Rectangle tabRect = tp.TabRect;
                        tp.MouseState = (tabRect.Contains(pt)) ? MouseState.Hover : MouseState.Out;
                    }
                }
                if (tp_close != null) tp_close.Close();
                MouseState = MouseState.Hover;
                Invalidate(true);
            }
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            MouseState = MouseState.Out;
            lock (Tabs)
            {
                foreach (RibbonTabItem rt in Tabs)
                {
                    rt.MouseState = MouseState.Out;
                }
            }

            OrbButtonMouseState = MouseState.Out;

            Btn_TabMenu.MouseLeave();
            Btn_Shrink.MouseLeave();
            UpdateGraphics();
        }
        #endregion
    }
}
