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
            ChannelName = channelName;
            Multimeter = dmm;
        }

        public string ChannelName { get; }

        public IMultimeter Multimeter { get; }

        public MultimeterConfig Config { get; set; }

        public void WriteSetting() => Multimeter.Multimeter_WriteSetting(ChannelName);

        public double ReadOutput() => Multimeter.Multimeter_Read(ChannelName);
    }
}
