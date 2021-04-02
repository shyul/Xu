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
    public partial class NiVB :
        IMixedSignalOscilloscope,
        IFunctionGenerator,
        IMultimeter,
        IPowerSupply,
        IDigitalControl
    {
        public NiVB(string resourceName) => ResouceName = resourceName;

        ~NiVB() => Dispose();

        public void Dispose() => Close();

        public string ResouceName { get; private set; }

        public void Open() 
        {
            Status = (NiVB_Status)Initialize(LIBRARY_VERSION, out Handle);
            Status = (NiVB_Status)NiMSO_Initialize(Handle, ResouceName, true, out NiMSO_Handle);
            Status = (NiVB_Status)NiFGEN_Initialize(Handle, ResouceName, true, out NiFGEN_Handle);
        }

        public void Close()
        {
            PWR_OFF(1);
            NiFGEN_Close(NiFGEN_Handle);
            NiMSO_Close(NiMSO_Handle);


            Finalize(Handle);
        }

        public void GetCalibrationInfo()
        {
            Timestamp calibrationDate = new();
            int recommendedCalibrationInterval = 0;
            int calibrationInterval = 0;
            Status = (NiVB_Status)Cal_GetCalibrationInformation(Handle, ResouceName, ref calibrationDate, ref recommendedCalibrationInterval, ref calibrationInterval);

            long epoch = 0;
            double seconeds = 0;

            ConvertTimestampToValues(calibrationDate, ref epoch, ref seconeds);

            Console.WriteLine("Timestamp Epoch: " + epoch + " | Timestamp Seconds: " + seconeds);

            DateTime calTime = TimeTool.FromEpoch(epoch).ToLocalTime();

            Console.WriteLine("Timestamp: " + calTime);

            Console.WriteLine("Recommended calibration interval: " + recommendedCalibrationInterval + " months, calibration interval: " + calibrationInterval + " months\n");

        }

        #region DLL Export

        private IntPtr Handle;
        private const string DLL_NAME = "nilcicapi.dll";
        private const uint LIBRARY_VERSION = 17874944;

        [DllImport(DLL_NAME, EntryPoint = "niVB_Initialize", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Initialize(uint version, out IntPtr libraryHandle);

        [DllImport(DLL_NAME, EntryPoint = "niVB_Finalize", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Finalize(IntPtr libraryHandle);

        [DllImport(DLL_NAME, EntryPoint = "niVB_ConvertTimestampToValues", CallingConvention = CallingConvention.Cdecl)]
        private static extern int ConvertTimestampToValues(Timestamp timestamp, ref long secondsSince1970, ref double fractionalSeconds);

        [DllImport(DLL_NAME, EntryPoint = "niVB_Cal_GetCalibrationInformation", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Cal_GetCalibrationInformation(IntPtr libraryHandle, [MarshalAs(UnmanagedType.LPStr)] string deviceName, ref Timestamp calibrationDate, ref int recommendedCalibrationInterval, ref int calibrationInterval);

        [StructLayout(LayoutKind.Sequential)]
        public struct Timestamp
        {
            public uint t1, t2, t3, t4;
        }

        #endregion DLL Export
    }
}
