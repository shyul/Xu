/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// All time related types and functions are here.
/// 
/// ***************************************************************************

using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Xu
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable, DataContract]
    public enum TimeUnit : int
    {
        Years = 31536000, // 365 days
        Months = 2419200, // 28 days
        Weeks = 604800,
        Days = 86400,
        Hours = 3600,
        Minutes = 60,
        Seconds = 1,
        None = 0
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable, DataContract]
    public struct Frequency : IEquatable<Frequency>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Unit"></param>
        /// <param name="Length"></param>
        public Frequency(TimeUnit Unit, int Length = 1)
        {
            this.Unit = Unit;
            this.Length = Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public Frequency(string s)
        {
            this = Parse(s);
        }

        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public TimeSpan Span
        {
            get
            {
                return Unit switch
                {
                    (TimeUnit.Years) => new TimeSpan(365 * Length, 0, 0, 0),
                    (TimeUnit.Months) => new TimeSpan(30 * Length, 0, 0, 0),// Notice: the month can be 28, 29, 30, 31. Missing situation awareness here.
                    (TimeUnit.Weeks) => new TimeSpan(7 * Length, 0, 0, 0),
                    (TimeUnit.Days) => new TimeSpan(Length, 0, 0, 0),
                    (TimeUnit.Hours) => new TimeSpan(0, Length, 0, 0),
                    (TimeUnit.Minutes) => new TimeSpan(0, 0, Length, 0),
                    (TimeUnit.Seconds) => new TimeSpan(0, 0, 0, Length),
                    (TimeUnit.None) => new TimeSpan(),
                    _ => throw new("Invalid TimeInterval Type!"),
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="cnt"></param>
        /// <returns></returns>
        public DateTime Align(DateTime time, int cnt = 0)
        {
            switch (Unit)
            {
                case (TimeUnit.Years):
                    time = time.AddYears(cnt * Length);
                    return new(time.Year, 1, 1);

                case (TimeUnit.Months):
                    if (Length == 2 || Length == 3 || Length == 4 || Length == 6 || Length == 12)
                    {
                        time = time.AddMonths(cnt * Length - time.Month % Length + 1);
                    }
                    else
                    {
                        time = time.AddMonths(cnt * Length);
                    }
                    return new(time.Year, time.Month, 1);

                case (TimeUnit.Weeks):
                    //int firstDayOffset = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
                    time = new DateTime(time.Year, time.Month, time.Day);
                    if (time.DayOfWeek == DayOfWeek.Monday) return time.AddDays(cnt * Length * 7 - 1);
                    else if (time.DayOfWeek == DayOfWeek.Tuesday) return time.AddDays(cnt * Length * 7 - 2);
                    else if (time.DayOfWeek == DayOfWeek.Wednesday) return time.AddDays(cnt * Length * 7 - 3);
                    else if (time.DayOfWeek == DayOfWeek.Thursday) return time.AddDays(cnt * Length * 7 - 4);
                    else if (time.DayOfWeek == DayOfWeek.Friday) return time.AddDays(cnt * Length * 7 - 5);
                    else if (time.DayOfWeek == DayOfWeek.Saturday) return time.AddDays(cnt * Length * 7 - 6);
                    else return time.AddDays(cnt * Length * 7); // Sunday

                case (TimeUnit.Days):
                    time = time.AddDays(cnt * Length);
                    return new(time.Year, time.Month, time.Day);

                case (TimeUnit.Hours):
                    if (Length == 2 || Length == 3 || Length == 4 || Length == 6 || Length == 8 || Length == 12 || Length == 24)
                    {
                        time = time.AddHours(cnt * Length - time.Hour % Length);
                    }
                    else
                    {
                        time = time.AddHours(cnt * Length);
                    }
                    return new(time.Year, time.Month, time.Day, time.Hour, 0, 0);

                case (TimeUnit.Minutes):
                    if (Length == 2 || Length == 3 || Length == 4 || Length == 5 || Length == 6 ||
                        Length == 10 || Length == 12 || Length == 15 || Length == 20 || Length == 30 || Length == 60)
                    {
                        time = time.AddMinutes(cnt * Length - time.Minute % Length);
                    }
                    else
                    {
                        time = time.AddMinutes(cnt * Length);
                    }
                    return new(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0);

                case (TimeUnit.Seconds):
                    if (Length == 2 || Length == 3 || Length == 4 || Length == 5 || Length == 6 ||
                        Length == 10 || Length == 12 || Length == 15 || Length == 20 || Length == 30 || Length == 60)
                    {
                        time = time.AddSeconds(cnt * Length - time.Second % Length);
                    }
                    else
                    {
                        time = time.AddSeconds(cnt * Length);
                    }
                    return new(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second);

                default:
                    throw new("Invalid TimeInterval Type!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="cnt"></param>
        /// <returns></returns>
        public Period AlignPeriod(DateTime time, int cnt = 0)
        {
            DateTime start = Align(time, cnt);

            return Unit switch
            {
                TimeUnit.Years => new Period(start, start.AddYears(Length)),
                TimeUnit.Months => new Period(start, start.AddMonths(Length)),
                TimeUnit.Weeks => new Period(start, start.AddDays(Length * 7)),
                TimeUnit.Days => new Period(start, start.AddDays(Length)),
                TimeUnit.Hours => new Period(start, start.AddHours(Length)),
                TimeUnit.Minutes => new Period(start, start.AddMinutes(Length)),
                TimeUnit.Seconds => new Period(start, start.AddSeconds(Length)),
                _ => throw new("Invalid TimeInterval Type!"),
            };
        }

        public static Period operator +(Period pd, Frequency f) => f.AlignPeriod(pd.Start, 1);
        public static Period operator -(Period pd, Frequency f) => f.AlignPeriod(pd.Start, -1);
        public static DateTime operator +(DateTime time, Frequency f)
        {
            int cnt = Convert.ToInt32(f.Length);
            return f.Unit switch
            {
                TimeUnit.Years => time.AddYears(cnt),
                TimeUnit.Months => time.AddMonths(cnt),
                TimeUnit.Weeks => time.AddDays(cnt * 7),
                TimeUnit.Days => time.AddDays(cnt),
                TimeUnit.Hours => time.AddHours(cnt),
                TimeUnit.Minutes => time.AddMinutes(cnt),
                TimeUnit.Seconds => time.AddSeconds(cnt),
                TimeUnit.None => time,
                _ => throw new("Invalid TimeInterval Type!"),
            };
        }
        public static DateTime operator -(DateTime time, Frequency f)
        {
            int cnt = (-1) * Convert.ToInt32(f.Length);
            return f.Unit switch
            {
                TimeUnit.Years => time.AddYears(cnt),
                TimeUnit.Months => time.AddMonths(cnt),
                TimeUnit.Weeks => time.AddDays(cnt * 7),
                TimeUnit.Days => time.AddDays(cnt),
                TimeUnit.Hours => time.AddHours(cnt),
                TimeUnit.Minutes => time.AddMinutes(cnt),
                TimeUnit.Seconds => time.AddSeconds(cnt),
                TimeUnit.None => time,
                _ => throw new("Invalid TimeInterval Type!"),
            };
        }

        public static int operator /(Period p, Frequency f)
        {
            return p.Span / f;
        }
        public static int operator /(TimeSpan sp, Frequency f)
        {
            return f.Unit switch
            {
                TimeUnit.Years => Convert.ToInt32((sp.TotalDays / (365 * f.Length)).ToInt64()),
                TimeUnit.Months => Convert.ToInt32((sp.TotalDays / (30 * f.Length)).ToInt64()),
                TimeUnit.Weeks => Convert.ToInt32((sp.TotalDays / (7 * f.Length)).ToInt64()),
                TimeUnit.Days => Convert.ToInt32((sp.TotalDays / f.Length).ToInt64()),
                TimeUnit.Hours => Convert.ToInt32((sp.TotalHours / f.Length).ToInt64()),
                TimeUnit.Minutes => Convert.ToInt32((sp.TotalMinutes / f.Length).ToInt64()),
                TimeUnit.Seconds => Convert.ToInt32((sp.TotalSeconds / f.Length).ToInt64()),
                TimeUnit.None => 0,
                _ => throw new("Invalid TimeInterval Type!"),
            };
        }
        public static Frequency operator -(Frequency f, int d)
        {
            int len = f.Length - d;
            if (len < 1) len = 1;
            return new(f.Unit, len);
        }
        public static Frequency operator +(Frequency f, int d) { return new(f.Unit, f.Length + d); }
        public static Frequency operator *(Frequency f, int d) { return new(f.Unit, f.Length * d); }

        public System.Timers.Timer Timer => new(1000 * Length * (int)Unit);

        #region Equality

        public bool Equals(Frequency value) => (Unit == value.Unit && Length == value.Length) || (Span == value.Span);
        public static bool operator ==(Frequency f1, Frequency f2) => f1.Equals(f2);
        public static bool operator !=(Frequency f1, Frequency f2) => !f1.Equals(f2);

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Frequency))
                return Equals((Frequency)obj);
            else
                return false;
        }

        public override int GetHashCode() => Unit.GetHashCode() ^ Length.GetHashCode();

        #endregion Equality

        [DataMember]
        public TimeUnit Unit { get; private set; }

        [DataMember]
        public int Length { get; private set; }

        public override string ToString()
        {
            switch (Unit)
            {
                case (TimeUnit.Years):
                    if (Length == 1)
                        return "Annually";
                    if (Length == 2)
                        return "Biennial";
                    else
                        return Length.ToString() + " Years";
                case (TimeUnit.Months):
                    if (Length == 1)
                        return "Monthly";
                    else if (Length == 3)
                        return "Quarterly";
                    else if (Length == 6)
                        return "Semiannual";
                    else
                        return Length.ToString() + " Months";
                case (TimeUnit.Weeks):
                    if (Length == 1)
                        return "Weekly";
                    if (Length == 2)
                        return "Biweekly";
                    else
                        return Length.ToString() + " Weeks";
                case (TimeUnit.Days):
                    if (Length == 1)
                        return "Daily";
                    else
                        return Length.ToString() + " Days";
                case (TimeUnit.Hours):
                    if (Length == 1)
                        return "Hourly";
                    else
                        return Length.ToString() + " Hours";
                case (TimeUnit.Minutes):
                    if (Length == 1)
                        return "1 Minute";
                    else
                        return Length.ToString() + " Minutes";
                case (TimeUnit.Seconds):
                    if (Length == 1)
                        return "1-Hertz";
                    else
                        return Length.ToString() + " Seconds";
                case (TimeUnit.None):
                    return "None";
                default: throw new("Invalid TimeInterval Type!");
            }
        }

        public static Frequency Parse(string s)
        {
            string[] a = s.Split(' ');
            if (a.Length == 1)
            {
                return (a[0]) switch
                {
                    ("Annually") => new Frequency(TimeUnit.Years),
                    ("Biennial") => new Frequency(TimeUnit.Years, 2),
                    ("Monthly") => new Frequency(TimeUnit.Months),
                    ("Quarterly") => new Frequency(TimeUnit.Months, 3),
                    ("Semiannual") => new Frequency(TimeUnit.Months, 6),
                    ("Weekly") => new Frequency(TimeUnit.Weeks),
                    ("Biweekly") => new Frequency(TimeUnit.Weeks),
                    ("Daily") => new Frequency(TimeUnit.Days),
                    ("1-Hour") => new Frequency(TimeUnit.Hours),
                    ("1-Minute") => new Frequency(TimeUnit.Minutes),
                    ("1-Hertz") => new Frequency(TimeUnit.Seconds),
                    _ => new Frequency(TimeUnit.None, 0),
                };
            }
            else if (a.Length == 2)
            {
                try
                {
                    int length = a[0].ToInt32(0);
                    return (a[1]) switch
                    {
                        ("Years") => new Frequency(TimeUnit.Years, length),
                        ("Months") => new Frequency(TimeUnit.Months, length),
                        ("Weeks") => new Frequency(TimeUnit.Weeks, length),
                        ("Days") => new Frequency(TimeUnit.Days, length),
                        ("Hours") => new Frequency(TimeUnit.Hours, length),
                        ("Minutes") => new Frequency(TimeUnit.Minutes, length),
                        ("Seconds") => new Frequency(TimeUnit.Seconds, length),
                        _ => new Frequency(TimeUnit.None, length),
                    };
                }
                catch (Exception e) when (e is FormatException || e is InvalidOperationException)
                {
                    return new(TimeUnit.None, 0);
                }
            }
            else
            {
                return new(TimeUnit.None, 0);
            }
        }

        [IgnoreDataMember, XmlIgnore]
        public static readonly Frequency Daily = new(TimeUnit.Days, 1);

        [IgnoreDataMember, XmlIgnore]
        public static readonly Frequency Monthly = new(TimeUnit.Months, 1);

        [IgnoreDataMember, XmlIgnore]
        public static readonly Frequency Quarterly = new(TimeUnit.Months, 3);

        [IgnoreDataMember, XmlIgnore]
        public static readonly Frequency Annually = new(TimeUnit.Years, 1);
    }
}
