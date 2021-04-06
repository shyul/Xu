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
            /*
            var config = new FunctionGeneratorSquareWaveConfig();
            NiVB.FunctionGeneratorChannel.Config = config;

            config.Frequency = TextBoxFrequency.Text.ToDouble(0);
            config.Amplitude = TextBoxAmplitude.Text.ToDouble(0);
            config.DutyCycle = TextBoxDutyCycle.Text.ToDouble(0);
            config.DcOffset = TextBoxDcOffset.Text.ToDouble(0);


            NiVB.FunctionGeneratorChannel.WriteSetting();
            NiVB.FunctionGeneratorChannel.ReadSetting();

            config = NiVB.FunctionGeneratorChannel.Config as FunctionGeneratorSquareWaveConfig;
            TextBoxFrequency.Text = config.Frequency.ToString();
            TextBoxAmplitude.Text = config.Amplitude.ToString();
            TextBoxDutyCycle.Text = config.DutyCycle.ToString();
            TextBoxDcOffset.Text = config.DcOffset.ToString();*/

            var config = new FunctionGeneratorArbitraryConfig();
            config.Waveform = new List<double>() { 1, 5, -2, 6, -3 };
            NiVB.FunctionGeneratorChannel.Config = config;

            config.SampleRate = TextBoxFrequency.Text.ToDouble(0);
            config.Gain = TextBoxAmplitude.Text.ToDouble(0);
            config.DcOffset = TextBoxDcOffset.Text.ToDouble(0);

            NiVB.FunctionGeneratorChannel.WriteSetting();
            NiVB.FunctionGeneratorChannel.ReadSetting();

            config = NiVB.FunctionGeneratorChannel.Config as FunctionGeneratorArbitraryConfig;
            TextBoxFrequency.Text = config.SampleRate.ToString();
            TextBoxAmplitude.Text = config.Gain.ToString();
            TextBoxDcOffset.Text = config.DcOffset.ToString();
        }

        private void BtnFgenON_Click(object sender, EventArgs e)
        {
            NiVB.FunctionGenerator_ON();
        }

        private void BtnFgenOFF_Click(object sender, EventArgs e)
        {
            NiVB.FunctionGenerator_OFF();
        }

        private void BtnTestMSO_Click(object sender, EventArgs e)
        {
            NiVB.TestMSO();
        }

        private void BtnP25WriteSettings_Click(object sender, EventArgs e)
        {
            var ch = NiVB.PowerSupplyP25VChannel;

            ch.Voltage = TextBoxP25Voltage.Text.ToDouble();
            ch.Current = TextBoxP25Current.Text.ToDouble();

            ch.WriteSetting();
            ch.ReadSetting();

            TextBoxP25Voltage.Text = ch.Voltage.ToString();
            TextBoxP25Current.Text = ch.Current.ToString();
        }

        private void BtnPowerTurnON_Click(object sender, EventArgs e)
        {
            NiVB.PowerSupply_ON();
            //NiVB.PowerSupplyN25VChannel.Enabled = true;
            Console.WriteLine("Power is: " + NiVB.PowerSupplyN25VChannel.Enabled);
        }

        private void BtnPowerTurnOFF_Click(object sender, EventArgs e)
        {
            NiVB.PowerSupply_OFF();
            //NiVB.PowerSupplyN25VChannel.Enabled = false;
            Console.WriteLine("Power is: " + NiVB.PowerSupplyN25VChannel.Enabled);
        }

        private void BtnN25WriteSettings_Click(object sender, EventArgs e)
        {
            var ch = NiVB.PowerSupplyN25VChannel;

            ch.Voltage = TextBoxN25Voltage.Text.ToDouble();
            ch.Current = TextBoxN25Current.Text.ToDouble();

            ch.WriteSetting();
            ch.ReadSetting();

            TextBoxN25Voltage.Text = ch.Voltage.ToString();
            TextBoxN25Current.Text = ch.Current.ToString();
        }

        private void BtnP6WriteSettings_Click(object sender, EventArgs e)
        {
            var ch = NiVB.PowerSupplyP6VChannel;

            ch.Voltage = TextBoxP6Voltage.Text.ToDouble();
            ch.Current = TextBoxP6Current.Text.ToDouble();

            ch.WriteSetting();
            ch.ReadSetting();

            TextBoxP6Voltage.Text = ch.Voltage.ToString();
            TextBoxP6Current.Text = ch.Current.ToString();
        }

        private void BtnTestDMMConfig_Click(object sender, EventArgs e)
        {
            NiVB.TestConfigDMM();
        }

        private void BtnTestDMMRead_Click(object sender, EventArgs e)
        {
            NiVB.TestReadDMM();
        }
    }
}
