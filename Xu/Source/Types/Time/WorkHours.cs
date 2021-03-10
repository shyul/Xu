/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
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
        public WorkHours(string timeZoneName, Dictionary<DayOfWeek, MultiTimePeriod> list)
        {
            TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
            List = list;
        }

        [DataMember]
        public Dictionary<DayOfWeek, MultiTimePeriod> List;

        [DataMember]
        public TimeZoneInfo TimeZoneInfo { get; private set; }

        [IgnoreDataMember]
        public DateTime CurrentTime => DateTime.Now.ToDestination(TimeZoneInfo);

        public MultiTimePeriod this[DateTime dt]
        {
            get
            {
                if (List.ContainsKey(dt.DayOfWeek))
                    return List[dt.DayOfWeek];
                else
                    return null;
            }
        }

        public bool IsWorkDate(DateTime time) => List.ContainsKey(time.DayOfWeek);

        public bool IsWorkTime(DateTime time)
        {
            if (List.ContainsKey(time.DayOfWeek))
                return List[time.DayOfWeek].Contains(time);
            else
                return false;
        }

        public bool IsWorkTime()
        {
            return IsWorkTime(DateTime.Now.ToDestination(TimeZoneInfo));
        }
    }
}
