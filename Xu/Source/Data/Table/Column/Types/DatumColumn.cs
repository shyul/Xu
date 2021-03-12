/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System;

namespace Xu
{
    public class DatumColumn : Column // public class DataColumn<T> : Column where T : IDatum
    {
        public DatumColumn(string name, Type datumType)
        {
            if (typeof(IDatum).IsAssignableFrom(datumType))
            {
                Name = Label = name;
                DatumType = datumType;
            }
            else
                throw new(datumType.FullName + " has to be IDatum");
        }

        public DatumColumn(string name, string label, Type datumType)
        {
            if (typeof(IDatum).IsAssignableFrom(datumType))
            {
                Name = name;
                Label = label;
                DatumType = datumType;
            }
            else
                throw new(datumType.FullName + " has to be IDatum");
        }

        public Type DatumType { get; }

        public override int GetHashCode() => Name.GetHashCode() ^ GetType().GetHashCode() ^ DatumType.FullName.GetHashCode();

        public override bool Equals(Column other) => other is DatumColumn dc && Equals(dc);

        public bool Equals(DatumColumn other) => DatumType == other.DatumType && Name == other.Name;

        public override bool Equals(object other)
        {
            if (other is DatumColumn dc)
                return Equals(dc);
            else
                return false;
        }

        public static bool operator !=(DatumColumn s1, DatumColumn s2) => !s1.Equals(s2);
        public static bool operator ==(DatumColumn s1, DatumColumn s2) => s1.Equals(s2);
    }

    public interface IDatum { }
}
