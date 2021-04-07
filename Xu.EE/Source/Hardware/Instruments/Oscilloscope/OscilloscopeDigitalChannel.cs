using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public class OscilloscopeDigitalChannel : OscilloscopeChannel, ISerialDataInput
    {
        public OscilloscopeDigitalChannel(string channelName, IOscilloscope device) : base(channelName, device)
        {

        }

        public List<bool> Data { get; set; }

        public bool Value { get => Data is not null ? Data.Last() : false; set { } }

        public double Threshold => throw new NotImplementedException();

        public override void WriteSetting()
        {

        }
    }
}
