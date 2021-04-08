using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Xu.EE.Visa
{
    public class MSOX : Oscilloscope, IFunctionGenerator
    {
        public MSOX(string resourceName) : base(resourceName)
        {
            //Reset();
        }

        public override void Open()
        {
            base.Open();

        }

        public override void Close()
        {
            // Remove all dependables, data sources and sinks as well;
            OscilloscopeAnalogChannels.Clear();
            FunctionGeneratorChannels.Clear();
            base.Close();
        }

        public Dictionary<string, FunctionGeneratorChannel> FunctionGeneratorChannels { get; } = new();

        public void FunctionGenerator_OFF(string channelName)
        {
            // 31 :WGEN Commands
        }

        public void FunctionGenerator_ON(string channelName)
        {

        }

        public void FunctionGenerator_ReadSetting(string channelName)
        {

        }

        public void FunctionGenerator_WriteSetting(string channelName)
        {

        }
    }
}
