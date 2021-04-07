using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public abstract class OscilloscopeChannel : ITriggerSource
    {
        protected OscilloscopeChannel(string channelName, IOscilloscope device)
        {
            Name = channelName;
            Device = device;
        }

        public string Name { get; }

        public bool Enabled { get; set; } = true;

        public IOscilloscope Device { get; }

        public abstract void WriteSetting();

        public TriggerEdge TriggerEdge { get; set; } = TriggerEdge.Rising;
    
    
    }
}
