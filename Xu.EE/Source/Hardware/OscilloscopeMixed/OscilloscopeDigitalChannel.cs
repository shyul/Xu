using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public abstract class OscilloscopeDigitalChannel : OscilloscopeChannel
    {
        public OscilloscopeDigitalChannel(string channelName, IOscilloscopeMixed device) : base(channelName, device)
        {
            DeviceMixed = device;
        }

        public IOscilloscopeMixed DeviceMixed { get; }

        public override void WriteSetting() => DeviceMixed.OscilloscopeDigital_WriteSetting(Name);

        public virtual double SampleRate { get; set; } = 1e9;
    }
}
