/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Xu.Chart
{
    public class AxisTickStyle
    {
        public ColorTheme Theme { get; set; } = new ColorTheme(Color.FromArgb(120, 120, 120), Color.FromArgb(210, 210, 210), Color.FromArgb(210, 210, 210));

        public Font Font { get; set; } = Main.Theme.Font;

        public bool HasLabel { get; set; } = false;

        public bool HasLine { get; set; } = false;
    }
}
