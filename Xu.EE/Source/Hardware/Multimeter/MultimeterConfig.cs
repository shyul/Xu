using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public abstract class MultimeterConfig
    {
        public bool IsAutoRange { get; set; }

        public double Range { get; set; }
    }

    public class MultimeterDcVoltageConfig : MultimeterConfig
    {

    }

    public class MultimeterAcVoltageConfig : MultimeterConfig
    {

    }

    public class MultimeterDcCurrentConfig : MultimeterConfig
    {

    }

    public class MultimeterAcCurrentConfig : MultimeterConfig
    {

    }

    public class MultimeterResistanceConfig : MultimeterConfig
    {

    }

    public class MultimeterCapacitanceConfig : MultimeterConfig
    {

    }
}
