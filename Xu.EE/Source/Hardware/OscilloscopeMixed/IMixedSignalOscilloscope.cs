using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface IMixedSignalOscilloscope : IOscilloscope
    {
        Dictionary<string, OscilloscopeDigitalChannel> OscilloscopeDigitalChannels { get; }

        void OscilloscopeDigital_WriteSetting(string channelName);

        double DigitalSampleRate { get; set; }
    }


}
