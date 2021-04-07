using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface IOscilloscope : IHardwareDevice
    {
        Dictionary<string, OscilloscopeAnalogChannel> OscilloscopeAnalogChannels { get; }

        ITriggerSource Oscilloscope_TriggerSource { get; set; }

        void Oscilloscope_WriteSetting();

        void OscilloscopeAnalog_WriteSetting(string channelName);

        void OscilloscopeAnalog_ReadSetting(string channelName);

        void Oscilloscope_Run();
    }
}
