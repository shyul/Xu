/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Xu.Chart
{
    public class OscillatorArea : Area
    {
        public OscillatorArea(ChartWidget chart, string name, float heightRatio)
        {
            Name = name;
            HeightRatio = heightRatio;
            Chart = chart;

            AxisLeft.Style[Importance.Minor].Font = Main.Theme.TinyFont;
            AxisLeft.Style[Importance.Minor].HasLabel = true;
            AxisLeft.Style[Importance.Minor].HasLine = false;

            AxisRight.Style[Importance.Minor].Font = Main.Theme.TinyFont;
            AxisRight.Style[Importance.Minor].HasLabel = true;
            AxisRight.Style[Importance.Minor].HasLine = true;
            AxisRight.Style[Importance.Minor].Theme.EdgePen.DashPattern = new float[] { 1, 2 };

            AxisRight.Style[Importance.Major].Font = Main.Theme.TinyFont;
            AxisRight.Style[Importance.Major].HasLabel = true;
            AxisRight.Style[Importance.Major].HasLine = true;

            AxisRight.Style[Importance.Major].Theme.EdgePen.Color = Color.FromArgb(96, 96, 96);
            AxisRight.Style[Importance.Major].Theme.EdgePen.DashPattern = new float[] { 5, 2, 5 };
            AxisRight.MinimumTickHeight = 20;
        }

        public double FixedTickStep_Right { get; set; } = double.NaN;

        public double FixedTickStep_Left { get; set; } = double.NaN;

        public double Reference { get; set; } = 0;

        public double UpperLimit { get; set; } = double.NaN;

        public double LowerLimit { get; set; } = double.NaN;

        public Color UpperColor
        {
            get
            {
                return UpperTheme.EdgeColor;
            }
            set
            {
                UpperTheme.ForeColor = value.Opaque(255);
                UpperTheme.FillColor = value.Opaque(127);
                UpperTheme.EdgeColor = value.Opaque(255);

                UpperTextTheme.EdgeColor = value.Opaque(255);
                UpperTextTheme.FillColor = value.Opaque(255).Brightness(0.5f);
                UpperTextTheme.ForeColor = (UpperTextTheme.FillColor.GetBrightness() < 0.8) ? Color.White : Color.DimGray;
            }
        }

        public Color LowerColor
        {
            get
            {
                return LowerTheme.EdgeColor;
            }
            set
            {
                LowerTheme.ForeColor = value.Opaque(255);
                LowerTheme.FillColor = value.Opaque(127);
                LowerTheme.EdgeColor = value.Opaque(255);

                LowerTextTheme.EdgeColor = value.Opaque(255);
                LowerTextTheme.FillColor = value.Opaque(255).Brightness(0.5f);
                LowerTextTheme.ForeColor = (LowerTextTheme.FillColor.GetBrightness() < 0.8) ? Color.White : Color.DimGray;
            }
        }

        public ColorTheme UpperTheme { get; } = new ColorTheme();

        public ColorTheme LowerTheme { get; } = new ColorTheme();

        public ColorTheme UpperTextTheme { get; } = new ColorTheme();

        public ColorTheme LowerTextTheme { get; } = new ColorTheme();

        public Rectangle UpperCanvas { get; set; }

        public Rectangle LowerCanvas { get; set; }

        public int UpperPix => UpperCanvas.Bottom;

        public int LowerPix => LowerCanvas.Top;

        public bool IsGradient { get; set; } = true; //false;

        public override void Coordinate()
        {
            RightCursorX = Right - 4; // AxisX.HalfTickWidth;
            LeftCursorX = Left + 4; // AxisX.HalfTickWidth;

            /// Reset and inflate axis' range 
            AxisLeft.Reset();
            AxisRight.Reset();

            lock (Series)
                foreach (Series ser in Series)
                {
                    ser.RefreshAxis(this, Table);
                }

            AxisRight.Coordinate(Height, Top);  // Setup the right axis which is the master axis.
            int actual_size_right = (AxisRight.HeightRatio * Height).ToInt32();
            int tickCount_right = (1.0 * actual_size_right / AxisRight.MinimumTickHeight).ToInt32(); // It needs at least 10 pixel for a tick
            double tickStep_right = AxisRight.Delta / tickCount_right;

            if (tickStep_right > 0)
            {
                if (!double.IsNaN(Reference))
                {
                    AxisRight.Range.Insert(Reference);
                    AxisRight.TickList.CheckAdd(Reference, (Importance.Major, Reference.ToSINumberString("0.##").String));
                }

                if (!double.IsNaN(UpperLimit))
                {
                    AxisRight.Range.Insert(UpperLimit);
                    AxisRight.TickList.CheckAdd(UpperLimit, (Importance.Major, UpperLimit.ToSINumberString("0.##").String));
                }

                if (!double.IsNaN(LowerLimit))
                {
                    AxisRight.Range.Insert(LowerLimit);
                    AxisRight.TickList.CheckAdd(LowerLimit, (Importance.Major, LowerLimit.ToSINumberString("0.##").String));
                }

                if (!double.IsNaN(FixedTickStep_Right))
                {
                    tickStep_right = FixedTickStep_Right;
                }
                else
                {
                    tickStep_right = tickStep_right.FitDacades(AxisRight.TickDacades);
                }

                int halfTickNumber = (Math.Max(AxisRight.Range.Maximum - Reference, Reference - AxisRight.Range.Minimum) / tickStep_right).ToInt32() + 1;

                for (int i = 1; i < halfTickNumber + 1; i++)
                {
                    double val = Reference + tickStep_right * i;
                    AxisRight.TickList.CheckAdd(val, (Importance.Minor, val.ToSINumberString("0.##").String));
                    AxisRight.Range.Insert(val);

                    val = Reference - tickStep_right * i;
                    AxisRight.TickList.CheckAdd(val, (Importance.Minor, val.ToSINumberString("0.##").String));
                    AxisRight.Range.Insert(val);
                }
            }
            else if (AxisRight.Delta == 0)
            {
                AxisRight.Range.Insert(1);
                AxisRight.Range.Insert(-1);
            }

            AxisLeft.Coordinate(Height, Top);
            int actual_size_left = (AxisLeft.HeightRatio * Height).ToInt32();
            int tickCount_left = (1.0 * actual_size_left / AxisLeft.MinimumTickHeight).ToInt32(); // It needs at least 10 pixel for a tick
            double tickStep_left = AxisLeft.Delta / tickCount_left;
            //Console.WriteLine("AxisLeft.Range = " + AxisLeft.Range.ToString());

            if (tickStep_left > 0)
            {
                if (!double.IsNaN(Reference))
                {
                    AxisLeft.Range.Insert(Reference);
                    AxisLeft.TickList.CheckAdd(Reference, (Importance.Major, Reference.ToSINumberString("0.##").String));
                }

                if (!double.IsNaN(UpperLimit))
                {
                    AxisLeft.Range.Insert(UpperLimit);
                    AxisLeft.TickList.CheckAdd(UpperLimit, (Importance.Major, UpperLimit.ToSINumberString("0.##").String));
                }

                if (!double.IsNaN(LowerLimit))
                {
                    AxisLeft.Range.Insert(LowerLimit);
                    AxisLeft.TickList.CheckAdd(LowerLimit, (Importance.Major, LowerLimit.ToSINumberString("0.##").String));
                }

                if (!double.IsNaN(FixedTickStep_Left))
                {
                    tickStep_left = FixedTickStep_Left;
                }
                else
                {
                    tickStep_left = tickStep_left.FitDacades(AxisLeft.TickDacades);
                }

                int halfTickNumber = (Math.Max(AxisLeft.Range.Maximum - Reference, Reference - AxisLeft.Range.Minimum) / tickStep_left).ToInt32() + 1;

                for (int i = 1; i < halfTickNumber + 1; i++)
                {
                    double val = Reference + tickStep_left * i;
                    AxisLeft.TickList.CheckAdd(val, (Importance.Minor, val.ToSINumberString("0.##").String));
                    AxisLeft.Range.Insert(val);

                    val = Reference - tickStep_left * i;
                    AxisLeft.TickList.CheckAdd(val, (Importance.Minor, val.ToSINumberString("0.##").String));
                    AxisLeft.Range.Insert(val);
                }
            }
            else if (AxisLeft.Delta == 0) 
            {
                AxisLeft.Range.Insert(1);
                AxisLeft.Range.Insert(-1);
            }

            // *****************************
            // Calculate Graphics Coordinate
            // *****************************

            if (!double.IsNaN(UpperLimit))
            {
                int limit_pix = AxisRight.ValueToPixel(UpperLimit);
                UpperCanvas = new Rectangle(Left, Top, Width, limit_pix - Top + 1);
            }

            if (!double.IsNaN(LowerLimit))
            {
                int limit_pix = AxisRight.ValueToPixel(LowerLimit);
                LowerCanvas = new Rectangle(Left, limit_pix, Width, Bottom - limit_pix);
            }

            // *****************************
            // Update Legend
            // *****************************

            UpdateLegend();
        }

        public void DrawLimitShade(Graphics g, GraphicsPath line)
        {
            /*
            int firstIndex = 0;
            if (StartPt < 0) firstIndex = -StartPt; // Fixing the empty filled pointers for the start blank area...
            int first_x = IndexToPixel(firstIndex);
            int last_x = IndexToPixel(pt - 1);*/

            RectangleF bounds = line.GetBounds();
            int first_x = bounds.Left.ToInt32();
            int last_x = bounds.Right.ToInt32();// + 1;
            int max_y = bounds.Top.ToInt32();
            int min_y = bounds.Bottom.ToInt32();

            if (!double.IsNaN(UpperLimit))
            {
                using GraphicsPath fillpath = new GraphicsPath();
                fillpath.AddPath(line, true);
                fillpath.AddLines(new Point[] { new Point(last_x, max_y), new Point(last_x, Bottom), new Point(first_x, Bottom) });
                g.SetClip(UpperCanvas);
                if (IsGradient)
                {
                    int height = UpperCanvas.Height - Math.Abs(max_y - UpperCanvas.Top);
                    if (height > 0)
                    {
                        using LinearGradientBrush br = new LinearGradientBrush(
                            new Rectangle(UpperCanvas.X, max_y, UpperCanvas.Width, height),
                            UpperColor.Opaque(255),
                            UpperColor.Opaque(100),
                            LinearGradientMode.Vertical);
                        g.FillPath(br, fillpath);
                    }
                }
                else
                {
                    using SolidBrush br = new SolidBrush(UpperColor);
                    g.FillPath(UpperTheme.FillBrush, fillpath);
                }
                g.ResetClip();
            }

            if (!double.IsNaN(LowerLimit))
            {
                using GraphicsPath fillpath = new GraphicsPath();
                fillpath.AddPath(line, true);
                fillpath.AddLines(new Point[] { new Point(last_x, min_y), new Point(last_x, Top), new Point(first_x, Top) });
                g.SetClip(LowerCanvas);
                if (IsGradient)
                {
                    int height = LowerCanvas.Height - Math.Abs(min_y - LowerCanvas.Bottom);
                    if (height > 0)
                    {
                        using LinearGradientBrush br = new LinearGradientBrush(
                            new Rectangle(LowerCanvas.X, LowerCanvas.Y, LowerCanvas.Width, height),
                            LowerColor.Opaque(100),
                            LowerColor.Opaque(255),
                            LinearGradientMode.Vertical);
                        g.FillPath(br, fillpath);
                    }
                }
                else
                {
                    using SolidBrush br = new SolidBrush(LowerColor);
                    g.FillPath(LowerTheme.FillBrush, fillpath);
                }
                g.ResetClip();
            }
        }
    }
}