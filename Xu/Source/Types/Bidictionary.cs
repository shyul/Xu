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
using System.Linq;
using System.Runtime.Serialization;

namespace Xu
{
    public class Bidictionary<T1, T2> // : IDictionary<T1, T2>, IDictionary<T2, T1>
    {
        public Dictionary<T1, T2> Forward { get; } = new Dictionary<T1, T2>();

        public Dictionary<T2, T1> Backward { get; } = new Dictionary<T2, T1>();


        public T2 this[T1 key]
        {
            get => Forward[key];

            set
            {
                Forward[key] = value;
                Backward[value] = key;
            }
        }

        public T1 this[T2 key]
        {
            get => Backward[key];

            set
            {
                Backward[key] = value;
                if (!Forward.ContainsKey(value)) 
                    Forward[value] = key;
            }
        }

        public void Clear()
        {
            Forward.Clear();
            Backward.Clear();
        }

        public int Count => Math.Max(Forward.Count, Backward.Count);



        public ICollection<T1> Keys => Forward.Keys;

        public ICollection<T2> Values => Backward.Keys;

        public void Add(T1 key, T2 value) => this[key] = value;

        public void Add(KeyValuePair<T1, T2> item) => this[item.Key] = item.Value;

        public void Add(T2 key, T1 value) => this[key] = value;

        public void Add(KeyValuePair<T2, T1> item) => this[item.Key] = item.Value;




        public bool ContainsKey(T1 key) => Forward.ContainsKey(key);

        public bool ContainsKey(T2 key) => Backward.ContainsKey(key);

        public bool Contains(KeyValuePair<T1, T2> item) => Forward.Contains(item);

        public bool Contains(KeyValuePair<T2, T1> item) => Backward.Contains(item);

        public bool Remove(T1 key)
        {
            if (ContainsKey(key))
            {
                T2 value = this[key];
                Forward.Remove(key);
                if (ContainsKey(value))
                {
                    Backward.Remove(value);
                }
                return true;
            }
            return false;
        }

        public bool Remove(T2 key)
        {
            if (ContainsKey(key))
            {
                T1 value = this[key];
                Backward.Remove(key);
                if (ContainsKey(value))
                {
                    Forward.Remove(value);
                }
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<T1, T2> item)
        {
            throw new NotImplementedException();
        }



        public bool Remove(KeyValuePair<T2, T1> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<T1, T2>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<T2, T1>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator() => Forward.GetEnumerator();




        public bool TryGetValue(T1 key, out T2 value)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(T2 key, out T1 value)
        {
            throw new NotImplementedException();
        }

        /*
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator<KeyValuePair<T2, T1>> IEnumerable<KeyValuePair<T2, T1>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }*/
    }
}
