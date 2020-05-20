/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Xu
{
    public class NumericColumn : IEquatable<NumericColumn>
    {
        public NumericColumn(string name) => Name = name;

        public string Name { get; set; }

        public string Label { get; set; } = string.Empty;

        #region Equality

        public override int GetHashCode() => Name.GetHashCode();

        public bool Equals(NumericColumn other) => Name == other.Name;

        public override bool Equals(object other)
        {
            if (other is NumericColumn dc)
                return Equals(dc);
            else
                return false;
        }

        public static bool operator !=(NumericColumn s1, NumericColumn s2) => !s1.Equals(s2);
        public static bool operator ==(NumericColumn s1, NumericColumn s2) => s1.Equals(s2);

        #endregion Equality
    }
}
