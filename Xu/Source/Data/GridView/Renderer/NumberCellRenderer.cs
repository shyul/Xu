/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Drawing;
using System.Security.Permissions;

namespace Xu.GridView
{
    public class NumberCellRenderer : IDataCellRenderer
    {
        public Font Font => Main.Theme.Font;

        public ColorTheme Theme { get; } = new ColorTheme();

        public bool AutoWidth { get; set; } = false;

        public int Width { get; set; } = 60;

        public int MinimumHeight { get; set; } = 22;

        public bool Visible { get; set; } = true;

        public string Format { get; set; } = "0.###";

        public void Draw(Graphics g, Rectangle bound, object obj)
        {
            string s = "-";

            if (obj is int d0)
            {
                s = d0.ToString(Format);
            }
            else if (obj is double d1 && (!double.IsNaN(d1))) 
            {
                s = d1.ToString(Format);
            }

            g.DrawString(s, Main.Theme.Font, Theme.ForeBrush, bound.Center(), AppTheme.TextAlignCenter);
        }
    }
}
