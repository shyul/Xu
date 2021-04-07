using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface IReceiver : IPort
    {
        void StartReceive();

        bool IsReceiving { get; }

        double SampleRate { get; set; }

        List<double> Samples { get; set; }
    }
}
