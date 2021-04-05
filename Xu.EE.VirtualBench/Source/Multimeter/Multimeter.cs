using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Xu.EE.VirtualBench
{
    public partial class NiVB
    {
        public Dictionary<string, MultimeterChannel> MultimeterChannels { get; } = new();

        public const string MultimeterChannelName = "dmm1";

        public NiVBMultimeterChannel MultimeterChannel => MultimeterChannels[MultimeterChannelName] as NiVBMultimeterChannel;


        public void Multimeter_WriteSetting(string channelName) 
        {
        
        }

        public double Multimeter_Read(string channelName)
        {
            Status = (NiVB_Status)NiDMM_Read(NiDMM_Handle, out double result);
            return result;
        }

        #region DLL Export

        private IntPtr NiDMM_Handle;

        [DllImport(DLL_NAME, EntryPoint = "niVB_DMM_Initialize", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDMM_Initialize(
            IntPtr libraryHandle,
            [MarshalAs(UnmanagedType.LPStr)] string deviceName,
            bool reset,
            out IntPtr instrumentHandle);

        [DllImport(DLL_NAME, EntryPoint = "niVB_DMM_ResetInstrument", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDMM_ResetInstrument(
            IntPtr instrumentHandle);

        [DllImport(DLL_NAME, EntryPoint = "niVB_DMM_Close", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDMM_Close(
            IntPtr instrumentHandle);


        [DllImport(DLL_NAME, EntryPoint = "niVB_DMM_ConfigureMeasurement", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDMM_ConfigureMeasurement(
            IntPtr instrumentHandle,
            uint function,
            bool autoRange,
            double manualRange);

        [DllImport(DLL_NAME, EntryPoint = "niVB_DMM_QueryMeasurement", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDMM_QueryMeasurement(
            IntPtr instrumentHandle,
            out uint function,
            out bool autoRange,
            out double manualRange);

        [DllImport(DLL_NAME, EntryPoint = "niVB_DMM_Read", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDMM_Read(
            IntPtr instrumentHandle,
            out double measurement);

        #endregion DLL Export
    }
}
