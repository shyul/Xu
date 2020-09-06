/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Xu
{
    public abstract class Column : IEquatable<Column>
    {
        public virtual string Name { get; set; } = string.Empty;

        public virtual string Label { get; set; } = string.Empty;

        #region Equality

        public override int GetHashCode() => Name.GetHashCode();

        public virtual bool Equals(Column other) => GetType() == other.GetType() && Name == other.Name;

        public override bool Equals(object other)
        {
            if (other is Column dc)
                return Equals(dc);
            else
                return false;
        }

        public static bool operator !=(Column s1, Column s2) => !s1.Equals(s2);
        public static bool operator ==(Column s1, Column s2) => s1.Equals(s2);

        #endregion Equality
    }
}
