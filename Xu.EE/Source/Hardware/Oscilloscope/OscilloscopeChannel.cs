using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public abstract class OscilloscopeChannel : IChannel
    {
        protected OscilloscopeChannel(string channelName, IOscilloscope device)
        {
            Name = channelName;
            Device = device;
        }

        public string Name { get; }

        public IOscilloscope Device { get; }

        public abstract void WriteSetting();

        public TriggerEdge TriggerEdge { get; set; } = TriggerEdge.Rising;
    }
}
