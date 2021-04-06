using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public class PowerMeterChannel : IPort
    {
        public string Name { get; }

        public bool Enabled { get; set; } = true;

        public IPowerMeter Device { get; }

        public void WriteSetting() => Device.PowerMeter_WriteSetting(Name);
    }
}
