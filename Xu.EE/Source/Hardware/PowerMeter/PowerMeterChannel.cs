using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public class PowerMeterChannel : IChannel
    {
        public string Name { get; }

        public IPowerMeter Device { get; }

        public void WriteSetting() => Device.PowerMeter_WriteSetting(Name);
    }
}
