/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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
    public static class Numbers
    {
        /// <summary>
        /// Golden (Fibonacci) Ratio
        /// </summary>
        public static readonly double Phi = (1 + Math.Sqrt(5)) / 2;

        /// <summary>
        /// Clean up the string before parsing it to numbers.
        /// </summary>
        private static readonly char[] m_charsToTrimEnd = { ',', '.', ' ' };

        private static string CleanUpNumString(string input) => Regex.Replace(input, "[^0-9.Ee-]", "").TrimEnd(m_charsToTrimEnd).Trim();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float ToFloat(this string input, float defaultValue = 0)
        {
            if (input == null) return defaultValue;

            try
            {
                string str = CleanUpNumString(input);
                if (string.IsNullOrWhiteSpace(str))
                    return defaultValue;
                else
                    return float.Parse(str, NumberStyles.Float | NumberStyles.Number | NumberStyles.AllowExponent);
            }
            catch (Exception e) when (e is FormatException || e is OverflowException)
            {
                Console.WriteLine("ToFloat Error: " + e + ", Input was: " + input);
                return defaultValue;
            }
        }

        /// <summary>
        /// TODO: Fix "8.9e-05", "6.0e-05", 5.0e-05, 2.0e-05
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(this string input, double defaultValue = double.NaN)
        {
            if (input == null) return defaultValue;

            try
            {
                string str = CleanUpNumString(input);
                if (string.IsNullOrWhiteSpace(str)) 
                    return defaultValue;
                else
                    return double.Parse(str, NumberStyles.Float | NumberStyles.Number | NumberStyles.AllowExponent); //return Convert.ToDouble(decimal.Parse(str));
            }
            catch (Exception e) when (e is FormatException || e is OverflowException)
            {
                Console.WriteLine("ToDouble Error: " + e + ", Input was: " + input);
                return defaultValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static ulong ToUInt64(this string input, ulong defaultValue = 0)
        {
            if (input == null) return defaultValue;

            try
            {
                string str = CleanUpNumString(input);
                if (str.Length > 0)
                {
                    return ulong.Parse(str);
                }
                else
                    return defaultValue;
            }
            catch (Exception e) when (e is FormatException || e is OverflowException)
            {
                Console.WriteLine("ToInt64 Error: " + e + ", Input was: " + input);
                return defaultValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ToInt64(this string input, long defaultValue = 0)
        {
            if (input == null) return defaultValue;

            try
            {
                string str = CleanUpNumString(input);
                if (str.Length > 0)
                {
                    return long.Parse(str);
                }
                else
                    return defaultValue;
            }
            catch (Exception e) when (e is FormatException || e is OverflowException)
            {
                Console.WriteLine("ToInt64 Error: " + e + ", Input was: " + input);
                return defaultValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static long ToInt64(this double input, long defaultValue = 0)
        {
            if (!(input is double.NaN))
                return Convert.ToInt64(Math.Round(input, MidpointRounding.AwayFromZero));
            else
                return defaultValue;
        }

        /// <summary>
        /// Convert Double to Int32 directly with truncation, and possibly but improbably crash your system
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ToInt32(this double input, long defaultValue = 0) => (int)ToInt64(input, defaultValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static uint ToUInt32(this string input, uint defaultValue = 0)
        {
            if (input == null) return defaultValue;

            try
            {
                string str = CleanUpNumString(input);
                if (str.Length > 0)
                {
                    return uint.Parse(str);
                }
                else
                    return defaultValue;
            }
            catch (Exception e) when (e is FormatException || e is OverflowException)
            {
                Console.WriteLine("ToInt64 Error: " + e + ", Input was: " + input);
                return defaultValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt32(this string input, int defaultValue = 0)
        {
            if (input == null) return defaultValue;

            try
            {
                string str = CleanUpNumString(input);
                if (str.Length > 0)
                {
                    return int.Parse(str);
                }
                else
                    return defaultValue;
            }
            catch (Exception e) when (e is FormatException || e is OverflowException)
            {
                Console.WriteLine("ToInt64 Error: " + e + ", Input was: " + input);
                return defaultValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ToInt32(this float input) => Convert.ToInt32(Math.Round(input, MidpointRounding.AwayFromZero));


        /// <summary>
        /// 
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int ToInt32(this byte[] buff, int offset)
        {
            if (offset + 5 > buff.Length) throw new OutOfMemoryException();

            if (BitConverter.IsLittleEndian)
            {
                return BitConverter.ToInt32(new Byte[] { buff[offset + 3], buff[offset + 2], buff[offset + 1], buff[offset] }, 0);
            }
            else
            {
                return BitConverter.ToInt32(new Byte[] { buff[offset], buff[offset + 1], buff[offset + 2], buff[offset + 3] }, 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double StdDev(this IEnumerable<double> values)
        {
            double value = 0;
            if (values.Count() > 0)
            {
                double avg = values.Average(); //Compute the Average 
                double sum = values.Sum(d => Math.Pow(d - avg, 2)); //Perform the Sum of (value-avg)_2_2     
                value = Math.Sqrt(sum / (values.Count() - 1));
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="dacades"></param>
        /// <returns></returns>
        public static double FitDacades(this double val, double[] dacades)
        {
            for (int i = -12; i <= 12; i++) // from small to big
            {
                double r = Math.Pow(10, i);
                for (int j = 0; j < dacades.Length; j++)
                {
                    double res = val / (r * dacades[j]);
                    if (res <= 1 && res >= -1)
                    {
                        return r * dacades[j];
                    }
                }
            }
            return val;
        }

        /// <summary>
        /// Check if a number is power of 2, with explanations here:
        /// https://stackoverflow.com/questions/600293/how-to-check-if-a-number-is-a-power-of-2
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPowerOf2(int value) => (value != 0) && ((value & (value - 1)) == 0);

        public static uint EndianInverse(uint input, int BitLength)
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

        public static IntPtr ToIntPtr32(short loWord, short hiWord) => new IntPtr(((int)hiWord) << 16 | (int)loWord);

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

        public static string[] SIPrefix = { "K", "M", "G", "T", "P", "E", "Z", "Y" };
        public static string[] SIPrefixFloat = { "m", "u", "n", "p", "f", "a", "z", "y" };

        /*
        public static string ToSIString(this double value, int integerPart = 1, string format = "0.00#")
        {
            double factor = value;

            double divisor = 10 ^ integerPart;
            double divisorFloat = 10 ^ -integerPart;

            if (value > -1 && value < 1) // float point mode
            {

            }
            else if (value >= 1000 || value <= -1000)
            {
                for (int i = 0; i < SIPrefix.Length; i++)
                {
                    factor = factor / 1e3;

                    if(factor >= -1000 || factor <= 1000)
                    {

                    }

                    if (value/1e3)
                }
            }
        }*/

        /// <summary>
        /// Convert large double number to finance number string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static (double Number, string String) ToSINumberString(this double input, string format = "")
        {
            double num = input;
            if ((input > -1e-9 && input <= -1e-12) || (input < 1e-9 && input >= 1e-12))
            {
                num = input * 1e12;
                return (num, num.ToNumberString(format) + "f");
            }
            else if ((input > -1e-6 && input <= -1e-9) || (input < 1e-6 && input >= 1e-9))
            {
                num = input * 1e9;
                return (num, num.ToNumberString(format) + "p");
            }
            else if ((input > -1e-3 && input <= -1e-6) || (input < 1e-3 && input >= 1e-6))
            {
                num = input * 1e6;
                return (num, num.ToNumberString(format) + "n");
            }
            else if ((input > -1 && input <= -1e-3) || (input < 1 && input >= 1e-3))
            {
                num = input * 1e3;
                return (num, num.ToNumberString(format) + "m");
            }
            else if ((input >= 1e3 && input < 1e6) || (input <= -1e3 && input > -1e6))
            {
                num = input / 1e3;
                return (num, num.ToNumberString(format) + "K");
            }
            else if ((input >= 1e6 && input < 1e9) || (input <= -1e6 && input > -1e9))
            {
                num = input / 1e6;
                return (num, num.ToNumberString(format) + "M");
            }
            else if ((input >= 1e9 && input < 1e12) || (input <= -1e9 && input > -1e12))
            {
                num = input / 1e9;
                return (num, num.ToNumberString(format) + "G");
            }
            else if (input >= 1e12 || input <= -1e12)
            {
                num = input / 1e12;
                return (num, num.ToNumberString(format) + "T");
            }
            else
            {
                return (num, input.ToString(format));
            }
        }

        /// <summary>
        /// Convert double number to finance number string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToNumberString(this double input, string format)
        {
            if (Double.IsNaN(input))
            {
                return "N/A";
            }
            else if (input >= 10000 || input <= -10000)
            {
                return input.ToString("N0", CultureInfo.InvariantCulture);
            }
            else if (input > -10 && input < 10)
            {
                input = ((input * 1000.0).ToInt64() / 1000.0);
                return input.ToString(format); //("0.###");
            }
            else
            {
                input = ((input * 1000.0).ToInt64() / 1000.0);
                return input.ToString(format); // ("0.##");
            }
        }


    }
}
