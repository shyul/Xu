using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE.Visa
{
    public class PowerSupply : ViClient, IPowerSupply
    {
        public PowerSupply(string resourceName) : base(resourceName)
        {


        }
    }
}
