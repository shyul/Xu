/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;

namespace Xu
{
    public static class IconTool
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="oldc">The color to be replace</param>
        /// <param name="newc">The color replace with</param>
        /// <returns></returns>
        public static Bitmap ReplaceColor(this Bitmap bmp, Color oldc, Color newc)
        {
            Bitmap newBmp = new Bitmap(bmp.Width, bmp.Height);
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color srcColor = bmp.GetPixel(x, y);
                    Color dstColor = Color.FromArgb(srcColor.R, srcColor.G, srcColor.B);

                    if (dstColor == oldc)
                    {
                        newBmp.SetPixel(x, y, Color.FromArgb(srcColor.A, newc));
                    }
                    else
                    {
                        newBmp.SetPixel(x, y, srcColor);
                    }
                }
            }
            return newBmp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="icon"></param>
        /// <param name="boundary"></param>
        /// <param name="oldc"></param>
        /// <param name="newc"></param>
        public static void DrawIcon(this Graphics g, Bitmap icon, Rectangle boundary, Color oldc, Color newc)
        {
            using (Bitmap newBmp = icon.ReplaceColor(oldc, newc))
            {
                if (boundary.Size == newBmp.Size)
                    g.DrawImageUnscaled(newBmp, boundary);
                else
                {
                    int x = boundary.Location.X + boundary.Size.Width / 2 - newBmp.Size.Width / 2;
                    int y = boundary.Location.Y + boundary.Size.Height / 2 - newBmp.Size.Height / 2;
                    g.DrawImageUnscaled(newBmp, new Rectangle(new Point(x, y), newBmp.Size));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="icon"></param>
        /// <param name="boundary"></param>
        /// <param name="newc"></param>
        public static void DrawIcon(this Graphics g, Bitmap icon, Rectangle boundary, Color newc) => g.DrawIcon(icon, boundary, Color.FromArgb(255, 0, 0, 0), newc);
    }
}
