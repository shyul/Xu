/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Math and numeric related basic functions.
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Xu
{
    public static partial class Algorithms
    {
        /// <summary>
        /// Check if a number is power of 2, with explanations here:
        /// https://stackoverflow.com/questions/600293/how-to-check-if-a-number-is-a-power-of-2
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPowerOf2(this int value) => (value != 0) && ((value & (value - 1)) == 0);

        public static uint EndianInverse(this uint input, int BitLength)
        {
            uint result = 0;
            for (int i = 0; i < BitLength; i++)
            {
                result = (result << 1) | (input & 1);
                input >>= 1;
            }
            return result;
        }

        /// <summary>
        /// Binary Converstions
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short GetLo16bit(int value) => (short)(value & 0xFFFF);
        public static short GetHi16bit(int value) => (short)((value >> 16) & 0xFFFF);

        public static IntPtr ToIntPtr32(short loWord, short hiWord) => new(((int)hiWord) << 16 | (int)loWord);

        /*
        public static short HiWord(int dwValue)
        {
            return (short)((dwValue >> 16) & 0xFFFF);
        }
        public static short LoWord(int dwValue)
        {
            return (short)(dwValue & 0xFFFF);
        }
        public static IntPtr MakeLParam(int LoWord, int HiWord)
        {
            return new IntPtr((HiWord << 16) | (LoWord & 0xffff));
        }
        */
    }
}
