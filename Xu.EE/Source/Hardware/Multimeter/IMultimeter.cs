using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface IMultimeter
    {
        Dictionary<string, MultimeterChannel> MultimeterChannels { get; }

        void Multimeter_WriteSetting(string channelName);

        void Multimeter_ReadSetting(string channelName);

        double Multimeter_Read(string channelName);
    }
}
