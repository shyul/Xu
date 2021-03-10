/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// All time related types and functions are here.
/// 
/// ***************************************************************************

using System;
using System.Globalization;

namespace Xu
{
    public static class TimeTool
    {
        public static DateTime MinInvalid { get; } = DateTime.MinValue.AddDays(1).Date;

        public static DateTime MaxInvalid { get; } = DateTime.MaxValue.AddDays(-1).Date;

        public static bool IsInvalid(this DateTime time) => time >= MaxInvalid || time <= MinInvalid;

        public static DateTime Epoch { get; } = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static double ToEpoch(this DateTime time) => (time.ToUtc() - Epoch).TotalSeconds;

        public static DateTime FromEpoch(long epoch) => Epoch.AddSeconds(epoch);

        public static (bool valid, DateTime time) ToDateTime(this string input, string format = "")
        {
            try
            {
                if (input == null)
                    return (false, Epoch);
                else
                {
                    string str = input.Trim();
                    if (str.Length < 5)
                        return (false, Epoch);
                    else
                    {
                        if (string.IsNullOrWhiteSpace(format))
                        {
                            DateTime t = DateTime.Parse(str);
                            return (true, t);
                        }
                        else
                        {
                            DateTime t = DateTime.ParseExact(str, format, CultureInfo.InvariantCulture);
                            return (true, t);
                        }
                    }
                }
            }
            catch //(Exception e)
            {
                Console.WriteLine("ToDateTime Error: " + input);
                return (false, Epoch);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localTime"></param>
        /// <param name="destinationTimeZoneInfo"></param>
        /// <returns></returns>
        public static DateTime ToDestination(this DateTime localTime, TimeZoneInfo destinationTimeZoneInfo)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(localTime.ToUtc(), destinationTimeZoneInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localTime"></param>
        /// <param name="timeZone"></param>
        /// <returns></returns>
        public static DateTime ToUtc(this DateTime localTime, TimeZoneInfo timeZone)
        {
            return TimeZoneInfo.ConvertTimeToUtc(localTime, timeZone);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localTime"></param>
        /// <returns></returns>
        public static DateTime ToUtc(this DateTime localTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(localTime, TimeZoneInfo.Local);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="utcTime"></param>
        /// <param name="timeZone"></param>
        /// <returns></returns>
        public static DateTime FromUtc(this DateTime utcTime, TimeZoneInfo timeZone)
        {
            DateTime time = new DateTime(utcTime.Year, utcTime.Month, utcTime.Day,
                utcTime.Hour, utcTime.Minute, utcTime.Second, DateTimeKind.Utc);
            return TimeZoneInfo.ConvertTimeFromUtc(time, timeZone);
        }

        /// <summary>
        /// Get the week number of the time
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
        }

        public static Time Time(this DateTime dt) => new Time(dt);
    }
}
