using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public class OscilloscopeAnalogChannel : OscilloscopeChannel, IFiniteADC
    {
        public OscilloscopeAnalogChannel(string channelName, IOscilloscope device) : base(channelName, device)
        {

        }

        public override void WriteSetting() => Device.OscilloscopeAnalog_WriteSetting(Name);

        public void Start() => Device.Oscilloscope_Run();



        public DateTime UpdateTime => throw new NotImplementedException();

        public bool AddDataConsumer(IDataConsumer idk)
        {
            throw new NotImplementedException();
        }

        public bool RemoveDataConsumer(IDataConsumer idk)
        {
            throw new NotImplementedException();
        }



        public bool IsReady => throw new NotImplementedException();


        public AnalogCoupling Coupling { get; set; } = AnalogCoupling.DC;

        public double VerticalRange
        {
            get => Range.Maximum - Range.Minimum;
            set
            {
                double offset = VerticalOffset;
                double halfRange = value / 2;
                Range.Set(offset - halfRange, offset + halfRange);
            }
        }

        public double VerticalOffset 
        {
            get => (Range.Maximum + Range.Minimum) / 2;

            set
            {
                double halfRange = VerticalRange / 2;
                Range.Set(value - halfRange, value + halfRange);
            }
        }

        public Range<double> Range { get; } = new(-5, 5);

        public double ProbeAttenuation { get; set; } = 1;

        public double TriggerLevel { get; set; }

        public double TriggerHysteresis { get; set; }

        public virtual double SampleRate { get; set; } = 500e6;

        public double InputImpedance { get; set; } = 1e6;

        public double BandWidthLimit { get; set; } = 10e9;

        public List<double> Samples { get; set; }


    }
}
