/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using System.Drawing.Drawing2D;

namespace Xu
{
    /*
    public enum BrushType 
    {
        Solid,
        Texture,
        HatchBrush,
        LinearGradient,
        PathGradient,
    }
    */
    public class ColorTheme
    {
        public ColorTheme() { }

        public ColorTheme(Color fore, Color fill)
        {
            ForeColor = fore;
            FillColor = fill;
            EdgeColor = fore;
        }

        public ColorTheme(Color fore, Color fill, Color edge, int foreWidth = 1, int fillWidth = 1, int edgeWidth = 1)
        {
            ForeColor = fore;
            FillColor = fill;
            EdgeColor = edge;
            ForeWidth = foreWidth;
            FillWidth = fillWidth;
            EdgeWidth = edgeWidth;
        }

        /*
        public BrushType BrushType { get; set; }

        public void ConfigBrush()
        {
            switch (BrushType)
            {
                case (BrushType.Texture):
                    break;
                case (BrushType.HatchBrush):
                    break;
                case (BrushType.LinearGradient):
                    break;
                case (BrushType.PathGradient):
                    break;
                default:
                case (BrushType.Solid):
                    break;
            }
        }
        */

        public Color ForeColor { get { return ForePen.Color; } set { ForePen.Color = value; ForeBrush.Color = value; } } //= Color.DimGray;

        public Color FillColor { get { return FillPen.Color; } set { FillPen.Color = value; FillBrush.Color = value; } } //= Color.White;

        public Color EdgeColor { get { return EdgePen.Color; } set { EdgePen.Color = value; EdgeBrush.Color = value; } } //= Color.DimGray;


        public float ForeWidth { get { return ForePen.Width; } set { ForePen.Width = value; } }


        public float FillWidth { get { return FillPen.Width; } set { FillPen.Width = value; } }


        public float EdgeWidth { get { return EdgePen.Width; } set { EdgePen.Width = value; } }


        public readonly SolidBrush ForeBrush = new(Color.DimGray);


        public readonly Pen ForePen = new(Color.DimGray) { StartCap = LineCap.Round, EndCap = LineCap.Round };


        public readonly SolidBrush FillBrush = new(Color.White);


        public readonly Pen FillPen = new(Color.White);


        public readonly SolidBrush EdgeBrush = new(Color.DimGray);


        public readonly Pen EdgePen = new(Color.DimGray) { StartCap = LineCap.Round, EndCap = LineCap.Round };
    }
}
