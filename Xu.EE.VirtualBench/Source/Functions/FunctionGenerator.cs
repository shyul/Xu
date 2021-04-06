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

        public void FunctionGenerator_WriteSetting(string channelName)
        {
            FunctionGeneratorChannel ch = FunctionGeneratorChannels[channelName];
            FunctionGeneratorConfig config = ch.Config;

            if (config is FunctionGeneratorArbitraryConfig arb)
            {

            }
            else
            {
                switch (config)
                {
                    case FunctionGeneratorTriangleWaveConfig cfg:
                        Status = (NiVB_Status)NiFGEN_ConfigureStandardWaveform(
                            NiFGEN_Handle, 2,
                            cfg.Amplitude,
                            cfg.DcOffset,
                            cfg.Frequency,
                            cfg.DutyCycle);
                        break;

                    case FunctionGeneratorSquareWaveConfig cfg:
                        Status = (NiVB_Status)NiFGEN_ConfigureStandardWaveform(
                            NiFGEN_Handle, 1,
                            cfg.Amplitude,
                            cfg.DcOffset,
                            cfg.Frequency,
                            cfg.DutyCycle);
                        break;

                    case FunctionGeneratorSineWaveConfig cfg:
                        Status = (NiVB_Status)NiFGEN_ConfigureStandardWaveform(
                            NiFGEN_Handle, 0,
                            cfg.Amplitude,
                            cfg.DcOffset,
                            cfg.Frequency,
                            50);
                        break;

                    case FunctionGeneratorDcConfig cfg:
                        Status = (NiVB_Status)NiFGEN_ConfigureStandardWaveform(
                            NiFGEN_Handle, 3,
                            1,
                            cfg.DcOffset,
                            0,
                            50);
                        break;

                    default: throw new Exception("Unsupported WaveFormType: " + config.GetType().FullName);
                }

                //Console.WriteLine("Frequency = " + fgch.Frequency + " | DutyCycle = " + fgch.DutyCycle + " | Amplitude = " + fgch.Amplitude + " | DcOffset = " + fgch.DcOffset);
            }
        }

        public void FunctionGenerator_ReadSetting(string channelName)
        {
            FunctionGeneratorChannel ch = FunctionGeneratorChannels[channelName];

            Status = (NiVB_Status)NiFGEN_QueryWaveformMode(
                NiFGEN_Handle,
                out uint waveformMode);

            if (waveformMode == 0)
            {
                Status = (NiVB_Status)NiFGEN_QueryStandardWaveform(
                    NiFGEN_Handle,
                    out uint function,
                    out double amplitude,
                    out double dcOffset,
                    out double frequency,
                    out double dutyCycle);

                switch (function)
                {
                    case 0:
                        if (ch.Config is not FunctionGeneratorSineWaveConfig)
                        {
                            ch.Config = new FunctionGeneratorSineWaveConfig();
                        }

                        var config0 = ch.Config as FunctionGeneratorSineWaveConfig;
                        config0.Amplitude = amplitude;
                        config0.DcOffset = dcOffset;
                        config0.Frequency = frequency;
                        break;

                    case 1:
                        if (ch.Config is not FunctionGeneratorSquareWaveConfig)
                        {
                            ch.Config = new FunctionGeneratorSquareWaveConfig();
                        }
                        var config1 = ch.Config as FunctionGeneratorSquareWaveConfig;
                        config1.Amplitude = amplitude;
                        config1.DcOffset = dcOffset;
                        config1.Frequency = frequency;
                        config1.DutyCycle = dutyCycle;
                        break;

                    case 2:
                        if (ch.Config is not FunctionGeneratorTriangleWaveConfig)
                        {
                            ch.Config = new FunctionGeneratorTriangleWaveConfig();
                        }
                        var config2 = ch.Config as FunctionGeneratorTriangleWaveConfig;
                        config2.Amplitude = amplitude;
                        config2.DcOffset = dcOffset;
                        config2.Frequency = frequency;
                        config2.DutyCycle = dutyCycle;
                        break;

                    case 3:
                        if (ch.Config is not FunctionGeneratorDcConfig)
                        {
                            ch.Config = new FunctionGeneratorDcConfig();
                        }

                        var config3 = ch.Config as FunctionGeneratorDcConfig;
                        config3.DcOffset = dcOffset;
                        break;

                    default: throw new Exception("Unknown Function: " + function);
                }
            }
            else if (waveformMode == 1)
            {


            }


        }

        public void FunctionGenerator_ON(string channelName = FunctionGeneratorChannelName)
        {
            Status = (NiVB_Status)NiFGEN_Run(NiFGEN_Handle);
        }

        public void FunctionGenerator_OFF(string channelName = FunctionGeneratorChannelName)
        {
            Status = (NiVB_Status)NiFGEN_Stop(NiFGEN_Handle);
        }

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
