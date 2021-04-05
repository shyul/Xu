using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    class NiVBOscilloscopeAnalogChannel : OscilloscopeAnalogChannel
    {
        public NiVBOscilloscopeAnalogChannel(string channelName, IOscilloscope device) : base(channelName, device)
        {

        }
    }
}
