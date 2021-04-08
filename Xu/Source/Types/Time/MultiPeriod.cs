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

        public MultiPeriod(IEnumerable<Period> pds)
        {
            pds.RunEach(n => Add(n));
        }

        [DataMember]
        public bool IsReadOnly { get; set; } = false;

        [DataMember]
        private List<Period> PeriodList { get; set; } = new();

        public override string ToString()
        {
            string st = "####### " + base.ToString() + " #######\n";

            st += "Overall Period: " + Period + "\n";

            int i = 0;
            foreach (var pd in PeriodList)
            {
                st += i + " | " + pd.ToString() + " " + (pd.IsEmpty ? "Empty" : pd.Span.TotalSeconds + "secs") + " " + (pd.IsCurrent ? "IsCurrent " : "NotCurrent ") + "\n";
                i++;
            }

            return st;
        }

        public Period Period
        {
            get
            {
                var list = PeriodList.OrderBy(n => n.Start);
                if (list.Count() > 0)
                {
                    return new Period(list.First().Start, list.Last().Stop);
                }
                else
                    return null;
            }
        }

        public List<Period> Split(Frequency freq)
        {
            List<Period> res = new();
            //var list = PeriodList.OrderBy(n => n.Start);

            //if (list.Count() > 0)
            if (Period is Period range)
            {
                //Period range = new Period(list.First().Start, list.Last().Stop);
                DateTime start = freq.Align(range.Start);// - freq.Span;

                while (start <= range.Stop)
                {
                    Period pd = new(start, start + freq.Span);
                    res.Add(pd);
                    start = pd.Stop;

                    if (!Contains(start))
                    {
                        var slist = PeriodList.Where(n => n.Start > start).OrderBy(n => n.Start);
                        if (slist.Count() > 0)
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

        public MultiPeriod Invert(Period pd)
        {
            MultiPeriod result = new(pd);

            foreach (Period pdm in PeriodList)
            {
                result.Remove(pdm);
            }

            return result;
        }

        public MultiPeriod Invert() => Invert(new Period(Start, Stop));

        public void Add(Period pd)
        {
            if (!IsReadOnly)
                lock (PeriodList)
                {
                    List<Period> ToRemove = new();
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
                        PeriodList.RemoveAll(n => n == pd);
                    }

                    List<Period> toRemove = new();
                    List<Period> toAdd = new();

                    foreach (Period item in PeriodList)
                    {
                        if (pd.Intersect(item))
                        {
                            isModified = true;

                            toRemove.CheckAdd(item);

                            Period[] res = item - pd;

                            toAdd.AddRange(res.Where(n => !n.IsEmpty));
                        }
                    }

                    foreach (Period item in toRemove)
                        PeriodList.Remove(item);

                    foreach (Period item in toAdd)
                    {
                        //Console.WriteLine("Adding fraction period " + item);
                        PeriodList.CheckAdd(item);
                    }
                }

            return isModified;
        }

        public void Remove(MultiPeriod mp)
        {
            var mp_ = mp.Invert(new Period(Start, Stop));
            foreach (var pd in mp_)
            {
                Remove(pd);
            }
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
