﻿/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// All time related types and functions are here.
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Xu
{
    /// <summary>
    /// A Segment of time with defined start and end.
    /// Comparison: TimeSpan is without defined start or end.
    /// </summary>
    [Serializable, DataContract]
    public class Period : IEquatable<Period>, IEquatable<DateTime>, IComparer<Period>, IComparable<Period>, IComparable<DateTime>
    {
        [IgnoreDataMember, XmlIgnore]
        public static Period Full => new(DateTime.MinValue.AddDays(7), DateTime.MaxValue.AddDays(-7));

        [IgnoreDataMember, XmlIgnore]
        public static Period Empty => new(DateTime.MinValue.AddDays(7), DateTime.MinValue.AddDays(7));

        #region Ctor

        public Period(Period period)
        {
            m_stop = period.Stop;
            m_start = period.Start;

            m_time = period.Start;
            IsCurrent = period.IsCurrent;
        }

        public Period(DateTime time, bool isCurrent = false)
        {
            m_stop = time;
            m_start = time;

            m_time = time;
            IsCurrent = isCurrent;
        }

        public Period(DateTime start, DateTime stop)
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

        public Period(DateTime start, TimeSpan span)
        {
            m_start = start;
            m_stop = start + span;

            m_time = m_start;
            IsCurrent = false;
        }

        public Period(DateTime start, Frequency freq)
        {
            m_start = start;
            m_stop = start + freq.Span;

            m_time = m_start;
            IsCurrent = false;
        }

        public Period(TimeSpan span, DateTime stop)
        {
            m_stop = stop;
            m_start = stop - span;

            m_time = m_start;
            IsCurrent = false;
        }

        public Period(Frequency freq, DateTime stop)
        {
            m_stop = stop;
            m_start = stop - freq.Span;

            m_time = m_start;
            IsCurrent = false;
        }

        public Period()
        {
            Reset();
        }

        #endregion Ctor

        public void Insert(DateTime time)
        {
            if (IsCurrent)
            {
                m_time = time;
            }
            else
            {
                if (time < m_start) m_start = time;
                if (time > m_stop) m_stop = time;
            }
        }

        public void Insert(Period pd) 
        {
            Insert(pd.Start);
            Insert(pd.Stop);
        }

        public void ChopStart(DateTime time)
        {
            if (IsCurrent)
            {
                if (time < DateTime.Now)
                {
                    m_time = time;
                    return;
                }
            }
            else if (m_start < time)
            {
                m_start = time;
            }

            if (m_start > m_stop) m_stop = time;
        }

        public void ChopStop(DateTime time)
        {
            if (IsCurrent)
            {
                if (time > DateTime.Now)
                {
                    m_time = time;
                    return;
                }
            }
            else if (m_stop > time)
            {
                m_stop = time;
            }

            if (m_start > m_stop) m_start = time;
        }

        public void SetStart(DateTime time)
        {
            if (IsCurrent)
            {
                if (time < DateTime.Now)
                {
                    m_time = time;
                    return;
                }
                else
                {
                    IsCurrent = false;
                    m_stop = time;
                }
            }

            m_start = time;
            if (m_start > m_stop) m_stop = m_start;
        }

        public void SetStop(DateTime time)
        {
            if (IsCurrent)
            {
                if (time > DateTime.Now)
                {
                    m_time = time;
                    return;
                }
                else
                {
                    IsCurrent = false;
                    m_start = time;
                }
            }

            m_stop = time;
            if (m_stop < m_start) m_start = m_stop;
        }

        public void Reset()
        {
            IsCurrent = false;
            m_stop = DateTime.MinValue;
            m_start = DateTime.MaxValue;
        }

        public List<Period> Split(Frequency freq)
        {
            Frequency unit = new(freq.Unit);
            DateTime start = unit.Align(m_start, -1);
            DateTime stop = unit.Align(m_stop, 1);

            List<Period> result = new();

            while (start < stop)
            {
                Period pd = new(start, start + freq);
                start = pd.Stop;
                result.Add(pd);
            }

            return result;
        }

        public static Period operator +(Period pd, DateTime t) { pd.Insert(t); return pd; }
        public static Period operator +(Period s1, Period s2) { s1.Insert(s2.m_start); s1.Insert(s2.m_stop); return s1; }

        public static Period[] operator -(Period s1, Period s2)
        {
            if (s1 == s2) 
            {
                return new Period[] { };
            }
            else if (s1.Contains(s2))
            {
                return new Period[] { new Period(s1.Start, s2.Start), new(s2.Stop, s1.Stop) };
            }
            else if (s2.Contains(s1))
            {
                s1.SetStart(s1.Stop); // Become Empty
            }
            else if (s1.Intersects(s2))
            {
                if (s1.Start < s2.Start)
                    s1.SetStop(s2.Start);
                else
                    s1.SetStart(s2.Stop);
            }

            return new Period[] { s1 };
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

        public static MultiPeriod operator -(Period s1, IEnumerable<Period> s2)
        {
            MultiPeriod mp = new() { s1 };

            foreach (Period pd in s2)
                mp.Remove(pd);

            return mp;
        }

        public static int operator /(Period p, Frequency f) => p.Span / f;

        public bool Contains(DateTime time) => time >= Start && time < Stop;
        public bool Contains(Period pd) => pd.Start >= Start && pd.Stop <= Stop;
        public bool Intersects(Period pd) => (pd.Start >= Start && pd.Start <= Stop) || (pd.Stop >= Start && pd.Stop <= Stop) || (Start >= pd.Start && Start <= pd.Stop) || (Stop >= pd.Start && Stop <= pd.Stop);

        //public bool Intersect(Period pd)

        /// <summary>
        /// The TimeSpan of the period
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public TimeSpan Span => Stop - Start;

        /// <summary>
        /// The Center Time
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public DateTime Center => Start.AddMilliseconds(Span.TotalMilliseconds / 2);

        [IgnoreDataMember, XmlIgnore, DisplayName("Start time")]
        public DateTime Start
        {
            get
            {
                if (!IsCurrent)
                {
                    return m_start;
                }
                else
                {
                    DateTime current = DateTime.Now;
                    return current > m_time ? m_time : current;
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
        public DateTime Stop
        {
            get
            {
                if (!IsCurrent)
                {
                    return m_stop;
                }
                else
                {
                    DateTime current = DateTime.Now;
                    return current < m_time ? m_time : current;
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
        private DateTime m_start;

        [DataMember]
        private DateTime m_stop;

        [DataMember]
        private DateTime m_time;

        [DataMember]
        public bool IsCurrent { get; private set; }

        [IgnoreDataMember, XmlIgnore]
        public bool IsEmpty => !IsCurrent && Start >= Stop;

        #region String

        public override string ToString() => ToString(Format);

        public string ToString(string formatString, char spliter = TextTool.ValueSeparator)
        {
            return Start.ToString(formatString) + spliter + Stop.ToString(formatString);
        }

        [IgnoreDataMember, XmlIgnore]
        private const string Format = "MM-dd-yyyy HH:mm:ss";

        #endregion

        #region Compare

        public int CompareTo(DateTime other)
        {
            if (other > Stop) return -1;
            else if (other < Start) return 1;
            else return 0;
        }

        public static bool operator >(Period s1, DateTime s2) => s1.CompareTo(s2) > 0;
        public static bool operator <(Period s1, DateTime s2) => s1.CompareTo(s2) < 0;
        public static bool operator >=(Period s1, DateTime s2) => s1.CompareTo(s2) >= 0;
        public static bool operator <=(Period s1, DateTime s2) => s1.CompareTo(s2) <= 0;

        public double Compare(Period other) => (Center - other.Center).TotalMilliseconds;
        public int Compare(Period x, Period y) => (int)x.Compare(y);
        public int CompareTo(Period other)
        {
            if (Compare(other) > 0) return 1;
            else if (Compare(other) < 0) return -1;
            else return 0;
        }

        public static bool operator >(Period s1, Period s2) => s1.CompareTo(s2) > 0;
        public static bool operator <(Period s1, Period s2) => s1.CompareTo(s2) < 0;
        public static bool operator >=(Period s1, Period s2) => s1.CompareTo(s2) >= 0;
        public static bool operator <=(Period s1, Period s2) => s1.CompareTo(s2) <= 0;

        #endregion Compare

        #region Equality

        public bool Equals(DateTime other) => Contains(other);
        public static bool operator ==(Period pd, DateTime t) => pd.Equals(t);
        public static bool operator !=(Period pd, DateTime t) => !pd.Equals(t);

        public bool Equals(Period other)
        {
            if (IsCurrent && other.IsCurrent)
                return (m_time == other.m_time);
            else if (!IsCurrent && !other.IsCurrent)
                return (m_start == other.m_start && m_stop == other.m_stop);
            else return false;
        }
        public static bool operator ==(Period s1, Period s2) => s1.Equals(s2);
        public static bool operator !=(Period s1, Period s2) => !s1.Equals(s2);

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
