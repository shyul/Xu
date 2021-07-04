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
using FTD2XX_NET;
using System.Threading;

namespace Xu.EE.FPGA
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
                Altera.ReadPinoutFileCycloneV(OpenFile.FileName);
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

        FTDI.FT_STATUS FtdiStatus { get; set; } = FTDI.FT_STATUS.FT_OK;
        FTDI FTDI_Channel_B { get; set; } = new FTDI();
        FTDI FTDI_Channel_A { get; set; } = new FTDI();

        uint FtdiDevCount = 0;

        FTDI.FT_DEVICE_INFO_NODE[] FtdiDeviceList { get; set; }

        private void BtnTestFTDI_Click(object sender, EventArgs e)
        {
     

            try
            {
                FtdiStatus = FTDI_Channel_B.GetNumberOfDevices(ref FtdiDevCount);
                Console.WriteLine("Device Count = " + FtdiDevCount);
                //this.FtdiDevCount = FtdiDevCount;
                FtdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[FtdiDevCount];
                FTDI_Channel_B.GetDeviceList(FtdiDeviceList);

                for(int i = 0; i < FtdiDevCount; i++) 
                {
                    FTDI.FT_DEVICE_INFO_NODE dev = FtdiDeviceList[i];
                    Console.WriteLine("Id = " + dev.ID + " Desc = " + dev.Description + " sn = " + dev.SerialNumber);
                }
            }
            catch
            {
                Console.WriteLine("Driver not loaded");
            }

            if (FtdiDevCount > 1)
            {
                //FtdiDevice.GetDeviceList()

                //FtdiStatus = FtdiDevice.OpenByDescription("UM232H");  // could replace line below
                FtdiStatus = FTDI_Channel_B.OpenBySerialNumber("FT4Q1LCFB");//.OpenByIndex(1);

                FtdiStatus = FTDI_Channel_A.OpenBySerialNumber("FT4Q1LCFA");//.OpenByIndex(1);

                FtdiStatus = FTDI.FT_STATUS.FT_OK;
                FtdiStatus |= FTDI_Channel_B.MPSSE_Init_SPI();
                FtdiStatus |= FTDI_Channel_B.Write(FTDI.SPI_CS_H);
                FtdiStatus |= FTDI_Channel_A.Write(new byte[] { 0x82, 0x1, 0x1 });

                FtdiStatus |= FTDI_Channel_A.MPSSE_Init_SPI();
                FtdiStatus |= FTDI_Channel_A.Write(new byte[] { 0x82, 0x0, 0x1F });


                Console.WriteLine(FtdiStatus);
                Thread.Sleep(10);
                FTDI_Channel_B.SPI_CS_Enable();
                Thread.Sleep(10);
                /*
                FtdiStatus = FtdiDevice.GetDeviceList(FtdiDeviceList);

                foreach(var dev in FtdiDeviceList) 
                {
                    Console.WriteLine(dev.Description);
                
                }*/
                //FtdiStatus = FtdiDevice
            }
        }

        private void BtnTestFTDISendData_Click(object sender, EventArgs e)
        {
            FTDI_Channel_B.SPI_Write(0x0, new byte[] { 0x81 });
            FTDI_Channel_B.SPI_Write(0x0, new byte[] { 0x18 });

            FTDI_Channel_B.SPI_Write(0x400, new byte[] { 0x55 });
            FTDI_Channel_B.SPI_Write(0x38, new byte[] { 0x60 });
            FTDI_Channel_B.SPI_Write(0x2E, new byte[] { 0x7F });
            FTDI_Channel_B.SPI_Write(0x34, new byte[] { 0x08 });
            FTDI_Channel_B.SPI_Write(0x35, new byte[] { 0x55 });
            FTDI_Channel_B.SPI_Write(0x31, new byte[] { 0x20 });
            FTDI_Channel_B.SPI_Write(0x12, new byte[] { 0xFF });

            FTDI_Channel_B.SPI_Write(0x18, new byte[] { 0x36 });
            FTDI_Channel_B.SPI_Write(0x19, new byte[] { 0x35 });
            FTDI_Channel_B.SPI_Write(0x28, new byte[] { 0x01 });
            /*
            while (true)
            {
                ftd.SPI_Write(i, new byte[] { 0x5A, (byte)(i & 0xFF), 0xA5 });
                i++;
                Thread.Sleep(20);
            }
            */
            //FtdiDevice.SPI_Enable();
        }

        private void BtnTestFTDIRecvData_Click(object sender, EventArgs e)
        {
            ushort i = 0;
            int j = 0x19;
            var data = FTDI_Channel_B.SPI_Read(j, j + 1);
            foreach (byte d in data)
            {
                Console.WriteLine("addr: 0x" + j.ToString("X") + " - data: 0x" + d.ToString("X"));
                j--;
            }
            /*
            while (true)
            {
                int j = 5;
                var data = ftd_b.SPI_Read(i, j);
                foreach(byte d in data) 
                {
                    j--;
                    Console.WriteLine(j + ": 0x" + d.ToString("X"));
                }
                i++;
                Thread.Sleep(20);
            }*/
        }
    }
}
