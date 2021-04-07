using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Xu;

namespace Xu.EE.VirtualBench
{
    public partial class NiVB
    {
        public Dictionary<string, Pin> DigitalIoPins { get; } = new();

        #region DLL Export

        private IntPtr NiDIO_Handle;

        [DllImport(DLL_NAME, EntryPoint = "niVB_Dig_Initialize", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDig_Initialize(
            IntPtr libraryHandle,
            [MarshalAs(UnmanagedType.LPStr)] string lines,
            bool reset,
            out IntPtr instrumentHandle);

        [DllImport(DLL_NAME, EntryPoint = "niVB_Dig_ResetInstrument", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDig_ResetInstrument(
            IntPtr instrumentHandle);

        [DllImport(DLL_NAME, EntryPoint = "niVB_Dig_Close", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDig_Close(
            IntPtr instrumentHandle);

        [DllImport(DLL_NAME, EntryPoint = "niVB_Dig_TristateLines", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDig_TristateLines(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string lines);

        [DllImport(DLL_NAME, EntryPoint = "niVB_Dig_ExportSignal", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDig_ExportSignal(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string lines,
            uint signal); // 0 = FGEN Start, 1 = MSO Trigger

        [DllImport(DLL_NAME, EntryPoint = "niVB_Dig_QueryExportSignal", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDig_QueryExportSigna(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string lines,
            out uint signal); // 0 = FGEN Start, 1 = MSO Trigger

        [DllImport(DLL_NAME, EntryPoint = "niVB_Dig_QueryLineConfiguration", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDig_QueryLineConfiguration(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string tristateLines,
            ulong tristateLinesSize,
            out ulong tristateLinesSizeOut,
            [MarshalAs(UnmanagedType.LPStr)] string staticLines,
            ulong staticLinesSize,
            out ulong staticLinesSizeOut,
            [MarshalAs(UnmanagedType.LPStr)] string exportLines,
            ulong exportLinesSize,
            out ulong exportLinesSizeOut);

        [DllImport(DLL_NAME, EntryPoint = "niVB_Dig_Write", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDig_Write(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string lines,
            bool[] data,
            ulong dataSize);

        [DllImport(DLL_NAME, EntryPoint = "niVB_Dig_Read", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDig_Read(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string lines,
            bool[] data,
            ulong dataSize,
            out ulong dataSizeOut);

        #endregion DLL Export
    }
}
