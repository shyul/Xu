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

namespace Xu.GridView
{
    public interface IGridRenderer
    {
        int MinimumHeight { get; set; }

        int Width { get; set; }

        bool AutoWidth { get; set; }

        bool Visible { get; set; }

        void Draw(Graphics g, Rectangle bound, object obj);
    }
}
