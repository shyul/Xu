using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface IPowerSupply
    {
        void PWR_ON(string channelName);

        void PWR_OFF(string channelName);
    }


}
