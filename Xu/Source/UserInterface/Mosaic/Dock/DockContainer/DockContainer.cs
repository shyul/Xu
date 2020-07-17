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
using System.Threading;
using System.Windows.Forms;

namespace Xu
{
    [DesignerCategory("Code")]
    public abstract class DockContainer : Tab, IResizable
    {
        #region Ctor

        /// <summary>
        /// 
        /// </summary>
        protected DockContainer() : base()
        {
            // Components
            Btn_ShowTabContextDropMenu.Command.Action =
                (IObject sender, string[] args, Progress<Event> progress, CancellationTokenSource cts) =>
                {
                    ShowMenu();
                };
        }
        #endregion

        #region Components

        public DockPane HostDockPane { get; private set; }

        public DockCanvas DockCanvas => HostDockPane is DockPane dp ? dp.DockCanvas : null;

        protected DockForm ActiveDockForm => ActiveTab is DockForm df ? df : null;

        public abstract bool IsRoot { get; }

        public override void ActivateTab(int index)
        {
            base.ActivateTab(index);
            if (ShowTab) CoordinateTabs();
            UpdateGraphics();
        }

        protected override void UpdateGraphics() => DockCanvas?.Invalidate(true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(EventArgs e)
        {
            DockCanvas.ActiveContainer = this;
            base.OnGotFocus(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (!(Parent is null))
            {
                //if ((typeof(DockPane)).IsAssignableFrom(Parent.GetType()))
                if (Parent is DockPane dkp)
                {
                    HostDockPane = dkp; // (DockPane)Parent;
                }
                // else if ((typeof(DockCanvas)).IsAssignableFrom(Parent.GetType()))
                else if (Parent is DockCanvas)// dkc)
                {
                    //Log.Debug("Containers is assigned to Mosaic now!");
                    HostDockPane = null;
                }
                else
                    throw new Exception("DockContainer can only be exsiting in DockPane / Parent: " + Parent.GetType().ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Order
        {
            get
            {
                return m_order;
            }
            set
            {
                m_order = value;
                lock (HostDockPane.DockContainers)
                {
                    HostDockPane.Sort();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="od"></param>
        public void SetOrder(int od)
        {
            m_order = od;
        }

        /// <summary>
        /// 1. Order will also play as the index in the Pane's ContainerList
        /// </summary>
        protected int m_order = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="side"></param>
        /// <param name="df"></param>
        public abstract void AddForm(DockStyle side, DockForm df);

        /// <summary>
        /// This is when it runs merge/promote the check
        /// </summary>
        /// <param name="df"></param>
        public virtual void RemoveForm(DockForm df)
        {
            if (Remove(df))
            {
                if (IsEmpty) Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsEmpty => (Count == 0);

        /// <summary>
        /// 
        /// </summary>
        public virtual void Close()
        {
            if (IsEmpty)
                HostDockPane.RemoveContainer(this);
            else
                throw new Exception("Can not close an non-zero container.");
        }

        #region Control

        /// <summary>
        /// 
        /// </summary>
        protected Click Btn_ShowTabContextDropMenu { get; set; } = new Click(new Command()
        {
            Description = "Tab Menu",
            IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>() {

                    { IconType.Normal, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Caption_PointDown }, } },

                    { IconType.Checked, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Caption_DotPointDown }, } },

                    { IconType.CheckedHover, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Caption_DotPointDown }, } },

                    { IconType.CheckedDown, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Caption_DotPointDown }, } },

                },
            //Action = (IItem sender, string[] args, Progress<Event> progress, CancellationTokenSource cts) => { ShowMenu(); },
        })
        { Checked = false };

        /// <summary>
        /// 
        /// </summary>
        protected virtual ContextDropMenu Menu { get { return MosaicForm.ContextDropMenu; } }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual ToolStripMenuItem[] GetTabsMenuItems()
        {
            List<ToolStripMenuItem> ts = new List<ToolStripMenuItem>();
            lock (Tabs)
                for (int i = 0; i < Count; i++)
                {
                    DockForm df = (DockForm)Tabs[i];
                    ToolStripMenuItem t = new ToolStripMenuItem()
                    {
                        Name = "ContainerMenuItem_" + i.ToString(),
                        Text = df.TabName,
                    };
                    if (df.HasIcon && df.Icon != null) t.Image = df.Icon;
                    int idx = i;
                    t.Click += new EventHandler((object sender, EventArgs e) =>
                    {
                        //Log.Debug("Menu Activate: " + df.TabName);
                        ActivateTab(idx);
                    });
                    ts.Add(t);
                }
            return ts.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ConfigDropMenu()
        {
            Menu.Clear();

            Menu.AddRange(GetTabsMenuItems());
            Menu.AddSeparator();

            // Custom controls are here.


        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ShowMenu()
        {
            ConfigDropMenu();
            Point StartPt = PointToScreen(new Point(Width - Menu.Width, Btn_ShowTabContextDropMenu.DropMenuOriginPoint.Y));
            Menu.Show(StartPt);
        }
        #endregion

        #endregion

        #region Layout

        protected LayoutStatus LayoutState = LayoutStatus.Idle;

        #region Resizing
        public double Ratio { get; private set; } = 1.0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public void ChangeSize(int size)
        {
            if (Order >= 1)
            {
                DockContainer adjacentContainer = HostDockPane.DockContainers[Order - 1];
                double r1 = adjacentContainer.Ratio;
                double r2 = Ratio;
                switch (LayoutType)
                {
                    case (LayoutType.Vertical):
                        double h1 = adjacentContainer.Height;
                        double h2 = Height;
                        adjacentContainer.Ratio = (r1 + r2) * (h1 + size) / (h1 + h2);
                        Ratio = r1 + r2 - adjacentContainer.Ratio;
                        break;
                    case (LayoutType.Horizontal):
                        double w1 = adjacentContainer.Width;
                        double w2 = Width;
                        adjacentContainer.Ratio = (r1 + r2) * (w1 + size) / (w1 + w2);
                        Ratio = r1 + r2 - adjacentContainer.Ratio;
                        break;
                    default:
                        break;
                }
                if (Ratio < 0)
                {
                    Ratio = 0;
                    adjacentContainer.Ratio = r1 + r2;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool Unlocked => DockCanvas.Unlocked;

        /// <summary>
        /// 
        /// </summary>
        public virtual LayoutType LayoutType => HostDockPane.LayoutType;

        /// <summary>
        /// 
        /// </summary>
        public virtual bool HasSizeGrip
        {
            get
            {
                if (HostDockPane != null && HostDockPane.Count > 0)
                    return (Unlocked & (this != HostDockPane.DockContainers[0]));
                else
                    return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Rectangle SizeGrip { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Control Area { get { return this; } }

        /// <summary>
        /// 
        /// </summary>
        public virtual Control Canvas { get { if (Parent != null) return Parent; else return this; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public virtual void OnGetSize(int size) { }
        #endregion

        #region Docking

        /// <summary>
        /// 
        /// </summary>
        public virtual LayoutType AllowedDockLayout { get; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Rectangle DockEdge
        {
            get
            {
                return new Rectangle(PointToScreen(m_edgeRect.Location), m_edgeRect.Size);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Point DockCenter
        {
            get
            {
                Point ct = new Point(m_edgeRect.X + m_edgeRect.Width / 2, m_edgeRect.Y + m_edgeRect.Height / 2);
                return PointToScreen(ct);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Rectangle Dock_C { get; private set; }
        public Rectangle Dock_T { get; private set; }
        public Rectangle Dock_B { get; private set; }
        public Rectangle Dock_L { get; private set; }
        public Rectangle Dock_R { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public void GetDockRect()
        {
            Point ct = DockCenter;
            if (!Contains(DockCanvas.ActiveDockForm) && ((int)AllowedDockLayout & 0x1) == 0x1)
            {
                Dock_C = new Rectangle(ct.X - 15, ct.Y - 15, 31, 31);
            }
            else
            {
                Dock_C = Rectangle.Empty;
            }

            if ((((int)AllowedDockLayout >> 1) & 0x1) == 0x1)
            {
                Dock_T = new Rectangle(ct.X - 15, ct.Y - 46, 31, 31);
                Dock_B = new Rectangle(ct.X - 15, ct.Y + 16, 31, 31);
            }
            else
            {
                Dock_T = Rectangle.Empty;
                Dock_B = Rectangle.Empty;
            }
            if ((((int)AllowedDockLayout >> 2) & 0x1) == 0x1)
            {
                Dock_L = new Rectangle(ct.X - 46, ct.Y - 15, 31, 31);
                Dock_R = new Rectangle(ct.X + 16, ct.Y - 15, 31, 31);
            }
            else
            {
                Dock_L = Rectangle.Empty;
                Dock_R = Rectangle.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void FinishDock()
        {
            DockStyle side = DockCanvas.FinishDock();
            ObsoletedEvent.Debug("Finish Docking, side: " + side);
            if (DockCanvas.NextContainer != null)
            {
                DockCanvas.NextContainer.AddForm(side, DockCanvas.ActiveDockForm);
                DockCanvas.NextContainer = null;
            }
        }

        #endregion

        #region TabDrag

        /// <summary>
        /// 
        /// </summary>
        protected int ActiveFormNewOrder = 0;

        #endregion

        #endregion

        #region Coordinate

        /// <summary>
        /// 
        /// </summary>
        private const int c_tabMargin = 3;

        /// <summary>
        /// 
        /// </summary>
        private const int c_iconMargin = 3;// (TabSize - IconSize) / 2;

        /// <summary>
        /// 
        /// </summary>
        private const int c_iconSize = 16;

        /// <summary>
        /// 
        /// </summary>
        private const int c_textHeight = 18 - 3; // TabSize - (TabMargin * 2);

        /// <summary>
        /// 
        /// </summary>
        private const int c_minTextWdith = 35;

        /// <summary>
        /// 
        /// </summary>
        public virtual void CoordinateTabs()
        {
            bool retry = false;
            int offset = m_edgeRect.Left;
            lock (Tabs)
            {
                for (int i = 0; i < Count; i++)
                {
                    DockForm df = (DockForm)Tabs[i];
                    //df.Bounds = _tabPanelBound;
                    int baseOffset = offset;
                    offset += c_tabMargin;
                    int textWidth = df.TabNameWidth;
                    if (textWidth < c_minTextWdith) textWidth = c_minTextWdith;

                    if (df.HasIcon && ShowIcon)
                    {
                        df.IconRect = new Rectangle(offset, c_iconMargin + m_edgeRect.Top, c_iconSize, c_iconSize);
                        offset += c_iconSize;
                    }
                    df.TabNameRect = new Rectangle(offset, c_tabMargin + m_edgeRect.Top, textWidth, c_textHeight);
                    offset += textWidth;
                    if (df.Btn_Pin.Enabled && ShowPin)
                    {
                        df.Btn_Pin.Bounds = new Rectangle(offset, c_iconMargin + m_edgeRect.Top, c_iconSize, c_iconSize);
                        offset += c_iconSize;
                    }
                    if (df.Btn_Close.Enabled && ShowClose)
                    {
                        df.Btn_Close.Bounds = new Rectangle(offset, c_iconMargin + m_edgeRect.Top, c_iconSize, c_iconSize);
                        offset += c_iconSize;
                    }
                    offset += c_tabMargin;
                    df.TabRect = new Rectangle(baseOffset, m_edgeRect.Top, offset - baseOffset, DockCanvas.GridTabHeight);

                    if (!df.ShowTab)
                    {
                        if (i > 0 && df == ActiveTab)
                        {
                            if (HasPin) df.IsPinned = true;
                            df.Order -= 2; // df.Order = df.Order - 2;
                            retry = true;
                            break;
                        }
                    }
                }
            }
            m_tabBound.Width = offset - m_edgeRect.Left;

            if (retry)
            {
                int activeTabOrder = ActiveTab.Order;
                lock (Tabs)
                    Sort();
                if (ActiveTab.Order != activeTabOrder)
                    CoordinateTabs();
            }
            if (ActiveTab != null)
            {
                ActiveTab.Bounds = m_tabPanelBound;
                ActiveTab.Coordinate();
            }
        }

        #endregion

        #region Mouse

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            if (Unlocked)
            {
                switch (LayoutState)
                {
                    case (LayoutStatus.Drag):
                        if (m_tabBound.Contains(pt))
                        {
                            lock (Tabs)
                            {
                                foreach (DockForm df in Tabs)
                                {
                                    Rectangle tabRect = df.TabRect;
                                    if (tabRect.Contains(pt))
                                    {
                                        ActiveFormNewOrder = df.Order;
                                        if (df != ActiveDockForm) df.MouseState = MouseState.Hover;
                                    }
                                    else
                                    {
                                        if (df != ActiveDockForm) df.MouseState = MouseState.Out;
                                    }
                                }
                                Invalidate();
                            }
                        }
                        else
                        {
                            ActiveFormNewOrder = 0;
                            LayoutState = LayoutStatus.Docking;
                            ObsoletedEvent.Debug("Start Docking");
                            // Start Dock here;
                            DockCanvas.StartDock();
                        }
                        return;
                    case (LayoutStatus.Docking):
                        if (m_tabBound.Contains(pt))
                        {
                            LayoutState = LayoutStatus.Drag;
                            ObsoletedEvent.Debug("Start Tab Drag");
                            DockCanvas.FinishDock();
                        }
                        else
                        {
                            if (e.Button == MouseButtons.Left)
                                // Work on Dock here;
                                DockCanvas.RefreshDock(DockCanvas); // pt, 
                            else
                            {
                                DockCanvas.FinishDock();
                                LayoutState = LayoutStatus.Idle;
                            }
                        }
                        Invalidate();
                        return;
                    case (LayoutStatus.Resizing):
                        if (MouseState == MouseState.Drag)
                        {
                            DockCanvas.RefreshResize();
                        }
                        else if (!SizeGrip.Contains(pt))
                        {
                            Cursor.Current = Cursors.Default;
                            LayoutState = LayoutStatus.Idle;
                            ObsoletedEvent.Debug("Stop Resizing");
                        }
                        return;
                    default:
                        if (SizeGrip.Contains(pt))
                        {
                            switch (LayoutType)
                            {
                                case (LayoutType.Horizontal):
                                    Cursor.Current = Cursors.SizeWE;
                                    break;
                                case (LayoutType.Vertical):
                                    Cursor.Current = Cursors.SizeNS;
                                    break;
                                default:
                                    break;
                            }
                            return;
                        }
                        Btn_ShowTabContextDropMenu.MouseMove(pt);
                        if (CustomMouseMove(e)) return;
                        else if (ShowTab)
                        {
                            Cursor.Current = Cursors.Default;
                            MouseState = MouseState.Hover;
                            lock (Tabs)
                            {
                                foreach (DockForm df in Tabs)
                                {
                                    Rectangle tabRect = df.TabRect;
                                    if (df.MouseState != MouseState.Drag && df.MouseState != MouseState.Down && df.ShowTab)
                                    {
                                        df.MouseState = (tabRect.Contains(pt)) ? MouseState.Hover : MouseState.Out;
                                        if (ShowClose) df.Btn_Close.MouseMove(pt);
                                        if (ShowPin) df.Btn_Pin.MouseMove(pt);
                                    }
                                    else if (df.MouseState == MouseState.Down
                                        && !(ShowClose && df.Btn_Close.IsHot)
                                        && !(ShowPin && df.Btn_Pin.IsHot)
                                        && e.Button == MouseButtons.Left) // Prevent draging when the button is pressed.
                                    {
                                        if (!m_tabBound.Contains(pt))
                                        {
                                            df.MouseState = MouseState.Drag;
                                            LayoutState = LayoutStatus.Docking;
                                            ObsoletedEvent.Debug("Start Docking"); // Start Dock here;
                                            DockCanvas.StartDock();
                                        }
                                        else if (!tabRect.Contains(pt))
                                        {
                                            df.MouseState = MouseState.Drag;
                                            LayoutState = LayoutStatus.Drag;
                                            ObsoletedEvent.Debug("Start Tab Drag");
                                        }
                                    }
                                }
                            }
                            Invalidate();
                        }
                        return;
                }
            }
            else
            {
                if (!CustomMouseMove(e)) base.OnMouseMove(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            DockCanvas.ActiveContainer = this;
            if (!ShowTab)
            {
                Focus();
                UpdateGraphics();
            }
            Point pt = new Point(e.X, e.Y);
            if (Unlocked && SizeGrip.Contains(pt))
            {
                MouseState = MouseState.Drag;
                LayoutState = LayoutStatus.Resizing;
                ObsoletedEvent.Debug("Start Resizing");
                DockCanvas.StartResize(this);
            }
            else if (LayoutState == LayoutStatus.Idle)
            {
                Btn_ShowTabContextDropMenu.MouseDown(pt, e.Button);
                if (!CustomMouseDown(e)) base.OnMouseDown(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            switch (LayoutState)
            {
                case (LayoutStatus.Drag):
                    if (ActiveDockForm != null)
                    {
                        lock (Tabs)
                        {
                            if (ActiveDockForm.Order > ActiveFormNewOrder) ActiveFormNewOrder--;
                            else if (ActiveDockForm.Order < ActiveFormNewOrder) ActiveFormNewOrder++;
                            ObsoletedEvent.Debug("Old: " + ActiveDockForm.Order + " New: " + ActiveFormNewOrder);
                            ActiveDockForm.Order = ActiveFormNewOrder;
                            Sort();
                            Coordinate();
                        }
                    }
                    break;
                case (LayoutStatus.Docking):
                    FinishDock();
                    break;
                case (LayoutStatus.Resizing):
                    DockCanvas.FinishResize();
                    break;
            }
            LayoutState = LayoutStatus.Idle;
            if (Btn_ShowTabContextDropMenu.MouseUp(pt, e.Button))
            {
                //ShowMenu(); , now handled by action inside the Click
            }
            CustomMouseUp(e);
            base.OnMouseUp(e);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void CustomMouseLeave()
        {
            Btn_ShowTabContextDropMenu.MouseLeave();
        }

        #endregion
    }
}