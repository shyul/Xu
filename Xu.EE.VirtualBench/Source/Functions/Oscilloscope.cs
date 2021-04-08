using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Xu;
using Xu.EE;

namespace Xu.EE.VirtualBench
{
    public partial class NiVB : IOscilloscope
    {
        public Dictionary<string, OscilloscopeAnalogChannel> OscilloscopeAnalogChannels { get; } = new();

        //public Dictionary<string, >

        public const string OscilloscopeAnalogChannel1Name = "mso/1";
        public const string OscilloscopeAnalogChannel2Name = "mso/2";

        public OscilloscopeAnalogChannel OscilloscopeAnalogChannel1 => OscilloscopeAnalogChannels[OscilloscopeAnalogChannel1Name];
        public OscilloscopeAnalogChannel OscilloscopeAnalogChannel2 => OscilloscopeAnalogChannels[OscilloscopeAnalogChannel2Name];



        public ITriggerSource Oscilloscope_TriggerSource
        {
            get => m_Oscilloscope_TriggerSource;

            set
            {
                m_Oscilloscope_TriggerSource = value;
                if (m_Oscilloscope_TriggerSource is OscilloscopeAnalogChannel ch)
                    Status = (NiVB_Status)NiMSO_ConfigureAnalogEdgeTrigger(NiMSO_Handle, ch.Name, (uint)ch.TriggerEdge, ch.TriggerLevel, ch.TriggerHysteresis, 0);
            }
        }

        private ITriggerSource m_Oscilloscope_TriggerSource;

        public void OscilloscopeTiming_WriteSetting()
        {
            Status = (NiVB_Status)NiMSO_ConfigureTiming(NiMSO_Handle, AnalogSampleRate, AcquisitionTime, PretriggerTime, 0);
        }

        public void OscilloscopeAnalog_WriteSetting(string channelName)
        {
            var channel = OscilloscopeAnalogChannels[channelName];

            Status = (NiVB_Status)NiMSO_ConfigureAnalogChannel(
                NiMSO_Handle,
                channel.Name,
                channel.Enabled,
                channel.VerticalRange,
                channel.VerticalOffset,
                channel.Attenuation.ToUInt32(1),
                (uint)(channel.Coupling == AnalogCoupling.DC ? 1 : 0));

            if (channel.BandWidthLimit > 100e6) channel.BandWidthLimit = 100e6;
            else if (channel.BandWidthLimit < 20e6) channel.BandWidthLimit = 20e6;

            Status = (NiVB_Status)NiMSO_ConfigureAnalogChannelCharacteristics(
                NiMSO_Handle,
                channel.Name,
                (uint)(channel.Impedance <= 50 ? 1 : 0),
                channel.BandWidthLimit);

            OscilloscopeAnalog_ReadSetting(channelName);
        }

        public void OscilloscopeAnalog_ReadSetting(string channelName)
        {
            var channel = OscilloscopeAnalogChannels[channelName];

            Status = (NiVB_Status)NiMSO_QueryAnalogChannel(
                NiMSO_Handle,
                channel.Name,
                out bool enabled,
                out double verticalRange,
                out double verticalOffset,
                out uint probeAttenuation,
                out uint coupling);

            channel.Enabled = enabled;
            channel.VerticalRange = verticalRange;
            channel.VerticalOffset = verticalOffset;
            channel.Attenuation = probeAttenuation;
            channel.Coupling = (coupling == 0) ? AnalogCoupling.AC : AnalogCoupling.DC;

            Status = (NiVB_Status)NiMSO_QueryAnalogChannelCharacteristics(
                NiMSO_Handle,
                channel.Name,
                out uint inputImpedance, //  (uint)(channel.InputImpedance <= 50 ? 1 : 0),
                out double bandWidthLimit);

            channel.Impedance = inputImpedance == 1 ? 50 : 1e6;
            channel.BandWidthLimit = bandWidthLimit;
        }

        public void Oscilloscope_WriteSetting()
        {
            OscilloscopeTiming_WriteSetting();
            OscilloscopeAnalog_WriteSetting(OscilloscopeAnalogChannel1Name);
            OscilloscopeAnalog_WriteSetting(OscilloscopeAnalogChannel2Name);

            //Status = (NiVB_Status)NiMSO_EnableDigitalChannels(NiMSO_Handle, "mso/d0:31, mso/clk0:1", true);
            Status = (NiVB_Status)NiMSO_EnableDigitalChannels(NiMSO_Handle, "mso/d0:31", true);
            //Status = (NiVB_Status)NiMSO_ConfigureAdvancedDigitalTiming(NiMSO_Handle, 1, 1e9, 0, 0.0);
            Status = (NiVB_Status)NiMSO_ConfigureAdvancedDigitalTiming(NiMSO_Handle, 0, 1e9, 0, 2.0);

        }

        public void Oscilloscope_Autosetup()
        {
            Status = (NiVB_Status)NiMSO_Autosetup(NiMSO_Handle);
        }

        public void Oscilloscope_Run()
        {
            Status = (NiVB_Status)NiMSO_Run(NiMSO_Handle);
        }

        public void Oscilloscope_GetData()
        {
            Status = (NiVB_Status)NiMSO_ReadAnalogDigitalU64(
               NiMSO_Handle,
               null,                               // out double[] analogData,
               0,                                  // ulong analogDataSize
               out ulong analogDataSize,           // out ulong analogDataSizeOut,
               out ulong analogDataStride,         // out ulong analogDataStride,
               out Timestamp _,                    // out Timestamp analogInitialTimestamp,
               null, // digitalData,               // out ulong[] digitalData,
               0,                                  // ulong digitalDataSize,
               out ulong digitalDataSize,          // out ulong digitalDataSizeOut,
               null, // digitalSampleTimestamps,   // out Timestamp[] digitalSampleTimestamps,
               0,                                  // ulong digitalSampleTimestampsSize,
               out ulong digitalTimestampsSize,    // out ulong digitalSampleTimestampsSizeOut,
               out Timestamp _,                    // out Timestamp digitalInitialTimestamp,
               out Timestamp _,                    // out Timestamp triggerTimestamp,
               out uint triggerReason);            // out uint triggerReason

            double[] analogData = new double[analogDataSize];
            ulong[] digitalData = new ulong[digitalDataSize];
            uint[] digitalSampleTimestamps = new uint[digitalTimestampsSize];

            Status = (NiVB_Status)NiMSO_ReadAnalogDigitalU64(
                NiMSO_Handle,
                analogData,                         // out double[] analogData, -> ch1, ch2, ch1, ch2, ... 6001 pts per channel
                analogDataSize,                     // ulong analogDataSize
                out ulong _,                        // out ulong analogDataSizeOut,
                out analogDataStride,               // out ulong analogDataStride,
                out Timestamp analogT0,             // out Timestamp analogInitialTimestamp,
                digitalData,                        // out ulong[] digitalData,
                digitalDataSize,                    // ulong digitalDataSize,
                out ulong _,                        // out ulong digitalDataSizeOut,
                digitalSampleTimestamps,            // out Timestamp[] digitalSampleTimestamps,
                digitalTimestampsSize,              // ulong digitalSampleTimestampsSize,
                out ulong _,                        // out ulong digitalSampleTimestampsSizeOut,
                out Timestamp digitalT0,            // out Timestamp digitalInitialTimestamp,
                out Timestamp triggerTimestamp,     // out Timestamp triggerTimestamp,
                out triggerReason);                 // out uint triggerReason

            Console.WriteLine("analogDataSize = " + analogDataSize);
            Console.WriteLine("analogDataStride = " + analogDataStride);
            Console.WriteLine("digitalDataSize = " + digitalDataSize);
            Console.WriteLine("digitalTimestampsSize = " + digitalTimestampsSize);

            Console.WriteLine("triggerReason = " + triggerReason);

            Console.WriteLine("analog T0 = " + ConvertTimestampToValues(analogT0));
            Console.WriteLine("digital T0 = " + ConvertTimestampToValues(digitalT0));

            int j = 0;
            foreach (var t0 in digitalSampleTimestamps)
            {
                Console.WriteLine("Digital Sample [" + j + "] = " + t0 + " | " + digitalData[j]);
                j++;
            }

            List<double> ch_1_data = new();
            List<double> ch_2_data = new();

            for (int i = 0; i < analogData.Length; i++)
            {
                if (i % 2 == 0)
                    ch_1_data.Add(analogData[i]);
                else
                    ch_2_data.Add(analogData[i]);
            }

            OscilloscopeAnalogChannel1.Samples = ch_1_data;
            OscilloscopeAnalogChannel2.Samples = ch_2_data;
        }

        public DateTime ConvertTimestampToValues(Timestamp t0)
        {
            ConvertTimestampToValues(t0, out long epoch, out double seconds);
            Console.WriteLine("epoch = " + epoch + " | seconds = " + seconds);
            DateTime t = TimeTool.FromEpoch(epoch).ToLocalTime();
            t.AddMilliseconds(seconds * 1000);
            return t;
        }

        public double AnalogSampleRate
        {
            get => OscilloscopeAnalogChannel1.SampleRate;

            set
            {
                OscilloscopeAnalogChannel1.SampleRate = value;
                OscilloscopeAnalogChannel2.SampleRate = value;
            }
        }

        public double DigitalSampleRate { get; set; } = 1e9;

        public double AcquisitionTime { get; set; } = 12e-6;

        public double PretriggerTime { get; set; } = 6e-6;

        public void TestMSO()
        {
            //Status = (NiVB_Status)NiMSO_ConfigureAnalogChannel(NiMSO_Handle, "mso/1:2", true, 10, 0, 1, 1);
            Oscilloscope_TriggerSource = OscilloscopeAnalogChannel1;
            Oscilloscope_WriteSetting();

            Console.WriteLine("Bandwidth Limit = " + OscilloscopeAnalogChannel1.BandWidthLimit);
            Oscilloscope_Run();
            Oscilloscope_GetData();
            /*
            foreach (double v in OscilloscopeAnalogChannel1.Samples)
            {
                Console.Write(v.ToString("0.##") + ", ");
            }*/
        }

        #region DLL Export

        private IntPtr NiMSO_Handle;

        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_Initialize", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_Initialize(
            IntPtr libraryHandle,
            [MarshalAs(UnmanagedType.LPStr)] string deviceName,
            bool reset,
            out IntPtr instrumentHandle);

        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_Autosetup", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_Autosetup(
            IntPtr instrumentHandle);



        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_Close", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_Close(
            IntPtr instrumentHandle);


        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_QueryEnabledAnalogChannels", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_QueryEnabledAnalogChannels(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string channel,
            long channelsSize,
            out long channelsSizeOut);


        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_ConfigureAnalogChannel", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_ConfigureAnalogChannel(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string channel,
            bool enableChannel,
            double verticalRange,
            double verticalOffset,
            uint probeAttenuation, // 1 = 1x, 10 = 10x
            uint verticalCoupling); // 0 = AC, 1 = DC

        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_QueryAnalogChannel", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_QueryAnalogChannel(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string channel,
            out bool enableChannel,
            out double verticalRange,
            out double verticalOffset,
            out uint probeAttenuation, // 1 = 1x, 10 = 10x
            out uint verticalCoupling); // 0 = AC, 1 = DC

        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_ConfigureAnalogChannelCharacteristics", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_ConfigureAnalogChannelCharacteristics(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string channel,
            uint inputImpedance, // 0 = 1MOhm, 1 = 50Ohm
            double bandwidthLimit);

        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_QueryAnalogChannelCharacteristics", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_QueryAnalogChannelCharacteristics(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string channel,
            out uint inputImpedance, // 0 = 1MOhm, 1 = 50Ohm
            out double bandwidthLimit);


        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_EnableDigitalChannels", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_EnableDigitalChannels(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string channel,
            bool enableChannel);


        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_ConfigureDigitalThreshold", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_ConfigureDigitalThreshold(
            IntPtr instrumentHandle,
            double threshold);


        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_ConfigureTiming", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_ConfigureTiming(
            IntPtr instrumentHandle,
            double sampleRate,
            double acquisitionTime,
            double pretriggerTime,
            uint samplingMode); // Sample = 0, PeakDetect = 1

        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_ConfigureAdvancedDigitalTiming", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_ConfigureAdvancedDigitalTiming(
            IntPtr instrumentHandle,
            uint digitalSampleRateControl, // 0 = Automatic, 1 = Manual
            double digitalSampleRate,
            uint bufferControl, // 0 = Automatic, 1 = Manual
            double bufferPretriggerPercent);

        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_ConfigureAnalogEdgeTrigger", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_ConfigureAnalogEdgeTrigger(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string triggerSource,
            uint triggerSlope, // 0 = Rising, 1 = Falling, 2 = Either
            double triggerLevel,
            double triggerHysteresis,
            uint triggerInstance); // A = 0, B = 1;

        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_ConfigureDigitalEdgeTrigger", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_ConfigureDigitalEdgeTrigger(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string triggerSource,
            uint triggerSlope,  // 0 = Rising, 1 = Falling, 2 = Either
            uint triggerInstance); // A = 0, B = 1;

        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_Run", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_Run(
            IntPtr instrumentHandle);

        /*
         * Transfers data from the instrument as long as the acquisition state is 
         * Acquisition Complete. If the state is either Running or Triggered, this 
         * method will wait until the state transitions to Acquisition Complete. If 
         * the state is Stopped, this method returns an error.
         */
        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_ReadAnalogDigitalU64", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_ReadAnalogDigitalU64(
            IntPtr instrumentHandle,
            double[] analogData,
            ulong analogDataSize,
            out ulong analogDataSizeOut,
            out ulong analogDataStride,
            out Timestamp analogInitialTimestamp,
            ulong[] digitalData,
            ulong digitalDataSize,
            out ulong digitalDataSizeOut,
            uint[] digitalSampleTimestamps,
            ulong digitalSampleTimestampsSize,
            out ulong digitalSampleTimestampsSizeOut,
            out Timestamp digitalInitialTimestamp,
            out Timestamp triggerTimestamp,
            out uint triggerReason); // 0 = Normal, 1 = Forced, 2 = Auto

        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_ConfigureStateMode", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_ConfigureStateMode(
            IntPtr instrumentHandle,
            bool enable,
            [MarshalAs(UnmanagedType.LPStr)] string clockChannel,
            uint clockEdge);  // 0 = Rising, 1 = Falling, 2 = Either

        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_ConfigureImmediateTrigger", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_ConfigureImmediateTrigger(
            IntPtr instrumentHandle);



        #endregion DLL Export
    }
}
