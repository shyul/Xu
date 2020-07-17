/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// All time related types and functions are here.
/// 
/// ***************************************************************************

using System;
using System.Runtime.Serialization;

namespace Xu
{
    /// <summary>
    /// Time Only: DateTime without Date
    /// </summary>
    [Serializable, DataContract]
    public struct Time : IEquatable<Time>, IEquatable<DateTime>, IComparable<Time>
    {
        public Time(int hour, int minute = 0, int second = 0, int millisecond = 0)
        {
            if (hour > 23) Hour = 23;
            else if (hour < 0) Hour = 0;
            else Hour = hour;

            if (minute > 59) Minute = 59;
            else if (minute < 0) Minute = 0;
            else Minute = minute;

            if (second > 59) Second = 59;
            else if (second < 0) Second = 0;
            else Second = second;

            if (millisecond > 999) Millisecond = 999;
            else if (millisecond < 0) Millisecond = 0;
            else Millisecond = millisecond;
        }

        public Time(DateTime value)
        {
            Hour = value.Hour;
            Minute = value.Minute;
            Second = value.Second;
            Millisecond = value.Millisecond;
        }

        [DataMember]
        public int Hour { get; private set; }

        [DataMember]
        public int Minute { get; private set; }

        [DataMember]
        public int Second { get; private set; }

        [DataMember]
        public int Millisecond { get; private set; }

        public Time AddHours(int value)
        {
            int n_Hour = Hour + value;
            n_Hour %= 24;

            return new Time(n_Hour, Minute, Second, Millisecond);
        }

        public Time AddMinutes(int value)
        {
            int total_Minute = Minute + value;
            int n_Minute = total_Minute % 60;

            int n_Hour = Hour + Math.Floor(total_Minute / 60D).ToInt32(0);
            n_Hour %= 24;

            return new Time(n_Hour, n_Minute, Second, Millisecond);
        }

        public Time AddSeconds(int value)
        {
            int total_Second = Second + value;
            int n_Second = total_Second % 60;

            int total_Minute = Minute + Math.Floor(total_Second / 60D).ToInt32(0);
            int n_Minute = total_Minute % 60;

            int n_Hour = Hour + Math.Floor(total_Minute / 60D).ToInt32(0);
            n_Hour %= 24;

            return new Time(n_Hour, n_Minute, n_Second, Millisecond);
        }

        public Time AddMilliseconds(int value)
        {
            int total_ms = Millisecond + value;
            int n_Millisecond = total_ms % 1000;

            int total_Second = Second + Math.Floor(total_ms / 1000D).ToInt32(0);
            int n_Second = total_Second % 60;

            int total_Minute = Minute + Math.Floor(total_Second / 60D).ToInt32(0);
            int n_Minute = total_Minute % 60;

            int n_Hour = Hour + Math.Floor(total_Minute / 60D).ToInt32(0);
            n_Hour %= 24;

            return new Time(n_Hour, n_Minute, n_Second, n_Millisecond);
        }

        [IgnoreDataMember]
        public int TotalMilliseconds => 1000 * (Hour * 3600 + Minute * 60 + Second) + Millisecond;

        [IgnoreDataMember]
        public double TotalSeconds => Hour * 3600 + Minute * 60 + Second + (Millisecond / 1000.0);

        [IgnoreDataMember]
        public double TotalMinutes => TotalSeconds * 60;

        [IgnoreDataMember]
        public double TotalHours => TotalSeconds * 3600;

        #region Compare

        public int CompareTo(Time other) => TotalMilliseconds - other.TotalMilliseconds;
        public static bool operator >(Time t1, Time t2) => t1.CompareTo(t2) > 0;
        public static bool operator <(Time t1, Time t2) => t1.CompareTo(t2) < 0;
        public static bool operator >=(Time t1, Time t2) => t1.CompareTo(t2) >= 0;
        public static bool operator <=(Time t1, Time t2) => t1.CompareTo(t2) <= 0;

        public int CompareTo(DateTime other) => TotalMilliseconds - (1000 * (other.Hour * 3600 + other.Minute * 60 + other.Second) + other.Millisecond);
        public static bool operator >(Time t1, DateTime t2) => t1.CompareTo(t2) > 0;
        public static bool operator <(Time t1, DateTime t2) => t1.CompareTo(t2) < 0;
        public static bool operator >=(Time t1, DateTime t2) => t1.CompareTo(t2) >= 0;
        public static bool operator <=(Time t1, DateTime t2) => t1.CompareTo(t2) <= 0;

        #endregion Compare

        #region Equality

        public bool Equals(Time other) => Hour == other.Hour && Minute == other.Minute && Second == other.Second && Millisecond == other.Millisecond;
        public static bool operator ==(Time s1, Time s2) => s1.Equals(s2);
        public static bool operator !=(Time s1, Time s2) => !s1.Equals(s2);

        public bool Equals(DateTime other) => Hour == other.Hour && Minute == other.Minute && Second == other.Second && Millisecond == other.Millisecond;
        public static bool operator ==(Time s1, DateTime s2) => s1.Equals(s2);
        public static bool operator !=(Time s1, DateTime s2) => !s1.Equals(s2);

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Time))
                return Equals((Time)obj);
            if (obj.GetType() == typeof(DateTime))
                return Equals((DateTime)obj);
            else
                return false;
        }

        public override int GetHashCode() => TotalMilliseconds;

        #endregion Equality

        public override string ToString() =>
            Hour.ToString("00") +
            ":" + Minute.ToString("00") +
            ((Second != 0 || Millisecond != 0) ? ":" + Second.ToString("00") : string.Empty) +
            (Millisecond != 0 ? "," + Millisecond.ToString("000") : string.Empty);

        public static Time FromDateTime(DateTime dt) => new Time(dt.Hour, dt.Minute, dt.Second, dt.Millisecond);

        [IgnoreDataMember]
        public static Time MinValue => new Time(0, 0, 0, 0);

        [IgnoreDataMember]
        public static Time MaxValue => new Time(23, 59, 59, 999);
    }
}
