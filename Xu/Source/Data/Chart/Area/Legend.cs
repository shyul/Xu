/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Xu.Chart
{
    /// <summary>
    /// The Legend in each area explaining the meanings of each series in that area
    /// Belongs to each series
    /// Drawing function, and cordinations
    /// Components:
    /// 1. Icon -> Cursor or last
    /// 2. Name of the series
    /// 3. Snapshot the data
    /// </summary>
    public class Legend : IArea
    {
        public Legend(Area area, string name, AlignType side, Importance importance, Size size)
        {
            Area = area;
            Name = name;
            AxisX = new DiscreteAxis(Area.Chart) { IndexCount = 5 };
            AxisRight = new ContinuousAxis(this, AlignType.Right, AlignType.Right, 1.0);
            Side = side;
            Importance = importance;
            Bounds = new Rectangle(new Point(), size);
        }

        public ChartWidget Chart => Area.Chart;

        protected Area Area;

        // Should generalize the drawing function in each series type to make it possible for Legend Icon.
        public string Name { get; set; }

        public string Label { get; set; }

        public Importance Importance { get; protected set; }

        public bool Enabled { get; set; } = true;

        public int Order { get; set; }

        public AlignType Side { get; protected set; }

        public virtual ColorTheme Theme => Area.Theme;

        public readonly List<Series> Series = new List<Series>();

        public int StopPt { get; set; }

        public int StartPt { get; set; }

        public int SelectedDataPoint => StopPt - 1;

        public int SelectedIndexPixel => IndexToPixel(IndexCount - 1);

        public virtual int IndexCount => AxisX.IndexCount;

        public DiscreteAxis AxisX { get; } //= new DiscreteAxis() { IndexCount = 5 };

        public Rectangle Bounds { get; set; } = new Rectangle();
        public Point Location { get => Bounds.Location; set { Bounds = new Rectangle(value, Bounds.Size); } }
        public Size Size { get => Bounds.Size; set { Bounds = new Rectangle(Bounds.Location, value); } }
        public int Top => Bounds.Top;
        public int Bottom => Bounds.Bottom;
        public int Left => Bounds.Left;
        public int Right => Bounds.Right;
        public int Height => Bounds.Height;
        public int Width => Bounds.Width;
        public Point Center => new Point((Left + Right) / 2, (Top + Bottom) / 2);
        public int LeftCursorX => Bounds.Left;
        public int RightCursorX => Bounds.Right;

        private readonly ContinuousAxis AxisRight;
        public ContinuousAxis AxisY(AlignType side) => AxisRight;

        public int IndexToPixel(int index) => AxisX.IndexToPixel(index) + Left;

        protected readonly Dictionary<Point, (string text, Font font, Brush brush)> ValueLabels = new Dictionary<Point, (string text, Font font, Brush brush)>();

        public readonly List<Rectangle> LabelBound = new List<Rectangle>();

        protected Point Origin => new Point(Right + 3, Top);

        public virtual void Coordinate(ITable table)
        {
            // Set Axis X
            AxisRight.Reset();
            foreach (Series ser in Series)
            {
                ser.RefreshAxis(this, table);

            }
            AxisRight.Coordinate(Height, Top);
            AxisX.Coordinate(Width);

            Point location = Origin;

            LabelBound.Clear();
            ValueLabels.Clear();

            int i = 0, labelWidth = 0, labelHeight = 0;
            foreach (Series ser in Series.OrderBy(n => n.Order).OrderByDescending(n => n.Importance).OrderBy(n => n.Side))
            {
                var labels = ser.ValueLabels(table, StopPt - 1);

                if (Importance == Importance.Huge)
                {
                    if (i == 0)
                    {
                        // Get the title area
                        Size textSize = TextRenderer.MeasureText(Label, Main.Theme.TitleFont);
                        ValueLabels.CheckAdd(location, (Label, Main.Theme.TitleFont, Area.Theme.ForeBrush));
                        LabelBound.Add(new Rectangle(location, textSize)); // Text top right corner
                        location.Y += textSize.Height - 2;
                    }

                    foreach ((string text, Font font, Brush brush) in labels)
                    {
                        Size textSize = TextRenderer.MeasureText(text, font);
                        ValueLabels.CheckAdd(location, (text, font, brush));

                        location.X += textSize.Width;
                        labelWidth += textSize.Width;
                        if (labelHeight < textSize.Height) labelHeight = textSize.Height;
                    }
                }
                else
                {
                    int j = 0;
                    foreach ((string text, Font font, Brush brush) in labels)
                    {
                        Size textSize;
                        if (i == 0 && j == 0)
                        {
                            Font titleFont = (ser.Importance > Importance.Minor) ? Main.Theme.FontBold : Main.Theme.Font;
                            string firstLabel = Label + text;
                            textSize = TextRenderer.MeasureText(firstLabel, titleFont);
                            ValueLabels.CheckAdd(location, (firstLabel, titleFont, brush));
                        }
                        else
                        {
                            textSize = TextRenderer.MeasureText(text, font);
                            ValueLabels.CheckAdd(location, (text, font, brush));
                        }

                        location.X += textSize.Width;
                        labelWidth += textSize.Width;
                        if (labelHeight < textSize.Height) labelHeight = textSize.Height;
                        j++;
                    }
                }

                i++;
            }

            LabelBound.Add(new Rectangle(Origin.X, location.Y, labelWidth + 5, labelHeight));
        }

        public virtual void Draw(Graphics g, ITable table)
        {
            /*
            using SolidBrush backBrush = new SolidBrush(Area.Chart.Theme.FillColor.Opaque(220));
            g.FillPath(backBrush, BackgroundPath);*/

            g.FillRectangle(Area.Chart.Theme.FillBrush, Bounds);
            g.DrawRectangle(Theme.EdgePen, Bounds);

            foreach (Series ser in Series)
            {
                g.SetClip(Bounds);
                ser.Draw(g, this, table);
                g.ResetClip();
            }

            foreach (Point pt in ValueLabels.Keys)
            {
                (string text, Font font, Brush brush) = ValueLabels[pt];
                g.DrawString(text, font, brush, pt, AppTheme.TextAlignDefault);
            }
        }

        public static SolidBrush LabelBrush(ColorTheme theme)
        {
            Color c = (theme.ForeColor.GetBrightness() > theme.EdgeColor.GetBrightness()) ?
                theme.EdgeColor.Opaque(255) : theme.ForeColor.Opaque(255);

            if (c.GetBrightness() > 0.5) c = c.Brightness(-0.2f);

            return new SolidBrush(c);
        }
    }
}
