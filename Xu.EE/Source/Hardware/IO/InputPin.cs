using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public class Pin : IDigitalPin
    {
        public string Name { get; }

        public bool Enabled { get; set; }

        public bool Value { get; set; }

        public double Threshold { get; set; }
    }
}
