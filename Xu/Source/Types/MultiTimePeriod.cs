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
    /// <summary>
    /// A list of times.
    /// </summary>
    [Serializable, DataContract]
    public class MultiTimePeriod : ICollection<Time>
    {
        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(Time item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(Time item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Time[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Time> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(Time item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }


}
