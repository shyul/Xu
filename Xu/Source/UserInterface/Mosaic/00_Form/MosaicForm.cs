/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Xu.WindowsNativeMethods;

namespace Xu
{
    [DesignerCategory("Code")]
    public class MosaicForm : Form
    {
        #region Caption Geometry
        public const int UpEdgeResizeGripMargin = 8;
        public const int RibbonToLeftWindowEdgeMargin = 6;
        public const int RibbonToToolBarMargin = 2;
        public static int PaneGripMargin = 4;
        public static int CaptionAreaSize = SystemInformation.CaptionHeight + UpEdgeResizeGripMargin; // 31
        #endregion

        public int ShowFormMsg { get; private set; }

        public static Color ActiveColor => DWMAPI.GetWindowColorizationColor(true);

        public float ScaleFactor => Xu.GUI.ScalingFactor();

        public override Font Font => Main.Theme.Font;

        public static string HelpLink { get; set; }

        #region Ctor
        public MosaicForm(int showFormMsg)
        {
            ShowFormMsg = showFormMsg;

            // Commands
            Command_Minimize = new Command()
            {
                Theme = new ColorTheme(Color.White, Color.Gray, Color.White),
                IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>()
                {
                    {  IconType.Normal, Icons.Caption_Minimize },
                },
                Action = (IObject sender, string[] args, Progress<Event> progress, CancellationTokenSource cts) =>
                { WindowState = FormWindowState.Minimized; },
            };

            Command_Maximize = new Command()
            {
                Theme = new ColorTheme(Color.White, Color.Gray, Color.White),
                IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>()
                {
                    {  IconType.Normal, Icons.Caption_Maximize },
                },
                Action = (IObject sender, string[] args, Progress<Event> progress, CancellationTokenSource cts) =>
                {
                    if (WindowState != FormWindowState.Maximized)
                    {
                        WindowState = FormWindowState.Maximized;
                    }
                },
            };

            Command_Restore = new Command()
            {
                Theme = new ColorTheme(Color.White, Color.Gray, Color.White),
                IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>()
                {
                    {  IconType.Normal, Icons.Caption_Restore },
                },
                Action = (IObject sender, string[] args, Progress<Event> progress, CancellationTokenSource cts) =>
                {
                    if (WindowState != FormWindowState.Normal)
                    {
                        WindowState = FormWindowState.Normal;
                    }
                },
            };

            Command_Close = new Command()
            {
                Theme = new ColorTheme(Color.White, Color.Red, Color.White),
                IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>()
                {
                    {  IconType.Normal, Icons.Caption_Close },
                },
                Action = (IObject sender, string[] args, Progress<Event> progress, CancellationTokenSource cts) =>
                {
                    Close(); // Should be app exit...
                },
            };



            // Components
            components = new Container();
            CaptionBar = new CaptionBar(this);
            Ribbon = new Ribbon();
            OrbMenu = new OrbMenu(this) { Visible = false };
            StatusPane = new StatusStrip();
            DockControl = new DockCanvas();

            Btn_Minimize = new ButtonWidget(Command_Minimize, true, true, false);
            Btn_Maximize = new ToggleWidget(Command_Maximize, Command_Restore, (WindowState == FormWindowState.Maximized), true, true, false);
            Btn_Close = new ButtonWidget(Command_Close, true, true, false);

            // Control Settings
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.Dpi; //AutoScaleMode = AutoScaleMode.Font; //AutoScaleDimensions = new SizeF(6F, 13F);
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            FormBorderStyle = FormBorderStyle.Sizable;
            ControlBox = false;
            ShowIcon = false;
            //Text = Program.Title;
            //Name = Program.Title;
            //Font = Theme.Font;
            //    Anchor = AnchorStyles.Top | AnchorStyles.Right,
            ClientSize = new Size(1600, 1100);
            BackColor = Color.White;
            IsRibbonShrink = true;

            Controls.Add(Ribbon);
            Controls.Add(Ribbon.RibbonContainer);
            //Controls.Add(OrbMenu);
            Controls.Add(DockControl);
            Controls.Add(StatusPane);

            Controls.Add(CaptionBar);

            Controls.Add(Btn_Minimize);
            Controls.Add(Btn_Maximize);
            Controls.Add(Btn_Close);

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

        public static Command Command_HelpLink = new Command()
        {
            Theme = new ColorTheme(Color.DimGray, Color.Gray),
            Action = (IObject sender, string[] args, Progress<Event> progress, CancellationTokenSource cts) =>
            {
                System.Diagnostics.Process.Start(HelpLink);
            },
        };

        public Command Command_Maximize;
        public Command Command_Restore;

        public Command Command_Minimize;
        public Command Command_Close;

        public ButtonWidget Btn_Minimize { get; set; }
        public ToggleWidget Btn_Maximize { get; set; }
        public ButtonWidget Btn_Close { get; set; }

        public CaptionBar CaptionBar { get; protected set; }
        public OrbMenu OrbMenu { get; protected set; }
        public Ribbon Ribbon { get; protected set; }
        public StatusStrip StatusPane { get; protected set; }
        public DockCanvas DockControl { get; protected set; }

        public void AddForm(DockForm df) => DockControl.AddForm(DockStyle.Fill, 0, df);
        public void AddForm(DockStyle postion, DockForm df) => DockControl.AddForm(postion, 0, df);
        public void AddForm(DockStyle postion, int index, DockForm df) => DockControl.AddForm(postion, index, df);

        #region Drop Menus ####################################################################### t.b.d
        public static ContextPane ContextPane { get; } = new ContextPane();
        public static ContextDropMenu ContextDropMenu { get; } = new ContextDropMenu();
        public void SetActivateOrbMenu(bool isActivate)
        {
            OrbMenu.Visible = isActivate;
            ContextPane.Show(this, new OrbMenuHost(OrbMenu), new Point(Ribbon.Bounds.Left, Ribbon.Bounds.Bottom));

        }
        #endregion

        public bool IsActivated { get; private set; } = false;

        protected void SetActivate()
        {
            if (IsActivated)// && ContainsFocus)
            {
                ForeColor = Color.White;
                Btn_Close.ForeColor = Color.LightGray; //Color.White;
                Btn_Maximize.ForeColor = Color.LightGray;
                Btn_Minimize.ForeColor = Color.LightGray;
            }
            else
            {
                ForeColor = Color.DimGray;
                Btn_Close.ForeColor = Color.DimGray;
                Btn_Maximize.ForeColor = Color.Gray;
                Btn_Minimize.ForeColor = Color.Gray;
            }
            Invalidate(true);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            SetActivate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            SetActivate();
            base.OnLostFocus(e);
        }
        #endregion

        #region Coordinate

        public bool IsRibbonShrink
        {
            get
            {
                return m_IsRibbonShrink;
            }
            set
            {
                m_IsRibbonShrink = value;
                UpdateShrink();
                Coordinate();
            }
        }

        private bool m_IsRibbonShrink = false;

        private void UpdateShrink()
        {
            if (m_IsRibbonShrink)
            {
                Ribbon.RibbonContainer.Visible = false;
                Ribbon.RibbonContainer.Coordinate();
                Ribbon.RibbonContainer.DeActivate();
                DockControl.Location = new Point(0, Ribbon.Bounds.Bottom);
            }
            else
            {
                Ribbon.RibbonContainer.Coordinate();
                Ribbon.RibbonContainer.Visible = true;
                Ribbon.RibbonContainer.ActivateDefaultTab();
                DockControl.Location = new Point(0, Ribbon.RibbonContainer.Bounds.Bottom);
            }
            DockControl.Size = new Size(ClientRectangle.Width, ClientRectangle.Height - DockControl.Location.Y - StatusPane.Height);
        }

        protected void Coordinate()
        {
            SuspendLayout();

            int ribbonWidth = Ribbon.OrbWidth;

            foreach (RibbonTabItem rt in Ribbon.Tabs)
            {
                ribbonWidth += rt.TabRect.Width;
            }
            ribbonWidth += 60;

            Ribbon.Bounds = new Rectangle(RibbonToLeftWindowEdgeMargin, UpEdgeResizeGripMargin, ribbonWidth, Ribbon.TabHeight);
            Ribbon.RibbonContainer.Location = new Point(0, Ribbon.Bounds.Bottom);
            Ribbon.RibbonContainer.Size = new Size(ClientRectangle.Width, Ribbon.PanelHeight);
            Ribbon.RibbonContainer.Visible = !IsRibbonShrink;

            //OrbMenu.Location = new Point(Ribbon.Bounds.Left, Ribbon.Bounds.Bottom);

            DockControl.Size = new Size(ClientRectangle.Width, ClientRectangle.Height - DockControl.Location.Y - StatusPane.Height);

            if (WindowState == FormWindowState.Maximized)
            {
                CaptionBar.Bounds = new Rectangle(Ribbon.Bounds.Right + RibbonToToolBarMargin, UpEdgeResizeGripMargin, Width - Ribbon.Bounds.Right - 120 - 17, 31 - UpEdgeResizeGripMargin);
                Btn_Minimize.Bounds = new Rectangle(ClientRectangle.Right - 120, 8, 40, 23);
                Btn_Maximize.Bounds = new Rectangle(Btn_Minimize.Bounds.Right, 8, 40, 23);
                //Btn_Maximize.Icon = Xu.Properties.Resources.Caption_RestoreNormal;
                Btn_Close.Bounds = new Rectangle(Btn_Maximize.Bounds.Right, 8, 40, 23);
            }
            else
            {
                CaptionBar.Bounds = new Rectangle(Ribbon.Bounds.Right + RibbonToToolBarMargin, UpEdgeResizeGripMargin, Width - Ribbon.Bounds.Right - 124 - 17, 31 - UpEdgeResizeGripMargin);
                Btn_Minimize.Bounds = new Rectangle(ClientRectangle.Right - 124, 0, 40, 25);
                Btn_Maximize.Bounds = new Rectangle(Btn_Minimize.Bounds.Right, 0, 40, 25);
                //Btn_Maximize.Icon = Xu.Properties.Resources.Caption_Maximize;
                Btn_Close.Bounds = new Rectangle(Btn_Maximize.Bounds.Right, 0, 40, 25);
            }

            ResumeLayout(false);
            PerformLayout();
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

        #region WndProc
        private bool m_painting = false;
        private static MARGINS m_Margins = new MARGINS(0, 0, CaptionAreaSize, 0);
        private const int BLACK_BRUSH = 4;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == ShowFormMsg)
            {
                ShowMe();
                base.WndProc(ref m);
            }
            else
            {
                switch (m.Msg)
                {
                    case (WindowsMessages.ACTIVATEAPP):
                        {
                            IsActivated = (m.WParam != IntPtr.Zero);
                            SetActivate();
                            base.WndProc(ref m);
                            break;
                        }
                    case (WindowsMessages.CREATE):
                        {
                            RECT rcClient = new RECT();
                            Xu.WindowsNativeMethods.User32.GetWindowRect(this.Handle, ref rcClient);
                            Xu.WindowsNativeMethods.User32.SetWindowPos(this.Handle, IntPtr.Zero,
                                         rcClient.Left, rcClient.Top,
                                         rcClient.Right - rcClient.Left, rcClient.Bottom - rcClient.Top,
                                         SWP.FRAMECHANGED);
                            m.Result = Xu.GUI.MSG_HANDLED;
                            base.WndProc(ref m);
                            break;
                        }
                    case (WindowsMessages.DWMCOMPOSITIONCHANGED):
                    case (WindowsMessages.ACTIVATE):
                        {
                            DWMAPI.DwmExtendFrameIntoClientArea(this.Handle, ref m_Margins);
                            m.Result = Xu.GUI.MSG_HANDLED;
                            base.WndProc(ref m);
                            break;
                        }
                    case (WindowsMessages.PAINT):
                        {
                            if (!m_painting)
                            {
                                m_painting = true;
                                PAINTSTRUCT ps = new PAINTSTRUCT();
                                IntPtr hdc = User32.BeginPaint(m.HWnd, ref ps);
                                //int w = ps.rcPaint.Right;
                                //int h = ps.rcPaint.Bottom;
                                //IntPtr backbuffDC = NativeMethods.CreateCompatibleDC(hdc);
                                User32.FillRect(ps.hdc, ref ps.rcPaint, Gdi32.GetStockObject(BLACK_BRUSH));
                                //DrawCaption(Graphics.FromHdc(ps.hdc));
                                User32.EndPaint(m.HWnd, ref ps);
                                m_painting = false;
                            }
                            base.WndProc(ref m);
                            break;
                        }
                    case (WindowsMessages.NCCALCSIZE):
                        {
                            if (m.WParam != IntPtr.Zero && m.Result == IntPtr.Zero)
                            {
                                NCCALCSIZE_PARAMS nc = (NCCALCSIZE_PARAMS)Marshal.PtrToStructure(m.LParam, typeof(NCCALCSIZE_PARAMS));
                                nc.rect0.Top -= CaptionAreaSize; // 31
                                nc.rect1 = nc.rect0;
                                Marshal.StructureToPtr(nc, m.LParam, false);
                                m.Result = WVR.VALIDRECTS;
                            }
                            base.WndProc(ref m);
                            break;
                        }
                    case (WindowsMessages.NCHITTEST):
                        {
                            if (m.Result == MouseHitTest.NOWHERE)
                            {
                                IntPtr res = IntPtr.Zero;
                                if (DWMAPI.DwmDefWindowProc(m.HWnd, m.Msg, m.WParam, m.LParam, ref res))
                                {
                                    m.Result = res;
                                }
                                else
                                {
                                    m.Result = HitTestNCA(Handle);
                                    //Console.WriteLine(res.ToString());
                                    //m.Result = res;
                                }
                            }
                            else
                                base.WndProc(ref m);
                            break;
                        }
                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
        }
        private IntPtr HitTestNCA(IntPtr hWnd)
        {
            Point ptMouse = Control.MousePosition;

            // Get the window rectangle.
            RECT rcWindow = new RECT();
            Xu.WindowsNativeMethods.User32.GetWindowRect(hWnd, ref rcWindow); // this.Handle

            // Get the frame rectangle, adjusted for the style without a caption.
            RECT rcFrame = new RECT();
            Xu.WindowsNativeMethods.User32.AdjustWindowRectEx(ref rcFrame, WindowStyles.OVERLAPPEDWINDOW & ~WindowStyles.CAPTION, false, 0); // The last is supposed to be NULL

            // Determine if the hit test is for resizing. Default middle (1,1).
            int uRow = 1;
            int uCol = 1;
            //bool fOnResizeBorder = false;

            // Determine if the point is at the left or right of the window.
            if (ptMouse.X >= rcWindow.Left && ptMouse.X < rcWindow.Left + UpEdgeResizeGripMargin)
            {
                uCol = 0; // left side
            }
            else if (ptMouse.X < rcWindow.Right && ptMouse.X >= rcWindow.Right - UpEdgeResizeGripMargin)
            {
                uCol = 2; // right side
            }

            // Determine if the point is at the top or bottom of the window.
            if (ptMouse.Y >= rcWindow.Top && ptMouse.Y < rcWindow.Top + CaptionAreaSize)
            {
                if (ptMouse.Y < (rcWindow.Top - rcFrame.Top))
                {
                    uRow = 0;
                }
                else if (uCol == 1)
                {
                    return MouseHitTest.CAPTION;
                }
            }
            else if (ptMouse.Y < rcWindow.Bottom && ptMouse.Y >= rcWindow.Bottom - UpEdgeResizeGripMargin)
            {
                uRow = 2;
            }

            IntPtr[,] hitTests =  { { MouseHitTest.TOPLEFT,    MouseHitTest.TOP,      MouseHitTest.TOPRIGHT },
                                    { MouseHitTest.LEFT,       MouseHitTest.NOWHERE,  MouseHitTest.RIGHT },
                                    { MouseHitTest.BOTTOMLEFT, MouseHitTest.BOTTOM,   MouseHitTest.BOTTOMRIGHT } };

            return hitTests[uRow, uCol];
        }

        private void ShowMe()
        {
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;
            //if (WindowState != FormWindowState.Normal)
            //   WindowState = FormWindowState.Normal;
            // get our current "TopMost" value (ours will always be false though)
            //bool top = TopMost;
            // make our form jump to the top of everything
            //TopMost = true;
            // set it back to whatever it was
            //TopMost = top;
            Activate();
        }
        #endregion
    }
}
