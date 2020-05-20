/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xu
{
    public abstract class GridWidget : Widget
    {
        public virtual int StartPt { get; set; } = 0;

        public virtual int StopPt { get; set; } = 295;


        public virtual ICollection<GridColumn> Columns { get; }

        public override void Coordinate()
        {
            var columnList = Columns.Where(n => n.Enabled)
            .OrderByDescending(n => n.Importance)
            .ThenBy(n => n.Order)
            .ThenByDescending(n => n.MinimumSize.Width);

            //Func<string, string, string, double> func;


            /// Draw ???
            /// Edit ???

            int width = 0, totalPropPix = 0, totalPropAvail = Width;
            List<GridColumn> columnsToShow = new List<GridColumn>();

            // Coordinate all Rectangles
            foreach (GridColumn gc in columnList)
            {
                width += gc.MinimumSize.Width;
                if (width < Width)
                {
                    columnsToShow.Add(gc);
                    gc.Hidden = false;

                    if (!gc.IsExactPixel.X)
                    {
                        totalPropPix += gc.MinimumSize.Width;
                    }
                    else
                    {
                        totalPropAvail -= gc.MinimumSize.Width;
                    }
                }
                else
                {
                    gc.Hidden = true;
                }
            }

            if (totalPropAvail < totalPropPix) throw new Exception("Grid Propotion Calculation is Wrong!");

            var colum = columnsToShow.Where(n => !n.IsExactPixel.X);

            int maxRowHeight = 0;
            foreach (GridColumn gc in colum)
            {
                gc.Size = new Size(gc.MinimumSize.Width * totalPropAvail / totalPropPix, gc.Size.Height);
                if (maxRowHeight < gc.MinimumSize.Height) maxRowHeight = gc.MinimumSize.Height;
            }

            //int numRow = Height / maxRowHeight; // Warning: do not divide by zero.

            // How to get the minimum cell height
            for (int i = StartPt; i < StopPt; i++)
            {


            }


        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (IsActive)
            {
                Graphics g = pe.Graphics;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                for (int i = 50; i < Height; i += 50)
                {
                    int y = i;// (i * Height / 10D).ToInt32();
                    g.DrawLine(new Pen(Color.Orange, 1), new Point(0, y), new Point(Width, y));
                }

                for (int i = 0; i < 12; i++)
                {
                    int x = (i * Width / 12D).ToInt32();
                    g.DrawLine(new Pen(Color.Green, 1), new Point(x, 0), new Point(x, Height));
                }
            }
        }
    }
}
