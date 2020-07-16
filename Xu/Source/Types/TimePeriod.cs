/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// All time related types and functions are here.
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Xu
{
    /// <summary>
    /// A Segment of time with defined start and end.
    /// </summary>
    [Serializable, DataContract]
    public struct TimePeriod : IEquatable<TimePeriod>
    {
        /// <summary>
        /// The Center Time
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public Time Center => Start.AddMilliseconds((Start.TotalMilliseconds - Stop.TotalMilliseconds) / 2);

        [IgnoreDataMember, XmlIgnore, DisplayName("Start time")]
        public Time Start
        {
            get
            {
                if (!IsCurrent)
                {
                    return m_start;
                }
                else
                {
                    Time current = Time.FromDateTime(DateTime.Now);
                    if (m_time < current) return m_time;
                    else return current;
                }
            }
            set
            {
                if (!IsCurrent)
                {
                    m_start = value;
                }
                else
                {
                    m_time = value;
                }
            }
        }

        [IgnoreDataMember, XmlIgnore, DisplayName("Stop time")]
        public Time Stop
        {
            get
            {
                if (!IsCurrent)
                {
                    return m_stop;
                }
                else
                {
                    Time current = Time.FromDateTime(DateTime.Now);
                    if (m_time > current) return m_time;
                    else return current;
                }
            }
            set
            {
                if (!IsCurrent)
                {
                    m_stop = value;
                }
                else
                {
                    m_time = value;
                }
            }
        }



        [DataMember]
        private Time m_start;

        [DataMember]
        private Time m_stop;

        [DataMember]
        private Time m_time;

        [DataMember]
        public bool IsCurrent { get; private set; }

        public bool Contains(DateTime time) => Start <= time && Stop > time;

        #region Compare

        public int CompareTo(DateTime other)
        {
            if (Stop < other) return -1;
            else if (Start > other) return 1;
            else return 0;
        }

        public static bool operator >(TimePeriod s1, DateTime s2) => s1.CompareTo(s2) > 0;
        public static bool operator <(TimePeriod s1, DateTime s2) => s1.CompareTo(s2) < 0;
        public static bool operator >=(TimePeriod s1, DateTime s2) => s1.CompareTo(s2) >= 0;
        public static bool operator <=(TimePeriod s1, DateTime s2) => s1.CompareTo(s2) <= 0;

        #endregion Compare

        #region Equality

        public bool Equals(DateTime other) => Contains(other);
        public static bool operator ==(TimePeriod pd, DateTime t) => pd.Equals(t);
        public static bool operator !=(TimePeriod pd, DateTime t) => !pd.Equals(t);

        public bool Equals(TimePeriod other)
        {
            if (IsCurrent && other.IsCurrent)
                return (m_time == other.m_time);
            else if (!IsCurrent && !other.IsCurrent)
                return (m_start == other.m_start && m_stop == other.m_stop);
            else return false;
        }
        public static bool operator ==(TimePeriod s1, TimePeriod s2) => s1.Equals(s2);
        public static bool operator !=(TimePeriod s1, TimePeriod s2) => !s1.Equals(s2);

        public override bool Equals(object other)
        {
            if (other is Period pd)
                return Equals(pd);
            else if (other is DateTime dt)
                return Equals(dt);
            else
                return false;
        }

        public override int GetHashCode() => IsCurrent ? m_time.GetHashCode() : m_start.GetHashCode() ^ m_stop.GetHashCode();

        #endregion Equality
    }
}
