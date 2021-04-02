using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface IPowerSupply
    {
        void PWR_ON(int ch_num);

        void PWR_OFF(int ch_num);
    }


}
