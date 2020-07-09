/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Docking Multi-Panel Control
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
    public sealed class SideDockContainer : DockContainer
    {
        #region Ctor

        /// <summary>
        /// 
        /// </summary>
        public SideDockContainer() : base()
        {
            ShowPin = false; // Do not show pin or close button on the tabs
            ShowClose = false;
            ShowIcon = true;
            ResumeLayout(true);
            PerformLayout();
        }

        #endregion

        #region Components

        /// <summary>
        /// 
        /// </summary>
        public override bool IsRoot { get { return true; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public override void ActivateTab(int index)
        {
            base.ActivateTab(index);
            UpdateCaption();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="side"></param>
        /// <param name="df"></param>
        public override void AddForm(DockStyle side, DockForm df)
        {
            if (side == DockStyle.None) return;
            df.MouseState = MouseState.Out;
            df.IsPinned = false;
            df.Dock = DockStyle.None;
            df.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

            DockContainer original_dc = null;
            bool dc_valid_remove = false;

            // If Dock Form was assigning to a container, the original container has to be coordinated
            if (!(df.HostContainer is null) && df.HostContainer is DockContainer dkc)
            {
                //original_dc = (DockContainer)df.HostContainer;
                original_dc = dkc;
                dc_valid_remove = original_dc.Remove(df);

                //if ((typeof(SideDockContainer)).IsAssignableFrom(original_dc.GetType()) && !original_dc.ShowTab)
                if (original_dc is SideDockContainer && !original_dc.ShowTab)
                {
                    original_dc.HostPane.Coordinate();
                    original_dc.HostPane.Invalidate(true);
                }
            }

            SuspendLayout();
            if (side == DockStyle.Fill || Count == 0)
            {
                Add(df);
            }
            else
            {
                HostPane.SuspendLayout();
                SideDockContainer dc_new;
                switch (side)
                {
                    case (DockStyle.Top):
                    case (DockStyle.Left):
                        dc_new = ((SideDockPane)HostPane).CreateContainer(Order);
                        break;
                    case (DockStyle.Bottom):
                    case (DockStyle.Right):
                    default:
                        dc_new = ((SideDockPane)HostPane).CreateContainer(Order + 1);
                        break;
                }
                dc_new.Add(df);
                HostPane.ResumeLayout(true);
            }

            if (original_dc != null && dc_valid_remove && original_dc.IsEmpty)
                original_dc.Close();

            if (HostPane != null)
                HostPane.CleanUp();

            Coordinate();
            //if (HostPane != null && !HostPane.Visible) HostPane.Visible = true;
            if (!Visible) Visible = true;
            ResumeLayout(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="df"></param>
        public override void RemoveForm(DockForm df)
        {
            if (Remove(df))
            {
                if (!ShowTab)
                {
                    HostPane.Coordinate();
                    HostPane.Invalidate(true);
                }
                if (IsEmpty) Close();
            }
        } // This is when it runs merge/promote the check
        #endregion

        #region Hidden Container Related

        /// <summary>
        /// 
        /// </summary>
        public void HideContainer()
        {
            Visible = false;
            m_canvas = HostPane.Dkc;
            m_showTab = false;
            Coordinate();
        }

        /// <summary>
        /// 
        /// </summary>
        public void UnhideContainer()
        {
            m_canvas = HostPane;
            m_showTab = true;
            Visible = true;
            Coordinate();
        }
        #endregion

        #region Layout

        /// <summary>
        /// 
        /// </summary>
        public override LayoutType AllowedDockLayout
        {
            get
            {
                if (ShowTab)
                {
                    switch (HostPane.Dock)
                    {
                        case (DockStyle.Left):
                        case (DockStyle.Right):
                            return LayoutType.Vertical | LayoutType.OverLay;
                        case (DockStyle.Bottom):
                        case (DockStyle.Top):
                        default:
                            return LayoutType.Horizontal | LayoutType.OverLay;
                    }
                }
                else
                {
                    return LayoutType.None;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override LayoutType LayoutType
        {
            get
            {
                if (ShowTab)
                {
                    switch (HostPane.Dock)
                    {
                        case (DockStyle.Left):
                        case (DockStyle.Right):
                            return LayoutType.Vertical;
                        case (DockStyle.Bottom):
                        case (DockStyle.Top):
                        default:  // Side Pane's Dock is always on the side
                            return LayoutType.Horizontal;
                    }
                }
                else
                {
                    switch (HostPane.Dock)
                    {
                        case (DockStyle.Left):
                        case (DockStyle.Right):
                            return LayoutType.Horizontal;
                        case (DockStyle.Bottom):
                        case (DockStyle.Top):
                        default:  // Side Pane's Dock is always on the side
                            return LayoutType.Vertical;
                    }
                }
            }
        }
        #region Resizing

        /// <summary>
        /// 
        /// </summary>
        public override Control Canvas { get { if (m_canvas == null) return HostPane; else return m_canvas; } }

        /// <summary>
        /// 
        /// </summary>
        private Control m_canvas;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public override void OnGetSize(int size)
        {
            if (ShowTab)
            {
                ChangeSize(size);
                HostPane.Coordinate();
                Point ct = new Point((SizeGrip.Left + SizeGrip.Right) / 2, (SizeGrip.Top + SizeGrip.Bottom) / 2);
                ct = PointToScreen(ct);
                switch (HostPane.Dock)
                {
                    case (DockStyle.Left):
                    case (DockStyle.Right):
                        Cursor.Position = new Point(Cursor.Position.X, ct.Y);
                        break;
                    case (DockStyle.Bottom):
                    case (DockStyle.Top):
                    default:
                        Cursor.Position = new Point(ct.X, Cursor.Position.Y);
                        break;
                }
            }
            else
            {
                switch (HostPane.Dock)
                {
                    case (DockStyle.Left):
                        Size = new Size(Width + size, Height);
                        break;
                    case (DockStyle.Right):
                        Size = new Size(Width - size, Height);
                        break;
                    case (DockStyle.Top):
                        Size = new Size(Width, Height + size);
                        break;
                    case (DockStyle.Bottom):
                        Size = new Size(Width, Height - size);
                        break;
                    default:
                        break;
                }
                ((SideDockPane)HostPane).CoordinateHidenContainer(this);
            }
        }

        #endregion

        #endregion

        #region Coordinate

        /// <summary>
        /// 
        /// </summary>
        private Click Btn_CaptionClose { get; } = new Click(new Command()
        {
            Description = "Close",
            Enabled = true,
            IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>() {

                    { IconType.Normal, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Caption_Close }, } },

                   // { IconType.Hover, new Dictionary<Size, Bitmap>() {
                   // { new Size(16, 16), Properties.Resources.Exit_16 }, } },

                   // { IconType.Down, new Dictionary<Size, Bitmap>() {
                   // { new Size(16, 16), Properties.Resources.Exit_Blue_16 }, } },
                },
            //Action = (IItem sender, string[] args, Progress<Event> progress, CancellationTokenSource cts) => { ShowMenu(); },
        });

        /// <summary>
        /// 
        /// </summary>
        private Click Btn_CaptionPin { get; } = new Click(new Command()
        {
            Description = "Pin to front",
            Enabled = true,
            IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>() {

                    { IconType.Normal, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Caption_PinUnset }, } },

                    { IconType.Checked, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Caption_PinSet }, } },

                },
        })
        { Checked = false };

        /// <summary>
        /// 
        /// </summary>
        private Click Btn_CornerOverflow { get; } = new Click(new Command()
        {
            Description = "Overflow Menu",
            Enabled = false,
            IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>() {

                    { IconType.Normal, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Caption_DotPointUp }, } },
                },
        });

        /// <summary>
        /// 
        /// </summary>
        private Rectangle m_captionRect;

        /// <summary>
        /// 
        /// </summary>
        private Rectangle m_captionGrip;

        /// <summary>
        /// 
        /// </summary>
        private Rectangle m_captionRectText;

        /// <summary>
        /// 
        /// </summary>
        private const int c_tabMargin = 3;

        /// <summary>
        /// 
        /// </summary>
        private const int c_iconMargin = 3;

        /// <summary>
        /// 
        /// </summary>
        private const int c_iconSize = 16;

        /// <summary>
        /// TabSize - (TabMargin * 2);
        /// </summary>
        private const int c_textHeight = 18;

        /// <summary>
        /// 
        /// </summary>
        private const int c_minTextWdith = 40;

        /// <summary>
        /// 
        /// </summary>
        public override void Coordinate()
        {
            if (HostPane != null)
            {
                int splitMargin = (Unlocked) ? MosaicForm.PaneGripMargin : 0;

                if (ShowTab)
                {
                    int tabSize = 1;
                    if (Count > 1)
                    {
                        tabSize = DockCanvas.SideTabHeight;
                    }

                    if (this == ((SideDockPane)HostPane).FirstHiddenContainer)
                    {
                        splitMargin = 0;
                    }
                    switch (HostPane.Dock)
                    {
                        case (DockStyle.Left):
                        case (DockStyle.Right):
                            SizeGrip = new Rectangle(0, 0, Width, splitMargin);
                            m_captionRect = new Rectangle(0, SizeGrip.Bottom, Width - 1, DockCanvas.SideCaptionHeight);
                            m_tabPanelBound = new Rectangle(1, m_captionRect.Bottom + 1, Width - 2, Height - DockCanvas.SideCaptionHeight - tabSize - SizeGrip.Height - 1);
                            m_tabBound = new Rectangle(1, m_tabPanelBound.Bottom + 1, Width - 2, tabSize);
                            m_edgeRect = new Rectangle(0, SizeGrip.Bottom, Width - 1, Height - SizeGrip.Height - 1);
                            break;
                        case (DockStyle.Bottom):
                        case (DockStyle.Top):
                        default:
                            SizeGrip = new Rectangle(0, 0, splitMargin, Height);
                            m_captionRect = new Rectangle(SizeGrip.Right, 0, Width - SizeGrip.Width - 1, DockCanvas.SideCaptionHeight);
                            m_tabPanelBound = new Rectangle(SizeGrip.Right + 1, DockCanvas.SideCaptionHeight + 1, Width - SizeGrip.Width - 2, Height - tabSize - DockCanvas.SideCaptionHeight - 1);
                            m_tabBound = new Rectangle(SizeGrip.Right + 1, m_tabPanelBound.Bottom + 1, Width - SizeGrip.Width - 2, tabSize);
                            m_edgeRect = new Rectangle(SizeGrip.Right, 0, Width - SizeGrip.Width - 1, Height - 1);
                            break;
                    }
                    CoordinateTabs();
                    int baseY = m_edgeRect.Bottom - tabSize;
                    Btn_CornerOverflow.Bounds = new Rectangle(Width - 2 - c_iconSize, baseY + 4, c_iconSize, c_iconSize);
                    Btn_CornerOverflow.Enabled = IsOverflow;
                }
                else
                {
                    switch (HostPane.Dock)
                    {
                        case (DockStyle.Left):
                            SizeGrip = new Rectangle(Width - splitMargin, 0, splitMargin, Height);
                            m_captionRect = new Rectangle(0, 0, Width - SizeGrip.Width, DockCanvas.SideCaptionHeight);
                            m_tabPanelBound = new Rectangle(1, m_captionRect.Bottom + 1, Width - SizeGrip.Width - 2, Height - DockCanvas.SideCaptionHeight - 2);
                            m_edgeRect = new Rectangle(0, 0, Width - SizeGrip.Width - 1, Height - 1);
                            break;
                        case (DockStyle.Right):
                            SizeGrip = new Rectangle(0, 0, splitMargin, Height);
                            m_captionRect = new Rectangle(SizeGrip.Right, 0, Width - SizeGrip.Width, DockCanvas.SideCaptionHeight);
                            m_tabPanelBound = new Rectangle(SizeGrip.Right + 1, m_captionRect.Bottom + 1, Width - SizeGrip.Width - 2, Height - DockCanvas.SideCaptionHeight - 2);
                            m_edgeRect = new Rectangle(SizeGrip.Right, 0, Width - SizeGrip.Width - 1, Height - 1);
                            break;
                        case (DockStyle.Bottom):
                            SizeGrip = new Rectangle(0, 0, Width, splitMargin);
                            m_captionRect = new Rectangle(0, SizeGrip.Bottom, Width - 1, DockCanvas.SideCaptionHeight);
                            m_tabPanelBound = new Rectangle(1, m_captionRect.Bottom + 1, Width - 2, Height - DockCanvas.SideCaptionHeight - SizeGrip.Height - 2);
                            m_edgeRect = new Rectangle(0, SizeGrip.Bottom, Width - 1, Height - SizeGrip.Height - 1);
                            break;
                        case (DockStyle.Top):
                        default:
                            SizeGrip = new Rectangle(0, Height - splitMargin, Width, splitMargin);
                            m_captionRect = new Rectangle(0, 0, Width - 1, DockCanvas.SideCaptionHeight);
                            m_tabPanelBound = new Rectangle(1, m_captionRect.Bottom + 1, Width - 2, Height - DockCanvas.SideCaptionHeight - SizeGrip.Height - 2);
                            m_edgeRect = new Rectangle(0, 0, Width - 1, Height - SizeGrip.Height - 1);
                            break;
                    }
                    lock (Tabs)
                        foreach (DockForm df in Tabs)
                        {
                            df.Bounds = m_tabPanelBound;
                        }
                }
                UpdateCaption();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CoordinateTabs()
        {
            bool retry = false;
            int tabSize = 1;
            if (Count > 1)
            {
                tabSize = DockCanvas.SideTabHeight;
            }
            int offset = m_edgeRect.Left;
            int baseY = m_edgeRect.Bottom - tabSize;

            lock (Tabs)
                for (int i = 0; i < Count; i++)
                {
                    DockForm df = (DockForm)Tabs[i];
                    df.Bounds = m_tabPanelBound;
                    int baseOffset = offset;
                    offset += c_tabMargin;

                    int textWidth = df.TabNameWidth + 2;
                    if (textWidth < c_minTextWdith) textWidth = c_minTextWdith;

                    if (df.HasIcon && ShowIcon)
                    {
                        df.IconRect = new Rectangle(offset, baseY + 4, c_iconSize, c_iconSize);
                        offset += c_iconSize;
                    }
                    offset += c_tabMargin - 1;
                    df.TabNameRect = new Rectangle(offset, baseY + c_tabMargin, textWidth, c_textHeight);
                    offset += textWidth;
                    //offset += _tabMargin;
                    df.TabRect = new Rectangle(baseOffset, baseY, offset - baseOffset, tabSize);

                    if (!df.ShowTab)
                    {
                        if (i > 0 && df == ActiveTab)
                        {
                            df.Order -= 2; // df.Order = df.Order - 2;
                            retry = true;
                            break;
                        }
                    }
                }
            m_tabBound.Width = offset - m_edgeRect.Left;
            if (retry)
            {
                Sort();
                CoordinateTabs();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateCaption()
        {
            int captionGripHeight = 8;
            int captionGripYOffset = (DockCanvas.SideCaptionHeight - captionGripHeight) / 2;
            if (ActiveTab != null)
            {
                int captionTextWidth = ActiveTab.TabNameWidth;
                m_captionRectText = new Rectangle(m_captionRect.Left + 3, m_captionRect.Y, captionTextWidth, m_captionRect.Height);
            }
            Btn_CaptionClose.Bounds = new Rectangle(m_captionRect.Right - c_tabMargin - c_iconSize, m_captionRect.Top + c_iconMargin, c_iconSize, c_iconSize);
            Btn_CaptionPin.Bounds = new Rectangle(Btn_CaptionClose.Left - c_iconSize, m_captionRect.Top + c_iconMargin, c_iconSize, c_iconSize);
            Btn_ShowTabContextDropMenu.Bounds = new Rectangle(Btn_CaptionPin.Left - c_iconSize, m_captionRect.Top + c_iconMargin, c_iconSize, c_iconSize);
            m_captionGrip = new Rectangle(m_captionRectText.Right, m_captionRect.Top + captionGripYOffset, m_captionRect.Width - 57 - m_captionRectText.Width, captionGripHeight);
        }

        /// <summary>
        /// 
        /// </summary>
        public override int TabLimit
        {
            get
            {
                return Btn_CornerOverflow.Bounds.Left;
            }
        }
        #endregion

        #region Paint

        /// <summary>
        /// 
        /// </summary>
        public readonly static HatchBrush ActiveCaptionGripBrush = new HatchBrush(HatchStyle.Percent20, ControlPaint.Light(Main.Theme.ActiveCursor.EdgeColor, 0.6f), Main.Theme.ActiveCursor.EdgeColor);// ControlPaint.Light(MoTheme.ActiveCursor.EdgeBrush.Color, 0.2f));
        public readonly static HatchBrush HighlightCaptionGripBrush = new HatchBrush(HatchStyle.Percent20, ControlPaint.Dark(Main.Theme.Highlight.EdgeColor, 0.2f), Main.Theme.Highlight.EdgeColor);
        public readonly static HatchBrush InactiveCaptionGripBrush = new HatchBrush(HatchStyle.Percent20, ControlPaint.Dark(Main.Theme.InactiveCursor.FillColor, 0.2f), Main.Theme.InactiveCursor.FillColor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            if (ActiveTab != null)
            {
                Graphics g = pe.Graphics;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                #region Render Tabs
                if (ShowTab) // Only draw Tabs when the pane is not hidden
                {
                    if (Count > 1)
                    {
                        List<Point> tabLine = new List<Point>() { new Point(m_edgeRect.Left, Height - DockCanvas.SideTabHeight) };
                        lock (Tabs)
                            foreach (DockForm df in Tabs)
                            {
                                if (df.Visible)
                                {
                                    Color backColor = df.BackColor;
                                    using SolidBrush backBrush = new SolidBrush(backColor);
                                    if (DrawTab(g, df, Main.Theme.DimTextBrush, backBrush))
                                    {
                                        Rectangle tabRect = df.TabRect;
                                        tabLine.Add(new Point(tabRect.Left, tabRect.Top + 1));
                                        tabLine.Add(new Point(tabRect.Left, tabRect.Bottom));
                                        tabLine.Add(new Point(tabRect.Right, tabRect.Bottom));
                                        tabLine.Add(new Point(tabRect.Right, tabRect.Top + 1));
                                    }
                                }
                                else if (df.MouseState == MouseState.Hover && MouseState != MouseState.Out)
                                {
                                    DrawTab(g, df, Main.Theme.GrayTextBrush, Main.Theme.Hover.FillBrush);
                                }
                                else
                                {
                                    DrawTab(g, df, Main.Theme.GrayTextBrush, AppTheme.TransparentBrush);
                                }

                            }
                        tabLine.Add(new Point(Width, Height - DockCanvas.SideTabHeight));
                        g.DrawLines(Main.Theme.Panel.EdgePen, tabLine.ToArray());
                        if (Btn_CornerOverflow.Enabled) Btn_CornerOverflow.PaintControl(g, Main.Theme.DimTextBrush.Color);
                    }
                }
                #endregion
                #region RenderTitle
                if (ContainsFocus)
                {
                    if (LayoutState == LayoutStatus.Drag || LayoutState == LayoutStatus.Docking)
                    {
                        if (!ShowTab) g.FillRectangle(Main.Theme.Highlight.EdgeBrush, SizeGrip);
                        g.DrawRectangle(Main.Theme.Highlight.EdgePen, m_edgeRect);
                        g.FillRectangle(Main.Theme.Highlight.EdgeBrush, m_captionRect);
                        g.FillRectangle(HighlightCaptionGripBrush, m_captionGrip);
                        g.FillRectangle(Main.Theme.Highlight.EdgeBrush, m_captionRectText);
                    }
                    else
                    {
                        if (!ShowTab) g.FillRectangle(Main.Theme.SizeGrip.FillBrush, SizeGrip);
                        g.DrawRectangle(Main.Theme.SizeGrip.EdgePen, m_edgeRect);
                        g.FillRectangle(Main.Theme.ActiveCursor.EdgeBrush, m_captionRect);
                        g.FillRectangle(ActiveCaptionGripBrush, m_captionGrip);
                        g.FillRectangle(Main.Theme.ActiveCursor.EdgeBrush, m_captionRectText);
                    }
                    PaintCaptionText(g, Main.Theme.LightTextBrush);
                }
                else
                {
                    if (!ShowTab) g.FillRectangle(Main.Theme.InactiveCursor.FillBrush, SizeGrip);
                    g.DrawRectangle(Main.Theme.Panel.EdgePen, m_edgeRect);
                    g.FillRectangle(Main.Theme.InactiveCursor.FillBrush, m_captionRect);
                    g.FillRectangle(InactiveCaptionGripBrush, m_captionGrip);
                    g.FillRectangle(Main.Theme.InactiveCursor.FillBrush, m_captionRectText);
                    PaintCaptionText(g, Main.Theme.DimTextBrush);
                }
                #endregion
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="br"></param>
        private void PaintCaptionText(Graphics g, SolidBrush br)
        {
            g.DrawString(ActiveTab.TabName, Font, br, m_captionRectText, AppTheme.TextAlignLeft);
            Btn_CaptionClose.PaintControl(g, br.Color);

            /*
            if (!ShowTab)
                Btn_CaptionPin.PaintControl(g, Xu.Properties.Resources.Caption_PinSet, br.Color);
            else
                Btn_CaptionPin.PaintControl(g, Xu.Properties.Resources.Caption_PinUnset, br.Color);
                */

            Btn_CaptionPin.Checked = !ShowTab;
            Btn_CaptionPin.PaintControl(g, br.Color);
            Btn_ShowTabContextDropMenu.PaintControl(g, br.Color);


            //Btn_ShowTabContextDropMenu.PaintControl(g, Xu.Properties.Resources.Caption_PointDown, br.Color);
        }
        #endregion

        #region Mouse

        /// <summary>
        /// 
        /// </summary>
        private MouseState c_captionMouseState = MouseState.Out;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override bool CustomMouseMove(MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            bool GotPt = Btn_CaptionClose.MouseMove(pt) | Btn_CaptionPin.MouseMove(pt) | Btn_CornerOverflow.MouseMove(pt);
            if (!GotPt)
            {
                if (c_captionMouseState != MouseState.Down && c_captionMouseState != MouseState.Drag)
                {
                    c_captionMouseState = m_captionGrip.Contains(pt) ? MouseState.Hover : MouseState.Out;
                }
                else if (c_captionMouseState == MouseState.Down && (!m_captionGrip.Contains(pt)) && e.Button == MouseButtons.Left && DockCanvas.ActiveDockForm != null)
                {
                    DockCanvas.ActiveDockForm.MouseState = MouseState.Drag;
                    LayoutState = LayoutStatus.Docking;
                    ObsoletedEvent.Debug("Start docking by caption");
                    // Start Dock here;
                    DockCanvas.StartDock();
                    GotPt = true;
                }
            }
            Invalidate();
            return GotPt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override bool CustomMouseDown(MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            bool GotPt = Btn_CaptionClose.MouseDown(pt, e.Button) | Btn_CaptionPin.MouseDown(pt, e.Button) | Btn_CornerOverflow.MouseDown(pt, e.Button);
            if (!GotPt)
            {
                if (c_captionMouseState != MouseState.Down && c_captionMouseState != MouseState.Drag)
                {
                    c_captionMouseState = m_captionGrip.Contains(pt) ? MouseState.Down : MouseState.Out;
                }
                ObsoletedEvent.Debug("_captionMouseState = " + c_captionMouseState.ToString());
            }
            Invalidate();
            return GotPt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override bool CustomMouseUp(MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            if (Btn_CornerOverflow.MouseUp(pt, e.Button))
            {
                Invalidate();
                Menu.Clear();
                Menu.AddRange(GetTabsMenuItems());
                Point StartPt = PointToScreen(new Point(Width - Menu.Width, m_tabBound.Top - Menu.Height));
                Menu.Show(StartPt);
            }
            if (Btn_CaptionClose.MouseUp(pt, e.Button))
            {
                Invalidate();
                if (ActiveTab != null) ActiveTab.Close();
                return true;
            }
            if (Btn_CaptionPin.MouseUp(pt, e.Button))
            {
                Invalidate();
                if (ShowTab) ((SideDockPane)HostPane).Hide(this);
                else ((SideDockPane)HostPane).Unhide(this);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void CustomMouseLeave()
        {
            base.CustomMouseLeave();
            Btn_CaptionClose.MouseLeave();
            Btn_CaptionPin.MouseLeave();
            Btn_CornerOverflow.MouseLeave();
            c_captionMouseState = MouseState.Out;
        }
        #endregion
    }
}
