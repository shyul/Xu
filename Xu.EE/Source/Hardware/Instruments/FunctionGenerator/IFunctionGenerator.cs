using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface IFunctionGenerator : IHardwareDevice
    {
        Dictionary<string, FunctionGeneratorChannel> FunctionGeneratorChannels { get; }

        void FunctionGenerator_WriteSetting(string channelName);

        void FunctionGenerator_ReadSetting(string channelName);

        void FunctionGenerator_ON(string channelName);

        void FunctionGenerator_OFF(string channelName);
    }
}
