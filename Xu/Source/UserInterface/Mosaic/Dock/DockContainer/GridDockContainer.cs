/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Docking Multi-Panel Control
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Xu
{
    [DesignerCategory("Code")]
    public sealed class GridDockContainer : DockContainer
    {
        #region Ctor

        /// <summary>
        /// 
        /// </summary>
        public GridDockContainer() : base()
        {
            SubPane = null;
            ResumeLayout(true);
            PerformLayout();
        }

        #endregion

        #region Components

        /// <summary>
        /// 
        /// </summary>
        public GridDockPane SubPane { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsRoot
        {
            get
            {
                //if (HostPane is null)
                //    return false;
                //else 
                if (HostPane is GridDockPane gdp)
                    return gdp.IsRoot;
                else
                    return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private GridDockContainer EnableSubPane(int i) // Push all tabs into the SubPane
        {
            LayoutType type = LayoutType.Horizontal;
            if (LayoutType == LayoutType.Horizontal) type = LayoutType.Vertical;
            SubPane = new GridDockPane() { LayoutType = type };
            SubPane.SuspendLayout();
            Controls.Add(SubPane);
            GridDockContainer dc_new = new GridDockContainer();
            GridDockContainer dc_temp = new GridDockContainer();
            dc_temp.AddRange(this);
            lock (SubPane.DockContainers)
            {
                SubPane.DockContainers.Add(dc_temp);
                SubPane.Controls.Add(dc_temp);
                SubPane.DockContainers.Insert(i, dc_new);
                SubPane.Controls.Add(dc_new);
                SubPane.Reorder();
            }
            Coordinate();
            SubPane.Coordinate();
            SubPane.Visible = true;
            SubPane.ResumeLayout(true);
            return dc_new;
        }

        /// <summary>
        /// 
        /// </summary>
        public void DisableSubPane()
        {
            if (SubPane.Count > 1) throw new Exception("Only when the SubPane with one containers can be disposed");

            if (SubPane.Count == 1) // SubPane Count has to be only one
            {
                GridDockContainer dc_only = (GridDockContainer)SubPane.DockContainers[0];
                if (dc_only.SubPane == null) // If the container is just tabs, move up.
                {
                    AddRange(dc_only);
                }
                else // If the only container is using SubPane, transfer up and merge
                {
                    ((GridDockPane)HostPane).InsertRange(Order, dc_only.SubPane);
                    dc_only.SubPane.Dispose();
                }
                dc_only.Dispose();
                SubPane.Dispose();
                while (SubPane.Disposing) ;
                SubPane = null;
                Coordinate();
                // If SubPane is disabled and the tab form count is still zero, then dispose this container as well.
                if (Count == 0) Close();
            }

            if (HostPane != null) HostPane.CleanUp();
        }

        /// <summary>
        /// 1. Only happens when SubPane is null!!!
        /// </summary>
        /// <param name="side"></param>
        /// <param name="df"></param>
        public override void AddForm(DockStyle side, DockForm df)
        {
            if (SubPane != null) throw new Exception("Can not add DockForm directly to a container with SubPane enabled, check how do you even get this container!");
            if (side == DockStyle.None) return;

            df.MouseState = MouseState.Out;
            df.Dock = DockStyle.None;
            df.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

            DockContainer original_dc = null;
            bool dc_valid_remove = false;

            if (df.HostContainer != null)
            {
                original_dc = (DockContainer)df.HostContainer;
                dc_valid_remove = original_dc.Remove(df);
                if ((typeof(SideDockContainer)).IsAssignableFrom(original_dc.GetType()) && !original_dc.ShowTab)
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
                GridDockContainer dc_new;
                if (HostPane.Count <= 1) // There is no way HostPane's Count could be 0 here.
                {
                    switch (side)
                    {
                        case (DockStyle.Top):
                            HostPane.LayoutType = LayoutType.Vertical;
                            dc_new = ((GridDockPane)HostPane).CreateContainer(0);
                            break;
                        case (DockStyle.Bottom):
                            HostPane.LayoutType = LayoutType.Vertical;
                            dc_new = ((GridDockPane)HostPane).CreateContainer(1);
                            break;
                        case (DockStyle.Left):
                            HostPane.LayoutType = LayoutType.Horizontal;
                            dc_new = ((GridDockPane)HostPane).CreateContainer(0);
                            break;
                        case (DockStyle.Right):
                        default:
                            HostPane.LayoutType = LayoutType.Horizontal;
                            dc_new = ((GridDockPane)HostPane).CreateContainer(1);
                            break;
                    }
                }
                else
                {
                    switch (LayoutType)
                    {
                        case (LayoutType.Vertical):
                            switch (side)
                            {
                                case (DockStyle.Top):
                                    dc_new = ((GridDockPane)HostPane).CreateContainer(Order);
                                    break;
                                case (DockStyle.Bottom):
                                    dc_new = ((GridDockPane)HostPane).CreateContainer(Order + 1);
                                    break;
                                case (DockStyle.Left):
                                    dc_new = EnableSubPane(0);
                                    break;
                                case (DockStyle.Right):
                                default:
                                    dc_new = EnableSubPane(1);
                                    break;
                            }
                            break;
                        case (LayoutType.Horizontal):
                        default:
                            switch (side)
                            {
                                case (DockStyle.Top):
                                    dc_new = EnableSubPane(0);
                                    break;
                                case (DockStyle.Bottom):
                                    dc_new = EnableSubPane(1);
                                    break;
                                case (DockStyle.Left):
                                    dc_new = ((GridDockPane)HostPane).CreateContainer(Order);
                                    break;
                                case (DockStyle.Right):
                                default:
                                    dc_new = ((GridDockPane)HostPane).CreateContainer(Order + 1);
                                    break;
                            }
                            break;
                    }
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
        /// This is when it runs merge/promote the check
        /// </summary>
        /// <param name="df"></param>
        public override void RemoveForm(DockForm df)
        {
            if (SubPane != null) throw new Exception("Can not remove DockForm from a container with SubPane enabled, check how do you even get this container!");
            if (Remove(df))
            {
                if (IsEmpty) Close();
            }
        }

        /// <summary>
        /// Check if this DockContainer is empty.
        /// </summary>
        public override bool IsEmpty => Count == 0 && SubPane == null;

        #endregion

        #region Docking

        /// <summary>
        /// 
        /// </summary>
        public override LayoutType AllowedDockLayout
        {
            get { return LayoutType.Horizontal | LayoutType.Vertical | LayoutType.OverLay; }
        }
        #endregion

        #region Resizing

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public override void OnGetSize(int size)
        {
            ChangeSize(size);
            HostPane.Coordinate();
            Point ct = new Point((SizeGrip.Left + SizeGrip.Right) / 2, (SizeGrip.Top + SizeGrip.Bottom) / 2);
            ct = PointToScreen(ct);
            switch (LayoutType)
            {
                case (LayoutType.Vertical):
                    Cursor.Position = new Point(Cursor.Position.X, ct.Y);
                    break;
                case (LayoutType.Horizontal):
                default:
                    Cursor.Position = new Point(ct.X, Cursor.Position.Y);
                    break;
            }
        }
        #endregion

        #region Coordinate

        /// <summary>
        /// 
        /// </summary>
        public override int TabLimit => Btn_ShowTabContextDropMenu.Bounds.Left;

        /// <summary>
        /// 
        /// </summary>
        public override void Coordinate()
        {
            int splitMargin = MosaicForm.PaneGripMargin;
            if (SubPane != null)
            {
                if (HasSizeGrip)
                {
                    switch (LayoutType)
                    {
                        case (LayoutType.Vertical):
                            SizeGrip = new Rectangle(0, 0, Width, splitMargin);
                            m_edgeRect = new Rectangle(0, splitMargin, Width, Height - splitMargin);
                            break;
                        case (LayoutType.Horizontal):
                        default:
                            SizeGrip = new Rectangle(0, 0, splitMargin, Height);
                            m_edgeRect = new Rectangle(splitMargin, 0, Width - splitMargin, Height);
                            break;
                    }
                }
                else
                {
                    SizeGrip = Rectangle.Empty;
                    m_edgeRect = new Rectangle(0, 0, Width, Height);
                }
                SubPane.Bounds = m_edgeRect;
            }
            else
            {
                int tabSize = DockCanvas.GridTabHeight;
                if (HasSizeGrip)
                {
                    switch (LayoutType)
                    {
                        case (LayoutType.Vertical):
                            SizeGrip = new Rectangle(0, 0, Width, splitMargin);
                            m_tabBound = new Rectangle(0, splitMargin, Width, tabSize);
                            m_tabPanelBound = new Rectangle(1, tabSize + splitMargin, Width - 2, Height - tabSize - splitMargin - 1);
                            m_captionStripRect = new Rectangle(0, tabSize + splitMargin - 3, Width, 3);
                            Btn_ShowTabContextDropMenu.Bounds = new Rectangle(Width - 19, splitMargin + 3, 16, 16);
                            m_edgeRect = new Rectangle(0, splitMargin, Width, Height - splitMargin);
                            break;
                        case (LayoutType.Horizontal):
                        default:
                            SizeGrip = new Rectangle(0, 0, splitMargin, Height);
                            m_tabBound = new Rectangle(splitMargin, 0, Width - splitMargin, tabSize);
                            m_tabPanelBound = new Rectangle(splitMargin + 1, tabSize, Width - splitMargin - 2, Height - tabSize - 1);
                            m_captionStripRect = new Rectangle(splitMargin, tabSize - 3, Width - splitMargin, 3);
                            Btn_ShowTabContextDropMenu.Bounds = new Rectangle(Width - 19, 3, 16, 16);
                            m_edgeRect = new Rectangle(splitMargin, 0, Width - splitMargin, Height);
                            break;
                    }
                }
                else
                {
                    SizeGrip = Rectangle.Empty;
                    m_tabBound = new Rectangle(0, 0, Width, tabSize);
                    m_tabPanelBound = new Rectangle(1, tabSize, Width - 2, Height - tabSize - 1);
                    m_captionStripRect = new Rectangle(0, tabSize - 3, Width, 3);
                    Btn_ShowTabContextDropMenu.Bounds = new Rectangle(Width - 19, 3, 16, 16);
                    m_edgeRect = new Rectangle(0, 0, Width, Height);
                }
                CoordinateTabs();
            }
        }
        #endregion

        #region Paint
        private Rectangle m_captionStripRect;
        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            bool isOverFlow = false;
            //g.FillRectangle(new SolidBrush(Color.Red), _sizeGrip);
            if (Count > 0 && SubPane == null)
            {
                g.DrawRectangle(Main.Theme.Panel.EdgePen, new Rectangle(m_edgeRect.X, m_edgeRect.Y, m_edgeRect.Width - 1, m_edgeRect.Height - 1));

                Brush _captionStripBrush = Main.Theme.InactiveCursor.EdgeBrush;
                lock (Tabs)
                    for (int i = 0; i < Count; i++)
                    {
                        DockForm df = (DockForm)Tabs[i];
                        if (!df.ShowTab) isOverFlow = true;
                        if (df.ShowTab || df.Order == 0)
                            if (df == ActiveTab)
                            {
                                if (df.MouseState == MouseState.Drag)
                                {
                                    _captionStripBrush = Main.Theme.Highlight.EdgeBrush;
                                    DrawTab(g, df, Main.Theme.LightTextBrush, Main.Theme.Highlight.EdgeBrush);
                                }
                                else if (df.ContainsFocus)
                                {
                                    _captionStripBrush = Main.Theme.ActiveCursor.EdgeBrush;
                                    DrawTab(g, df, Main.Theme.LightTextBrush, Main.Theme.ActiveCursor.EdgeBrush);
                                }
                                else
                                {
                                    try
                                    {
                                        DrawTab(g, df, Main.Theme.LightTextBrush, Main.Theme.InactiveCursor.EdgeBrush);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("$$$$$$$ Do not draw GridDockContainer if it is not shown!!! " + e.Message);
                                    }
                                }
                                
                            }
                            else
                            {
                                if (df.MouseState == MouseState.Hover && MouseState != MouseState.Out)
                                {
                                    DrawTab(g, df, Main.Theme.LightTextBrush, Main.Theme.Hover.EdgeBrush);
                                }
                                else
                                {
                                    if (df.HasIcon && ShowIcon)
                                        g.DrawIcon(df.Icon, df.IconRect, Main.Theme.DimTextBrush.Color);

                                    //Rectangle tabNameRect = df.TabNameRect;
                                    //g.DrawString(df.TabName, df.Font, MoTheme.DimTextBrush, new Rectangle(tabNameRect.X, tabNameRect.Y, tabNameRect.Width, tabNameRect.Height- 2), TabTextFormat);
                                    g.DrawString(df.TabName, df.Font, Main.Theme.DimTextBrush, df.TabNameRect, AppTheme.TextAlignLeft);

                                    if (ShowPin && df.Btn_Pin.Enabled && df.IsPinned)
                                    {
                                        //df.Btn_Pin.Checked = df.IsPinned;
                                        df.Btn_Pin.PaintControl(g, Main.Theme.InactiveCursor.EdgeColor);
                                        //df.Btn_Pin.PaintControl(g, Xu.Properties.Resources.Caption_PinSet, Main.Theme.InactiveCursor.EdgeColor);
                                    }

                                }
                            }
                    }
                g.FillRectangle(_captionStripBrush, m_captionStripRect);

                Btn_ShowTabContextDropMenu.Checked = isOverFlow;
                Btn_ShowTabContextDropMenu.PaintControl(g, Main.Theme.DimTextBrush.Color);
                /*
                // Drop down menu button related
                if (isOverFlow)
                {
                    Btn_ShowTabContextDropMenu.PaintControl(g, Properties.Resources.Caption_DotPointDown, Main.Theme.DimTextBrush.Color);
                }
                else
                {
                    Btn_ShowTabContextDropMenu.PaintControl(g, Properties.Resources.Caption_PointDown, Main.Theme.DimTextBrush.Color);
                }*/
            }
            // if(SubPane != null) g.FillRectangle(new SolidBrush(Color.Green), _edgeRect);


        }

        #endregion
    }
}
