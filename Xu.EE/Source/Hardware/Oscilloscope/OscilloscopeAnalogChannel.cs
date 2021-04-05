using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public abstract class OscilloscopeChannel : IChannel
    {
        protected OscilloscopeChannel(string channelName, IOscilloscope device)
        {
            Name = channelName;
            Oscilloscope = device;
        }

        public string Name { get; }

        public IOscilloscope Oscilloscope { get; }

        public abstract void WriteSetting();

    }


        public class OscilloscopeAnalogChannel : OscilloscopeChannel
    {
        public OscilloscopeAnalogChannel(string channelName, IOscilloscope device) : base(channelName, device)
        {

        }



        public override void WriteSetting() => Oscilloscope.OscilloscopeAnalog_WriteSetting(Name);

        public bool Enabled { get; set; } = true;

        public AnalogCoupling Coupling { get; } = AnalogCoupling.DC;

        public double VerticalRange { get; set; } = 10;

        public double VerticalOffset { get; set; } = 0;

        public double ProbeAttenuation { get; set; } = 1;




        public TriggerEdge TriggerEdge { get; set; } = TriggerEdge.Rising;

        public double TriggerLevel { get; set; }

        public double TriggerHysteresis { get; set; }







        public IEnumerable<double> Result { get; set; }


    }
}
