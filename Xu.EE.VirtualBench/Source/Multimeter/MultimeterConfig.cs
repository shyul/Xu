using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Xu.EE.VirtualBench
{
    public enum NiVB_DMM_InputResistance : uint
    {
        Res_10MOhm = 0,
        Res_10GOhm = 1,
    }

    public enum NiVB_DMM_CurrentTerminal : uint 
    {
        Low = 0,
        High = 1,
    }

    public class MultimeterDcVoltageConfigNiVB : MultimeterDcVoltageConfig
    {
        public NiVB_DMM_InputResistance InputResistance { get; set; } = NiVB_DMM_InputResistance.Res_10MOhm;
    }

    public class MultimeterDcCurrentConfigNiVB : MultimeterDcCurrentConfig
    {
        public NiVB_DMM_CurrentTerminal Terminal { get; set; } = NiVB_DMM_CurrentTerminal.Low;
    }

    public class MultimeterAcCurrentConfigNiVB : MultimeterAcCurrentConfig
    {
        public NiVB_DMM_CurrentTerminal Terminal { get; set; } = NiVB_DMM_CurrentTerminal.Low;
    }
}
