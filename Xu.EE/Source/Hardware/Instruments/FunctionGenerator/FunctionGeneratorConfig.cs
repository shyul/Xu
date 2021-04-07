using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public abstract class FunctionGeneratorConfig
    {
        public double DcOffset { get; set; } = 0;
    }

    public class FunctionGeneratorArbitraryConfig : FunctionGeneratorConfig
    {
        public double Gain { get; set; } = 1;

        public double SampleRate { get; set; } = 100e6;

        public List<double> Waveform { get; set; }
    }

    public class FunctionGeneratorDcConfig : FunctionGeneratorConfig
    {

    }

    public class FunctionGeneratorSineWaveConfig : FunctionGeneratorDcConfig
    {
        public double Amplitude { get; set; } = 1;

        public double Frequency { get; set; } = 5e5;
    }

    public class FunctionGeneratorSquareWaveConfig : FunctionGeneratorSineWaveConfig
    {
        public double DutyCycle { get; set; } = 50;
    }

    public class FunctionGeneratorTriangleWaveConfig : FunctionGeneratorSquareWaveConfig
    {

    }
}
