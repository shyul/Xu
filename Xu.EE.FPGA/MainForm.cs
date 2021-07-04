using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xu.EE.FPGA
{
    public partial class MainForm : Form
    {
        FPGA FPGA { get; set; } = new FPGA();

        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnImportXilinxComponent_Click(object sender, EventArgs e)
        {
            OpenFile.Filter = "Xilinx Package File (*.txt) | *.txt";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                FPGA = new FPGA();
                FPGA.ReadXilinxPackageFile(OpenFile.FileName);
            }
        }
    }
}
