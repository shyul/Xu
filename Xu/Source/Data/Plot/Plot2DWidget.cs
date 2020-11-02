/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace Xu.Plot
{
    public abstract class Plot2DWidget : DockForm
    {
        protected Plot2DWidget(string name) : base(name, true)
        {
            HasIcon = true;
            Btn_Pin.Enabled = true;
            Btn_Close.Enabled = true;


        }
    }

    public interface ITrace
    {
        (double X, double Y) this[int i] { get; }
    }

    public class Area
    {
        public ContinuousAxis AxixX { get; }

        public ContinuousAxis AxixY { get; }
    }

    public class ContinuousAxis
    {
        public bool IsLogarithmic { get; set; } = false;

        public AlignType Align { get; protected set; } = AlignType.Right;

        public Range<double> Range { get; } = new Range<double>(double.MaxValue, double.MinValue);

        public double Delta => Range.Maximum - Range.Minimum;

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

        public int Pixel_Near { get; protected set; }

        public int Pixel_Far { get; protected set; }

        public int Pixel_Count => Pixel_Far - Pixel_Near;

        public virtual void Coordinate(int size, int offset)
        {
            switch (Align)
            {
                case AlignType.Left:
                    Pixel_Near = offset;
                    Pixel_Far = Pixel_Near + size;
                    break;

                default:
                case AlignType.Right:
                    Pixel_Far = offset + size;
                    Pixel_Near = Pixel_Far - size;
                    break;
            }
        }
    }
}
