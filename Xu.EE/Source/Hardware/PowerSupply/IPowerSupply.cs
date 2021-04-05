using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface IPowerSupply
    {
        Dictionary<string, PowerSupplyChannel> PowerSupplyChannels { get; }

        void PowerSupply_WriteSetting(string channelName);

        void PowerSupply_ReadSetting(string channelName);

        (double voltage, double current) PowerSupply_ReadOutput(string channelName);

        void PowerSupply_ON(string channelName);

        void PowerSupply_OFF(string channelName);
    }


}
