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

    public class FunctionGeneratorArbitraryConfig : FunctionGeneratorConfig, IFiniteDAC
    {
        public FunctionGeneratorArbitraryConfig(FunctionGeneratorChannel channel)
        {
            Channel = channel;
        }

        public FunctionGeneratorChannel Channel { get; }

        public double Gain { get; set; } = 1;

        public double SampleRate { get; set; } = 100e6;

        public Range<double> Range { get; } = new Range<double>(-5, 5);

        public List<double> Samples { get; set; }



        public bool IsReady => false;

        public string Name => Channel.Name;

        public bool Enabled => Channel.Enabled && Channel.Config == this;

        public void DataIsUpdated(IDataProvider provider)
        {

        }

        public void Dispose()
        {

        }

        public void Start()
        {

            Channel.Enabled = true;
        }


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
