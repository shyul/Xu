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
        public NiVB(string resourceName)
        {
            ResourceName = resourceName;
        }

        ~NiVB() => Dispose();

        public void Dispose() => Close();

        public string ResourceName { get; private set; }

        public void Open()
        {
            Status = (NiVB_Status)Initialize(LIBRARY_VERSION, out Handle);



            Status = (NiVB_Status)NiDMM_Initialize(Handle, ResourceName, true, out NiDMM_Handle);
            MultimeterChannels[MultimeterChannelName] = new MultimeterChannel(MultimeterChannelName, this);


            Status = (NiVB_Status)NiMSO_Initialize(Handle, ResourceName, true, out NiMSO_Handle);
            OscilloscopeAnalogChannels[OscilloscopeAnalogChannel1Name] = new OscilloscopeAnalogChannel(OscilloscopeAnalogChannel1Name, this);
            OscilloscopeAnalogChannels[OscilloscopeAnalogChannel2Name] = new OscilloscopeAnalogChannel(OscilloscopeAnalogChannel2Name, this);

            Status = (NiVB_Status)NiFGEN_Initialize(Handle, ResourceName, true, out NiFGEN_Handle);
            FunctionGeneratorChannels[FunctionGeneratorChannelName] = new FunctionGeneratorChannel(FunctionGeneratorChannelName, this);

            Status = (NiVB_Status)NiPS_Initialize(Handle, ResourceName, true, out NiPS_Handle);
            PowerSupplyChannels[PowerSupplyP6VName] = new PowerSupplyChannel(PowerSupplyP6VName, this, new Range<double>(0, 6), new Range<double>(0, 1));
            PowerSupplyChannels[PowerSupplyP25VName] = new PowerSupplyChannel(PowerSupplyP25VName, this, new Range<double>(0, 25), new Range<double>(0, 0.5));
            PowerSupplyChannels[PowerSupplyN25VName] = new PowerSupplyChannel(PowerSupplyN25VName, this, new Range<double>(-25, 0), new Range<double>(0, 0.5));
        
        
        }

        public void Close()
        {
            FunctionGenerator_OFF();
            FunctionGeneratorChannels.Clear();
            NiFGEN_Close(NiFGEN_Handle);





            PowerSupply_OFF();


            




            NiMSO_Close(NiMSO_Handle);




            Finalize(Handle);
        }

        public void GetCalibrationInfo()
        {
            Status = (NiVB_Status)Cal_GetCalibrationInformation(Handle, ResourceName, out Timestamp calibrationDate, out int recommendedCalibrationInterval, out int calibrationInterval);
            Console.WriteLine("Timestamp: " + ConvertTimestampToValues(calibrationDate));
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
        private static extern int ConvertTimestampToValues(Timestamp timestamp, out long secondsSince1970, out double fractionalSeconds);

        [DllImport(DLL_NAME, EntryPoint = "niVB_Cal_GetCalibrationInformation", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Cal_GetCalibrationInformation(
            IntPtr libraryHandle,
            [MarshalAs(UnmanagedType.LPStr)] string deviceName,
            out Timestamp calibrationDate,
            out int recommendedCalibrationInterval,
            out int calibrationInterval);

        [StructLayout(LayoutKind.Sequential)]
        public struct Timestamp
        {
            public uint t1, t2, t3, t4;
        }

        #endregion DLL Export
    }
}
