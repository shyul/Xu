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

        ~Oscilloscope() 
        {
            Dispose();
        }

        public override void Open()
        {
            base.Open();
     
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

            // :CHAN1:BWL?;RANG?;OFFS?\n
            // 0;+18.4E+00;+0.0E+00\n
        }

        public void OscilloscopeAnalog_WriteSetting(string channelName)
        {
            // :CHANnel1:RANGe 10
            // :CHANnel1:OFFSet 1.25 // :CHANnel1:OFFSet -0.5
            // :CHANnel1:PROBe:COUPling AC
            // :CHANnel1:SCALe 500E-3 // :CHANnel1:SCALe 0.625
            // :CHANnel1:PROBe 10 //myScope.WriteString ":CHANnel1:PROBe 10,RAT"
            // :CHANnel1:PROBe:ATTenuation DIV10 1154A Probe Only
            // :CHAN1:COUP AC | DC


     

            var ch = OscilloscopeAnalogChannels[channelName];
            string config = ":CHAN" + channelName +
                ":RANG " + ch.VerticalRange +
                ";OFFS " + ch.VerticalOffset +
                ";COUP " + (ch.Coupling == AnalogCoupling.AC ? "AC" : "DC") +
                ";IMP " + (ch.Impedance == 50 ? "FIFTY" : "ONEM") + //FIFTY
                ";DISP " + (ch.Enabled ? "1" : "0") +
                ";BWL 0" +
                ";INV 0";
        }

        /*
        :MTES:ENAB 1
        :CHAN1:RANG +4.00E+00;OFFS +0.0E+00;COUP DC;IMP ONEM;DISP 1;BWL 0;INV 0
        :CHAN1:LAB "1";UNIT VOLT;PROB +1.0E+00;PROB:SKEW +0.0E+00;STYP SING

        :CHAN2:RANG +16.0E+00;OFFS +1.62400E+00;COUP DC;IMP FIFT;DISP 0;BWL 0;INV 0
        :CHAN2:LAB "2";UNIT VOLT;PROB +1.0E+00;PROB:SKEW +0.0E+00;STYP SING

        :CHAN3:RANG +40.0E+00;OFFS +0.0E+00;COUP DC;IMP ONEM;DISP 0;BWL 0;INV 0
        :CHAN3:LAB "3";UNIT VOLT;PROB +1.0E+00;PROB:SKEW +0.0E+00;STYP SING

        :CHAN4:RANG +40.0E+00;OFFS +0.0E+00;COUP DC;IMP ONEM;DISP 0;BWL 0;INV 0
        :CHAN4:LAB "4";UNIT VOLT;PROB +1.0E+00;PROB:SKEW +0.0E+00;STYP SING

        :EXT:BWL 0;IMP ONEM;RANG +5E+00;UNIT VOLT;PROB +1.0E+00;PROB:STYP SING
        :TIM:MODE MAIN;REF CENT;MAIN:RANG +50.00E-09;POS +0.0E+00
        :TRIG:MODE EDGE;SWE AUTO;NREJ 0;HFR 0;HOLD +60E-09
        :TRIG:EDGE:SOUR CHAN1;LEV -75.00E-03;SLOP POS;REJ OFF;COUP DC
        :ACQ:MODE RTIM;TYPE NORM;COMP 100;COUNT 8;SEGM:COUN 2
        :DISP:LAB 0;CONN 1;PERS MIN;SOUR PMEM1
        :HARD:APR "";AREA SCR;FACT 0;FFE 0;INKS 1;PAL NONE;LAY PORT
        :SAVE:FIL "mask_0"
        :SAVE:IMAG:AREA GRAT;FACT 0;FORM NONE;INKS 0;PAL COL
        :SAVE:WAV:FORM NONE
        :MTES:SOUR CHAN1;ENAB 1;LOCK 1
        :MTES:AMAS:SOUR CHAN1;UNIT DIV;XDEL +3.00000000E-001;YDEL +2.00000000E-001
        :MTES:SCAL:BIND 0;X1 +0.0E+00;XDEL +1.0000E-09;Y1 +0.0E+00;Y2 +1.00000E+00
        :MTES:RMOD FOR;RMOD:TIME +1E+00;WAV 1000;SIGM +6.0E+00
        :MTES:RMOD:FACT:STOP 0;PRIN 0;SAVE 0
         */

        public void Oscilloscope_WriteSetting()
        {

        }

        public void Oscilloscope_Run()
        {

        }


    }
}
