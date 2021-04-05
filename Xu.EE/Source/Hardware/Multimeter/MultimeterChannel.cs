using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public class MultimeterChannel : IChannel
    {
        public MultimeterChannel(string channelName, IMultimeter dmm)
        {
            Name = channelName;
            Device = dmm;
        }

        public string Name { get; }

        public IMultimeter Device { get; }

        public MultimeterConfig Config { get; set; }

        public void WriteSetting() => Device.Multimeter_WriteSetting(Name);

        public double ReadOutput() => Device.Multimeter_Read(Name);
    }
}
