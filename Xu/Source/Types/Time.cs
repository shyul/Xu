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

        public Time AddHours(int value) => AddMilliseconds(value * 60 * 60 * 1000);

        public Time AddMinutes(int value) => AddMilliseconds(value * 60 * 1000);

        public Time AddSeconds(int value) => AddMilliseconds(value * 1000);

        public Time AddMilliseconds(int value)
        {
            int total_ms = TotalMilliseconds + value;

            double totalHour = total_ms / 3600000;
            int hour = Math.Floor(totalHour).ToInt32(0);
            int n_Hour = hour % 24;

            double totalMinute = (totalHour - hour) * 60;
            int n_Minute = Math.Floor(totalMinute).ToInt32(0);

            double totalSecond = (totalMinute - Minute) * 60;
            int n_Second = Math.Floor(totalSecond).ToInt32(0);

            double ms = (totalSecond - Second) * 1000;
            int n_Millisecond = Math.Floor(ms).ToInt32(0);

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

        public static Time FromDateTime(DateTime dt) => new Time(dt.Hour, dt.Minute, dt.Second, dt.Millisecond);

        [IgnoreDataMember]
        public static Time MinValue => new Time(0, 0, 0, 0);

        [IgnoreDataMember]
        public static Time MaxValue => new Time(23, 59, 59, 999);
    }
}
