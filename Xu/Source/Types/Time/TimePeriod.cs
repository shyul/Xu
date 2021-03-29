/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
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
    public struct TimePeriod : IEquatable<TimePeriod>, IComparable<DateTime>, IComparable<Time>
    {
        public static TimePeriod Full => new(new Time(0), new Time(23, 59, 59, 999));

        public static TimePeriod Empty => new(new Time(23, 59, 59, 999), new Time(23, 59, 59, 999));

        public TimePeriod(Time time, bool isCurrent = false)
        {
            m_stop = time;
            m_start = time;

            m_time = time;
            IsCurrent = isCurrent;
        }

        public TimePeriod(Time start, Time stop)
        {
            if (start > stop)
            {
                m_start = stop;
                m_stop = start;
            }
            else
            {
                m_start = start;
                m_stop = stop;
            }

            m_time = m_start;
            IsCurrent = false;
        }

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

        public void Reset()
        {
            IsCurrent = false;
            m_stop = Time.MinValue;
            m_start = Time.MaxValue;
        }


        [IgnoreDataMember, XmlIgnore]
        public bool IsEmpty => !IsCurrent && (Start == Stop || (m_stop == Time.MinValue && m_start == Time.MaxValue));

        public bool Contains(Time time) => Start <= time && Stop > time;
        public bool Contains(DateTime time) => Start <= time && Stop > time;
        public bool Contains(TimePeriod pd) => pd.Start >= Start && pd.Stop <= Stop;
        public bool Intersect(TimePeriod pd) => (pd.Start >= Start && pd.Start <= Stop) || (pd.Stop >= Start && pd.Stop <= Stop) || (Start >= pd.Start && Start <= pd.Stop) || (Stop >= pd.Start && Stop <= pd.Stop);

        public void Insert(Time time)
        {
            if (IsCurrent)
            {
                m_time = time;
            }
            else
            {
                if (time < m_start) m_start = time;
                else if (time > m_stop) m_stop = time;
            }
        }
        /*
        public void Insert(Period pd)
        {
            Insert(pd.Start);
            Insert(pd.Stop);
        }*/

        public void SetStart(Time time)
        {
            if (IsCurrent)
            {
                if (m_time < DateTime.Now)
                {
                    m_time = time;
                    return;
                }
                else
                {
                    IsCurrent = false;
                    m_stop = m_time;
                }
            }

            m_start = time;
            if (m_start > m_stop) m_stop = m_start;
        }

        public void SetStop(Time time)
        {
            if (IsCurrent)
            {
                if (m_time > DateTime.Now)
                {
                    m_time = time;
                    return;
                }
                else
                {
                    IsCurrent = false;
                    m_start = m_time;
                }
            }

            m_stop = time;
            if (m_stop < m_start) m_start = m_stop;
        }

        public static TimePeriod operator +(TimePeriod pd, Time t) { pd.Insert(t); return pd; }
        public static TimePeriod operator +(TimePeriod s1, TimePeriod s2) { s1.Insert(s2.m_start); s1.Insert(s2.m_stop); return s1; }

        public static TimePeriod[] operator -(TimePeriod s1, TimePeriod s2)
        {
            if (s1.Contains(s2))
            {
                return new TimePeriod[] { new TimePeriod(s1.Start, s2.Start), new TimePeriod(s2.Stop, s1.Stop) };
            }
            else if (s2.Contains(s1))
            {
                s1.SetStart(s1.Stop); // Become Empty
            }
            else if (s1.Intersect(s2))
            {
                if (s1.Start < s2.Start)
                    s1.SetStop(s2.Start);
                else
                    s1.SetStart(s2.Stop);
            }

            return new TimePeriod[] { s1 };
        }

        /*
        public static Period[] operator +(ICollection<Period> s1, ICollection<Period> s2)
        {
            MultiPeriod mp = new MultiPeriod();

            foreach (Period pd in s1)
            {
                mp.Add(pd);
            }

            foreach (Period pd in s2)
            {
                mp.Add(pd);
            }

            return mp.Values;
        }
        */

        public static MultiTimePeriod operator -(TimePeriod s1, IEnumerable<TimePeriod> s2)
        {
            MultiTimePeriod mp = new() { s1 };

            foreach (TimePeriod pd in s2)
                mp.Remove(pd);

            return mp; //.Values;
        }

        //public static int operator /(Period p, Frequency f) => p.Span / f;

        #region Compare

        public int CompareTo(DateTime other)
        {
            if (Stop <= other)
                return -1;
            else if (Start > other)
                return 1;
            else
                return 0;
        }

        public static bool operator >(TimePeriod s1, DateTime s2) => s1.CompareTo(s2) > 0;
        public static bool operator <(TimePeriod s1, DateTime s2) => s1.CompareTo(s2) < 0;
        public static bool operator >=(TimePeriod s1, DateTime s2) => s1.CompareTo(s2) >= 0;
        public static bool operator <=(TimePeriod s1, DateTime s2) => s1.CompareTo(s2) <= 0;

        public int CompareTo(Time other)
        {
            if (Stop <= other)
            {
                return Stop.TotalMilliseconds - other.TotalMilliseconds - 1;
            }
            else if (Start > other)
            {
                return Start.TotalMilliseconds - other.TotalMilliseconds;
            }
            else
                return 0;
        }

        public static bool operator >(TimePeriod t1, Time t2) => t1.CompareTo(t2) > 0;
        public static bool operator <(TimePeriod t1, Time t2) => t1.CompareTo(t2) < 0;
        public static bool operator >=(TimePeriod t1, Time t2) => t1.CompareTo(t2) >= 0;
        public static bool operator <=(TimePeriod t1, Time t2) => t1.CompareTo(t2) <= 0;


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

        public override string ToString() => m_start.ToString() + TextTool.ValueSeparator + m_stop.ToString();
    }
}
