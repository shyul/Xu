using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public abstract class MultimeterConfig
    {
        public bool IsAutoRange { get; set; } = true;

        public double Range { get; set; } = 10;
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

    public class MultimeterDiodeConfig : MultimeterConfig
    {

    }

    public class MultimeterCapacitanceConfig : MultimeterConfig
    {

    }

    public class MultimeterTemperatureConfig : MultimeterConfig
    {

    }
}
