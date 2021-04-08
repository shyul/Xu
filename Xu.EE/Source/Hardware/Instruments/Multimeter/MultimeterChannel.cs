using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public class MultimeterChannel : IAnalogPin
    {
        public MultimeterChannel(int chNum, string channelName, IMultimeter dmm)
        {
            ChannelNumber = chNum;
            Name = channelName;
            Device = dmm;
        }

        public int ChannelNumber { get; }

        public string Name { get; }

        public bool Enabled { get; set; } = true;

        public IMultimeter Device { get; }

        public Range<double> Range => new(0, 10);

        public bool IsAutoRange { get; set; } = true;

        public MultimeterConfig Config { get; set; }

        public void WriteSetting() => Device.Multimeter_WriteSetting(Name);

        public void ReadSetting() => Device.Multimeter_ReadSetting(Name);

        public double Value
        {
            get => Device.Multimeter_Read(Name);

            set { }
        }
    }
}
