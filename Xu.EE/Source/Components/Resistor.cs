/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2018 Xu Li - shyu.lee@gmail.com
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Xu;

namespace Xu.EE
{
    [Serializable, DataContract]
    public enum ResistorType
    {
        THICK_FILM,
        THIN_FILM,
        PPM_50,
        WIDE_TERM,
        CURRENT_SENSE,
    }

    /// <summary>
    /// Value in Ohm
    /// </summary>
    [Serializable, DataContract]
    public class Resistor : LumpedComponent
    {
        public Resistor()
        {
            Level = 210;
            Unit = string.Empty;
            Tolerance = 1;

            TemperatureRange = new Range<double>(-55, 155);

            SymbolName = "RESISTOR";
            SymbolPath = @"Basic\Basic.SchLib";

            FootprintPath = @"Basic\Resistor.PcbLib";

            SimDescription = "Ideal Simulation Data";
            SimKind = "General";
            SimSubKind = "Resistor";
            SimSpicePrefix = "X";
            SimNetlist = string.Empty;
            SimPortMap = "(1:1),(2:2)";
            SimFile = string.Empty;
            SimModel = "Ideal";
        }

        [DataMember]
        public ResistorType ResistorType { get; set; } = ResistorType.THICK_FILM;

        [DataMember]
        public double Voltage { get; set; } = 0;

        [DataMember]
        public double PowerRating { get; set; } = -1;

        [DataMember]
        public virtual double TemperatureCoefficient { get; set; } = 200; // PPM/K

        [IgnoreDataMember]
        public override string Name => Comment + "_" + Tolerance.ToString("0.#") + "%";// _" + PowerRating + "W";

        [IgnoreDataMember]
        public override string Comment
        {
            get
            {
                if (Value >= 1e3 && Value < 1e6) return (Value / 1e3).ToString() + "K";
                else if (Value >= 1e6 && Value < 1e9) return (Value / 1e6).ToString() + "M";
                else if (Value >= 1e9) return (Value / 1e9).ToString() + "G";
                return Value.ToString() + "R";
            }
        }

        [IgnoreDataMember]
        public override string Description => ("RES," + MountType + "," + PackageName + "," + Comment.ToUpper() + "," + ToleranceDescription + "," +
            Voltage + "V," + PowerRating + "W,±" + TemperatureCoefficient.ToString() + "PPM/K," + ResistorType + "," + 
            TemperatureRange.ToStringShort() + "DEG(" + TempRangeType + ")," + Tag.ToUpper()).Trim(',');
    }
}
