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

        public Dictionary<string, FunctionGeneratorChannel> FunctionGeneratorChannels => throw new NotImplementedException();

        public string ResouceName => throw new NotImplementedException();

        public void FunctionGenerator_OFF(string channelName)
        {
            throw new NotImplementedException();
        }

        public void FunctionGenerator_ON(string channelName)
        {
            throw new NotImplementedException();
        }

        public void FunctionGenerator_WriteSetting(string channelName)
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }
    }
}
