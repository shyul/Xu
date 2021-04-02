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
            NiVB.FGEN_Frequency = TextBoxFrequency.Text.ToDouble(0);
            NiVB.FGEN_Amplitude = TextBoxAmplitude.Text.ToDouble(0);
            NiVB.FGEN_DutyCycle = TextBoxDutyCycle.Text.ToDouble(0);
            NiVB.FGEN_DcOffset = TextBoxDcOffset.Text.ToDouble(0);
            NiVB.FGEN_WriteSetting();
        }

        private void BtnFgenON_Click(object sender, EventArgs e)
        {
            NiVB.FGEN_ON(1);
        }

        private void BtnFgenOFF_Click(object sender, EventArgs e)
        {
            NiVB.FGEN_OFF(1);
        }
    }
}
