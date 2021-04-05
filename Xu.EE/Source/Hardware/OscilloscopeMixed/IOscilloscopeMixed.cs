using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface IOscilloscopeMixed : IOscilloscope
    {
        Dictionary<string, OscilloscopeDigitalChannel> OscilloscopeDigitalChannels { get; }

        void OscilloscopeDigital_WriteSetting(string channelName);


    }


}
