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
    public static partial class Calc
    {
        public const double PI = Math.PI;
        public static double Rad(double freq) => 2 * PI * freq;

        public const double E = Math.E;
        public const double LightSpeed = 299792458; // Unit: meter/second
        public const double VacuumPermittivity = 1e7 / (4 * PI * LightSpeed * LightSpeed); //public static double VacuumPermittivity => 1e7 / (4 * PI * Math.Pow(LightSpeed, 2));
        public static double Capacitance(double dielectricConst, double area, double thickness) => dielectricConst * VacuumPermittivity * area / thickness;


    }
}
