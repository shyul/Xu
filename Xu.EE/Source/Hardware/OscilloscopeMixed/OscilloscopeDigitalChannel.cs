using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public abstract class OscilloscopeDigitalChannel : OscilloscopeChannel
    {
        public OscilloscopeDigitalChannel(string channelName, IOscilloscope device) : base(channelName, device)
        {

        }


    }
}
