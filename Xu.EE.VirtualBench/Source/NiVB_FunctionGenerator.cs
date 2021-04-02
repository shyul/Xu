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
        public int FGEN_MaximumChannelNumber { get; } = 1;

        public void FGEN_WriteSetting(int ch_num = 1)
        {
            if (ch_num > 1)
                throw new Exception("Only " + FGEN_MaximumChannelNumber + " is supported, you are trying to assign " + ch_num);

            Console.WriteLine("Frequency = " + FGEN_Frequency + " | Amplitude = " + FGEN_Amplitude);

            Status = (NiVB_Status)NiFGEN_ConfigureStandardWaveform(NiFGEN_Handle, m_FGEN_WaveFormType, FGEN_Amplitude, FGEN_DcOffset, FGEN_Frequency, FGEN_DutyCycle);
        }

        public void FGEN_ON(int _)
        {
            Status = (NiVB_Status)NiFGEN_Run(NiFGEN_Handle);
        }

        public void FGEN_OFF(int _)
        {
            Status = (NiVB_Status)NiFGEN_Stop(NiFGEN_Handle);
        }

        public WaveFormType FGEN_WaveFormType
        {
            get
            {
                return m_FGEN_WaveFormType switch
                {
                    0 => WaveFormType.Sine,
                    1 => WaveFormType.Square,
                    2 => WaveFormType.Triangle,
                    3 => WaveFormType.DC,
                    _ => WaveFormType.Unknown,
                };
            }
            set
            {
                m_FGEN_WaveFormType = value switch
                {
                    WaveFormType.Sine => 0,
                    WaveFormType.Square => 1,
                    WaveFormType.Triangle => 2,
                    WaveFormType.DC => 3,
                    _ => throw new Exception("Unsupported WaveFormType: " + value),
                };
            }
        }

        private uint m_FGEN_WaveFormType = 2;

        public double FGEN_Amplitude { get; set; } = 2;
         
        public double FGEN_DcOffset { get; set; } = 0;

        public double FGEN_Frequency { get; set; } = 1e6;

        public double FGEN_DutyCycle { get; set; } = 50;

        #region DLL Export

        private IntPtr NiFGEN_Handle;

        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_Initialize", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_Initialize(
            IntPtr libraryHandle,
            [MarshalAs(UnmanagedType.LPStr)] string deviceName,
            bool reset,
            out IntPtr instrumentHandle);



        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_Close", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_FGEN_Close(IntPtr instrumentHandle);

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
