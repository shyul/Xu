using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xu.Test.Scheduler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ScheduledTask st = new ScheduledTask(new Frequency(TimeUnit.Seconds, 1));
            st.Action = (IItem source, string[] args, Progress<Event> progress, CancellationTokenSource cts) => 
            {
                Console.WriteLine(Program.Sch.Count + " | " + DateTime.Now + ": " + st.Frequency);
            };
            Program.Sch.AddTask(st);

            ScheduledTask st2 = new ScheduledTask(new Frequency(TimeUnit.Seconds, 5));
            st2.Action = (IItem source, string[] args, Progress<Event> progress, CancellationTokenSource cts) => 
            {
                Console.WriteLine(Program.Sch.Count + " | " + DateTime.Now + ": " + st2.Frequency);
            };
            Program.Sch.AddTask(st2);


            ScheduledTask st3 = new ScheduledTask(DateTime.Now.AddSeconds(3));
            st3.Action = (IItem source, string[] args, Progress<Event> progress, CancellationTokenSource cts) => 
            {
                Console.WriteLine(Program.Sch.Count + " | " + DateTime.Now + ": Delayed Task only run once");
            };
            Program.Sch.AddTask(st3);


            ScheduledTask st4 = new ScheduledTask(new Period(DateTime.Now.AddSeconds(2), DateTime.Now.AddSeconds(8)), new Frequency(TimeUnit.Seconds, 1));
            st4.Action = (IItem source, string[] args, Progress<Event> progress, CancellationTokenSource cts) => 
            {
                Console.WriteLine(Program.Sch.Count + " | " + DateTime.Now + ": only run for a while");
            };
            Program.Sch.AddTask(st4);

            ScheduledTask st5 = new ScheduledTask(new Period(DateTime.Now.AddSeconds(5), DateTime.Now.AddSeconds(12)));
            st5.Action = (IItem source, string[] args, Progress<Event> progress, CancellationTokenSource cts) => {
                while (!cts.IsCancellationRequested)
                {
                    Console.Write(".");
                    Thread.Sleep(50);
                }
                Console.WriteLine(": ## finshed ##");
            };
            Program.Sch.AddTask(st5);
        }
    }
}
