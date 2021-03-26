/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
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
    public abstract class Tab : UserControl//, IContainer
    {
        #region Ctor
        protected Tab()
        {
            // Components
            components = new Container();
            //m_tabs = new List<TabItem>();
            // Control Settings
            SuspendLayout();
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.Selectable, false);
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }
        #endregion

        #region Components

        /// <summary>
        /// 
        /// </summary>
        private IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void UpdateGraphics()
        {
            if (Parent != null) Parent.Invalidate(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool ShowTab { get { return m_showTab; } }

        protected bool m_showTab = true;

        /// <summary>
        /// 
        /// </summary>
        public virtual bool ShowIcon { get { return m_showIcon; } set { m_showIcon = value; Coordinate(); } }
        protected bool m_showIcon = true;

        /// <summary>
        /// 
        /// </summary>
        public virtual bool ShowPin { get { return m_showPin; } set { m_showPin = value; Coordinate(); } }
        protected bool m_showPin = true;

        /// <summary>
        /// 
        /// </summary>
        public virtual bool ShowClose { get { return m_showClose; } set { m_showClose = value; Coordinate(); } }
        protected bool m_showClose = true;

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsOverflow
        {
            get
            {
                lock (Tabs)
                {
                    foreach (TabItem tp in Tabs)
                        if (!tp.ShowTab)
                            return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int ActiveTabIndex { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual TabItem ActiveTab { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public readonly List<TabItem> Tabs = new();

        /// <summary>
        /// 
        /// </summary>
        public virtual int Count => Tabs.Count;

        /// <summary>
        /// 
        /// </summary>
        public virtual void Sort()
        {
            if (Count > 1)
            {
                for (int i = 0; i < Count; i++)
                {
                    TabItem tp = Tabs[i];
                    //if (!tp.IsPinned) tp.SetOrder(tp.Order + Count);
                    if (!tp.IsPinned) tp.Order += Count;
                }
                Tabs.Sort((f1, f2) => f1.Order.CompareTo(f2.Order));
                for (int i = 0; i < Count; i++)
                {
                    Tabs[i].Order = i;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool HasPin
        {
            get
            {
                lock (Tabs)
                {
                    foreach (TabItem tp in Tabs)
                        if (tp.IsPinned)
                            return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeActivate() => ActiveTab = null;

        /// <summary>
        /// 
        /// </summary>
        public void ActivateDefaultTab()
        {
            if (Count > 0 && ActiveTab == null) ActivateTab(0);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void ActivateLastTab()
        {
            if (Count > 0) ActivateTab(Count - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public virtual void ActivateTab(int index)
        {
            //Log.Debug("ActivateTab: " + index.ToString());
            lock (Tabs)
            {
                if (index >= 0 && index < Count)
                {
                    TabItem tp = Tabs[index];
                    foreach (TabItem cs in Tabs)
                    {
                        if (cs != tp)
                        {
                            cs.Visible = false;
                            cs.SuspendLayout();
                        }
                        else
                        {
                            ActiveTab = tp;
                            cs.Bounds = m_tabPanelBound;
                            cs.Visible = true;
                            //cs.Coordinate();
                            cs.ResumeLayout(true);
                            cs.PerformLayout();
                            cs.Focus();
                        }
                    }
                    ActiveTabIndex = index;
                }
            }
            UpdateGraphics();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tp"></param>
        public virtual void ActivateTab(TabItem tp)
        {
            lock (Tabs)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (tp == Tabs[i])
                    {
                        ActivateTab(i);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tc"></param>
        protected virtual void AddRange(Tab tc)
        {
            lock (Tabs)
            {
                lock (tc.Tabs)
                {
                    List<TabItem> Temp = new();
                    foreach (Control cs in tc.Controls)
                    {
                        if (typeof(TabItem).IsAssignableFrom(cs.GetType()))
                        {
                            Temp.Add((TabItem)cs);
                        }
                    }
                    foreach (TabItem tp in tc.Tabs)
                    {
                        if (Temp.Contains(tp) && !Tabs.Contains(tp))
                        {
                            tp.Order += Count;
                            Tabs.Add(tp);
                        }
                    }
                    foreach (TabItem tp in Temp)
                    {
                        if (!Tabs.Contains(tp))
                        {
                            Temp.Remove(tp);
                        }
                    }
                    Controls.AddRange(Temp.ToArray());
                    tc.Tabs.Clear();
                }
                Sort();
            }
            ActivateLastTab();
            Coordinate();
            UpdateGraphics();
            if (!Visible) Visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tp"></param>
        public virtual void Add(TabItem tp)
        {
            lock (Tabs)
            {
                if (!Controls.Contains(tp))
                {
                    foreach (Control cs in Controls)
                    {
                        cs.Visible = false;
                    }
                    tp.Visible = true;
                    Controls.Add(tp);
                }
                else
                {
                    foreach (Control cs in Controls)
                    {
                        cs.Visible = (cs == tp);
                    }
                }
                if (!Tabs.Contains(tp))
                {
                    tp.Order = Count;
                    tp.MouseState = MouseState.Out;
                    Tabs.Add(tp);
                }
                Sort();
            }
            ActivateTab(tp);
            Coordinate();
            UpdateGraphics();
            tp.HostContainer = this;
            if (!Visible) Visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public virtual bool Remove(TabItem tp)
        {
            bool hasItem = false;
            lock (Tabs)
            {
                if (Tabs.Contains(tp))
                {
                    Tabs.Remove(tp);
                    hasItem = true;
                }

                if (tp == ActiveTab)
                    ActivateLastTab();

                if (Controls.Contains(tp))
                {
                    Controls.Remove(tp);
                    hasItem = true;
                }

                if (Count <= 0)
                {
                    ActiveTabIndex = -1;
                    ActiveTab = null;
                }
                if (hasItem) Sort();
            }
            if (hasItem)
            {
                Coordinate();
                UpdateGraphics();
            }
            return hasItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public virtual bool Contains(TabItem tp)
        {
            lock (Tabs)
            {
                if (!Tabs.Contains(tp) && Controls.Contains(tp)) Controls.Remove(tp);
                return Tabs.Contains(tp);
            }
        }
        #endregion

        #region Coordinate

        /// <summary>
        /// 
        /// </summary>
        protected Rectangle m_tabBound;

        /// <summary>
        /// 
        /// </summary>
        protected Rectangle m_tabPanelBound;

        /// <summary>
        /// 
        /// </summary>
        protected Rectangle m_edgeRect;

        /// <summary>
        /// 
        /// </summary>
        public virtual int TabLimit => m_edgeRect.Right;

        /// <summary>
        /// 
        /// </summary>
        public abstract void Coordinate();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        //[OnDeserializing]
        protected override void OnResize(EventArgs e)
        {
            Coordinate();
            base.OnResize(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        //[OnDeserializing]
        protected override void OnClientSizeChanged(EventArgs e)
        {
            Coordinate();
            base.OnClientSizeChanged(e);
        }
        #endregion

        #region Paint

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="tp"></param>
        /// <param name="textBrush"></param>
        /// <param name="backBrush"></param>
        /// <returns></returns>
        //[OnDeserializing]
        protected virtual bool DrawTab(Graphics g, TabItem tp, SolidBrush textBrush, Brush backBrush)
        {
            if (tp.ShowTab)
            {
                g.FillRectangle(backBrush, tp.TabRect);

                if (tp.HasIcon && tp.Icon != null && ShowIcon)
                    g.DrawIcon(tp.Icon, tp.IconRect, textBrush.Color);

                g.DrawString(tp.TabName, tp.Font, textBrush, tp.TabNameRect, AppTheme.TextAlignLeft);
                /*
                if (ShowPin && tp.Btn_Pin.Enabled)
                    if (tp.IsPinned)
                        tp.Btn_Pin.PaintControl(g, Xu.Properties.Resources.Caption_PinSet, textBrush.Color);
                    else
                        tp.Btn_Pin.PaintControl(g, Xu.Properties.Resources.Caption_PinUnset, textBrush.Color);
                */
                if (ShowPin && tp.Btn_Pin.Enabled)
                    if (tp.IsPinned)
                        tp.Btn_Pin.PaintControl(g, textBrush.Color);
                    else
                        tp.Btn_Pin.PaintControl(g, textBrush.Color);

                if (ShowClose && tp.Btn_Close.Enabled)
                    tp.Btn_Close.PaintControl(g, textBrush.Color);

                return true;
            }
            else
                return false;
        }
        #endregion

        #region Mouse Events

        /// <summary>
        /// 
        /// </summary>
        public virtual MouseState MouseState { get; set; } = MouseState.Out;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (ShowTab)
            {
                Point pt = new(e.X, e.Y);
                lock (Tabs)
                    foreach (TabItem tp in Tabs)
                    {
                        Rectangle tabRect = tp.TabRect;
                        if (tp.MouseState != MouseState.Drag && tp.MouseState != MouseState.Down && ShowTab && tp.ShowTab)
                        {
                            tp.MouseState = (tabRect.Contains(pt)) ? MouseState.Hover : MouseState.Out;
                            if (ShowClose) tp.Btn_Close.MouseMove(pt);
                            if (ShowPin) tp.Btn_Pin.MouseMove(pt);
                        }
                    }
                MouseState = MouseState.Hover;
                Invalidate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (ShowTab)
            {
                bool FoundTab = false;
                Point pt = new(e.X, e.Y);
                lock (Tabs)
                    for (int i = 0; i < Count; i++)
                    {
                        TabItem tp = Tabs[i];
                        if (tp.MouseState == MouseState.Hover && ShowTab && tp.ShowTab)
                        {
                            tp.MouseState = MouseState.Down;
                            if (ShowClose) tp.Btn_Close.MouseDown(pt, e.Button);
                            if (ShowPin) tp.Btn_Pin.MouseDown(pt, e.Button);
                            ActivateTab(i);
                            tp.RefreshAllWidgets();
                            FoundTab = true;
                        }
                    }
                if (!FoundTab && ActiveTab != null)
                {
                    ActiveTab.Focus();
                    UpdateGraphics();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (ShowTab)
            {
                Point pt = new(e.X, e.Y);
                TabItem tp_close = null, tp_pin = null;
                lock (Tabs)
                    foreach (TabItem tp in Tabs)
                    {
                        Rectangle tabRect = tp.TabRect;
                        if (tp.MouseState == MouseState.Down && ShowTab && tp.ShowTab)
                        {
                            if (ShowClose && tp.Btn_Close.MouseUp(pt, e.Button)) tp_close = tp;
                            if (ShowPin && tp.Btn_Pin.MouseUp(pt, e.Button))
                            {
                                //Console.WriteLine("Pin is pressed");
                                tp_pin = tp;
                            }
                        }
                        tp.MouseState = (tabRect.Contains(pt) && ShowTab) ? MouseState.Hover : MouseState.Out;
                    }

                if (tp_close != null)
                    tp_close.Close();

                if (tp_pin != null)
                {
                    //Console.WriteLine("Pin setting is applied.");
                    //tp_pin.IsPinned = !tp_pin.IsPinned;
                    if (tp_pin.IsPinned)
                        tp_pin.Order = -1;
                    Sort();
                    Coordinate();
                }

                MouseState = MouseState.Hover;
                Invalidate(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            MouseState = MouseState.Out;
            lock (Tabs)
                foreach (TabItem tp in Tabs)
                {
                    tp.MouseState = MouseState.Out;
                    tp.Btn_Close.MouseLeave();
                    tp.Btn_Pin.MouseLeave();
                }
            CustomMouseLeave();
            Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected virtual bool CustomMouseMove(MouseEventArgs e) { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected virtual bool CustomMouseDown(MouseEventArgs e) { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected virtual bool CustomMouseUp(MouseEventArgs e) { return false; }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CustomMouseLeave() { }
        #endregion
    }
}