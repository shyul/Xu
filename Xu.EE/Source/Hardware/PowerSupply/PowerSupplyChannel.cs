using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public class PowerSupplyChannel : IOutputChannel
    {
        public PowerSupplyChannel(string channelName, IPowerSupply powerSupply, Range<double> voltageRange, Range<double> currentRange)
        {
            ChannelName = channelName;
            PowerSupply = powerSupply;
            VoltageRange = voltageRange;
            CurrentRange = currentRange;
        }


        public string ChannelName { get; }

        public void WriteSetting() => PowerSupply.PowerSupply_WriteSetting(ChannelName);

        public void ReadSetting() => PowerSupply.PowerSupply_ReadSetting(ChannelName);

        public (double voltage, double current) ReadOutput() => PowerSupply.PowerSupply_ReadOutput(ChannelName);

        public IPowerSupply PowerSupply { get; }

        public void ON() => PowerSupply.PowerSupply_ON(ChannelName);

        public void OFF() => PowerSupply.PowerSupply_OFF(ChannelName);

        public PowerSupplyMode Mode { get; set; } = PowerSupplyMode.ConstantVoltage;

        public double Voltage
        {
            get => m_Voltage;

            set
            {
                double v = value;
                if (v > VoltageRange.Max) v = VoltageRange.Max;
                else if (v < VoltageRange.Min) v = VoltageRange.Min;
                m_Voltage = v;
            }
        }

        private double m_Voltage;

        public double Current
        {
            get => m_Current;

            set
            {
                double i = value;
                if (i > CurrentRange.Max) i = CurrentRange.Max;
                else if (i < CurrentRange.Min) i = CurrentRange.Min;
                m_Current = i;
            }
        }

        public double m_Current;

        public Range<double> VoltageRange { get; }

        public Range<double> CurrentRange { get; }

    }

    public enum PowerSupplyMode : int
    {
        ConstantCurrent,
        ConstantVoltage
    }
}
