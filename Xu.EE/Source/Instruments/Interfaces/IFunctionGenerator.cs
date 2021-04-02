using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface IFunctionGenerator : IHardwareResouce
    {
        int FGEN_MaximumChannelNumber { get; }

        void FGEN_WriteSetting(int ch_num);

        void FGEN_ON(int ch_num);

        void FGEN_OFF(int ch_num);

        WaveFormType FGEN_WaveFormType { get; set; }

        double FGEN_Amplitude { get; set; }

        double FGEN_DcOffset { get; set; }

        double FGEN_Frequency { get; set; }

        double FGEN_DutyCycle { get; set; }
    }

    public enum WaveFormType : int
    {
        DC,
        Sine,
        Square,
        Triangle,

        Unknown = 0,
    }

    public abstract class FunctionGeneratorChannel 
    {
        public FunctionGeneratorChannel(IFunctionGenerator fgen, int channelNum)
        {
            ChannelNumber = channelNum;
            FunctionGenerator = fgen;
        }

        public int ChannelNumber { get; }

        public IFunctionGenerator FunctionGenerator { get; }

        public void ON() => FunctionGenerator.FGEN_ON(ChannelNumber);

        public void OFF() => FunctionGenerator.FGEN_OFF(ChannelNumber);




        public WaveFormType FGEN_WaveFormType { get; set; }

        public double FGEN_Amplitude { get; set; }

        public double FGEN_DcOffset { get; set; }

        public double FGEN_Frequency { get; set; }

        public double FGEN_DutyCycle { get; set; }
    }
}
