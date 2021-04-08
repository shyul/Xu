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

            //:WGEN:OUTPut 1
        }

        public void FunctionGenerator_ON(string channelName)
        {
            //:WGEN:OUTPut 0
        }

        public void FunctionGenerator_ReadSetting(string channelName)
        {

        }

        public void FunctionGenerator_WriteSetting(string channelName)
        {
            var ch = FunctionGeneratorChannels[channelName];

            Dictionary<string, string> param = new();
            param["FREQ"] = "0";
            param["FUNC"] = "SIN"; // SQU,, RAMP, PULS, NOIS, DC
            param["FUNC:PULS:WIDT"] = "0";
            param["FUNC:RAMP:SYMM"] = "0";
            param["FUNC:SQU:DCYC"] = "0";
            param["MOD:AM:DEPT"] = "0";
            param["MOD:AM:FREQ"] = "0";
            param["MOD:FM:DEV"] = "0";

            param["MOD:FM:FREQ"] = "0";
            param["MOD:FSK:FREQ"] = "0";

            // MOD:FSK:RATE
            // MOD:FUNC | SIN, SQU, RAMP
            // MOD:FUNC:RAMP:SYMM
            // MOD:NOIS
            // MOD:STAT 0, 1
            // MOD:TYPE | AM, FM, FSK

            // OUTP:LOAD | ONEM | FIFT
            // PER | NR3 format
            // 
            // RST
            // VOLT
            // VOLT:HIGH
            // VOLT:LOW
            // VOLT:OFFS


            Write("WGEN" + ch.ChannelNumber.ToString(), param);
        }
    }
}
