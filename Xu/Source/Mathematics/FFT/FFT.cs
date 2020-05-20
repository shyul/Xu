/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Radix-2 Software FFT
/// 
/// ***************************************************************************

using System;
using System.Numerics;

namespace Xu
{
    public class FFT
    {
        public FFT(int length = 65536, WindowsType type = WindowsType.FlatTop, double[] winF = null, int[] winParam = null)
        {
            if (length > 4 && Numbers.IsPowerOf2(length))
            {
                Length = length;

                if (type == WindowsType.Custom)
                {
                    if (winF?.Length == length)
                        WinF = winF;
                    else
                        throw new ArgumentException("Custom Window's array length has to match FFT length");
                }
                else
                    WinF = WindowFunction.GetWindow(length, type);

                Wn = new Complex[Length];
                double ang = 2 * Math.PI / Length;
                Complex w = new Complex(Math.Cos(ang), -Math.Sin(ang));
                Wn[0] = new Complex(1.0, 0.0);

                int n = Length / 2;
                for (int i = 1; i < n; i++)
                    Wn[i] = Wn[i - 1] * w;
            }
            else
                throw new ArgumentException("Length must be greater than 4 and power of 2");
        }

        public int Length { get; private set; } = 1024;
        public WindowsType WindowType { get; private set; }

        public Complex[] Wn { get; private set; }
        public double[] WinF { get; private set; }

        public Complex[] Transform(Complex[] Input)
        {
            if (Input.Length == Length)
            {
                // Apply window to input sample
                for (int i = 0; i < Length; i++)
                    Input[i] = Input[i] * WinF[i];

                int LengthBy2 = Length / 2;
                int LengthBy4 = LengthBy2 / 2;
                int m = 0;
                int w = 1;

                // Transform Radix-2
                while (LengthBy2 >= 1)
                {
                    int d = 0;
                    for (int i = 0; i < w; i++)
                    {
                        d = Length / w;
                        for (int j = 0; j < LengthBy2; j++)
                        {
                            Complex TmpA = Input[i * d + j] + Input[i * d + LengthBy2 + j];
                            Complex TmpB = (Input[i * d + j] - Input[i * d + LengthBy2 + j]) * Wn[w * j];
                            Input[i * d + j] = TmpA;
                            Input[i * d + LengthBy2 + j] = TmpB;
                        }
                    }
                    LengthBy2 /= 2;
                    LengthBy4 = LengthBy2 / 2;
                    m += 1;
                    w *= 2;
                }

                Complex[] Result = new Complex[Length];

                // Re-order output array.
                for (uint i = 0; i < Input.Length; i++)
                {
                    Result[i] = Input[Numbers.EndianInverse(i, m)];
                }

                return Result;
            }
            else
                throw new ArgumentException("Input array length must be equal to the FFT length.");
        }
    }
}
