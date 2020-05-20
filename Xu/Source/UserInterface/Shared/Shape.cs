/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Xu
{
    public static class ShapeTool
    {
        #region Rectangle

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static GraphicsPath ToRoundRect(this Rectangle rect, int radius)
        {
            if (radius > 0)
                return rect.ToRoundRect(radius, radius, true, true, true, true);
            else
                return rect.ToRoundRect(0, 0, false, false, false, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="xradius"></param>
        /// <param name="yradius"></param>
        /// <param name="round_ul"></param>
        /// <param name="round_ur"></param>
        /// <param name="round_lr"></param>
        /// <param name="round_ll"></param>
        /// <returns></returns>
        public static GraphicsPath ToRoundRect(this Rectangle rect,
            int xradius, int yradius,
            bool round_ul, bool round_ur, bool round_lr, bool round_ll)
        {
            // Make a GraphicsPath to draw the rectangle.
            Point point1, point2;
            GraphicsPath path = new GraphicsPath();

            // Upper left corner.
            if (round_ul)
            {
                Rectangle corner = new Rectangle(rect.Left, rect.Top, 2 * xradius, 2 * yradius);
                path.AddArc(corner, 180, 90);
                point1 = new Point(rect.Left + xradius, rect.Top);
            }
            else point1 = new Point(rect.Left, rect.Top);

            // Top side.
            if (round_ur)
                point2 = new Point(rect.Right - xradius, rect.Top);
            else
                point2 = new Point(rect.Right, rect.Top);
            path.AddLine(point1, point2);

            // Upper right corner.
            if (round_ur)
            {
                Rectangle corner = new Rectangle(rect.Right - 2 * xradius, rect.Top, 2 * xradius, 2 * yradius);
                path.AddArc(corner, 270, 90);
                point1 = new Point(rect.Right + 1, rect.Top + yradius);
            }
            else point1 = new Point(rect.Right + 1, rect.Top);

            // Right side.
            if (round_lr)
                point2 = new Point(rect.Right + 1, rect.Bottom - yradius + 1);
            else
                point2 = new Point(rect.Right + 1, rect.Bottom + 1);
            path.AddLine(point1, point2);

            // Lower right corner.
            if (round_lr)
            {
                Rectangle corner = new Rectangle(rect.Right - 2 * xradius, rect.Bottom - 2 * yradius, 2 * xradius, 2 * yradius);
                path.AddArc(corner, 0, 90);
                point1 = new Point(rect.Right - xradius + 1, rect.Bottom + 1);
            }
            else point1 = new Point(rect.Right + 1, rect.Bottom + 1);

            // Bottom side.
            if (round_ll)
                point2 = new Point(rect.Left + xradius, rect.Bottom + 1);
            else
                point2 = new Point(rect.Left, rect.Bottom + 1);
            path.AddLine(point1, point2);

            // Lower left corner.
            if (round_ll)
            {
                Rectangle corner = new Rectangle(rect.Left, rect.Bottom - 2 * yradius, 2 * xradius, 2 * yradius);
                path.AddArc(corner, 90, 90);
                point1 = new Point(rect.Left, rect.Bottom - yradius + 1);
            }
            else point1 = new Point(rect.Left, rect.Bottom + 1);

            // Left side.
            if (round_ul)
                point2 = new Point(rect.Left, rect.Top + yradius);
            else
                point2 = new Point(rect.Left, rect.Top);
            path.AddLine(point1, point2);

            // Join with the start point.
            path.CloseFigure();

            return path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static GraphicsPath ToRoundRect(this RectangleF rect, float radius) => rect.ToRoundRect(radius, radius, true, true, true, true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="xradius"></param>
        /// <param name="yradius"></param>
        /// <param name="round_ul"></param>
        /// <param name="round_ur"></param>
        /// <param name="round_lr"></param>
        /// <param name="round_ll"></param>
        /// <returns></returns>
        public static GraphicsPath ToRoundRect(this RectangleF rect,
            float xradius, float yradius,
            bool round_ul, bool round_ur, bool round_lr, bool round_ll)
        {
            // Make a GraphicsPath to draw the rectangle.
            PointF point1, point2;
            GraphicsPath path = new GraphicsPath();

            // Upper left corner.
            if (round_ul)
            {
                RectangleF corner = new RectangleF(rect.Left, rect.Top, 2 * xradius, 2 * yradius);
                path.AddArc(corner, 180, 90);
                point1 = new PointF(rect.Left + xradius, rect.Top);
            }
            else point1 = new PointF(rect.Left, rect.Top);

            // Top side.
            if (round_ur)
                point2 = new PointF(rect.Right - xradius, rect.Top);
            else
                point2 = new PointF(rect.Right, rect.Top);
            path.AddLine(point1, point2);

            // Upper right corner.
            if (round_ur)
            {
                RectangleF corner = new RectangleF(rect.Right - 2 * xradius, rect.Top, 2 * xradius, 2 * yradius);
                path.AddArc(corner, 270, 90);
                point1 = new PointF(rect.Right + 1, rect.Top + yradius);
            }
            else point1 = new PointF(rect.Right + 1, rect.Top);

            // Right side.
            if (round_lr)
                point2 = new PointF(rect.Right + 1, rect.Bottom - yradius + 1);
            else
                point2 = new PointF(rect.Right + 1, rect.Bottom + 1);
            path.AddLine(point1, point2);

            // Lower right corner.
            if (round_lr)
            {
                RectangleF corner = new RectangleF(rect.Right - 2 * xradius, rect.Bottom - 2 * yradius, 2 * xradius, 2 * yradius);
                path.AddArc(corner, 0, 90);
                point1 = new PointF(rect.Right - xradius + 1, rect.Bottom + 1);
            }
            else point1 = new PointF(rect.Right + 1, rect.Bottom + 1);

            // Bottom side.
            if (round_ll)
                point2 = new PointF(rect.Left + xradius, rect.Bottom + 1);
            else
                point2 = new PointF(rect.Left, rect.Bottom + 1);
            path.AddLine(point1, point2);

            // Lower left corner.
            if (round_ll)
            {
                RectangleF corner = new RectangleF(rect.Left, rect.Bottom - 2 * yradius, 2 * xradius, 2 * yradius);
                path.AddArc(corner, 90, 90);
                point1 = new PointF(rect.Left, rect.Bottom - yradius + 1);
            }
            else point1 = new PointF(rect.Left, rect.Bottom + 1);

            // Left side.
            if (round_ul)
                point2 = new PointF(rect.Left, rect.Top + yradius);
            else
                point2 = new PointF(rect.Left, rect.Top);
            path.AddLine(point1, point2);

            // Join with the start point.
            path.CloseFigure();

            return path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Rectangle Shrink(this Rectangle rect, int value)
        {
            if (value > rect.Width / 2) value = rect.Width / 2;
            if (value > rect.Height / 2) value = rect.Height / 2;
            return new Rectangle(rect.X + value, rect.Y + value, rect.Width - 2 * value, rect.Height - 2 * value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="radius"></param>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        public static void FillRectangleStyle2010(this Graphics g, Rectangle rect, int radius, Color c1, Color c2)
        {
            //rectangle for gradient, half upper and lower
            RectangleF halfup = new RectangleF(rect.Left, rect.Top, rect.Width, rect.Height);
            RectangleF halfdown = new RectangleF(rect.Left, rect.Top + (rect.Height / 2) - 1, rect.Width, rect.Height);

            using GraphicsPath thePath = rect.ToRoundRect(radius);
            // Paint Background
            using (LinearGradientBrush lgb = new LinearGradientBrush(halfup, c2, c1, 90f, true))
            {
                Blend blend = new Blend(4)
                {
                    Positions = new float[] { 0, 0.18f, 0.35f, 1f },
                    Factors = new float[] { 0f, .4f, .9f, 1f }
                };

                lgb.Blend = blend;
                g.FillPath(lgb, thePath);
                lgb.Dispose();
                //for half lower, we paint using radial gradient
                using GraphicsPath p = new GraphicsPath();
                p.AddEllipse(halfdown); //make it radial
                using PathGradientBrush gradient = new PathGradientBrush(p)
                {
                    WrapMode = WrapMode.Clamp,
                    CenterPoint = new PointF(Convert.ToSingle(halfdown.Left + halfdown.Width / 2), Convert.ToSingle(halfdown.Bottom)),
                    CenterColor = c2,
                    SurroundColors = new Color[] { c1 }
                };

                blend = new Blend(4)
                {
                    Positions = new float[] { 0, 0.15f, 0.4f, 1f },
                    Factors = new float[] { 0f, .3f, 1f, 1f }
                };
                gradient.Blend = blend;

                g.FillPath(gradient, thePath);
            }

            // Paint Edge
            using GraphicsPath gborderDark = thePath;
            using (Pen p = new Pen(c2, 1))
            {
                g.DrawPath(p, gborderDark);
            }
        }

        public static void FillRectangleE(this Graphics g, Brush brush, Rectangle rectangle) =>
            g.FillRectangle(brush, new Rectangle(rectangle.Location, new Size(rectangle.Width + 1, rectangle.Height + 1)));


        #endregion

        #region Cursors

        public static GraphicsPath LeftCursor(Point location, Size size, int arrowLength, int corner)
        {
            int half_height = (size.Height / 2.0).ToInt32();

            Point[] pts = new Point[8]
            {
                new Point (location.X + arrowLength + corner, location.Y - half_height),
                new Point (location.X + size.Width - corner, location.Y - half_height),
                new Point (location.X + size.Width, location.Y - half_height + corner),

                new Point (location.X + size.Width, location.Y + half_height - corner),
                new Point (location.X + size.Width - corner, location.Y + half_height),

                new Point (location.X + arrowLength + corner, location.Y + half_height),
                new Point (location.X, location.Y),
                new Point (location.X + arrowLength + corner, location.Y - half_height),
            };

            GraphicsPath path = new GraphicsPath();
            path.AddLines(pts);

            return path;
        }

        public static GraphicsPath RightCursor(Point location, Size size, int arrowLength, int corner)
        {
            int half_height = (size.Height / 2.0).ToInt32();

            Point[] pts = new Point[8]
            {
                new Point (location.X - size.Width, location.Y - half_height + corner),
                new Point (location.X - size.Width + corner, location.Y - half_height),

                new Point (location.X - arrowLength - corner, location.Y - half_height),
                new Point (location.X, location.Y),
                new Point (location.X - arrowLength - corner, location.Y + half_height),

                new Point (location.X - size.Width + corner, location.Y + half_height),
                new Point (location.X - size.Width, location.Y + half_height - corner),
                new Point (location.X - size.Width, location.Y - half_height + corner),
            };

            GraphicsPath path = new GraphicsPath();
            path.AddLines(pts);

            return path;
        }

        public static GraphicsPath DualCursor(Point location, Size size, int arrowLength, int corner)
        {
            int half_width = (size.Width / 2.0).ToInt32();
            int half_height = (size.Height / 2.0).ToInt32();

            Point[] pts = new Point[]
            {
                new Point (location.X - half_width, location.Y - half_height),
                new Point (location.X + half_width, location.Y - half_height),
                new Point (location.X + half_width + arrowLength, location.Y),
                new Point (location.X + half_width, location.Y + half_height),
                new Point (location.X - half_width, location.Y + half_height),
                new Point (location.X - half_width - arrowLength, location.Y),
                new Point (location.X - half_width, location.Y - half_height),
            };

            GraphicsPath path = new GraphicsPath();
            path.AddLines(pts);

            return path;
        }

        public static void DrawLeftCursor(this Graphics g, string text, Font font, ColorTheme theme, Point location, int maxHeight = 16, int minWidth = 1, int arrowSize = 4, int corner = 1)
        {
            location.X += 1;
            Size textSize = TextRenderer.MeasureText(text, font);

            int height = textSize.Height;
            if (height > maxHeight) height = maxHeight;

            Size size = new Size(textSize.Width + 6, height);
            if (size.Width < minWidth) size.Width = minWidth;

            using (GraphicsPath gp = LeftCursor(location, size, arrowSize, corner))
            {
                g.FillPath(theme.FillBrush, gp);
                g.DrawPath(theme.EdgePen, gp);
            }

            Point textPt = new Point(location.X + arrowSize + 1, location.Y + 1);
            g.DrawString(text, font, theme.ForeBrush, textPt, AppTheme.TextAlignLeft);
        }

        public static void DrawRightCursor(this Graphics g, string text, Font font, ColorTheme theme, Point location, int maxHeight = 16, int minWidth = 1, int arrowSize = 4, int corner = 1)
        {
            Size textSize = TextRenderer.MeasureText(text, font);

            int height = textSize.Height;
            if (height > maxHeight) height = maxHeight;

            Size size = new Size(textSize.Width + 5, height);
            if (size.Width < minWidth) size.Width = minWidth;

            using (GraphicsPath gp = RightCursor(location, size, arrowSize, corner))
            {
                g.FillPath(theme.FillBrush, gp);
                g.DrawPath(theme.EdgePen, gp);
            }

            Point textPt = new Point(location.X - arrowSize, location.Y + 1);
            g.DrawString(text, font, theme.ForeBrush, textPt, AppTheme.TextAlignRight);
        }

        public static void DrawDualCursor(this Graphics g, string text, Font font, ColorTheme theme, Point location, int maxHeight = 16, int minWidth = 1, int arrowSize = 4, int corner = 1)
        {
            Size textSize = TextRenderer.MeasureText(text, font);

            int height = textSize.Height;
            if (height > maxHeight) height = maxHeight;

            Size size = new Size(textSize.Width + 6, height);
            if (size.Width < minWidth) size.Width = minWidth;

            using (GraphicsPath gp = DualCursor(location, size, arrowSize, corner))
            {
                g.FillPath(theme.FillBrush, gp);
                g.DrawPath(theme.EdgePen, gp);
            }

            Point textPt = new Point(location.X, location.Y + 1);
            g.DrawString(text, font, theme.ForeBrush, textPt, AppTheme.TextAlignCenter);
        }

        #endregion

        #region Tags

        public static GraphicsPath LeftTag(Point location, Size size, Size arrowSize, int corner)
        {
            //int half_width = (size.Width / 2.0).ToInt32();
            int half_height = (size.Height / 2.0).ToInt32();

            //location.X += half_width + arrowSize.Height;
            //location.X += 1;
            Point[] pts = new Point[12]
            {
                new Point (location.X + arrowSize.Height, location.Y - half_height + corner),
                new Point (location.X + arrowSize.Height + corner, location.Y - half_height),
                new Point (location.X + size.Width + arrowSize.Height - corner, location.Y - half_height),

                new Point (location.X + size.Width + arrowSize.Height, location.Y - half_height + corner),
                new Point (location.X + size.Width + arrowSize.Height, location.Y + half_height - corner),
                new Point (location.X + size.Width + arrowSize.Height - corner, location.Y + half_height),

                new Point (location.X + arrowSize.Height + corner, location.Y + half_height),
                new Point (location.X + arrowSize.Height, location.Y + half_height - corner),

                new Point (location.X + arrowSize.Height, location.Y + arrowSize.Width / 2),
                new Point (location.X + arrowSize.Height - arrowSize.Height, location.Y),
                new Point (location.X + arrowSize.Height , location.Y - arrowSize.Width / 2),

                new Point (location.X + arrowSize.Height, location.Y - half_height + corner),
            };

            GraphicsPath path = new GraphicsPath();
            path.AddLines(pts);

            return path;
        }

        // Still need to change the center 
        public static GraphicsPath RightTag(Point location, Size size, Size arrowSize, int corner)
        {
            //int half_width = (size.Width / 2.0).ToInt32();
            int half_height = (size.Height / 2.0).ToInt32();

            //location.X += - half_width - arrowSize.Height;

            Point[] pts = new Point[12]
            {
                new Point (location.X - size.Width - arrowSize.Height, location.Y - half_height + corner),
                new Point (location.X - size.Width - arrowSize.Height + corner, location.Y - half_height),
                new Point (location.X - arrowSize.Height- corner, location.Y - half_height),
                new Point (location.X - arrowSize.Height, location.Y - half_height + corner),

                new Point (location.X - arrowSize.Height, location.Y - arrowSize.Width / 2),
                new Point (location.X - arrowSize.Height + arrowSize.Height, location.Y),
                new Point (location.X - arrowSize.Height, location.Y + arrowSize.Width / 2),

                new Point (location.X - arrowSize.Height, location.Y + half_height - corner),
                new Point (location.X - arrowSize.Height - corner, location.Y + half_height),
                new Point (location.X - size.Width - arrowSize.Height + corner, location.Y + half_height),
                new Point (location.X - size.Width - arrowSize.Height, location.Y + half_height - corner),

                new Point (location.X - size.Width - arrowSize.Height, location.Y - half_height + corner),
            };

            GraphicsPath path = new GraphicsPath();
            path.AddLines(pts);

            return path;
        }

        public static GraphicsPath UpTag(Point location, Size size, Size arrowSize, int corner)
        {
            int half_width = (size.Width / 2.0).ToInt32();
            int half_height = (size.Height / 2.0).ToInt32();
            location = new Point(location.X, location.Y - 1);

            Point[] pts = new Point[12]
            {
                new Point (location.X - half_width, location.Y - half_height + corner),
                new Point (location.X - half_width + corner, location.Y - half_height),
                new Point (location.X - arrowSize.Width / 2, location.Y - half_height),

                new Point (location.X, location.Y - half_height - + arrowSize.Height),
                new Point (location.X + arrowSize.Width / 2, location.Y - half_height),
                new Point (location.X + half_width - corner, location.Y - half_height),

                new Point (location.X + half_width, location.Y - half_height + corner),
                new Point (location.X + half_width, location.Y + half_height - corner),
                new Point (location.X + half_width - corner, location.Y + half_height),

                new Point (location.X - half_width + corner, location.Y + half_height),
                new Point (location.X - half_width, location.Y + half_height - corner),
                new Point (location.X - half_width, location.Y - half_height + corner)
            };

            GraphicsPath path = new GraphicsPath();
            path.AddLines(pts);

            return path;
        }

        public static GraphicsPath DownTag(Point location, Size size, Size arrowSize, int corner)
        {
            int half_width = (size.Width / 2.0).ToInt32();
            int half_height = (size.Height / 2.0).ToInt32();
            location = new Point(location.X, location.Y - 1);

            Point[] pts = new Point[12]
            {
                new Point (location.X - half_width, location.Y - half_height + corner),
                new Point (location.X - half_width + corner, location.Y - half_height),
                new Point (location.X + half_width - corner, location.Y - half_height),

                new Point (location.X + half_width, location.Y - half_height + corner),
                new Point (location.X + half_width, location.Y + half_height - corner),
                new Point (location.X + half_width - corner, location.Y + half_height),

                new Point (location.X + arrowSize.Width / 2, location.Y + half_height),
                new Point (location.X, location.Y + half_height + arrowSize.Height),
                new Point (location.X - arrowSize.Width / 2, location.Y + half_height),

                new Point (location.X - half_width + corner, location.Y + half_height),
                new Point (location.X - half_width, location.Y + half_height - corner),
                new Point (location.X - half_width, location.Y - half_height + corner)
            };

            GraphicsPath path = new GraphicsPath();
            path.AddLines(pts);

            return path;
        }

        public static GraphicsPath UpDownTag(Point location, Size size, Size arrowSize, int corner)
        {
            int half_width = (size.Width / 2.0).ToInt32();
            int half_height = (size.Height / 2.0).ToInt32();
            location = new Point(location.X, location.Y - 1);

            Point[] pts = new Point[15]
            {
                new Point (location.X - half_width, location.Y - half_height + corner),
                new Point (location.X - half_width + corner, location.Y - half_height),
                new Point (location.X - arrowSize.Width / 2, location.Y - half_height),
                new Point (location.X, location.Y - half_height - arrowSize.Height),
                new Point (location.X + arrowSize.Width / 2, location.Y - half_height),

                new Point (location.X + half_width - corner, location.Y - half_height),
                new Point (location.X + half_width, location.Y - half_height + corner),
                new Point (location.X + half_width, location.Y + half_height - corner),
                new Point (location.X + half_width - corner, location.Y + half_height),
                new Point (location.X + arrowSize.Width / 2, location.Y + half_height),

                new Point (location.X, location.Y + half_height + arrowSize.Height),
                new Point (location.X - arrowSize.Width / 2, location.Y + half_height),
                new Point (location.X - half_width + corner, location.Y + half_height),
                new Point (location.X - half_width, location.Y + half_height - corner),
                new Point (location.X - half_width, location.Y - half_height + corner)
            };

            GraphicsPath path = new GraphicsPath();
            path.AddLines(pts);

            return path;
        }

        public static GraphicsPath Tag(Point location, Size size, int corner)
        {
            int half_width = (size.Width / 2.0).ToInt32();
            int half_height = (size.Height / 2.0).ToInt32();
            location = new Point(location.X, location.Y - 1);

            Point[] pts = new Point[9]
            {
                new Point (location.X - half_width, location.Y - half_height + corner),
                new Point (location.X - half_width + corner, location.Y - half_height),

                new Point (location.X + half_width - corner, location.Y - half_height),
                new Point (location.X + half_width, location.Y - half_height + corner),

                new Point (location.X + half_width, location.Y + half_height - corner),
                new Point (location.X + half_width - corner, location.Y + half_height),

                new Point (location.X - half_width + corner, location.Y + half_height),
                new Point (location.X - half_width, location.Y + half_height - corner),

                new Point (location.X - half_width, location.Y - half_height + corner),
            };

            GraphicsPath path = new GraphicsPath();
            path.AddLines(pts);

            return path;
        }

        #endregion
    }
}
