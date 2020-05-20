/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Xu
{
    public sealed class ObjectColumn : IEquatable<ObjectColumn>
    {
        public ObjectColumn(string name, Type type)
        {
            Type = type;
            Name = name;
        }

        public Type Type { get; private set; }

        public string Name { get; private set; }

        public string Label { get; set; } = string.Empty;

        #region Equality

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode();

        public bool Equals(ObjectColumn other) => Name == other.Name;

        public static bool operator !=(ObjectColumn s1, ObjectColumn s2) => !s1.Equals(s2);
        public static bool operator ==(ObjectColumn s1, ObjectColumn s2) => s1.Equals(s2);

        public override bool Equals(object other)
        {
            if (other is ObjectColumn dc)
                return Equals(dc);
            else
                return false;
        }

        #endregion Equality
    }
}
