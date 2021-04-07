using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Xu.EE.Visa
{
    public class Oscilloscope : ViClient, IOscilloscope
    {
        public Oscilloscope(string resourceName) : base(resourceName)
        {
            //Reset();
        }

        public override void Open()
        {
            base.Open();
            Console.WriteLine(VendorName + " | " + Model + " | " + SerialNumber + " | " + DeviceVersion);
        }

        public Dictionary<string, OscilloscopeAnalogChannel> OscilloscopeAnalogChannels { get; } = new();

        public ITriggerSource Oscilloscope_TriggerSource
        {
            get => m_Oscilloscope_TriggerSource;

            set
            {
                m_Oscilloscope_TriggerSource = value;
                if (m_Oscilloscope_TriggerSource is OscilloscopeAnalogChannel ch)
                {

                }
            }
        }

        private ITriggerSource m_Oscilloscope_TriggerSource;

        public double AnalogSampleRate
        {
            get => 0; // OscilloscopeAnalogChannel1.SampleRate;

            set
            {
                //OscilloscopeAnalogChannel1.SampleRate = value;
                //OscilloscopeAnalogChannel2.SampleRate = value;
            }
        }



        public void OscilloscopeAnalog_ReadSetting(string channelName)
        {
            //:CHANnel1:RANGe?
            //:CHANnel1:OFFSet?
            //:CHANnel1:SCALe?\n
            //:CHANnel1:INPut?  --- ONEM
            //:CHANnel1:PROBe:ID?\n -- NONE
            //:CHANnel1:PROBe:INFO?\n
            //:CHANnel1:PROBe?\n   //:CHANnel1:PROBe:ATTenuation?
            //:CHANnel1:PROBe:SKEW?\n -- +1.67E-09\n, always there
        }

        public void OscilloscopeAnalog_WriteSetting(string channelName)
        {
            // :CHANnel1:RANGe 10
            // :CHANnel1:OFFSet 1.25 // :CHANnel1:OFFSet -0.5
            // :CHANnel1:PROBe:COUPling AC
            // :CHANnel1:SCALe 500E-3 // :CHANnel1:SCALe 0.625
            // :CHANnel1:PROBe 10 //myScope.WriteString ":CHANnel1:PROBe 10,RAT"
            // :CHANnel1:PROBe:ATTenuation DIV10 1154A Probe Only
            // 

        }

        public void Oscilloscope_WriteSetting()
        {

        }

        public void Oscilloscope_Run()
        {

        }


    }
}
