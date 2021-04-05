using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xu.EE.VirtualBench;

namespace Xu.EE.TestApp
{
    public partial class MainForm : Form
    {
        private NiVB NiVB { get; set; }// = new NiVB("VB8012-309528E");

        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnInitialize_Click(object sender, EventArgs e)
        {
            NiVB = new NiVB(TextBoxResouceName.Text);
            NiVB.Open();
            NiVB.GetCalibrationInfo();
        }

        private void BtnSetFGEN_Click(object sender, EventArgs e)
        {
            NiVB.FunctionGeneratorChannel.Frequency = TextBoxFrequency.Text.ToDouble(0);
            NiVB.FunctionGeneratorChannel.Amplitude = TextBoxAmplitude.Text.ToDouble(0);
            NiVB.FunctionGeneratorChannel.DutyCycle = TextBoxDutyCycle.Text.ToDouble(0);
            NiVB.FunctionGeneratorChannel.DcOffset = TextBoxDcOffset.Text.ToDouble(0);
            NiVB.FunctionGeneratorChannel.WriteSetting();
        }

        private void BtnFgenON_Click(object sender, EventArgs e)
        {
            NiVB.FGEN_ON();
        }

        private void BtnFgenOFF_Click(object sender, EventArgs e)
        {
            NiVB.FGEN_OFF();
        }

        private void BtnTestMSO_Click(object sender, EventArgs e)
        {
            NiVB.TestMSO();
        }
    }
}
