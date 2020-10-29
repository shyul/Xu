/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Drawing;

namespace Xu
{
    public static class ColorTool
    {
        /// <summary>
        /// Quickly adjust the transparency of the color
        /// </summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="alpha">New alpha channel.</param>
        /// <returns></returns>
        public static Color Opaque(this Color color, int alpha) => Color.FromArgb(alpha, color);

        /// <summary>
        /// Creates color with corrected brightness.
        /// * Developed by Pavel Vladov from Internet
        /// </summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="factor">The brightness correction factor. 
        /// Must be between -1 and 1. 
        /// Negative values produce darker colors.</param>
        /// <returns></returns>
        public static Color Brightness(this Color color, float factor)
        {
            float r = color.R;
            float g = color.G;
            float b = color.B;

            if (factor < 0)
            {
                factor = 1 + factor;
                r *= factor;
                g *= factor;
                b *= factor;
            }
            else
            {
                r = (255 - r) * factor + r;
                g = (255 - g) * factor + g;
                b = (255 - b) * factor + b;
            }

            return Color.FromArgb(color.A, r.ToInt32(), g.ToInt32(), b.ToInt32());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="H"></param>
        /// <param name="S"></param>
        /// <param name="L"></param>
        /// <returns></returns>
        public static Color FromAhsl(float A, float H, float S, float L)
        {
            int alpha = (A * 255).ToInt32();

            if (alpha < 0) alpha = 0;
            else if (alpha > 255) alpha = 255;

            while (H < 0) H += 360;
            while (H > 360) H -= 360;

            if (S < 0) S = 0;
            else if (S > 1) S = 1;

            if (L < 0) L = 0;
            else if (L > 1) L = 1;

            if (S == 0)
            {
                return Color.FromArgb(
                    alpha,
                    (L * 255).ToInt32(),
                    (L * 255).ToInt32(),
                    (L * 255).ToInt32());
            }

            float H0 = H / 60.0f;
            int index = Convert.ToInt32(Math.Floor(H0));

            float C = S * (1 - Math.Abs(2 * L - 1));
            float X = C * (1 - Math.Abs((H0 % 2) - 1));
            float m = L - C / 2;

            int iC = ((C + m) * 255).ToInt32();
            int iX = ((X + m) * 255).ToInt32();
            int i0 = (m * 255).ToInt32();

            return index switch
            {
                1 => Color.FromArgb(alpha, iX, iC, i0),
                2 => Color.FromArgb(alpha, i0, iC, iX),
                3 => Color.FromArgb(alpha, i0, iX, iC),
                4 => Color.FromArgb(alpha, iX, i0, iC),
                5 => Color.FromArgb(alpha, iC, i0, iX),
                _ => Color.FromArgb(alpha, iC, iX, i0),
            };
        }
    }
}
