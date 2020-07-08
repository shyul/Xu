/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Xu.Chart
{
    public class ContinuousAxis
    {
        public ContinuousAxis(IArea area, AlignType labelSide, AlignType align, double heightRatio)
        {
            HeightRatio = heightRatio;
            LabelSide = labelSide;
            Align = align;
            Area = area;
        }

        public readonly Dictionary<Importance, AxisTickStyle> Style = new Dictionary<Importance, AxisTickStyle>()
        {
            { Importance.Tiny,  new AxisTickStyle() },
            { Importance.Micro, new AxisTickStyle() },
            { Importance.Minor, new AxisTickStyle() },
            { Importance.Major, new AxisTickStyle() },
        };

        public IArea Area { get; protected set; }

        public readonly Range<double> Range = new Range<double>(double.MaxValue, double.MinValue);

        public double Delta => Range.Maximum - Range.Minimum;

        public double HeightRatio { get; set; } = 1;

        public AlignType Align { get; protected set; } = AlignType.Right;

        public AlignType LabelSide { get; protected set; } = AlignType.Left;

        public readonly Dictionary<double, (Importance Importance, string Label)> TickList = new Dictionary<double, (Importance Importance, string Label)>();

        public virtual void Reset()
        {
            Range.Reset(double.MaxValue, double.MinValue);
            TickList.Clear();
        }

        protected virtual double GetPixelRatio(double val)
        {
            if (Range.Maximum > Range.Minimum)
                return (val - Range.Minimum) / Delta;
            else
                return 0;
        }

        public virtual int ValueToPixel(double val)
        {
            return Align switch
            {
                AlignType.Left => (Pixel_Near + (Pixel_Count * GetPixelRatio(val))).ToInt32(),
                _ => (Pixel_Far - (Pixel_Count * GetPixelRatio(val))).ToInt32(),
            };
        }

        public virtual double PixelToValue(int pix)
        {
            return Align switch
            {
                AlignType.Left => Range.Minimum + ((pix - Pixel_Near) * Delta) / Pixel_Count,
                _ => Range.Minimum + ((Pixel_Far - pix) * Delta) / Pixel_Count,
            };
        }

        public virtual string PixelToString(int pix) => PixelToValue(pix).ToSINumberString("G4").String;

        public double[] TickDacades = new double[]
            { 0.1, 0.2, 0.25, 0.3, 0.4, 0.5, 0.6, 0.8, 1 };

        public virtual int MinimumTickHeight { get; set; } = 30;

        public int Actual_Size { get; set; }

        public int Pixel_Near { get; protected set; }

        public int Pixel_Far { get; protected set; }

        public int Pixel_Count => Pixel_Far - Pixel_Near;

        public virtual void Coordinate(int size, int offset)
        {
            switch (Align)
            {
                case (AlignType.Left):
                    Pixel_Near = offset;
                    Pixel_Far = Pixel_Near + size;
                    break;

                case (AlignType.Center):
                    int ct = offset + (size / 2.0).ToInt32();
                    Pixel_Near = ct - (size / 2.0).ToInt32();
                    Pixel_Far = ct + (size / 2.0).ToInt32();
                    break;

                case (AlignType.Right):
                    Pixel_Far = offset + size;
                    Pixel_Near = Pixel_Far - size;
                    break;
            }
        }

        public virtual void Draw(Graphics g, Rectangle bounds, int labelOffset)
        {
            foreach (var tk in TickList.OrderBy(n => n.Value.Importance))
            {
                AxisTickStyle style = Style[tk.Value.Importance];
                int location = ValueToPixel(tk.Key);

                if (style.HasLine)
                    g.DrawLine(style.Theme.EdgePen, bounds.Left, location, bounds.Right, location);

                if (location < bounds.Bottom && location > bounds.Top)
                    if (LabelSide == AlignType.Left)
                    {
                        g.DrawLine(Area.Theme.EdgePen, bounds.Left, location, bounds.Left - 2, location);

                        if (style.HasLabel) 
                        {
                            if(tk.Value.Importance > Importance.Minor) 
                            {
                            
                            }

                            g.DrawString(tk.Value.Label, style.Font, style.Theme.ForeBrush,
                                new Point(bounds.Left - labelOffset, location), AppTheme.TextAlignRight);


                        }
     
                    }
                    else
                    {
                        g.DrawLine(Area.Theme.EdgePen, bounds.Right, location, bounds.Right + 2, location);

                        if (style.HasLabel) 
                        {
                            if (tk.Value.Importance > Importance.Minor)
                            {
                                Size text_size = TextRenderer.MeasureText(tk.Value.Label, style.Font);
                                using GraphicsPath gp = ShapeTool.Tag(new Point(bounds.Right + labelOffset - 1 + text_size.Width / 2, location - 1), new Size(text_size.Width - 2, text_size.Height - 2), 1);
                                g.FillPath(Area.Chart.Theme.FillBrush, gp);
                                g.DrawPath(Area.Theme.EdgePen, gp);
                                g.DrawString(tk.Value.Label, style.Font, style.Theme.ForeBrush, new Point(bounds.Right + labelOffset, location), AppTheme.TextAlignLeft);
                            }
                            else
                            {
                                g.DrawString(tk.Value.Label, style.Font, style.Theme.ForeBrush,
                                    new Point(bounds.Right + labelOffset, location), AppTheme.TextAlignLeft);
                            }
                        }
                    
                    }
            }
        }
    }
}
