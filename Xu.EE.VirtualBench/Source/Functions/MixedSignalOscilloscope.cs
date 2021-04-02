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
        public void DSO_Run() 
        {
        
        }

        public string DSO_TriggerSource { get; set; }

        public double DSO_TriggerLevel { get; set; }

        public double DSO_TriggerHysteresis { get; set; }

        public void TestMSO() 
        {
            Status = (NiVB_Status)NiMSO_ConfigureAnalogChannel(NiMSO_Handle, "mso/1:2", true, 10, 0, 1, 1);
            Status = (NiVB_Status)NiMSO_EnableDigitalChannels(NiMSO_Handle, "mso/d0:31, mso/clk0:1", true);
            Status = (NiVB_Status)NiMSO_ConfigureTiming(NiMSO_Handle, 500e6, 12e-6, 6e-6, 0);
            Status = (NiVB_Status)NiMSO_ConfigureAdvancedDigitalTiming(NiMSO_Handle, 1, 1e9, 0, 0.0);
            Status = (NiVB_Status)NiMSO_ConfigureAnalogEdgeTrigger(NiMSO_Handle, "mso/1", 0, 1, 0, 0);
            Status = (NiVB_Status)NiMSO_Run(NiMSO_Handle);

            //ulong analogDataSize, analogDataStride, digitalDataSize, digitalTimestampsSize;
            /*
            double[] analogData = new double[0];
            ulong[] digitalData = new ulong[0];
            Timestamp[] digitalSampleTimestamps = new Timestamp[0];
            */
            Status = (NiVB_Status)NiMSO_ReadAnalogDigitalU64(
                NiMSO_Handle,
                null, // analogData,                              // out double[] analogData,
                0,                                  // ulong analogDataSize
                out ulong analogDataSize,           // out ulong analogDataSizeOut,
                out ulong analogDataStride,         // out ulong analogDataStride,
                out Timestamp _,                    // out Timestamp analogInitialTimestamp,
                null, // digitalData,                      // out ulong[] digitalData,
                0,                                  // ulong digitalDataSize,
                out ulong digitalDataSize,          // out ulong digitalDataSizeOut,
                null, // digitalSampleTimestamps,        // out Timestamp[] digitalSampleTimestamps,
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
                analogData,                     // out double[] analogData,
                analogDataSize,                                  // ulong analogDataSize
                out ulong _,                   // out ulong analogDataSizeOut,
                out analogDataStride,               // out ulong analogDataStride,
                out Timestamp analogT0,                    // out Timestamp analogInitialTimestamp,
                digitalData,                      // out ulong[] digitalData,
                digitalDataSize,                                  // ulong digitalDataSize,
                out ulong _,           // out ulong digitalDataSizeOut,
                digitalSampleTimestamps,                  // out Timestamp[] digitalSampleTimestamps,
                digitalTimestampsSize,                                  // ulong digitalSampleTimestampsSize,
                out ulong _,    // out ulong digitalSampleTimestampsSizeOut,
                out Timestamp digitalT0,                    // out Timestamp digitalInitialTimestamp,
                out Timestamp triggerTimestamp,                    // out Timestamp triggerTimestamp,
                out triggerReason);                        // out uint triggerReason


            List<double> adata = new List<double>(analogData);

            //double[] analogData = new double[analogDataSize];
            //ulong[] digitalData = new ulong[digitalDataSize];
            //Timestamp[] digitalTimestamps = new Timestamp[digitalTimestampsSize];

            Console.WriteLine("analogDataSize = " + analogDataSize);
            Console.WriteLine("analogDataStride = " + analogDataStride);
            Console.WriteLine("digitalDataSize = " + digitalDataSize);
            Console.WriteLine("digitalTimestampsSize = " + digitalTimestampsSize);

            Console.WriteLine("analog = " + adata.Sum() / adata.Count);
            Console.WriteLine("triggerReason = " + triggerReason);

            foreach(double v in analogData)
            {
                Console.Write(v.ToString("0.##") + ", ");
            }

            long epoch = 0;
            double seconeds = 0;
            ConvertTimestampToValues(analogT0, ref epoch, ref seconeds);
            DateTime analogTime = TimeTool.FromEpoch(epoch).ToLocalTime();
            Console.WriteLine("analogTime = " + analogTime);
            /*
                         IntPtr instrumentHandle,
            out double[] analogData,
            ulong analogDataSize,
            out ulong analogDataSizeOut,
            out ulong analogDataStride,
            out Timestamp analogInitialTimestamp,
            out ulong[] digitalData,

            ulong digitalDataSize,
            out ulong digitalDataSizeOut,
            out Timestamp[] digitalSampleTimestamps,

            ulong digitalSampleTimestampsSize,
            out ulong digitalSampleTimestampsSizeOut,
            out Timestamp digitalInitialTimestamp,
            out Timestamp triggerTimestamp,
            out uint triggerReason); // 0 = Normal, 1 = Forced, 2 = Auto
             */
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
            uint samplingMode); // Sampe = 0, PeakDetect = 1

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
