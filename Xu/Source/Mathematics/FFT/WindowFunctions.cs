/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Math and numeric related basic functions.
/// 
/// ***************************************************************************

using System;
using System.Runtime.Serialization;

namespace Xu
{
    [Serializable, DataContract]
    public enum WindowsType : int
    {
        Custom = 0,
        Rectangle = 1,
        Triangle = 2,

        Hanning = 3,
        Hamming = 4,

        Nuttall,
        Blackman,
        BlackmanNuttall,
        BlackmanHarris,

        FlatTop
    }

    public static class WindowFunction
    {
        public static double[] GetWindow(int N, WindowsType type, int[] winParam = null)
        {
            double[] data = new double[N];

            switch (type)
            {
                case WindowsType.Rectangle:
                    {
                        for (int i = 0; i < N; i++)
                            data[i] = 1;
                    }
                    break;

                case WindowsType.Triangle:
                    {
                        double N1 = N + 1;
                        for (int i = 0; i < N; i++)
                            data[i] = 1D - Math.Abs((2 * i - N1)) / N;
                    }
                    break;

                case WindowsType.Hanning:
                    {
                        double a = Math.PI * 2 / (N - 1);
                        for (int i = 0; i < N; i++)
                            data[i] = 0.5 - 0.5 * Math.Cos(i * a);
                    }
                    break;

                case WindowsType.Hamming:
                    {
                        double a = Math.PI * 2 / (N - 1);
                        for (int i = 0; i < N; i++)
                            data[i] = 0.53836 - 0.46164 * Math.Cos(i * a);
                    }
                    break;

                case WindowsType.Nuttall:
                    {
                        double a0 = Math.PI * 2 / (N - 1);
                        double a1 = 2 * a0;
                        double a2 = 3 * a0;
                        for (int i = 0; i < N; i++)
                        {
                            data[i] = 0.355768
                                - 0.487396 * Math.Cos(a0 * i)
                                + 0.144232 * Math.Cos(a1 * i)
                                - 0.012604 * Math.Cos(a2 * i);
                        }
                    }
                    break;

                case WindowsType.Blackman:
                    {
                        double a0 = Math.PI * 2 / (N - 1);
                        double a1 = 2 * a0;
                        for (int i = 0; i < N; i++)
                        {
                            data[i] = 0.42 - 0.5 * Math.Cos(a0 * i) + 0.08 * Math.Cos(a1 * i);
                        }
                    }
                    break;

                case WindowsType.BlackmanNuttall:
                    {
                        double a0 = Math.PI * 2 / (N - 1);
                        double a1 = 2 * a0;
                        double a2 = 3 * a0;
                        for (int i = 0; i < N; i++)
                        {
                            data[i] = 0.3635819
                                - 0.4891775 * Math.Cos(a0 * i)
                                + 0.1365995 * Math.Cos(a1 * i)
                                - 0.0106411 * Math.Cos(a2 * i);
                        }
                    }
                    break;

                case WindowsType.BlackmanHarris:
                    {
                        double a0 = Math.PI * 2 / (N - 1);
                        double a1 = 2 * a0;
                        double a2 = 3 * a0;
                        for (int i = 0; i < N; i++)
                        {
                            data[i] = 0.35875
                                - 0.48829 * Math.Cos(a0 * i)
                                + 0.14128 * Math.Cos(a1 * i)
                                - 0.01168 * Math.Cos(a2 * i);
                        }
                    }
                    break;

                case WindowsType.FlatTop:
                    {
                        double a0 = Math.PI * 2 / (N - 1);
                        double a1 = 2 * a0;
                        double a2 = 3 * a0;
                        double a3 = 4 * a0;
                        for (int i = 0; i < N; i++)
                        {
                            data[i] = 1
                                - 1.93 * Math.Cos(i * a0)
                                + 1.29 * Math.Cos(i * a1)
                                - 0.388 * Math.Cos(i * a2)
                                + 0.032 * Math.Cos(i * a3);
                        }
                    }
                    break;

                default:
                    for (int i = 0; i < N; i++)
                        data[i] = 0;
                    break;
            }
            return data;
        }
    }
}
