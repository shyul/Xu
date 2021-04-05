using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface IPowerMeter
    {
        Dictionary<string, PowerMeterChannel> PowerMeterChannels { get; }

        void PowerMeter_WriteSetting(string channelName);

        void PowerMeter_ReadSetting(string channelName);
    }
}
