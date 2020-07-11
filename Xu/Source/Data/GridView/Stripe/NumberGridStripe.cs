/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;

namespace Xu.GridView
{
    public class NumberGridStripe : GridStripe
    {
        public NumericColumn Column { get; set; }

        public override void Draw(Graphics g, Rectangle bound, ITable table, int index)
        {
            double value = table[index, Column];
            g.DrawString(value.ToString(), Main.Theme.Font, Theme.ForeBrush, bound.Location);
        }
    }
}
