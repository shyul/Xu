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

        public AnalogCoupling Coupling { get; set; } = AnalogCoupling.DC;

        public double VerticalRange { get; set; } = 10;

        public double VerticalOffset { get; set; } = 0;

        public double ProbeAttenuation { get; set; } = 1;

        public double TriggerLevel { get; set; }

        public double TriggerHysteresis { get; set; }

        public virtual double SampleRate { get; set; } = 500e6;

        public double InputImpedance { get; set; } = 1e6;

        public double BandWidthLimit { get; set; } = 10e9;


        public List<double> Result { get; set; }
    }
}
