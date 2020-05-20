/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Threading;

namespace Xu
{
    [Serializable, DataContract]
    public class EventContainer : IItem, IDisposable
    {
        public virtual void Dispose()
        {

        }

        #region Events

        [DataMember]
        protected readonly Queue<Event> EventList = new Queue<Event>();

        [IgnoreDataMember]
        public int Count => EventList.Count;
        public virtual void Clear() => EventList.Clear();

        #endregion

        #region IItem
        public string Name { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public ColorTheme Theme { get; set; }
        public Importance Importance { get; set; }
        public bool Enabled { get; set; }
        public int Order { get; set; }
        public HashSet<string> Tags { get; set; }
        public ulong Uid { get; set; }


        #endregion
    }


    public static class ObsoletedEvent
    {
        public static EventDockPanel OutputPanel;
        internal static List<string> OutputMessages = new List<string>();

        [Conditional("DEBUG")]
        public static void Debug(string str)
        {
            Console.WriteLine(str);
            OutputMessages.Add(str);

            while (OutputMessages.Count > 500)
            {
                OutputMessages.RemoveAt(0);
            }

            if (OutputPanel != null)
            {
                //OutputPanel?.Invoke(new MethodInvoker(delegate () { OutputPanel.Invalidate(true); }));
            }
        }
    }
}
