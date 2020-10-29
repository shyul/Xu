/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Docking Multi-Panel Control
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
    public abstract class DockPane : ContainerControl
    {
        #region Ctor
        protected DockPane()
        {
            // Components
            components = new Container();
            // m_dockContainers = new List<DockContainer>();
            // Control Settings
            SuspendLayout();
            DoubleBuffered = true;
            SetStyle(ControlStyles.Selectable, false);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //SetStyle(ControlStyles.ContainerControl, true);
            Size = new Size(250, 250);
            //BackColor = Main.Theme.Panel.FillColor;
            Visible = false;
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

        public override Color BackColor => Main.Theme.Panel.FillColor;

        public DockCanvas DockCanvas { get; protected set; }

        public readonly List<DockContainer> DockContainers = new List<DockContainer>();

        public virtual int Count => DockContainers.Count;

        public virtual void Reorder()
        {
            if (Count > 1)
            {
                for (int i = 0; i < Count; i++) DockContainers[i].SetOrder(i);
            }
            Coordinate();
        }

        public virtual void Sort()
        {
            if (Count > 1)
            {
                DockContainers.Sort((f1, f2) => f1.Order.CompareTo(f2.Order));
                for (int i = 0; i < Count; i++) DockContainers[i].SetOrder(i);
            }
            Coordinate();
        }

        public abstract void AddForm(int index, DockForm df);

        public virtual void RemoveContainer(DockContainer dc)
        {
            SuspendLayout();
            lock (DockContainers)
            {
                if (!DockContainers.Contains(dc)) throw new Exception("the Container is not listed in this Pane, this is impossible, please check.");
                DockContainers.Remove(dc);
                Controls.Remove(dc);
                dc.Dispose();
                Reorder();
            }
            Coordinate();
            Visible = (Count > 0);
            while (dc.Disposing) ;
            ResumeLayout(true);
        }

        public abstract void CleanUp();

        public abstract void Close();

        public virtual DockContainer MousePointToContainer()
        {
            lock (DockContainers)
                for (int i = 0; i < Count; i++)
                {
                    DockContainer dc = DockContainers[i];
                    if (dc.ClientRectangle.Contains(dc.PointToClient(Control.MousePosition)))
                    {
                        return dc;
                    }
                }
            return null;
        }

        #endregion

        #region Resizing

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Unlocked")]
        public bool Unlocked { get { return (DockCanvas.Unlocked); } }

        /// <summary>
        /// 
        /// </summary>
        public virtual LayoutType LayoutType { get; set; } = LayoutType.Horizontal;

        #endregion

        #region Coordinate

        /// <summary>
        /// 
        /// </summary>
        public abstract void Coordinate();

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
        protected MouseState MouseState { get; set; } = MouseState.Out;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            MouseState = MouseState.Out;
            lock (DockContainers)
                foreach (DockContainer dc in DockContainers)
                {
                    dc.MouseState = MouseState.Out;
                }
            Invalidate();
        }

        #endregion
    }
}