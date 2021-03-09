/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// All time related types and functions are here.
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Xu
{
    /// <summary>
    /// A list of periods.
    /// </summary>
    [Serializable, DataContract]
    public class MultiPeriod : ICollection<Period>
    {
        public MultiPeriod() { }

        public MultiPeriod(Period pd)
        {
            PeriodList.Add(pd);
        }

        [DataMember]
        public bool IsReadOnly { get; set; } = false;

        [DataMember]
        private HashSet<Period> PeriodList { get; set; } = new HashSet<Period>();

        [IgnoreDataMember]
        public int Count => PeriodList.Count;

        [IgnoreDataMember]
        public bool IsEmpty => PeriodList.Count < 1;

        public void Clear() => PeriodList.Clear();

        public IEnumerable<Period> Get(Period pd) => PeriodList.Where(n => n.Contains(pd));

        public IEnumerable<Period> Get(DateTime time) => PeriodList.Where(n => n.Contains(time));

        public IEnumerator<Period> GetEnumerator() => PeriodList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => PeriodList.GetEnumerator();

        public void CopyTo(Period[] array, int arrayIndex) => PeriodList.CopyTo(array, arrayIndex);

        public bool Contains(DateTime time)
        {
            lock (PeriodList)
            {
                foreach (Period item in PeriodList) if (item.Contains(time)) return true;
                return false;
                //return Get(time).Count() > 0;
            }
        }

        public bool Contains(Period pd)
        {
            lock (PeriodList)
            {
                if (PeriodList.Contains(pd)) return true;
                foreach (Period item in PeriodList) if (item.Contains(pd)) return true;
                return false;
                //return Get(pd).Count() > 0;
            }
        }

        public void Add(Period pd)
        {
            if (!IsReadOnly)
                lock (PeriodList)
                {
                    List<Period> ToRemove = new List<Period>();
                    foreach (Period item in PeriodList)
                    {
                        if (item.Intersect(pd))
                        {
                            ToRemove.Add(item);
                            pd += item;
                        }
                    }
                    foreach (Period item in ToRemove) PeriodList.Remove(item);
                    PeriodList.Add(pd);
                }
        }

        public bool Remove(Period pd)
        {
            bool isModified = false;

            if (!IsReadOnly)
                lock (PeriodList)
                {
                    if (PeriodList.Contains(pd))
                    {
                        isModified = true;
                        PeriodList.Remove(pd);
                    }
                    else
                    {
                        List<Period> toRemove = new List<Period>();
                        List<Period> toAdd = new List<Period>();

                        foreach (Period item in PeriodList)
                        {
                            if (pd.Intersect(item))
                            {
                                isModified = true;

                                toRemove.CheckAdd(item);

                                Period[] res = item - pd;
                                foreach (Period resPd in res)
                                {
                                    if (!resPd.IsEmpty) toAdd.Add(resPd);
                                }
                            }
                        }

                        foreach (Period item in toRemove) PeriodList.Remove(item);
                        foreach (Period item in toAdd) PeriodList.CheckAdd(item);
                    }
                }

            return isModified;
        }

        [IgnoreDataMember]
        public bool IsCurrent
        {
            get
            {
                var res = PeriodList.OrderBy(n => n.Start);
                if (res.Count() > 0)
                {
                    return res.Last().IsCurrent;
                }
                return false;
            }
        }

        [IgnoreDataMember]
        public DateTime Start
        {
            get
            {
                var res = PeriodList.OrderBy(n => n.Start);
                if (res.Count() > 0)
                {
                    return res.First().Start;
                }
                return DateTime.MaxValue;
            }
        }

        [IgnoreDataMember]
        public DateTime Stop
        {
            get
            {
                var res = PeriodList.OrderBy(n => n.Stop);
                if (res.Count() > 0)
                {
                    return res.Last().Stop;
                }
                return DateTime.MinValue;
            }
        }
    }
}
