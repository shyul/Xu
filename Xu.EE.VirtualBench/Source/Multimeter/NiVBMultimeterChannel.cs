using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Xu.EE.VirtualBench
{
    public class NiVBMultimeterChannel : MultimeterChannel
    {
        public NiVBMultimeterChannel(string channelName, IMultimeter dmm) : base(channelName, dmm)
        {

        }

    }
}
