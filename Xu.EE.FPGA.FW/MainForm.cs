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
                FPGA.ImportXilinxPackageFile(OpenFile.FileName);
            }
        }

        private void BtnImportAlteraPackageFile_Click(object sender, EventArgs e)
        {
            OpenFile.Filter = "DIYed Alera Package File (*.csv) | *.csv";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                FPGA = new FPGA();
                FPGA.ImportAlteraPackageFile(OpenFile.FileName);
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
                FPGA.ImportAltiumPinMapReport(OpenFile.FileName);
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

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFile.Filter = "Quartus Spec File (*.qsf) | *.qsf";

            if (SaveFile.ShowDialog() == DialogResult.OK && FPGA is not null)
            {
                FPGA.ExportQuartusSpecFileAssignment(SaveFile.FileName);
            }
        }

        private void BtnExportNets_Click(object sender, EventArgs e)
        {
            SaveFile.Filter = "System Verilog (*.sv) | *.sv";

            if (SaveFile.ShowDialog() == DialogResult.OK && FPGA is not null)
            {
                FPGA.ExportAllSignals(SaveFile.FileName);
            }
        }

        private void BtnImportQuartusPinFile_Click(object sender, EventArgs e)
        {
            OpenFile.Filter = "Quartus Pin File (*.pin) | *.pin";

            if (OpenFile.ShowDialog() == DialogResult.OK && FPGA is not null)
            {
                FPGA.ImportQuartusPinFile(OpenFile.FileName);
            }
        }
    }
}
