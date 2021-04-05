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
        public Dictionary<string, FunctionGeneratorChannel> FunctionGeneratorChannels { get; } = new();

        public const string FunctionGeneratorChannelName = "fgen1";

        public FunctionGeneratorChannel FunctionGeneratorChannel => FunctionGeneratorChannels[FunctionGeneratorChannelName];

        public void FGEN_WriteSetting(string channelName)
        {
            FunctionGeneratorChannel fgch = FunctionGeneratorChannels[channelName];

            if (fgch.IsArbitrary)
            {


            }
            else
            {
                Console.WriteLine("Frequency = " + fgch.Frequency + " | DutyCycle = " + fgch.DutyCycle + " | Amplitude = " + fgch.Amplitude + " | DcOffset = " + fgch.DcOffset);

                uint waveform = FunctionGeneratorChannels[channelName].WaveFormType switch
                {
                    WaveFormType.Sine => 0,
                    WaveFormType.Square => 1,
                    WaveFormType.Triangle => 2,
                    WaveFormType.DC => 3,
                    _ => throw new Exception("Unsupported WaveFormType: " + fgch.WaveFormType),
                };

                Status = (NiVB_Status)NiFGEN_ConfigureStandardWaveform(NiFGEN_Handle, waveform,
                    fgch.Amplitude,
                    fgch.DcOffset,
                    fgch.Frequency,
                    fgch.DutyCycle);
            }
        }

        public void FGEN_ON(string _)
        {
            Status = (NiVB_Status)NiFGEN_Run(NiFGEN_Handle);
        }

        public void FGEN_OFF(string _)
        {
            Status = (NiVB_Status)NiFGEN_Stop(NiFGEN_Handle);
        }

        public void FGEN_ON() => FGEN_ON(FunctionGeneratorChannelName);

        public void FGEN_OFF() => FGEN_OFF(FunctionGeneratorChannelName);



        #region DLL Export

        private IntPtr NiFGEN_Handle;

        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_Initialize", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_Initialize(
            IntPtr libraryHandle,
            [MarshalAs(UnmanagedType.LPStr)] string deviceName,
            bool reset,
            out IntPtr instrumentHandle);



        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_Close", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_Close(IntPtr instrumentHandle);

        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_ResetInstrument", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_ResetInstrument(IntPtr instrumentHandle);



        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_Run", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_Run(IntPtr instrumentHandle);

        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_Stop", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_Stop(IntPtr instrumentHandle);

        // niVB_FGEN_GenerationStatus_Running = 0,
        // niVB_FGEN_GenerationStatus_Stopped = 1,
        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_QueryGenerationStatus", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_QueryGenerationStatus(IntPtr instrumentHandle, out uint status);



        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_EnableFilter", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_EnableFilter(IntPtr instrumentHandle, bool enableFilter);

        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_QueryFilter", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_QueryFilter(IntPtr instrumentHandle, out bool enableFilter);

        // niVB_FGEN_WaveformMode_Standard = 0,
        // niVB_FGEN_WaveformMode_Arbitrary = 1,
        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_QueryWaveformMode", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_QueryWaveformMode(IntPtr instrumentHandle, out uint waveformMode);

        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_ConfigureStandardWaveform", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_ConfigureStandardWaveform(
            IntPtr instrumentHandle,
            uint waveformFunction,
            double amplitude,
            double dcOffset,
            double frequency,
            double dutyCycle);

        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_QueryStandardWaveform", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_QueryStandardWaveform(
            IntPtr instrumentHandle,
            out uint waveformFunction,
            out double amplitude,
            out double dcOffset,
            out double frequency,
            out double dutyCycle);



        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_ConfigureArbitraryWaveform", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_ConfigureArbitraryWaveform(
            IntPtr instrumentHandle,
            double[] waveform,
            ulong waveformSize,
            double samplePeriod);

        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_ConfigureArbitraryWaveformGainAndOffset", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_ConfigureArbitraryWaveformGainAndOffset(
            IntPtr instrumentHandle,
            double gain,
            double dcOffset);


        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_QueryArbitraryWaveform", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_QueryArbitraryWaveform(
            IntPtr instrumentHandle,
            out double sampleRate);

        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_QueryArbitraryWaveformGainAndOffset", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_QueryArbitraryWaveformGainAndOffset(
            IntPtr instrumentHandle,
            out double gain,
            out double dcOffset);




        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_SelfCalibrate", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_SelfCalibrate(IntPtr instrumentHandle);

        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_InitializeCalibration", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_InitializeCalibration(
            IntPtr libraryHandle,
            [MarshalAs(UnmanagedType.LPStr)] string deviceName,
            [MarshalAs(UnmanagedType.LPStr)] string password,
            out IntPtr calibrationHandle);

        #endregion DLL Export
    }
}
