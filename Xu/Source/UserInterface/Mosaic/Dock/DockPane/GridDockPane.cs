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
using System.Windows.Forms;

namespace Xu
{
    [DesignerCategory("Code"), Serializable]
    public sealed class GridDockPane : DockPane
    {
        #region Ctor

        /// <summary>
        /// 
        /// </summary>
        public GridDockPane() : base()
        {
            // Control Settings
            ResumeLayout(true);
            PerformLayout();
        }

        #endregion

        #region Components

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Root Pane")]
        public bool IsRoot { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            IsRoot = false;
            if (Parent != null)
            {
                Console.WriteLine("GridDockPane Parent is: " + Parent.GetType().ToString());
                // if ((typeof(DockCanvas)).IsAssignableFrom(Parent.GetType()))
                if (Parent is DockCanvas dkc)
                {
                    Console.WriteLine("Again GridDockPane Parent is: " + Parent.GetType().ToString());
                    Dock = DockStyle.Fill;
                    IsRoot = true;
                    DockCanvas = dkc; // (DockCanvas)Parent;
                }
                //else if ((typeof(GridDockContainer)).IsAssignableFrom(Parent.GetType()))
                else if (Parent is GridDockContainer gdc)
                {
                    Console.WriteLine("Again GridDockPane Parent is: " + Parent.GetType().ToString());
                    Dock = DockStyle.None;
                    Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                    IsRoot = false;
                    DockCanvas = gdc.HostDockPane.DockCanvas;// ((GridDockContainer)Parent).HostPane.Dkc;
                }
                else
                    throw new Exception("GridDockPane can only be exsiting in Mosaic or GridDockContainer / Parent: " + Parent.GetType().ToString());
            }
            else DockCanvas = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="df"></param>
        public override void AddForm(int index, DockForm df)
        {
            if (index >= Count)
            {
                //index = Count;
                CreateContainer().AddForm(DockStyle.Fill, df);
                return;
            }
            else
            {
                if (index < 0) index = 0;
                DockContainers[index].AddForm(DockStyle.Fill, df);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GridDockContainer CreateContainer()
        {
            return CreateContainer(Count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public GridDockContainer CreateContainer(int index)
        {
            GridDockContainer dc_new = new GridDockContainer();
            SuspendLayout();
            lock (DockContainers)
            {
                if (index > Count) index = Count;
                if (index < 0) index = 0;
                DockContainers.Insert(index, dc_new);
                Controls.Add(dc_new);
                Reorder();
            }
            if (!Visible && Count > 0) Visible = true;
            ResumeLayout(true);
            return dc_new;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="pane"></param>
        public void InsertRange(int index, GridDockPane pane)
        {
            DockContainer[] dc_subs = pane.DockContainers.ToArray();
            SuspendLayout();
            lock (DockContainers)
            {
                if (index >= Count) index = Count - 1;
                if (index < 0) index = 0;
                DockContainers.InsertRange(index, dc_subs);
                Controls.AddRange(dc_subs);
                Reorder();
            }
            ResumeLayout(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dc"></param>
        public override void RemoveContainer(DockContainer dc)
        {
            if (!(IsRoot && Count <= 1))
                base.RemoveContainer(dc); // If it is root, we should keep at least one container here.

            CleanUp();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CleanUp()
        {
            if (Count <= 1)
            {
                //Log.Debug("Simple GridPane Clean Up");
                Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Close()
        {
            if (!IsRoot && Parent != null)
            {
                ((GridDockContainer)Parent).DisableSubPane();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override DockContainer MousePointToContainer()
        {
            for (int i = 0; i < Count; i++)
            {
                GridDockContainer dc = (GridDockContainer)DockContainers[i];
                if (dc.ClientRectangle.Contains(dc.PointToClient(Control.MousePosition)))
                {
                    if (dc.SubPane == null) return dc;
                    else return dc.SubPane.MousePointToContainer();
                }
            }
            return null;
        }
        #endregion

        #region Coordinate

        /// <summary>
        /// 
        /// </summary>
        public override void Coordinate()
        {
            int splitMargin = MosaicForm.PaneGripMargin;
            double TotalRatio = 0; // Must be caclulated and total of the count of containers

            lock (DockContainers)
                if (Count > 0)
                {
                    foreach (GridDockContainer dc in DockContainers)
                    {
                        TotalRatio += dc.Ratio;
                    }

                    int containerDepth;
                    int containerBase = 0;
                    int containerBaseFixed = 0;
                    double factor;

                    switch (LayoutType)
                    {
                        case (LayoutType.Vertical):
                            containerDepth = Width;
                            if (Unlocked)
                            {
                                factor = (Height - ((TotalRatio - 1) * splitMargin)) / TotalRatio;
                            }
                            else
                            {
                                factor = Height / TotalRatio;
                            }
                            for (int i = 0; i < Count; i++)
                            {
                                GridDockContainer dc = (GridDockContainer)DockContainers[i];
                                int h = Convert.ToInt32((factor * dc.Ratio).ToInt64());
                                if (Unlocked && i != 0)
                                {
                                    h += splitMargin;
                                }
                                if (i == Count - 1)
                                {
                                    h = Height - containerBase;
                                }
                                dc.Location = new Point(containerBaseFixed, containerBase);
                                dc.Width = containerDepth;
                                dc.Height = h;
                                containerBase = dc.Bottom;
                            }
                            break;
                        case (LayoutType.Horizontal):
                            containerDepth = Height;
                            if (Unlocked)
                            {
                                factor = (Width - ((TotalRatio - 1) * splitMargin)) / TotalRatio;
                            }
                            else
                            {
                                factor = Width / TotalRatio;
                            }
                            for (int i = 0; i < Count; i++)
                            {
                                GridDockContainer dc = (GridDockContainer)DockContainers[i];
                                int w = Convert.ToInt32((factor * dc.Ratio).ToInt64());
                                if (Unlocked && i != 0)
                                {
                                    w += splitMargin;
                                }
                                if (i == Count - 1)
                                {
                                    w = Width - containerBase;
                                }
                                dc.Location = new Point(containerBase, containerBaseFixed);
                                dc.Width = w;
                                dc.Height = containerDepth;
                                containerBase = dc.Right;
                            }
                            break;
                    }
                }
        }

        #endregion
    }
}
