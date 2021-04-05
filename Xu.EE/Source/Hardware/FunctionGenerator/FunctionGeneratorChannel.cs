using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public class FunctionGeneratorChannel : IOutputChannel
    {
        public FunctionGeneratorChannel(string channelName, IFunctionGenerator fgen)
        {
            Name = channelName;
            FunctionGenerator = fgen;
        }

        public string Name { get; }

        public IFunctionGenerator FunctionGenerator { get; }

        public void WriteSetting() => FunctionGenerator.FunctionGenerator_WriteSetting(Name);

        public void ON() => FunctionGenerator.FunctionGenerator_ON(Name);

        public void OFF() => FunctionGenerator.FunctionGenerator_OFF(Name);




        public WaveFormType WaveFormType { get; set; } = WaveFormType.Triangle;

        public double Amplitude { get; set; }

        public double DcOffset { get; set; }

        public double Frequency { get; set; }

        public double DutyCycle { get; set; }





        public bool IsArbitrary { get; set; } = false;

        public double ArbitrarySampleRate { get; set; }

        public IEnumerable<double> ArbitraryWaveForm { get; set; }


    }
}
