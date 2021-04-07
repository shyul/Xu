using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface IADC : IDataAcquisition, IDataProvider
    {
        double SampleRate { get; set; }

        Range<double> Range { get; }

        double BandWidthLimit { get; }

        List<double> Samples { get; set; }
    }

    public interface IDataAcquisition : IPort
    {
        void Start();


        // Shall I assign this to ITriggerSource?
        bool IsReady { get; }
    }
}
