/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Xu
{
    public abstract class Column : IObject, IOrdered, IEquatable<Column>
    {
        public virtual string Name { get; set; } = string.Empty;

        public virtual string Label { get; set; } = string.Empty;

        public virtual string Description { get; set; } = string.Empty;

        public virtual bool Enabled { get; set; } = true;

        public virtual int Order { get; set; } = 0;

        public virtual Importance Importance { get; set; }

        public virtual bool AutoWidth { get; set; } = false;

        public virtual int Width { get; set; }

        #region Equality

        public override int GetHashCode() => Name.GetHashCode();

        public virtual bool Equals(Column other) => Name == other.Name;

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
