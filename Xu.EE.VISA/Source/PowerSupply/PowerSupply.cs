using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE.Visa
{
    public class PowerSupply : ViClient, IPowerSupply
    {
        public PowerSupply(string resourceName) : base(resourceName)
        {


        }

        public Dictionary<string, PowerSupplyChannel> PowerSupplyChannels { get; } = new();

        public void PowerSupply_ON(string channelName = "all")
        {

        }

        public void PowerSupply_OFF(string channelName = "all")
        {

        }

        public bool PowerSupply_Enabled(string channelName = "all")
        {
            return false;
        }

        public void PowerSupply_WriteSetting(string channelName)
        {

        }

        public void PowerSupply_ReadSetting(string channelName)
        {

        }

        public (double voltage, double current) PowerSupply_ReadOutput(string channelName)
        {
            return (0, 0);
        }
    }
}
