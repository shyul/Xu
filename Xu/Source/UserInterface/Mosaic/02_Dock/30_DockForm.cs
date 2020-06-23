/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Docking Multi-Panel Control
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace Xu
{
    [Flags]
    public enum DockTabType
    {
        None = 0,
        Center = 1,
        Side = 2,
        Bottom = 4,
        Top = 8,
    }

    [DesignerCategory("Code")]
    public abstract class DockTab : TabItem
    {
        #region Ctor
        protected DockTab(string formName, bool enableUiUpdate = false) : base(formName)
        {
            Name = formName;

            if (enableUiUpdate)
            {
                UpdateUITask = new Task(() => UpdateUIWorker(), UpdateUITask_Cts.Token);
                UpdateUITask.Start();
            }
        }
        #endregion

        #region Components
        protected DockCanvas DockControl { get { return (((DockContainer)HostContainer).DockControl); } }
        protected DockContainer DockContainer { get { return ((DockContainer)HostContainer); } }

        // To be deloyed to the RibbonTab
        public RibbonTabItem RibbonTabItem { get; set; }

        public DockTabType Type { get; set; }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (!(Parent is null) && !(Parent is DockContainer))
            {
                throw new Exception("DockForm can only be exsiting in DockContainer / Parent: " + Parent.GetType().ToString());
            }
            Coordinate();
        }
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            DockCanvas.ActiveDockForm = this;
            if (Parent != null) DockCanvas.ActiveContainer = (DockContainer)Parent;
            if (DockControl != null) DockControl.Invalidate(true);
        }

        public override void Close()
        {
            ObsoletedEvent.Debug(TabName + ": The Tab is closing");
            UpdateUITask_Cts.Cancel();
            HostContainer.Remove(this);
            Dispose();
            while (Disposing) ;
        }

        #endregion

        #region Update UI

        public virtual bool ReadyToShow { get => IsActive && m_ReadyToShow; set { m_ReadyToShow = value; } }

        protected bool m_ReadyToShow = false;

        public readonly Task UpdateUITask;

        public readonly CancellationTokenSource UpdateUITask_Cts = new CancellationTokenSource();

        public void SetRefreshUI() => m_RefreshUI = true;

        protected bool m_RefreshUI = false;

        protected virtual void UpdateUIWorker()
        {
            while (!UpdateUITask_Cts.IsCancellationRequested)
            {
                if (m_RefreshUI)
                {
                    UpdateUI();
                    m_RefreshUI = false;
                }
                Thread.Sleep(5);
            }
        }

        public virtual void UpdateUI()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    CoordinateLayout();
                    Invalidate(true);
                });
            }
            else
            {
                CoordinateLayout();
                Invalidate(true);
            }
        }

        #endregion Update UI

        #region Coordinate

        public virtual bool IsActive
        {
            get
            {
                if (Width < 1)
                    return false;
                else if (!(Parent is null) && !(HostContainer is null))
                {
                    if (HostContainer.ActiveTab != this) return false;
                }
                else if (Parent is null || HostContainer is null)
                {
                    return false;
                }
                return true;
            }
        }

        #endregion

        #region Docking

        #endregion

        #region Mouse
        protected override void OnMouseClick(MouseEventArgs e)
        {
            //Focus();
            //DockControl.Invalidate(true);
            base.OnMouseClick(e);
        }
        #endregion

        #region Paint

        public readonly object GraphicsLockObject = new object();

        #endregion
    }
}
