using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface IFunctionGenerator : IHardwareResouce
    {
        Dictionary<string, FunctionGeneratorChannel> FunctionGeneratorChannels { get; }

        void FGEN_WriteSetting(string channelName);

        void FGEN_ON(string channelName);

        void FGEN_OFF(string channelName);
    }
}
