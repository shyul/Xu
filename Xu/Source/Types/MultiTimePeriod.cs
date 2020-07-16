﻿/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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
    /// A list of times.
    /// </summary>
    [Serializable, DataContract]
    public class MultiTimePeriod : ICollection<TimePeriod>
    {
        public MultiTimePeriod() { }

        public MultiTimePeriod(TimePeriod pd)
        {
            PeriodList.Add(pd);
        }

        [DataMember]
        public bool IsReadOnly { get; set; } = false;

        [DataMember]
        private HashSet<TimePeriod> PeriodList { get; } = new HashSet<TimePeriod>();

        [IgnoreDataMember]
        public int Count => PeriodList.Count;

        public void Clear() => PeriodList.Clear();

        //[IgnoreDataMember]
        //public IEnumerable<TimePeriod> Values => PeriodList.ToArray();
        public IEnumerable<TimePeriod> Get(Time time) => PeriodList.Where(n => n.Contains(time));
        public IEnumerable<TimePeriod> Get(DateTime time) => PeriodList.Where(n => n.Contains(time));
        public IEnumerable<TimePeriod> Get(TimePeriod pd) => PeriodList.Where(n => n.Contains(pd));
        public IEnumerator<TimePeriod> GetEnumerator() => PeriodList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => PeriodList.GetEnumerator();

        public void CopyTo(TimePeriod[] array, int arrayIndex) => PeriodList.CopyTo(array, arrayIndex);

        public bool Contains(Time time)
        {
            lock (PeriodList)
            {
                foreach (TimePeriod item in PeriodList) if (item.Contains(time)) return true;
                return false;
                //return Get(time).Count() > 0;
            }
        }

        public bool Contains(DateTime time)
        {
            lock (PeriodList)
            {
                foreach (TimePeriod item in PeriodList) if (item.Contains(time)) return true;
                return false;
                //return Get(time).Count() > 0;
            }
        }

        public bool Contains(TimePeriod pd)
        {
            lock (PeriodList)
            {
                if (PeriodList.Contains(pd)) return true;
                foreach (TimePeriod item in PeriodList) if (item.Contains(pd)) return true;
                return false;
                //return Get(pd).Count() > 0;
            }
        }

        public void Add(TimePeriod pd)
        {
            if (!IsReadOnly)
                lock (PeriodList)
                {
                    List<TimePeriod> ToRemove = new List<TimePeriod>();
                    foreach (TimePeriod item in PeriodList)
                    {
                        if (item.Intersect(pd))
                        {
                            ToRemove.Add(item);
                            pd += item;
                        }
                    }
                    foreach (TimePeriod item in ToRemove) PeriodList.Remove(item);
                    PeriodList.Add(pd);
                }
        }

        public bool Remove(TimePeriod pd)
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
                        List<TimePeriod> toRemove = new List<TimePeriod>();
                        List<TimePeriod> toAdd = new List<TimePeriod>();

                        foreach (TimePeriod item in PeriodList)
                        {
                            if (pd.Intersect(item))
                            {
                                isModified = true;

                                toRemove.CheckAdd(item);

                                TimePeriod[] res = item - pd;
                                foreach (TimePeriod resPd in res)
                                {
                                    if (!resPd.IsEmpty) toAdd.Add(resPd);
                                }
                            }
                        }

                        foreach (TimePeriod item in toRemove) PeriodList.Remove(item);
                        foreach (TimePeriod item in toAdd) PeriodList.CheckAdd(item);
                    }
                }

            return isModified;
        }


    }


}
