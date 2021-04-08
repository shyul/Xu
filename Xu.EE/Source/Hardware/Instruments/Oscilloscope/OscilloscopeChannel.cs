using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public abstract class OscilloscopeChannel : ITriggerSource
    {
        protected OscilloscopeChannel(int chNum, string channelName, IOscilloscope device)
        {
            ChannelNumber = chNum;
            Name = channelName;
            Device = device;
        }

        public int ChannelNumber { get; }

        public string Name { get; }

        public bool Enabled { get; set; } = true;

        public IOscilloscope Device { get; }

        public abstract void WriteSetting();

        public TriggerEdge TriggerEdge { get; set; } = TriggerEdge.Rising;
    
    
    }
}
