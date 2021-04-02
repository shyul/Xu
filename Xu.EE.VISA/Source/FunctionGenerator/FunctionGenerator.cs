using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE.Visa
{
    public class FunctionGenerator : ViClient, IFunctionGenerator
    {
        public FunctionGenerator(string resourceName) : base(resourceName)
        {


        }

        public int FGEN_MaximumChannelNumber => throw new NotImplementedException();

        public WaveFormType FGEN_WaveFormType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double FGEN_Amplitude { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double FGEN_DcOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double FGEN_Frequency { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double FGEN_DutyCycle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string ResouceName => throw new NotImplementedException();



        public void FGEN_OFF(int ch_num)
        {
            throw new NotImplementedException();
        }

        public void FGEN_ON(int ch_num)
        {
            throw new NotImplementedException();
        }

        public void FGEN_WriteSetting(int ch_num)
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }
    }
}
