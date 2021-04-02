using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface IOscilloscope : IHardwareResouce
    {
        // Define Trigger Channel




        void DSO_Run();

        string DSO_TriggerSource { get; set; }

        double DSO_TriggerLevel { get; set; }

        double DSO_TriggerHysteresis { get; set; }







    }
}
