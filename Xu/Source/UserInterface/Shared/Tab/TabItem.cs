/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Xu
{
    [DesignerCategory("Code")]
    public abstract class TabItem : UserControl, IComponent
    {
        #region Ctor
        protected TabItem(string tabName)
        {
            // Control Settings
            SuspendLayout();
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
            //Dock = DockStyle.None;
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            Font = Main.Theme.Font;
            TabName = tabName;
        }
        #endregion

        #region Components
        /// <summary>
        /// The Host Tab for this TabItem, the host tab where this tab belongs to.
        /// </summary>
        public Tab HostContainer { get; set; } // => (Tab)Parent;

        /// <summary>
        /// Assign the HostContainer to the Tab which is holding this tabItem and change the settings of this TabItem
        /// </summary>
        /// <param name="e"></param>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (Parent != null)
            {
                if (Parent is Tab tb) // (typeof(Tab)).IsAssignableFrom(Parent.GetType()))
                {
                    HostContainer = tb; // (Tab)Parent;
                    Font = HostContainer.Font;
                }
                else
                {
                    throw new Exception("TabPanel can only be exsiting in TabPanelContainer / Parent: " + Parent.GetType().ToString());
                }
            }
            else
            {
                HostContainer = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Close()
        {
            ObsoletedEvent.Debug(TabName + ": The Tab is closing");
            HostContainer.Remove(this);
            Dispose();
            while (Disposing) ;
        }

        #endregion

        #region Tab Properties

        /// <summary>
        /// 
        /// </summary>
        public int Order { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public Color Color { get; set; } = Color.Transparent;

        /// <summary>
        /// 
        /// </summary>
        public bool HasIcon { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public Bitmap Icon { get; set; } = Xu.Properties.Resources.Blank_16;

        /// <summary>
        /// 
        /// </summary>
        public string TabName { get; set; } = "TabPanel";

        /// <summary>
        /// 
        /// </summary>
        public Font TabNameFont { get; set; } = null;

        #region Control

        /// <summary>
        /// 
        /// </summary>
        public virtual Click Btn_Close { get; } = new Click(new Command()
        {
            Description = "Close",
            Enabled = false,
            IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>() {
                    { IconType.Normal, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Caption_Close }, } },

                    { IconType.Hover, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Exit_16 }, } },

                    { IconType.Down, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Exit_Blue_16 }, } },
                },
            //Action = (IItem sender, string[] args, Progress<Event> progress, CancellationTokenSource cts) => { ShowMenu(); },
        });

        /// <summary>
        /// 
        /// </summary>
        public virtual Click Btn_Pin { get; } = new Click(new Command()
        {
            Description = "Pin to front",
            Enabled = false,
            IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>() {
                    { IconType.Normal, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Caption_PinUnset }, } },

                    { IconType.Checked, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Caption_PinSet }, } },

                    { IconType.CheckedHover, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Caption_PinSet }, } },

                    { IconType.CheckedDown, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Caption_PinSet }, } },
                },
        })
        { Checked = false, Toggle = true, };

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsPinned
        {
            get
            {
                return Btn_Pin.Checked;
            }
            set
            {
                Btn_Pin.Checked = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool ShowTab
        {
            get
            {
                return (TabRect.Right <= HostContainer.TabLimit);
            }
        }

        #endregion

        #endregion

        #region Coordinate

        /// <summary>
        /// 
        /// </summary>
        public Rectangle TabRect { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Rectangle TabNameRect { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int TabNameWidth
        {
            get
            {
                if (TabNameFont != null)
                    return (TextRenderer.MeasureText(TabName, TabNameFont).Width * 1.05f).ToInt32();
                else if (HostContainer != null && HostContainer.Font != null)
                    return (TextRenderer.MeasureText(TabName, HostContainer.Font).Width * 1.05f).ToInt32();
                else
                    return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Rectangle IconRect { get; set; }

        protected virtual void CoordinateLayout()
        {
            ResumeLayout(true);
            PerformLayout();
        }

        //protected delegate void CoordinateDelegate();

        /// <summary>
        /// 
        /// </summary>
        public virtual void Coordinate()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    CoordinateLayout();
                });
            }
            else
            {
                CoordinateLayout();
            }
        }

        public virtual void RefreshAllWidgets() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            Coordinate();
            base.OnResize(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClientSizeChanged(EventArgs e)
        {
            Coordinate();
            base.OnClientSizeChanged(e);
        }
        #endregion

        #region Mouse

        /// <summary>
        /// 
        /// </summary>
        public virtual MouseState MouseState { get; set; } = MouseState.Out;
        #endregion
    }
}
