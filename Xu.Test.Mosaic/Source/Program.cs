/// ***************************************************************************
/// Pacmio Research Enivironment
/// GPL 2001-2007, 2014-2018 Xu Li - shyu.lee@gmail.com
/// 
/// ***************************************************************************

using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Xu;
using Xu.WindowsNativeMethods;

namespace Mosaic
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (InstanceMutex.WaitOne(TimeSpan.Zero, true))
            {
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    User32.SetProcessDPIAware();
                    ActiveColor = DWMAPI.GetWindowColorizationColor(true);
                    ScaleFactor = GUI.ScalingFactor();

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new PacMain());
                }
                else
                {
                    MessageBox.Show("Windows 10 64-bit is required to run this application :)");
                }
            }
            else
            {
                // send our Win32 message to make the currently running instance
                // jump on top of all the other windows
                User32.PostMessage(HWND.BROADCAST, SHOW_PACMIO, IntPtr.Zero, IntPtr.Zero);
            }
            InstanceMutex.ReleaseMutex();
        }

        internal static Color ActiveColor;
        internal static float ScaleFactor;

        public static string Title = ((Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), true))[0] as AssemblyTitleAttribute).Title;
        internal static string GUID = ((Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), true))[0] as GuidAttribute).Value;  //Assembly.GetExecutingAssembly().GetType().GUID.ToString()
        private static Mutex InstanceMutex = new Mutex(true, GUID); // new Mutex(true, "{8F6F0AC6-B9A1-45fd-A8CF-72F04E6BDE8F}");
        internal static readonly int SHOW_PACMIO = User32.RegisterWindowMessage("SHOW_PACMIO");
    }

    public static class Settings
    {
        public static string TitleText => Application.ProductName + " - Rev " + Application.ProductVersion;
        public static string HelpLink = "https://github.com/shyul/pacman/wiki";
    }

}
