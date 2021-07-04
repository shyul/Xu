using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xu;

namespace Xu.EE.FPGA.FW
{
    public partial class MainForm : Form
    {
        FPGA FPGA { get; set; } = new FPGA();

        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnImportXilinxPackageFile_Click(object sender, EventArgs e)
        {
            OpenFile.Filter = "Xilinx Package File (*.txt) | *.txt";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                FPGA = new FPGA();
                FPGA.ReadXilinxPackageFile(OpenFile.FileName);
            }
        }

        private void BtnExportCSVFile_Click(object sender, EventArgs e)
        {
            SaveFile.Filter = "CSV File (*.csv) | *.csv";

            if (SaveFile.ShowDialog() == DialogResult.OK && FPGA is not null)
            {
                FPGA.ExportPins(SaveFile.FileName);
            }
        }

        private void BtnImportAltiumPinMapReport_Click(object sender, EventArgs e)
        {
            OpenFile.Filter = "Altium Pin Map File (*.csv) | *.csv";

            if (OpenFile.ShowDialog() == DialogResult.OK && FPGA is not null)
            {
                FPGA.ReadAltiumPinMapReport(OpenFile.FileName);
            }
        }

        private void BtnExportXilinxXDCFile_Click(object sender, EventArgs e)
        {
            SaveFile.Filter = "Xilinx Constraint File (*.xdc) | *.xdc";

            if (SaveFile.ShowDialog() == DialogResult.OK && FPGA is not null)
            {
                FPGA.ExportXdcConstraint(SaveFile.FileName);
            }
        }
    }
}
