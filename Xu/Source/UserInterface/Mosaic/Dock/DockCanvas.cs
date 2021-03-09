/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Docking Multi-Panel Control
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Xu
{
    [DesignerCategory("Code")]
    public class DockCanvas : UserControl
    {
        #region Geometry
        public static int GridTabHeight = 22;
        public static int SideTabHeight = 22;
        public static int SideHiddenTabHeight = 25;
        public static int SideCaptionHeight = 22;
        public static int SideToolStripHeight = 24;
        #endregion

        #region Ctor
        public DockCanvas()
        {
            // Components
            components = new Container();

            // Control Settings
            SuspendLayout();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left;
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            Text = string.Empty;
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            ClientSize = new Size(1600, 1100);
            BackColor = Main.Theme.Panel.FillColor;
            // Add Controls
            Controls.Add(CenterDockPane = new GridDockPane());
            Controls.Add(LeftDockPane = new SideDockPane(DockStyle.Left));
            Controls.Add(RightDockPane = new SideDockPane(DockStyle.Right));
            Controls.Add(BottomDockPane = new SideDockPane(DockStyle.Bottom));
            Controls.Add(TopDockPane = new SideDockPane(DockStyle.Top));
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

        protected SideDockPane LeftDockPane { get; set; }
        protected SideDockPane RightDockPane { get; set; }
        protected SideDockPane BottomDockPane { get; set; }
        protected SideDockPane TopDockPane { get; set; }
        protected GridDockPane CenterDockPane { get; set; }

        public void AddForm(DockStyle postion, int index, DockForm df)
        {
            if (InvokeRequired)
            {
                this?.Invoke(() =>
                {
                    M_AddForm(postion, index, df);
                });
                /*
                Invoke((MethodInvoker)delegate
                {
                    DockControl.AddForm(postion, index, df);
                });*/
            }
            else
            {
                M_AddForm(postion, index, df);
            }
        }

        protected void M_AddForm(DockStyle postion, int index, DockForm df)
        {
            switch (postion)
            {
                case (DockStyle.Top):
                    TopDockPane.AddForm(index, df);
                    break;
                case (DockStyle.Bottom):
                    BottomDockPane.AddForm(index, df);
                    break;
                case (DockStyle.Left):
                    LeftDockPane.AddForm(index, df);
                    break;
                case (DockStyle.Right):
                    RightDockPane.AddForm(index, df);
                    break;
                default:
                    CenterDockPane.AddForm(index, df);
                    break;
            }
        }

        #endregion

        #region Layout
        public static bool Unlocked { get; set; } = true;
        /*
        {
            get { return m_unlocked; }
            set
            {
                m_unlocked = value;
            }
        }
        private static bool m_unlocked = true;*/
        public static bool IsMenuFadeEnabled => SystemInformation.IsMenuAnimationEnabled && SystemInformation.IsMenuFadeEnabled;

        private static HelperOverlay HelperOverlay { get; set; }

        private static void DisposeHf()
        {
            if (HelperOverlay != null)
            {
                lock (HelperOverlay)
                {
                    HelperOverlay.Close();
                    HelperOverlay.Dispose();
                    while (HelperOverlay.Disposing) ;
                    HelperOverlay = null;
                }
            }
            if (ActiveFormPic != null)
            {
                lock (ActiveFormPic)
                {
                    ActiveFormPic.Dispose();
                }
                ActiveFormPic = null;
            }
        }
        public static DockForm ActiveDockForm
        {
            get
            {
                return m_activeDockForm;
            }
            set
            {
                m_activeDockForm = value;
                Console.Write("Activate Form: " + m_activeDockForm.TabName);
            }
        }
        private static DockForm m_activeDockForm = null;

        public static DockContainer ActiveDockContainer
        {
            get
            {
                return m_activeDockContainer;
            }
            set
            {
                HideOtherSideContainer(value);
                m_activeDockContainer = value;
                if (m_activeDockContainer != null)
                    Console.Write("Activate Container: " + m_activeDockContainer.Count.ToString() + " / isRoot:" + m_activeDockContainer.IsRoot);
            }
        }
        private static DockContainer m_activeDockContainer = null;

        public static void HideOtherSideContainer(DockContainer dc)
        {
            if (m_activeDockContainer != null
                && (typeof(SideDockContainer)).IsAssignableFrom(m_activeDockContainer.GetType())
                && !m_activeDockContainer.ShowTab
                && m_activeDockContainer != dc
                )
            {
                m_activeDockContainer.Visible = false;
            }
        }

        #region Docking
        private static Bitmap ActiveFormPic;
        public static DockContainer NextContainer { get; set; } = null;

        internal static void StartDock()
        {
            if (HelperOverlay != null) DisposeHf();
            ActiveFormPic = ActiveDockForm.ToBitmap();
            HelperOverlay = new HelperOverlay()
            {
                Opacity = 0.8D,
                Location = new Point(0, 0),
                Size = SystemInformation.VirtualScreen.Size,
            };
            lock (HelperOverlay)
            {
                HelperOverlay.Paint += DockHelper_Paint;
                HelperOverlay.Invalidate();
                HelperOverlay.Show();
            }
        }

        internal static void RefreshDock(DockCanvas Mo) //Point pt, 
        {
            if (HelperOverlay != null)
            {
                DockContainer dc = null;
                foreach (Control cs in Mo.Controls)
                {
                    //if (typeof(DockPane).IsAssignableFrom(cs.GetType()))
                    if (cs is DockPane dockPane)
                    {
                        dc = dockPane.MousePointToContainer();
                        if (dc != null) break;
                    }
                }
                if (dc != null)
                {
                    if (NextContainer != dc)
                    {
                        NextContainer = dc;
                        NextContainer.GetDockRect();
                    }
                }
                HelperOverlay.Invalidate();
            }
            if (ActiveDockContainer != null) ActiveDockContainer.Invalidate();
        }

        internal static DockStyle FinishDock()
        {
            DisposeHf();
            if (NextContainer != null)
            {
                if (NextContainer.Dock_C != Rectangle.Empty)
                {
                    if (NextContainer.Dock_C.Contains(Control.MousePosition)) return DockStyle.Fill;
                }
                if (NextContainer.Dock_T != Rectangle.Empty)
                {
                    if (NextContainer.Dock_T.Contains(Control.MousePosition)) return DockStyle.Top;
                }
                if (NextContainer.Dock_B != Rectangle.Empty)
                {
                    if (NextContainer.Dock_B.Contains(Control.MousePosition)) return DockStyle.Bottom;
                }
                if (NextContainer.Dock_L != Rectangle.Empty)
                {
                    if (NextContainer.Dock_L.Contains(Control.MousePosition)) return DockStyle.Left;
                }
                if (NextContainer.Dock_R != Rectangle.Empty)
                {
                    if (NextContainer.Dock_R.Contains(Control.MousePosition)) return DockStyle.Right;
                }
            }
            return DockStyle.None;
        }

        private static void DockHelper_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //Point pt = HelperOverlay.PointToClient(Control.MousePosition);

            if (NextContainer != null && !(NextContainer == ActiveDockContainer && ActiveDockContainer.Count < 2))
            {
                Rectangle ContainerRect = NextContainer.DockEdge;
                Rectangle TampRect; // = Rectangle.Empty;

                if (NextContainer.Dock_C != Rectangle.Empty)
                {
                    if (NextContainer.Dock_C.Contains(Control.MousePosition)) g.FillRectangle(Main.Theme.HelperMask.FillBrush, ContainerRect);
                }
                if (NextContainer.Dock_T != Rectangle.Empty)
                {
                    TampRect = new Rectangle(ContainerRect.X, ContainerRect.Y, ContainerRect.Width, ContainerRect.Height / 2);
                    if (NextContainer.Dock_T.Contains(Control.MousePosition)) g.FillRectangle(Main.Theme.HelperMask.FillBrush, TampRect);
                }
                if (NextContainer.Dock_B != Rectangle.Empty)
                {
                    TampRect = new Rectangle(ContainerRect.X, ContainerRect.Y + ContainerRect.Height / 2, ContainerRect.Width, ContainerRect.Height / 2);
                    if (NextContainer.Dock_B.Contains(Control.MousePosition)) g.FillRectangle(Main.Theme.HelperMask.FillBrush, TampRect);
                }
                if (NextContainer.Dock_L != Rectangle.Empty)
                {
                    TampRect = new Rectangle(ContainerRect.X, ContainerRect.Y, ContainerRect.Width / 2, ContainerRect.Height);
                    if (NextContainer.Dock_L.Contains(Control.MousePosition)) g.FillRectangle(Main.Theme.HelperMask.FillBrush, TampRect);
                }
                if (NextContainer.Dock_R != Rectangle.Empty)
                {
                    TampRect = new Rectangle(ContainerRect.X + ContainerRect.Width / 2, ContainerRect.Y, ContainerRect.Width / 2, ContainerRect.Height);
                    if (NextContainer.Dock_R.Contains(Control.MousePosition)) g.FillRectangle(Main.Theme.HelperMask.FillBrush, TampRect);
                }
            }

            g.DrawImage(ActiveFormPic, Control.MousePosition);

            if (NextContainer != null && !(NextContainer == ActiveDockContainer && ActiveDockContainer.Count < 2))
            {
                if (NextContainer.Dock_C != Rectangle.Empty) g.DrawImage(Xu.Properties.Resources.Dock_Center, NextContainer.Dock_C);
                if (NextContainer.Dock_T != Rectangle.Empty) g.DrawImage(Xu.Properties.Resources.Dock_Top, NextContainer.Dock_T);
                if (NextContainer.Dock_B != Rectangle.Empty) g.DrawImage(Xu.Properties.Resources.Dock_Bottom, NextContainer.Dock_B);
                if (NextContainer.Dock_L != Rectangle.Empty) g.DrawImage(Xu.Properties.Resources.Dock_Left, NextContainer.Dock_L);
                if (NextContainer.Dock_R != Rectangle.Empty) g.DrawImage(Xu.Properties.Resources.Dock_Right, NextContainer.Dock_R);
            }
        }
        #endregion

        #region Resizing
        private static IResizable ResizeTarget;
        private static Point StartPt;
        private static Point StopPt;
        private static Rectangle SizeGrip;

        internal static void StartResize(IResizable target)
        {
            if (HelperOverlay != null) DisposeHf();
            ResizeTarget = target;
            StartPt = target.Canvas.PointToClient(Control.MousePosition);
            HelperOverlay = new HelperOverlay()
            {
                Opacity = 0.5D,
                Location = target.Canvas.PointToScreen(Point.Empty),
                Size = target.Canvas.ClientSize,
            };

            SizeGrip = new Rectangle(HelperOverlay.PointToClient(target.Area.PointToScreen(target.SizeGrip.Location)), target.SizeGrip.Size);

            lock (HelperOverlay)
            {
                HelperOverlay.Paint += ResizeHelper_Paint;
                RefreshResize();
                HelperOverlay.Show();
            }
        }
        internal static void RefreshResize()
        {
            if (ResizeTarget != null)
            {
                Cursor.Current = ResizeTarget.LayoutType switch
                {
                    (LayoutType.Vertical) => Cursors.SizeNS,
                    (LayoutType.Horizontal) => Cursors.SizeWE,
                    _ => Cursors.Cross,
                };
            }
            if (HelperOverlay != null) HelperOverlay.Invalidate(true);
        }
        internal static void FinishResize()
        {
            DisposeHf();
            StopPt = ResizeTarget.Canvas.PointToClient(Control.MousePosition);
            switch (ResizeTarget.LayoutType)
            {
                case (LayoutType.Vertical):
                    ResizeTarget.OnGetSize(StopPt.Y - StartPt.Y);
                    break;
                case (LayoutType.Horizontal):
                    ResizeTarget.OnGetSize(StopPt.X - StartPt.X);
                    break;
                default:
                    int dx = StopPt.X - StartPt.X;
                    int dy = StopPt.Y - StartPt.Y;
                    double d = Math.Sqrt(dx ^ 2 + dy ^ 2);
                    ResizeTarget.OnGetSize(Convert.ToInt32(d));
                    break;
            }
        }
        private static void ResizeHelper_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Point pt = HelperOverlay.PointToClient(Control.MousePosition);
            switch (ResizeTarget.LayoutType)
            {
                case (LayoutType.Vertical):
                    g.FillRectangle(Main.Theme.HelperMask.FillBrush, new Rectangle(new Point(SizeGrip.X, SizeGrip.Y + (pt.Y - StartPt.Y)), SizeGrip.Size));
                    break;
                case (LayoutType.Horizontal):
                    g.FillRectangle(Main.Theme.HelperMask.FillBrush, new Rectangle(new Point(SizeGrip.X + (pt.X - StartPt.X), SizeGrip.Y), SizeGrip.Size));
                    break;
                default:
                    break;
            }
        }
        protected override void OnResize(EventArgs e)
        {
            //Coordinate();
            base.OnResize(e);
        }
        protected override void OnClientSizeChanged(EventArgs e)
        {
            //Coordinate();
            base.OnClientSizeChanged(e);
        }
        #endregion

        #endregion

        #region Paint
        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            //g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            g.DrawLine(Main.Theme.Panel.EdgePen, new Point(0, 0), new Point(Width, 0));
        }
        #endregion
    }
}
