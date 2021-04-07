using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;

namespace Xu.EE.VirtualBench
{
    public enum NiVB_DMM_InputResistance : uint
    {
        Res_10MOhm = 0,
        Res_10GOhm = 1,
    }

    public enum NiVB_DMM_CurrentTerminal : uint
    {
        Low = 0,
        High = 1,
    }

    public class MultimeterDcVoltageConfigNiVB : MultimeterDcVoltageConfig
    {
        public NiVB_DMM_InputResistance InputResistance { get; set; } = NiVB_DMM_InputResistance.Res_10MOhm;
    }

    public class MultimeterDcCurrentConfigNiVB : MultimeterDcCurrentConfig
    {
        public NiVB_DMM_CurrentTerminal Terminal { get; set; } = NiVB_DMM_CurrentTerminal.Low;
    }

    public class MultimeterAcCurrentConfigNiVB : MultimeterAcCurrentConfig
    {
        public NiVB_DMM_CurrentTerminal Terminal { get; set; } = NiVB_DMM_CurrentTerminal.Low;
    }

    public partial class NiVB : IMultimeter
    {
        public void TestConfigDMM()
        {
            var ch = MultimeterChannels[MultimeterChannelName];
            var config = ch.Config = new MultimeterDcVoltageConfig();
            ch.IsAutoRange = true;
            ch.WriteSetting();
            ch.ReadSetting();
        }

        public void TestReadDMM()
        {
            var ch = MultimeterChannels[MultimeterChannelName];
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(ch.Value);
                Thread.Sleep(100);
            }
        }

        public Dictionary<string, MultimeterChannel> MultimeterChannels { get; } = new();

        public const string MultimeterChannelName = "dmm/1";

        public void Multimeter_WriteSetting(string channelName)
        {
            var ch = MultimeterChannels[channelName];
            var config = ch.Config;

            if (config is not null)
            {
                uint function = config switch
                {
                    MultimeterDcVoltageConfig => 0,
                    MultimeterAcVoltageConfig => 1,
                    MultimeterDcCurrentConfig => 2,
                    MultimeterAcCurrentConfig => 3,
                    MultimeterResistanceConfig => 4,
                    MultimeterDiodeConfig => 5,
                    _ => throw new Exception("Unsupported function: " + config.GetType().FullName)
                };

                Status = (NiVB_Status)NiDMM_ConfigureMeasurement(NiDMM_Handle, function, ch.IsAutoRange, ch.Range.Maximum);

                switch (config)
                {
                    case MultimeterDcVoltageConfigNiVB cfg:
                        Status = (NiVB_Status)NiDMM_ConfigureDCVoltage(NiDMM_Handle, (uint)cfg.InputResistance);
                        break;

                    case MultimeterDcCurrentConfigNiVB cfg:
                        Status = (NiVB_Status)NiDMM_ConfigureDCCurrent(NiDMM_Handle, (uint)cfg.Terminal);
                        break;

                    case MultimeterAcCurrentConfigNiVB cfg:
                        Status = (NiVB_Status)NiDMM_ConfigureACCurrent(NiDMM_Handle, (uint)cfg.Terminal);
                        break;
                }
            }
        }

        public void Multimeter_ReadSetting(string channelName = MultimeterChannelName)
        {
            var ch = MultimeterChannels[channelName];
            Status = (NiVB_Status)NiDMM_QueryMeasurement(NiDMM_Handle, out uint function, out bool isAutoRange, out double range);

            switch (function)
            {
                case 0:
                    if (ch.Config is not MultimeterDcVoltageConfigNiVB)
                    {
                        ch.Config = new MultimeterDcVoltageConfigNiVB();
                    }

                    Status = (NiVB_Status)NiDMM_QueryDCVoltage(NiDMM_Handle, out uint inputResistance);
                    (ch.Config as MultimeterDcVoltageConfigNiVB).InputResistance = inputResistance == 0 ? NiVB_DMM_InputResistance.Res_10MOhm : NiVB_DMM_InputResistance.Res_10GOhm;
                    break;

                case 1 when ch.Config is not MultimeterAcVoltageConfig:
                    ch.Config = new MultimeterAcVoltageConfig();
                    break;

                case 2:
                    if (ch.Config is not MultimeterDcCurrentConfigNiVB)
                    {
                        ch.Config = new MultimeterDcCurrentConfigNiVB();
                    }

                    Status = (NiVB_Status)NiDMM_QueryDCCurrent(NiDMM_Handle, out uint autoRangeTerminalDc);
                    (ch.Config as MultimeterDcCurrentConfigNiVB).Terminal = autoRangeTerminalDc == 0 ? NiVB_DMM_CurrentTerminal.Low : NiVB_DMM_CurrentTerminal.High;
                    break;

                case 3:
                    if (ch.Config is not MultimeterAcCurrentConfigNiVB)
                    {
                        ch.Config = new MultimeterAcCurrentConfigNiVB();
                    }

                    Status = (NiVB_Status)NiDMM_QueryACCurrent(NiDMM_Handle, out uint autoRangeTerminalAc);
                    (ch.Config as MultimeterAcCurrentConfigNiVB).Terminal = autoRangeTerminalAc == 0 ? NiVB_DMM_CurrentTerminal.Low : NiVB_DMM_CurrentTerminal.High;
                    break;


                case 4 when ch.Config is not MultimeterResistanceConfig:
                    ch.Config = new MultimeterResistanceConfig();
                    break;

                case 5 when ch.Config is not MultimeterDiodeConfig:
                    ch.Config = new MultimeterDiodeConfig();
                    break;

                default: throw new Exception("Unknown Function: " + function);
            }

            ch.IsAutoRange = isAutoRange;
            ch.Range.Set(0, range);
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

        [DllImport(DLL_NAME, EntryPoint = "niVB_DMM_ConfigureDCVoltage", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDMM_ConfigureDCVoltage(
            IntPtr instrumentHandle,
            uint inputResistance);

        [DllImport(DLL_NAME, EntryPoint = "niVB_DMM_ConfigureDCCurrent", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDMM_ConfigureDCCurrent(
            IntPtr instrumentHandle,
            uint autoRangeTerminal);

        [DllImport(DLL_NAME, EntryPoint = "niVB_DMM_ConfigureACCurrent", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDMM_ConfigureACCurrent(
            IntPtr instrumentHandle,
            uint autoRangeTerminal);

        [DllImport(DLL_NAME, EntryPoint = "niVB_DMM_QueryDCVoltage", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDMM_QueryDCVoltage(
            IntPtr instrumentHandle,
            out uint inputResistance);

        [DllImport(DLL_NAME, EntryPoint = "niVB_DMM_QueryDCCurrent", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDMM_QueryDCCurrent(
            IntPtr instrumentHandle,
            out uint autoRangeTerminal);

        [DllImport(DLL_NAME, EntryPoint = "niVB_DMM_QueryACCurrent", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiDMM_QueryACCurrent(
            IntPtr instrumentHandle,
            out uint autoRangeTerminal);

        #endregion DLL Export
    }
}
