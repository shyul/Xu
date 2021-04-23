using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Xu.EE
{
    public enum PinType : int
    {
        Passive = 0,
        Input = 1,
        Output = 2,
        IO = 3,
        Power = 4,
    }

    public class SchematicPin
    {
        public string Designator { get; set; }

        public string Name { get; set; }

        public bool IsClock { get; set; } = false;

        public bool IsLowActive { get; set; } = false;

        public PinType Type { get; set; } = PinType.Passive;
    }

    public static class Schematic
    {

    }
}
