/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

namespace Xu
{
    public class Scheduler : IItem//, IDisposable
    {
        public Scheduler(int delay = 500)
        {
            Delay = delay;
            Start();
        }

        ~Scheduler() => Dispose();

        public void Dispose()
        {
            Stop();
            Clear();
        }

        #region Tasks

        // https://www.geeksforgeeks.org/linked-list-implementation-in-c-sharp/
        protected readonly List<ScheduledTask> TaskList = new List<ScheduledTask>();  // protected by lock(_tasks)

        public int Count => TaskList.Count;

        public virtual void Sort()
        {
            if(Count > 1)
            {
                TaskList.Sort((f1, f2) => f1.Order.CompareTo(f2.Order));
                for (int i = 0; i < Count; i++)
                {
                    TaskList[i].Order = i;
                }
            }
        }

        public virtual void Clear()
        {
            lock (TaskList)
            {
                foreach (ScheduledTask ist in TaskList)
                {
                    ist.Stop();
                }

                TaskList.Clear();
            }
        }

        public virtual void AddTask(ScheduledTask ist)
        {
            lock (TaskList)
            {
                TaskList.Add(ist);
                Sort();
            }
        }

        public virtual void RemoveTask(ScheduledTask ist)
        {
            ist.Stop();
            lock (TaskList)
            {
                TaskList.Remove(ist);
                ist.Dispose();
                ist = null;
            }
            // Remove the object
            // Kill the task if it is still running...
        }

        #endregion

        #region Running Task

        public virtual void Start()
        {
            Cts = new CancellationTokenSource();
            RunTask = new Task(() => {
                while (!Cts.IsCancellationRequested)
                {
                    if (!Hold) RunTasks();
                    Thread.Sleep(Delay);
                }
            }, Cts.Token);

            RunTask.Start();
        }

        public virtual void Stop()
        {
            Cts.Cancel();
        }

        public int Delay { get; set; } = 500;

        public bool Hold { get; set; } = false;

        protected CancellationTokenSource Cts { get; set; }

        protected Task RunTask { get; set; }

        protected virtual void RunTasks()
        {
            for (int i = 0; i < Count; i++)
            {
                ScheduledTask ist = TaskList.ElementAt(i);
                if (ist.Check(DateTime.Now))
                {
                    RemoveTask(ist); // Delayed Removal would be appreciated.
                    if (Count < 1) return;
                    i--;
                    if (i < 0) i = 0;
                }
            }
        }

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
}
