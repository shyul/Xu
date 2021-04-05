using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public class Pin
    {
        public string Name { get; }

        public bool Value { get; set; }
    }

    public class InputPin : Pin
    {
        public double Threshold { get; set; }
    }

    public class InputClock : InputPin
    {

    }

    public class ParallelPort 
    {
        public Dictionary<string, Pin> Pins { get; } = new Dictionary<string, Pin>();
    
    }
}
