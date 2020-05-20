/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Text based data functions.
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Xu
{
    public static partial class Shapes
    {
        /// <summary>
        /// Warp the string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="font"></param>
        /// <param name="maxWidth"></param>
        /// <returns></returns>
        public static List<string> Wordwarp(this string input, Font font, int maxWidth)
        {
            Size strSize = TextRenderer.MeasureText(input, font);
            if (strSize.Width < maxWidth)
            {
                return new List<string> { input };
            }
            else
            {
                List<string> lines = new List<string>();
                string[] words = input.Split(' ');
                string temp = string.Empty;
                foreach (string word in words)
                {
                    temp += word + " ";
                    int stringSize = TextRenderer.MeasureText(temp.TrimEnd(new char[] { ' ' }), font).Width;

                    if (stringSize > maxWidth)
                    {
                        lines.Add(temp.TrimEnd(new char[] { ' ' }));
                        temp = string.Empty;
                    }
                }
                return lines;
            }
        }

        /// <summary>
        /// Warp the string with limited width and line count
        /// </summary>
        /// <param name="input"></param>
        /// <param name="font"></param>
        /// <param name="maxLineCnt"></param>
        /// <param name="maxWidth"></param>
        /// <param name="lineWidth"></param>
        /// <returns></returns>
        public static List<string> Wordwarp(this string input, Font font, int maxLineCnt, int maxWidth, out int lineWidth)
        {
            Size strSize = TextRenderer.MeasureText(input, font);
            if (strSize.Width < maxWidth)// / maxLineCnt)
            {
                lineWidth = strSize.Width;
                return new List<string> { input };
            }
            else
            {
                int actualWidth = 0;
                int avg = (int)Math.Ceiling((double)(strSize.Width / maxLineCnt));
                if (avg < maxWidth) maxWidth = avg;

                List<string> lines = new List<string>();
                string[] words = input.Split(' ');
                float spaceWidth = TextRenderer.MeasureText(" ", font).Width;
                string temp = string.Empty;
                foreach (string word in words)
                {
                    temp += word + " ";
                    int stringSize = TextRenderer.MeasureText(temp.TrimEnd(new char[] { ' ' }), font).Width;
                    if (actualWidth < stringSize) actualWidth = stringSize;
                    if (stringSize > maxWidth)
                    {
                        lines.Add(temp.TrimEnd(new char[] { ' ' }));
                        if (lines.Count >= maxLineCnt)
                        {
                            lineWidth = actualWidth;
                            return lines;
                        }
                        temp = string.Empty;
                    }
                }
                lineWidth = actualWidth;
                return lines;
            }
        }
    }
}
