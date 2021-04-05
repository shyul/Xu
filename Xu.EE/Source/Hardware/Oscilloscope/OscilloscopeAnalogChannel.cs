using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public class OscilloscopeAnalogChannel : OscilloscopeChannel
    {
        public OscilloscopeAnalogChannel(string channelName, IOscilloscope device) : base(channelName, device)
        {

        }

        public override void WriteSetting() => Device.OscilloscopeAnalog_WriteSetting(Name);

        public bool Enabled { get; set; } = true;

        public AnalogCoupling Coupling { get; } = AnalogCoupling.DC;

        public double VerticalRange { get; set; } = 10;

        public double VerticalOffset { get; set; } = 0;

        public double ProbeAttenuation { get; set; } = 1;

        public double TriggerLevel { get; set; }

        public double TriggerHysteresis { get; set; }

        public virtual double SampleRate { get; set; } = 500e6;




        public List<double> Result { get; set; }
    }
}
