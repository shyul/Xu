/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Radix-2 Software FFT
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Xu.EE
{
    public static class SignalTool
    {


        public static double Amplitude(IEnumerable<double> signal)
            => signal.Count() > 0 ? (signal.Max() - signal.Min()) : double.NaN;

        public static double PhaseDifference(IEnumerable<double> signal1, IEnumerable<double> signal2)
        {
            
            double ref1 = signal1.Average();
            double ref2 = signal2.Average();


            double count = Math.Min(signal1.Count(), signal2.Count());

            List<(int index, double s1, double s2)> list = new();

            for (int i = 0; i < count; i++) 
            {
                list.Add((i, signal1.ElementAt(i), signal2.ElementAt(2)));
            }

            for (int i = 1; i < count - 1; i++)
            {
                var (index_prev, s1_prev, s2_prev) = list[i - 1];
                var (index, s1, s2) = list[i];
                var (index_next, s1_next, s2_next) = list[i + 1];

                if(s1_prev < ref1 && s1 >= ref1 && s1_next > ref1) 
                {
                
                }

            }

            return 0;
        }
    }
}
