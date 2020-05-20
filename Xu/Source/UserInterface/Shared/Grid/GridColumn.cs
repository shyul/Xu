/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System.Drawing;

namespace Xu
{
    public class GridColumn
    {
        public bool Enabled { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }



        public Importance Importance { get; set; }

        public ColorTheme Theme { get; set; }

        /// <summary>
        /// Exact Pixel or proportional
        /// </summary>
        public (bool X, bool Y) IsExactPixel { get; set; } = (false, false);

       
        public virtual Size MinimumSize { get; } = new Size();

        public virtual Size Size { get; set; } = new Size();

        public bool Hidden { get; set; } = false;

       

        public virtual ICollection<Rectangle> Bounds { get; }

        public virtual void Coordinate(Rectangle area) 
        {
        
        
        }

        public virtual void Draw(Graphics g, int pt) 
        {
        
        }
    }
}
