/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// All time related types and functions are here.
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Xu
{
    /// <summary>
    /// Describing the work hours within a typical week.
    /// </summary>
    [Serializable, DataContract]
    public class WorkHours
    {
        public WorkHours(string timeZoneName, Dictionary<DayOfWeek, List<(Time Start, Time Stop)>> list)
        {
            TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
            List = list;
        }

        [DataMember]
        public TimeZoneInfo TimeZoneInfo { get; private set; }

        public Dictionary<DayOfWeek, List<(Time Start, Time Stop)>> List;

        public bool IsWorkDate(DateTime time) => List.ContainsKey(time.DayOfWeek);

        public bool IsWorkTime(DateTime time)
        {
            if (List.ContainsKey(time.DayOfWeek))
            {
                Time t = new Time(time);
                foreach ((Time Start, Time Stop) in List[time.DayOfWeek])
                {
                    if (t >= Start && t < Stop) return true;
                }
                return false;
            }
            else
                return false;
        }

        public bool IsWorkTime()
        {
            return IsWorkTime(DateTime.Now.ToDestination(TimeZoneInfo));
        }
    }
}
