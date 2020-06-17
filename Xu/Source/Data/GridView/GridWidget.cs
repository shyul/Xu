/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace Xu.Chart
{
    public abstract class GridWidget : DockTab
    {
        protected GridWidget(string name) : base(name)
        {

        }

        public virtual ColorTheme Theme { get; } = new ColorTheme();

        public virtual string Label { get; set; }

        public virtual string Description { get; set; }

        public abstract ITable Table { get; }

        public virtual int DataCount => Table.Count;

        public virtual int StartPt => StopPt - IndexCount;

        public virtual int StopPt { get; set; } // = 295;

        public virtual int IndexCount { get; set; } = 295;

        #region Coordinate

        public bool ReadyToShow { get; set; } = false;

        protected override void CoordinateLayout()
        {
            ResumeLayout(true);
            if (IsActive && ReadyToShow && Table is ITable)
                lock (GraphicsObjectLock)
                {


                }
            PerformLayout();
        }

        #endregion Coordinate
    }
}
