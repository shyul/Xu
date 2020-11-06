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
    public static class Const
    {
        public static string[] SIPrefix { get; } = { "K", "M", "G", "T", "P", "E", "Z", "Y" };
        public static string[] SIPrefixFloat { get; } = { "m", "μ", "n", "p", "f", "a", "z", "y" };


        public const double Deca = 10;
        public const double Hecto = 1e2;
        public const double Kilo = 1e3;
        public const double Mega = 1e6;
        public const double Giga = 1e9;
        public const double Tera = 1e12;
        public const double Peta = 1e15;
        public const double Exa = 1e18;
        public const double Zetta = 1e21;
        public const double Yotta = 1e24;

        public const double Deci = 0.1;
        public const double Centi = 0.01;
        public const double Milli = 1e-3;
        public const double Micro = 1e-6; // μ
        public const double Nano = 1e-9;
        public const double Pico = 1e-12;
        public const double Femto = 1e-15;
        public const double Atto = 1e-18;
        public const double Zepto = 1e-21;
        public const double Yocto = 1e-24;
    }
}
