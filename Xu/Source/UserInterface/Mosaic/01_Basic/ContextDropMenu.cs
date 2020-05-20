/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Xu
{
    [DesignerCategory("Code")]
    public class ContextDropMenu : ContextMenuStrip
    {
        public ContextDropMenu() : base() { components = new Container(); }
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void AddSeparator()
        {
            ToolStripSeparator separator = new ToolStripSeparator();
            components.Add(separator);
            Items.Add(separator);
        }

        public void Add(ToolStripItem item)
        {
            components.Add(item);
            if (item != null)
                Items.Add(item);
        }

        public void AddRange(ToolStripItem[] items)
        {
            SuspendLayout();
            lock (items)
            {
                foreach (ToolStripItem t in items)
                {
                    Add(t);
                }
            }
            ResumeLayout(false);
            PerformLayout();
        }

        public void Clear()
        {
            SuspendLayout();
            List<ToolStripItem> items = new List<ToolStripItem>();
            foreach (ToolStripItem t in Items)
            {
                items.Add(t);
            }
            Items.Clear();
            ToolStripItem[] tss = items.ToArray();
            foreach (ToolStripItem t in tss)
            {
                t.Dispose();
            }
            ResumeLayout(false);
        }
    }
}
