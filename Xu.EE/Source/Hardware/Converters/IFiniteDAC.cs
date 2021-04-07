using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface IFiniteDAC : IDataAcquisition, IDataConsumer
    {
        double SampleRate { get; set; }

        Range<double> Range { get; }

        List<double> Samples { get; set; }
    }
}
