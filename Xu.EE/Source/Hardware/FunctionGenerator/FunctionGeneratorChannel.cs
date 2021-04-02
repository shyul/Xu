using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public abstract class FunctionGeneratorChannel : IOutputChannel
    {
        public FunctionGeneratorChannel(IFunctionGenerator fgen, int channelNum)
        {
            ChannelNumber = channelNum;
            FunctionGenerator = fgen;
        }

        public int ChannelNumber { get; }

        public void WriteSetting() { }

        public IFunctionGenerator FunctionGenerator { get; }




        public void ON() => FunctionGenerator.FGEN_ON(ChannelNumber);

        public void OFF() => FunctionGenerator.FGEN_OFF(ChannelNumber);







        public WaveFormType WaveFormType { get; set; }

        public double Amplitude { get; set; }

        public double DcOffset { get; set; }

        public double Frequency { get; set; }

        public double DutyCycle { get; set; }





        public bool IsArbitrary { get; set; }

        public double ArbitrarySampleRate { get; set; }

        public IEnumerable<double> ArbitraryWaveForm { get; set; }


    }
}
