/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Runtime.Serialization;
//using System.

namespace Xu
{
    /// <summary>
    /// The Statis Event Handler Delegate
    /// </summary>
    /// <param name="statusCode"></param>
    /// <param name="time"></param>
    /// <param name="message"></param>
    public delegate void StatusEventHandler(int statusCode, DateTime time, string message = "");

    /// <summary>
    /// Status is the arm, Event is the trigger.
    /// </summary>
    [Serializable, DataContract]
    public class Status : Event
    {
        public float Percent { get; set; }

    }
}
