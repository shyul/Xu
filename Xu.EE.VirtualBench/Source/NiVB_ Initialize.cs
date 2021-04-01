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
        IDisposable,
        IOscilloscope,
        IFunctionGenerator,
        IMultimeter,
        IPowerSupply
    {
        public NiVB(string resourceName)
        {
            ResouceName = resourceName;
            Status = (NiVB_Status)Initialize(LIBRARY_VERSION, out Handle);



        }

        ~NiVB()
        {
            Dispose();
        }

        public void Dispose()
        {
            Finalize(Handle);
        }

        public string ResouceName { get; }


        public void GetCalibrationInfo()
        {
            Timestamp calibrationDate = new Timestamp();
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


        private IntPtr Handle = new IntPtr();
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

    }


}
