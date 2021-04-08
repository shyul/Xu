using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public abstract class OscilloscopeAnalogProbe
    {

        public virtual string ModelName { get; set; }
        
        public virtual double Attenuation { get; } = 1;

        public virtual string InputConfig { get; }
    }
}
