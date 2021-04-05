using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public enum MultimeterMode : uint
    {
        DCVolt = 0,
        ACVolt = 1,
        DCCurrent = 2,
        ACCurrent = 3,
        Resistance = 4,
        Diode = 5,
        Capacitance = 6,
        Temperature = 7
    }
}
