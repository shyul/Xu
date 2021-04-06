using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public class MultimeterChannel : IAnalogPort
    {
        public MultimeterChannel(string channelName, IMultimeter dmm)
        {
            Name = channelName;
            Device = dmm;
        }

        public string Name { get; }

        public bool Enabled { get; set; } = true;

        public IMultimeter Device { get; }

        public MultimeterConfig Config { get; set; }

        public void WriteSetting() => Device.Multimeter_WriteSetting(Name);

        public double Value
        {
            get => Device.Multimeter_Read(Name);

            set { }
        }
    }
}
