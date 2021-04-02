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




}
