/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Physiscs Contants
/// 
/// ***************************************************************************

using System;
using System.Numerics;

namespace Xu.EE
{
    public static partial class Calculate
    {
        public const double PI = Math.PI;

        public static double FrequencyToRadians(this double freq) => 2 * PI * freq;
        public static double DegreesToRadians(this double degrees) => (PI * degrees) / 180;

        public static double RadiansToDegrees(this double radians) => radians * 180 / PI;

        public const double E = Math.E;
        public const double LightSpeed = 299792458; // Unit: meter/second
        public const double VacuumPermitivity = 1e7 / (4 * PI * LightSpeed * LightSpeed); //public static double VacuumPermittivity => 1e7 / (4 * PI * Math.Pow(LightSpeed, 2));
        public static double Capacitance(double dielectricConst, double area, double thickness) 
            => (dielectricConst * VacuumPermitivity * area) / thickness;



    }
}
