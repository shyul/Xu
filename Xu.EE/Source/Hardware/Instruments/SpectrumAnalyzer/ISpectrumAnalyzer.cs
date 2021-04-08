using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface ISpectrumAnalyzer
    {
        Dictionary<string, SpectrumAnalyzerChannel> SpectrumAnalyzerChannels { get; }

        ITriggerSource SpectrumAnalyzer_TriggerSource { get; set; }

        void SpectrumAnalyzer_WriteSetting();

        void SpectrumAnalyzer_WriteSetting(string channelName);

        void SpectrumAnalyzer_ReadSetting(string channelName);


    }
}
