using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface IOscilloscope : IHardwareResouce
    {
        // Define Trigger Channel

        Dictionary<string, OscilloscopeAnalogChannel> OscilloscopeAnalogChannels { get; }

        void OscilloscopeAnalog_WriteSetting(string channelName);


        OscilloscopeChannel DSO_TriggerSource { get; set; }

        double AnalogSampleRate { get; set; }


        void Oscilloscope_WriteSetting();

        void Oscilloscope_Run();









    }
}
