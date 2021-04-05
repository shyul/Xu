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

        public void PWR_OFF(string channelName)
        {
            throw new NotImplementedException();
        }

        public void PWR_ON(string channelName)
        {
            throw new NotImplementedException();
        }
    }
}
