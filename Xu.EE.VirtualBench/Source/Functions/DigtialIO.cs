using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE.VirtualBench
{
    public partial class NiVB : ISpiMaster
    {
        public Dictionary<string, Pin> DigitalIoPins { get; } = new();

        #region DLL Export

        private IntPtr NiDIO_Handle;

        #endregion DLL Export
    }
}
