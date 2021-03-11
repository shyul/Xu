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
        
        public List<Period> Split(Frequency freq)
        {
            List<Period> res = new List<Period>();
            var list = PeriodList.OrderBy(n => n.Start);

            if (list.Count() > 0)
            {
                Period range = new Period(list.First().Start, list.Last().Stop);
                DateTime start = freq.Align(range.Start);// - freq.Span;

                while(start <= range.Stop) 
                {
                    Period pd = new Period(start, start + freq.Span);
                    res.Add(pd);
                    start = pd.Stop;

                    if (!Contains(start))
                    {
                        var slist = PeriodList.Where(n => n.Start > start).OrderBy(n => n.Start);
                        if(slist.Count() > 0) 
                        {
                            start = freq.Align(slist.First().Start); // Find next start and align it.
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return res;
        }

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
                return PeriodList.Where(n => n.Contains(time)).Count() > 0;
                // .OrderBy(n => n.Start);
                //foreach (Period item in PeriodList) if (item.Contains(time)) return true;
                //return false;
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

            if (!IsReadOnly && !pd.IsEmpty)
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
