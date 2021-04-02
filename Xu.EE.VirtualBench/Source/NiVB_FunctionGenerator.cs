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
        private IntPtr NiFGEN_Handle;

        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_Initialize", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_Initialize(
            IntPtr libraryHandle,
            [MarshalAs(UnmanagedType.LPStr)] string deviceName,
            bool reset,
            out IntPtr instrumentHandle);

        [DllImport(DLL_NAME, EntryPoint = "niVB_FGEN_ConfigureStandardWaveform", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiFGEN_ConfigureStandardWaveform(
            IntPtr instrumentHandle,
            uint waveformFunction,
            double amplitude,
            double dcOffset,
            double frequency,
            double dutyCycle);


        public int FGEN_MaximumChannelNumber { get; } = 1;

        public void FGEN_Initialize() 
        {
            Status = (NiVB_Status)NiFGEN_Initialize(Handle, ResouceName, true, out NiFGEN_Handle);
        }

        public void FGEN_WriteSetting(int ch_num = 1)
        {
            if (ch_num > 1)
                throw new Exception("Only " + FGEN_MaximumChannelNumber + " is supported, you are trying to assign " + ch_num);

            Status = (NiVB_Status)NiFGEN_ConfigureStandardWaveform(NiFGEN_Handle, m_FGEN_WaveFormType, FGEN_Amplitude, FGEN_DcOffset, FGEN_Frequency, FGEN_DutyCycle);
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
    }
}
