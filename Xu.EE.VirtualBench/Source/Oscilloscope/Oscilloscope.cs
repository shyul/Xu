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
    public partial class NiVB
    {
        public Dictionary<string, OscilloscopeAnalogChannel> OscilloscopeAnalogChannels { get; } = new();

        public const string OscilloscopeAnalogChannel1Name = "mso/1";
        public const string OscilloscopeAnalogChannel2Name = "mso/2";

        public OscilloscopeAnalogChannel OscilloscopeAnalogChannel1 => OscilloscopeAnalogChannels[OscilloscopeAnalogChannel1Name];
        public OscilloscopeAnalogChannel OscilloscopeAnalogChannel2 => OscilloscopeAnalogChannels[OscilloscopeAnalogChannel2Name];

        public void OscilloscopeTiming_WriteSetting(ITriggerSource source)
        {
            DSO_TriggerSource = source;
            Status = (NiVB_Status)NiMSO_ConfigureTiming(NiMSO_Handle, AnalogSampleRate, AcquisitionTime, PretriggerTime, 0);
            if (DSO_TriggerSource is OscilloscopeAnalogChannel ch)
                Status = (NiVB_Status)NiMSO_ConfigureAnalogEdgeTrigger(NiMSO_Handle, ch.Name, (uint)ch.TriggerEdge, ch.TriggerLevel, ch.TriggerHysteresis, 0);
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
                channel.ProbeAttenuation.ToUInt32(1),
                (uint)(channel.Coupling == AnalogCoupling.DC ? 1 : 0));
        }

        public void Oscilloscope_WriteSetting()
        {
            OscilloscopeTiming_WriteSetting(OscilloscopeAnalogChannel1);
            OscilloscopeAnalog_WriteSetting(OscilloscopeAnalogChannel1Name);
            OscilloscopeAnalog_WriteSetting(OscilloscopeAnalogChannel2Name);

            //Status = (NiVB_Status)NiMSO_ConfigureAnalogChannel(NiMSO_Handle, "mso/1:2", true, 10, 0, 1, 1);
            Status = (NiVB_Status)NiMSO_EnableDigitalChannels(NiMSO_Handle, "mso/d0:31, mso/clk0:1", true);
            Status = (NiVB_Status)NiMSO_ConfigureAdvancedDigitalTiming(NiMSO_Handle, 1, 1e9, 0, 0.0);
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
            Timestamp[] digitalSampleTimestamps = new Timestamp[digitalTimestampsSize];

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

            long epoch = 0;
            double seconeds = 0;
            ConvertTimestampToValues(analogT0, ref epoch, ref seconeds);
            DateTime analogTime = TimeTool.FromEpoch(epoch).ToLocalTime();
            Console.WriteLine("analogTime = " + analogTime);

            List<double> ch_1_data = new List<double>();
            List<double> ch_2_data = new List<double>();

            for (int i = 0; i < analogData.Length; i++)
            {
                if (i % 2 == 0)
                    ch_1_data.Add(analogData[i]);
                else
                    ch_2_data.Add(analogData[i]);
            }

            OscilloscopeAnalogChannel1.Result = ch_1_data;
            OscilloscopeAnalogChannel2.Result = ch_2_data;
        }

        public ITriggerSource DSO_TriggerSource { get; set; } //= OscilloscopeAnalogChannel1;

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
            Oscilloscope_WriteSetting();
            Oscilloscope_Run();
            Oscilloscope_GetData();

            foreach (double v in OscilloscopeAnalogChannel1.Result)
            {
                Console.Write(v.ToString("0.##") + ", ");
            }
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

        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_ConfigureAnalogChannel", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_ConfigureAnalogChannel(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string channel,
            bool enableChannel,
            double verticalRange,
            double verticalOffset,
            uint probeAttenuation, // 1 = 1x, 10 = 10x
            uint verticalCoupling); // 0 = AC, 1 = DC

        [DllImport(DLL_NAME, EntryPoint = "niVB_MSO_EnableDigitalChannels", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiMSO_EnableDigitalChannels(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string channel,
            bool enableChannel);


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
            Timestamp[] digitalSampleTimestamps,
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
