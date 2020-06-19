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
using System.Windows.Media.TextFormatting;

namespace Xu.GridView
{
    public class StringGridStripe : GridStripe
    {
        public StringColumn Column { get; set; }

        public override void Draw(Graphics g, Rectangle bound, ITable table, int index)
        {
            if (table is IStringTable ct)
            {
                string value = ct[index, Column]; // It might be some other object
                g.DrawString(value, Main.Theme.Font, Theme.ForeBrush, bound.Location);
            }
        }
    }
}
