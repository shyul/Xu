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
    public enum MountType
    {
        SM,
        TH,
        MODULE
    }

    public enum TempRangeType
    {
        TJ,
        TA
    }

    [Serializable, DataContract]
    public abstract class Component : IGeneralItem
    {
        [DataMember]
        public virtual string Name { get; set; }

        [DataMember]
        public virtual string Comment { get; set; }

        [DataMember]
        public virtual string Description { get; set; }

        [DataMember]
        public LifeCycle LifeCycle { get; set; } = LifeCycle.Preliminary;

        [DataMember]
        public virtual HashSet<string> Tags { get; set; } = new HashSet<string>();

        [IgnoreDataMember]
        public string Tag
        {
            get
            {
                string value = string.Empty;
                foreach (string t in Tags)
                {
                    value += t + ",";
                }
                return value.Trim(',');
            }
        }

        [DataMember]
        public int Level { get; set; } = -1;

        [DataMember]
        public ulong Id { get; set; } = 0;

        [DataMember]
        public char Revision { get; set; } = 'A';

        [DataMember]
        public virtual (uint Id, string Name) Variation { get; set; } = (1, string.Empty);

        [IgnoreDataMember]
        public string ItemId => Level + "-" + Id.ToString("000000") + Revision + "-" + Variation.Id.ToString("00");

        [DataMember]
        public MountType MountType { get; set; } = MountType.SM;

        [DataMember]
        public double Height { get; set; } = -1;

        [DataMember]
        public string PackageName { get; set; } // 0201, 0402, SIZE_A, and so on...

        [DataMember]
        public TempRangeType TempRangeType { get; set; } = TempRangeType.TJ;

        [DataMember]
        public Range<double> TemperatureRange { get; set; }

        [DataMember]
        public string VendorName { get; set; }

        [DataMember]
        public string VendorPartNumber { get; set; }

        [DataMember]
        public virtual string TableName { get; set; }

        [DataMember]
        public virtual string TablePath { get; set; }

        [DataMember]
        public virtual string SymbolName { get; set; } // "CAP"

        [DataMember]
        public virtual string SymbolPath { get; set; } // "Standard\Basic.SchLib"

        [DataMember]
        public virtual string FootprintName { get; set; } // 0201, 0402, SIZE_A, and so on...

        [DataMember]
        public virtual string FootprintPath { get; set; } // 0201, 0402, SIZE_A, and so on...

        [DataMember]
        public virtual string SimulationValue { get; set; }

        [DataMember]
        public virtual string SimDescription { get; set; }

        [DataMember]
        public virtual string SimKind { get; set; }

        [DataMember]
        public virtual string SimSubKind { get; set; }

        [DataMember]
        public virtual string SimSpicePrefix { get; set; }

        [DataMember]
        public virtual string SimNetlist { get; set; }

        [DataMember]
        public virtual string SimPortMap { get; set; }

        [DataMember]
        public virtual string SimFile { get; set; }

        [DataMember]
        public virtual string SimModel { get; set; }

        [DataMember]
        public virtual string SimParameters { get; set; }

        [IgnoreDataMember]
        public object[] DataRow => new object[] { Name, ItemId, Comment, SimulationValue, PackageName,
            Description, VendorName, VendorPartNumber, SymbolName, SymbolPath, FootprintName, FootprintPath,
            SimDescription, SimKind, SimSubKind, SimSpicePrefix, SimNetlist,
            SimPortMap, SimFile, SimModel, SimParameters };
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable, DataContract]
    public class LumpedComponent : Component
    {
        [DataMember]
        public double Value { get; set; }

        [DataMember]
        public string Unit { get; set; } = string.Empty;

        [IgnoreDataMember]
        public override string SimulationValue => Value.ToString("0.##") + Unit;

        [DataMember]
        public double Tolerance { get; set; } // In percentage, example: 10%

        [DataMember]
        public string ToleranceDescription { get; set; } // "10%"
    }
}
