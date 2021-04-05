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
        public Dictionary<string, PowerSupplyChannel> PowerSupplyChannels { get; } = new();

        public const string PowerSupplyP6VName = "ps/+6V";
        public const string PowerSupplyP25VName = "ps/+25V";
        public const string PowerSupplyN25VName = "ps/-25V";

        public PowerSupplyChannel PowerSupplyP6VChannel => PowerSupplyChannels[PowerSupplyP6VName];
        public PowerSupplyChannel PowerSupplyP25VChannel => PowerSupplyChannels[PowerSupplyP25VName];
        public PowerSupplyChannel PowerSupplyN25VChannel => PowerSupplyChannels[PowerSupplyN25VName];

        public void PowerSupply_ON(string channelName = "all")
        {
            Status = (NiVB_Status)NiPS_EnableAllOutputs(NiPS_Handle, true);
        }

        public void PowerSupply_OFF(string channelName = "all")
        {
            Status = (NiVB_Status)NiPS_EnableAllOutputs(NiPS_Handle, false);
        }

        public void PowerSupply_WriteSetting(string channelName)
        {
            PowerSupplyChannel psch = PowerSupplyChannels[channelName];

            if (psch.Mode == PowerSupplyMode.ConstantVoltage)
            {
                Status = (NiVB_Status)NiPS_ConfigureVoltageOutput(NiPS_Handle,
                    psch.Name,
                    psch.Voltage,
                    psch.Current);
            }
            else if (psch.Mode == PowerSupplyMode.ConstantCurrent)
            {
                Status = (NiVB_Status)NiPS_ConfigureCurrentOutput(NiPS_Handle,
                    psch.Name,
                    psch.Current,
                    psch.Voltage);
            }
        }

        public void PowerSupply_ReadSetting(string channelName)
        {
            PowerSupplyChannel psch = PowerSupplyChannels[channelName];

            if (psch.Mode == PowerSupplyMode.ConstantVoltage)
            {
                Status = (NiVB_Status)NiPS_QueryVoltageOutput(NiPS_Handle,
                    psch.Name,
                    out double voltage,
                    out double current);

                psch.Voltage = voltage;
                psch.Current = current;
            }
            else if (psch.Mode == PowerSupplyMode.ConstantCurrent)
            {
                Status = (NiVB_Status)NiPS_QueryCurrentOutput(NiPS_Handle,
                    psch.Name,
                    out double current,
                    out double voltage);

                psch.Voltage = voltage;
                psch.Current = current;
            }
        }

        public (double voltage, double current) PowerSupply_ReadOutput(string channelName)
        {
            PowerSupplyChannel psch = PowerSupplyChannels[channelName];
            Status = (NiVB_Status)NiPS_ReadOutput(NiPS_Handle,
                psch.Name,
                out double actualVoltageLevel,
                out double actualCurrentLevel,
                out uint state); // 0 = Constant Current, 1 = Constant Voltage

            psch.Mode = state == 1 ? PowerSupplyMode.ConstantVoltage : PowerSupplyMode.ConstantCurrent;

            return (actualVoltageLevel, actualCurrentLevel);
        }

        #region DLL Export

        private IntPtr NiPS_Handle;

        [DllImport(DLL_NAME, EntryPoint = "niVB_PS_Initialize", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiPS_Initialize(
            IntPtr libraryHandle,
            [MarshalAs(UnmanagedType.LPStr)] string deviceName,
            bool reset,
            out IntPtr instrumentHandle);

        [DllImport(DLL_NAME, EntryPoint = "niVB_PS_ResetInstrument", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiPS_ResetInstrument(IntPtr instrumentHandle);


        [DllImport(DLL_NAME, EntryPoint = "niVB_PS_Close", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiPS_Close(IntPtr instrumentHandle);


        [DllImport(DLL_NAME, EntryPoint = "niVB_PS_ConfigureVoltageOutput", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiPS_ConfigureVoltageOutput(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string channel,
            double voltageLevel,
            double currentLimit);

        [DllImport(DLL_NAME, EntryPoint = "niVB_PS_QueryVoltageOutput", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiPS_QueryVoltageOutput(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string channel,
            out double voltageLevel,
            out double currentLimit);


        [DllImport(DLL_NAME, EntryPoint = "niVB_PS_ConfigureCurrentOutput", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiPS_ConfigureCurrentOutput(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string channel,
            double currentLevel,
            double voltageLimit);

        [DllImport(DLL_NAME, EntryPoint = "niVB_PS_QueryCurrentOutput", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiPS_QueryCurrentOutput(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string channel,
            out double currentLevel,
            out double voltageLimit);


        [DllImport(DLL_NAME, EntryPoint = "niVB_PS_ReadOutput", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiPS_ReadOutput(
            IntPtr instrumentHandle,
            [MarshalAs(UnmanagedType.LPStr)] string channel,
            out double actualVoltageLevel,
            out double actualCurrentLevel,
            out uint state); // 0 = Constant Current, 1 = Constant Voltage

        [DllImport(DLL_NAME, EntryPoint = "niVB_PS_EnableAllOutputs", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NiPS_EnableAllOutputs(
            IntPtr instrumentHandle,
            bool enableOutputs);

        #endregion DLL Export
    }
}
