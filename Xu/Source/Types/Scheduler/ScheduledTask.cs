/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Xu
{
    /// <summary>
    /// Types of the Contacting methods of Non-physical Address
    /// </summary>
    [Serializable, DataContract, Flags]
    public enum ScheduledTaskType : int
    {
        [EnumMember]
        OneTime = 1,

        [EnumMember]
        Period = 2,

        [EnumMember]
        Frequent = 4,
    }

    /// <summary>
    /// Shall I merge this with IStatus???
    /// The problem is, some status is not a task...
    /// Or shall I just derive it from command?
    /// </summary>
    [Serializable, DataContract]
    public class ScheduledTask : Command
    {
        public ScheduledTask(DateTime time)
        {
            Type = ScheduledTaskType.OneTime;
            NextExecTime = time;
            LastExecTime = DateTime.MaxValue;
        }

        public ScheduledTask(Period pd)
        {
            Type = ScheduledTaskType.Period;
            Period = pd;
        }

        public ScheduledTask(Frequency freq)
        {
            Type = ScheduledTaskType.Frequent;
            NextExecTime = DateTime.Now;
            Frequency = freq;
        }

        public ScheduledTask(DateTime time, Frequency freq)
        {
            Type = ScheduledTaskType.OneTime | ScheduledTaskType.Frequent;
            NextExecTime = time;
            Frequency = freq;
        }

        public ScheduledTask(Period pd, Frequency freq)
        {
            Type = ScheduledTaskType.Period | ScheduledTaskType.Frequent;
            Period = pd;
            Frequency = freq;
            NextExecTime = pd.Start;
        }

        [DataMember, Browsable(true), ReadOnly(true)]
        [Description("Type")]
        public ScheduledTaskType Type { get; set; }

        // Total number of executions expected
        [DataMember, Browsable(true), ReadOnly(true)]
        public int Count { get; set; }

        // Total number of executions already done
        [DataMember, Browsable(true), ReadOnly(true)]
        public int LastExecCount { get; set; }

        // Next Expected Exec Time
        [DataMember, Browsable(true), ReadOnly(true)]
        public DateTime NextExecTime { get; set; } = DateTime.MinValue;

        [DataMember, Browsable(true), ReadOnly(true)]
        public DateTime LastExecTime { get; set; }

        [DataMember, Browsable(true), ReadOnly(true)]
        public Frequency Frequency { get; set; }

        [DataMember, Browsable(true), ReadOnly(true)]
        public Period Period { get; set; }

        [IgnoreDataMember]
        protected Task Task { get; set; }

        [IgnoreDataMember, Browsable(true), ReadOnly(true)]
        public TaskStatus Status => (Task is null) ? TaskStatus.Faulted : Task.Status;

        public bool IsBusy => Status == TaskStatus.Running;

        // If is task is running, then let it run ? or kill and restart??
        public override void Start()
        {
            // If Task is Runnning
            if (Enabled && Status != TaskStatus.Running) // Do not disrupt a running Task...
            {
                if (!(Task is null)) Task.Dispose();

                if (!(TaskControl.Cts is null)) TaskControl.Cts = new CancellationTokenSource();
                Task = new Task(() => { Action.Invoke(TaskControl); }, TaskControl.Cts.Token);
                Task.Start();
            }
        }

        public override void Stop()
        {
            base.Stop();
            Task.Wait(TaskControl.TimeOut.TotalMilliseconds.ToInt32());
            //while (Status == TaskStatus.Running) ;
        }

        public virtual bool Check(DateTime now)
        {
            bool isTerminated = false;
            switch (Type)
            {
                case (ScheduledTaskType.OneTime):
                    if (now > LastExecTime && !IsBusy)
                    {
                        isTerminated = true;
                    }
                    else if (now >= NextExecTime && !IsBusy)
                    {
                        LastExecTime = now;
                        Start();
                    }
                    break;
                case (ScheduledTaskType.Period):
                    if (Period == now && !IsBusy)
                    {
                        LastExecTime = now;
                        Start();
                    }
                    else if (Period < now) // && IsBusy)
                    {
                        if (IsBusy) Stop();
                        isTerminated = true;
                        // and remove itself...
                    }
                    break;
                case (ScheduledTaskType.Frequent): // NextExecTime = DateTime.Now;
                // Start the Frequent Task at a defined Time
                case (ScheduledTaskType.OneTime | ScheduledTaskType.Frequent): // NextExecTime = defined;
                    // Should set this when ever it is added to the list
                    if (now >= NextExecTime && !IsBusy)
                    {
                        NextExecTime = now + Frequency;
                        LastExecTime = now;
                        Start();
                    }
                    break;
                case (ScheduledTaskType.Period | ScheduledTaskType.Frequent):

                    // Should set this when ever it is added to the list
                    // NextExecTime = Period.Start;
                    if (Period == now && !IsBusy)
                    {
                        if (now >= NextExecTime && !IsBusy)
                        {
                            NextExecTime = now + Frequency;
                            LastExecTime = now;
                            Start();
                        }
                    }
                    else if (Period < now) // && IsBusy)
                    {
                        if (IsBusy) Stop();
                        isTerminated = true;
                        // and remove itself...
                    }
                    break;

                default: break;
            }

            return isTerminated;

        }
    }
}
