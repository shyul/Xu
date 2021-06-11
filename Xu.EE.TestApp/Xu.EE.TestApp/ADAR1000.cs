using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAR1000
{
    public class BeamPositionVectorModulator 
    {
        public bool QPolarity { get; set; }

        public int QGain { get; set; }

        public Dictionary<double, (bool i_pol, int i_gain, bool q_pol, int q_gain)> LookupTable { get; } = new Dictionary<double, (bool i_pol, int i_gain, bool q_pol, int q_gain)>()
        {
            { 0, (true, 0x3F, true, 0x20) },
            { 2.8125, (true, 0x3F, true, 0x21) },
            { 5.625, (true, 0x3F, true, 0x20) },
            { 8.4375, (true, 0x3F, true, 0x20) },
            { 11.25, (true, 0x3F, true, 0x20) },
            { 14.0625, (true, 0x3F, true, 0x20) },
        };

    }
}
