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
using Xu.EE.Visa;

namespace Xu.EE.TestApp
{
    public partial class MainForm : Form
    {
        private NiVB NiVB { get; set; }// = new NiVB("VB8012-309528E");
        private Oscilloscope MSOX { get; set; }

        private FunctionGenerator FGEN{ get; set; }

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

        private void BtnConnectMSO_Click(object sender, EventArgs e)
        {
            MSOX = new Oscilloscope(TextBoxKeySight.Text);
            MSOX.Open();
        }

        private void BtnConnectFGEN_Click(object sender, EventArgs e)
        {
            FGEN = new FunctionGenerator(TextBoxKeySightFGEN.Text);
            FGEN.Open();

            var config = new FunctionGeneratorArbitraryConfig(FGEN.Channel1);
            //var config = new FunctionGeneratorTriangleWaveConfig();

            FGEN.Channel1.Config = config;
            //config.Frequency = 94256.1245671212;
           // config.Amplitude = 1.238883423;
            config.DcOffset = 2;
            //config.Phase = 12.66766664;
            //config.DutyCycle = 50;

            FGEN.Channel1.WriteSetting();
            FGEN.Channel1.ReadSetting();

            //config = FGEN.Channel1.Config as FunctionGeneratorTriangleWaveConfig;

            //Console.WriteLine("Frequency = " + config.Frequency + " | Phase = " + config.Phase + " | Amplitude = " + config.Amplitude + " | DcOffset = " + config.DcOffset + " | DutyCycle = " + config.DutyCycle);

            FGEN.Channel1.Enabled = true;
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

            var config = new FunctionGeneratorArbitraryConfig(NiVB.FunctionGeneratorChannel);
            //config.Waveform = new List<double>() { -1, 2, -2, 4, -3, 6, -4, 8, -5, 8, -4, 6, -3, 4, -2, 2, -1 };
            config.Samples = new List<double>() { 0, 0, 0, 0.8, -0.5, 1.25, -1.0, 1.5, -1.8, 1.1, -2.6, 1.1, -1.8, 1.5, -1.0, 1.25, -0.5, 0.8, 0, 0, 0 };
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

        private void BtnTestImportAlteraPins_Click(object sender, EventArgs e)
        {
            OpenFile.Filter = "Comma-separated values file (*.csv) | *.csv";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                Altera.ReadPinoutFile(OpenFile.FileName);
            }
        }

        private void BtnImportCSVPins_Click(object sender, EventArgs e)
        {
            OpenFile.Filter = "Comma-separated values file (*.csv) | *.csv";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                Schematic.ReadPinoutFile(OpenFile.FileName);
            }
        }
    }
}
