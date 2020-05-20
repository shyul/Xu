/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// All time related types and functions are here.
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Xu
{    /// <summary>
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
        private HashSet<Period> PeriodList { get; set; } = new HashSet<Period>();

        [IgnoreDataMember]
        public int Count => PeriodList.Count;

        [DataMember]
        public bool IsReadOnly { get; set; } = false;

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

        public void Clear() => PeriodList.Clear();

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

        public bool Contains(DateTime time)
        {
            lock (PeriodList)
            {
                foreach (Period item in PeriodList) if (item.Contains(time)) return true;
                return false;
                //return Get(time).Count() > 0;
            }
        }

        public IEnumerable<Period> Values => PeriodList.ToArray();

        public IEnumerable<Period> Get(Period pd) => PeriodList.Where(n => n.Contains(pd));

        public IEnumerable<Period> Get(DateTime time) => PeriodList.Where(n => n.Contains(time));

        public IEnumerator<Period> GetEnumerator() => PeriodList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => PeriodList.GetEnumerator();

        public void CopyTo(Period[] array, int arrayIndex) => PeriodList.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// A list of period tethered with assigned object to each period.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable, DataContract]
    public class MultiPeriod<T> : IDictionary<Period, T> //where T : IEquatable<T>
    {
        [DataMember]
        private SortedDictionary<Period, T> PeriodList { get; set; } = new SortedDictionary<Period, T>();

        [IgnoreDataMember]
        public DateTime Start
        {
            get
            {
                if (Count > 0)
                    return PeriodList.First().Key.Start;
                else
                    return DateTime.MinValue;
            }
        }

        [IgnoreDataMember]
        public DateTime Stop
        {
            get
            {
                if (Count > 0)
                    return PeriodList.Last().Key.Stop;
                else
                    return DateTime.MaxValue;
            }
        }

        [IgnoreDataMember]
        public T this[Period pd] { get { TryGetValue(pd, out T val); return val; } set { Add(pd, value); } }

        [IgnoreDataMember]
        public T this[DateTime time]
        {
            get
            {
                if (Count > 0)
                {
                    if (time < Start) return PeriodList.First().Value;
                    else if (time >= Stop) return PeriodList.Last().Value;

                    foreach (var pair in PeriodList) if (pair.Key.Contains(time)) return pair.Value;

                    return default;
                }
                else
                    return default;
            }
        }

        [IgnoreDataMember]
        public Period LastStreak
        {
            get
            {
                if (Count > 0)
                {
                    Period pd = PeriodList.Last().Key;
                    foreach(var item in PeriodList.Reverse()) 
                    {
                        if (pd.Intersect(item.Key)) 
                        {
                            pd.Insert(item.Key.Start);
                            pd.Insert(item.Key.Stop);
                        }
                    }
                    
                    return pd;
                }
                else
                    return Period.Full;
            }
        }

        [IgnoreDataMember]
        public int Count => PeriodList.Count;

        [DataMember]
        public bool IsReadOnly { get; set; } = false;

        [IgnoreDataMember]
        public bool IsCurrent
        {
            get
            {
                if (Count > 0)
                {
                    return PeriodList.Last().Key.IsCurrent;
                }
                return false;
            }
        }

        public void Clear() { lock (PeriodList) PeriodList.Clear(); }

        public void Merge(MultiPeriod<T> source)
        {
            lock (PeriodList)
            {
                lock (source)
                    for (int i = 0; i < source.Count; i++)
                    {
                        var pair = source.ElementAt(i);
                        Add(pair);
                    }
            }
        }

        public void Add(KeyValuePair<Period, T> item) => Add(item.Key, item.Value);

        public void Add(DateTime start, DateTime stop, T value) => Add(new Period(start, stop), value);

        public void Add(Period pd, T value)
        {
            if (!IsReadOnly && !pd.IsEmpty)
            {
                lock (PeriodList)
                {
                    // Remove the intersect segments
                    HashSet<Period> toRemove = new HashSet<Period>();

                    // Add the fragments
                    HashSet<(Period key, T value)> toAddFragment = new HashSet<(Period key, T value)>();

                    foreach (Period existPd in PeriodList.Keys)
                    {
                        if (pd.Intersect(existPd))
                        {
                            toRemove.CheckAdd(existPd);

                            if (PeriodList[existPd].Equals(value))
                            {
                                pd += existPd; // Merge the Period into one
                            }
                            else
                            {
                                Period[] fragmentPds = existPd - pd;
                                foreach (Period fragmentPd in fragmentPds)
                                {
                                    if (!fragmentPd.IsEmpty) toAddFragment.Add((fragmentPd, PeriodList[existPd]));
                                }
                            }
                        }
                    }

                    foreach (Period item in toRemove) PeriodList.Remove(item);
                    foreach (var (key, val) in toAddFragment) PeriodList[key] = val;

                    PeriodList[pd] = value;
                }
            }
        }

        public bool Remove(KeyValuePair<Period, T> item) { if (this[item.Key].Equals(item.Value)) return Remove(item.Key); else return false; }

        public void Remove(DateTime start, DateTime stop) => Remove(new Period(start, stop));

        public bool Remove(Period pd)
        {
            bool isModified = false;

            if (!IsReadOnly)
                lock (PeriodList)
                {
                    if (PeriodList.ContainsKey(pd))
                    {
                        isModified = true;
                        PeriodList.Remove(pd);
                    }
                    else
                    {
                        List<Period> toRemove = new List<Period>();
                        List<(Period key, T value)> toAdd = new List<(Period key, T value)>();

                        foreach (Period item in PeriodList.Keys)
                        {
                            if (pd.Intersect(item))
                            {
                                isModified = true;

                                toRemove.CheckAdd(item);

                                Period[] res = item - pd;
                                foreach (Period resPd in res)
                                {
                                    if (!resPd.IsEmpty) toAdd.Add((resPd, PeriodList[item]));
                                }
                            }
                        }

                        foreach (Period item in toRemove) PeriodList.Remove(item);
                        foreach (var (key, val) in toAdd) PeriodList[key] = val;
                    }
                }

            return isModified;
        }

        public bool ContainsKey(Period key) => PeriodList.ContainsKey(key);

        public bool Contains(KeyValuePair<Period, T> item) => PeriodList.Contains(item);

        public bool Contains(Period pd)
        {
            lock (PeriodList)
            {
                if (PeriodList.ContainsKey(pd)) return true;
                foreach (Period item in PeriodList.Keys) if (item.Contains(pd)) return true;
                return false;
            }
        }

        public bool Contains(DateTime time)
        {
            lock (PeriodList)
            {
                foreach (Period item in PeriodList.Keys) if (item.Contains(time)) return true;
                return false;
            }
        }

        public bool TryGetValue(Period key, out T value)
        {
            lock (PeriodList)
            {
                if (PeriodList.ContainsKey(key))
                {
                    value = PeriodList[key];
                    return true;
                }

                foreach (Period item in PeriodList.Keys)
                    if (item.Contains(key))
                    {
                        value = PeriodList[item];
                        return true;
                    }

                value = (T)default;
                return false;
            }
        }



        [IgnoreDataMember]
        public ICollection<Period> Keys => PeriodList.Keys;

        [IgnoreDataMember]
        public ICollection<T> Values => ((IDictionary<Period, T>)PeriodList).Values;

        public IEnumerable<KeyValuePair<Period, T>> Get(DateTime time) => PeriodList.Where(n => n.Key.Contains(time));

        public IEnumerable<KeyValuePair<Period, T>> Get(Period pd) => PeriodList.Where(n => n.Key.Intersect(pd));

        public IEnumerator<KeyValuePair<Period, T>> GetEnumerator() => PeriodList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => PeriodList.GetEnumerator();

        public void CopyTo(KeyValuePair<Period, T>[] array, int arrayIndex) => PeriodList.CopyTo(array, arrayIndex);
    }
}
