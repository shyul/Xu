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
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;

namespace Xu
{
    public class QueueValue<T>
    {
        public T Value
        {
            get
            {
                return m_LastValue.Value;
            }
            set
            {
                // value > 0
                m_LastValue = (DateTime.Now, value);
                Queue.Enqueue(m_LastValue);
                while (Queue.Count > MaxQueueLength && Queue.TryDequeue(out _)) ;
            }
        }

        public DateTime Time => m_LastValue.Time;

        private (DateTime Time, T Value) m_LastValue = (DateTime.MinValue, default(T));

        public int MaxQueueLength { get; set; } = 100;

        public ConcurrentQueue<(DateTime Time, T Value)> Queue { get; protected set; } = new ConcurrentQueue<(DateTime Time, T Value)>();
    }
}
