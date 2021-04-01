using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE.Visa
{
    public class Multimeter : ViClient, IMultimeter
    {
        public Multimeter(string resourceName) : base(resourceName)
        {


        }
    }
}
