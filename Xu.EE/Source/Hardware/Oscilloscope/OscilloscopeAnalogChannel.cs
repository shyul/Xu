using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public abstract class OscilloscopeAnalogChannel : IChannel
    {
        public string ChannelName { get; }

        public void WriteSetting() { }



        public IOscilloscope Oscilloscope { get; }

        public bool Enabled { get; set; }

      

        public double VerticalRange { get; set; }

        public double VerticalOffset { get; set; }



        public double SampleRate { get; set; }

        public IEnumerable<double> Result { get; set; }


    }
}
