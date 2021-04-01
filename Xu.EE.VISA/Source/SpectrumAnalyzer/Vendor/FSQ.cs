using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Xu.EE.Visa
{
    public enum FSQMode
    {
        SpectrumAnalyzer,
    }

    public enum FSQScreen
    {
        A,
        B,
    }
    public class FSQ : SpectrumAnalyzer
    {
        public FSQ(string resourceName) : base(resourceName) 
        {
        
        
        }
    }
}
