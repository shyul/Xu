/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
//using System.

namespace Xu
{
    [Serializable, DataContract]
    public enum EventType : int
    {
        Error = -2,
        Warning = -1,
        Information = 0,
        Hightlight = 1,
        Progress = 10,
        Completion = 100,
       
    }

    /// <summary>
    /// Status is the arm, Event is the trigger.
    /// </summary>
    [Serializable, DataContract]
    public class Event : IOrdered
    {
        public EventType Type { get; set; }

        public DateTime Time { get; set; }

        public IOrdered Source { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }

        public Importance Importance { get; set; }

        public bool Enabled { get; set; }

        public int Order { get; set; }

    }

    public delegate void EventCaller(Event ie);




}
