/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Xu
{
    [Serializable, DataContract]
    public class Range<T> : IComparable<T>, IEquatable<T>, IEquatable<Range<T>> where T : IComparable<T>, IEquatable<T>
    {
        public Range(T value) => Set(value);

        public Range(T min, T max) => Reset(min, max);


        public void Reset(T min, T max)
        {
            Minimum = min;
            Maximum = max;
        }

        public void Set(T value)
        {
            Minimum = value;
            Maximum = value;
        }

        public void Set(T value1, T value2)
        {
            if (value1.CompareTo(value2) < 0)
            {
                Minimum = value1;
                Maximum = value2;
            }
            else
            {
                Minimum = value2;
                Maximum = value1;
            }
        }

        public bool Insert(T value)
        {
            bool modified = false;
            //Console.WriteLine("\n\nInsert: " + value.ToString());

            if (value.CompareTo(Maximum) > 0)
            {
                //Console.WriteLine("Before Max = " + Maximum.ToString());
                Maximum = value;
                //Console.WriteLine("Max = " + Maximum.ToString());
                modified = true;
            }

            if (value.CompareTo(Minimum) < 0)
            {

                Minimum = value;
                //Console.WriteLine("Min = " + Minimum.ToString());
                modified = true;
            }

            return modified;
        }

        public bool Insert(ICollection<T> values)
        {
            bool hasChanged = false;

            foreach (T value in values)
            {
                if (Insert(value)) hasChanged = true;
            }

            return hasChanged;
        }

        public bool Contains(T value) => (value.CompareTo(Maximum) <= 0) && (value.CompareTo(Minimum) >= 0);

        public bool Contains(Range<T> other) => Contains(other.Maximum) && Contains(other.Minimum);

        public bool Intersects(Range<T> other) => Contains(other.Maximum) || Contains(other.Minimum) || other.Contains(Maximum) || other.Contains(Minimum);

        public bool Equals(T other) => Contains(other);

        public bool Equals(Range<T> other) => other.Minimum.Equals(Minimum) && other.Maximum.Equals(Maximum);
        public override bool Equals(object obj)
        {
            //if (obj is null)
            //return this is null;

            if (obj.GetType() == typeof(Range<T>))
                return Equals((Range<T>)obj);
            else if (obj.GetType() == typeof(T))
                return Equals((T)obj);
            else
                return false;
        }

        public static bool operator !=(Range<T> s1, Range<T> s2) => !s1.Equals(s2);
        public static bool operator ==(Range<T> s1, Range<T> s2) => s1.Equals(s2);
        public static bool operator !=(Range<T> s1, T s2) => !s1.Equals(s2);
        public static bool operator ==(Range<T> s1, T s2) => s1.Equals(s2);

        int IComparable<T>.CompareTo(T other)
        {
            if (other.CompareTo(Maximum) > 0)
                return other.CompareTo(Maximum);
            else if (other.CompareTo(Minimum) < 0)
                return other.CompareTo(Minimum);
            else
                return 0;
        }

        public static bool operator <(T s1, Range<T> s2) => s1.CompareTo(s2.Minimum) < 0;
        public static bool operator >(T s1, Range<T> s2) => s1.CompareTo(s2.Maximum) > 0;
        /*
        public static bool operator <=(Range<T> left, Range<T> right)
        {
            return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
        }

        public static bool operator >=(Range<T> left, Range<T> right)
        {
            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
        }
        */
        public override string ToString() => "Minimum: " + Minimum + " | Maximum: " + Maximum;
        public string ToStringShort() => Minimum + "-" + Maximum;
        public override int GetHashCode() => Maximum.GetHashCode() ^ Minimum.GetHashCode();

        [IgnoreDataMember, Browsable(false)]
        public (T Min, T Max) Pair => (Minimum, Maximum);

        [IgnoreDataMember, Browsable(false)]
        public T Min => Minimum;

        [IgnoreDataMember, Browsable(false)]
        public T Max => Maximum;

        [DataMember, Browsable(true), DisplayName("Minimum Value")]
        public T Minimum { get; private set; }

        [DataMember, Browsable(true), DisplayName("Maximum Value")]
        public T Maximum { get; private set; }
    }
}
