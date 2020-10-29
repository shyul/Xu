/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Radix-2 Software FFT
/// 
/// ***************************************************************************

using System;
using System.Numerics;

namespace Xu.EE
{
    public static class Waveform
    {
        public static double[] Sine(int N = 65536, double cycle = 1)
        {
            double ang = Math.PI * 2 * cycle / (N - 1);

            double[] data = new double[N];
            for (int i = 0; i < N; i++)
                data[i] = Math.Sin(i * ang);

            return data;
        }

        public static Complex[] ComplexSine(int N = 65536, double cycle = 1)
        {
            double ang = Math.PI * 2 * cycle / (N - 1);

            Complex[] data = new Complex[N];
            for (int i = 0; i < N; i++)
                data[i] = new Complex(Math.Cos(i * ang), Math.Sin(i * ang));

            return data;
        }
    }
}
