using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xu.EE.VirtualBench;

namespace Xu.EE.TestApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            NiVB nivb = new NiVB("VB8012-309528E");

            nivb.GetCalibrationInfo();

            Application.Run(new MainForm());
        }
    }
}
