﻿/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Drawing;

namespace Xu.GridView
{
    public class TextGridRenderer : IGridRenderer
    {
        public Font Font => Main.Theme.Font;

        public ColorTheme Theme { get; } = new ColorTheme();

        public bool AutoWidth { get; set; } = false;

        public int Width { get; set; } = 60;

        public int MinimumHeight { get; set; } = 22;

        public bool Visible { get; set; } = true;

        public void Draw(Graphics g, Rectangle bound, object obj)
        {
            if (obj is null) return;
            g.DrawString(obj.ToString(), Main.Theme.Font, Theme.ForeBrush, bound.Location);
        }
    }
}
