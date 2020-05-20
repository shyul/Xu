/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// All time related types and functions are here.
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Xu
{
    /// <summary>
    /// Time Only: DateTime without Date
    /// </summary>
    [Serializable, DataContract]
    public struct Time : IEquatable<Time>, IEquatable<DateTime>
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

        public void AddHours(int value)
        {
            int hour = Hour + value;
            if (hour > 23) Hour = 23;
            else if (hour < 0) Hour = 0;
            else Hour = hour;
        }

        public void AddMinutes(int value)
        {
            int minute = Minute + value;
            if (minute > 59) Minute = 59;
            else if (minute < 0) Minute = 0;
            else Minute = minute;
        }

        [IgnoreDataMember]
        public double TotalSeconds => Hour * 3600 + Minute * 60 + Second + Millisecond / 1000.0;

        [IgnoreDataMember]
        public double TotalMinutes => TotalSeconds * 60;

        [IgnoreDataMember]
        public double TotalHours => TotalSeconds * 3600;

        #region Operators

        public static bool operator >(Time t1, Time t2) => t1.TotalSeconds > t2.TotalSeconds;
        public static bool operator <(Time t1, Time t2) => t1.TotalSeconds < t2.TotalSeconds;
        public static bool operator >=(Time t1, Time t2) => t1.TotalSeconds >= t2.TotalSeconds;
        public static bool operator <=(Time t1, Time t2) => t1.TotalSeconds <= t2.TotalSeconds;

        #endregion Operators

        public bool Equals(Time other) => (Hour == other.Hour && Minute == other.Minute && Second == other.Second && Millisecond == other.Millisecond);
        public static bool operator ==(Time s1, Time s2) => s1.Equals(s2);
        public static bool operator !=(Time s1, Time s2) => !s1.Equals(s2);

        public bool Equals(DateTime other) => (Hour == other.Hour && Minute == other.Minute && Second == other.Second && Millisecond == other.Millisecond);
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

        public override int GetHashCode() => Hour.GetHashCode() ^ Minute.GetHashCode() ^ Second.GetHashCode() ^ Millisecond.GetHashCode();
    }
}
