using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE.Visa
{
    public class FunctionGenerator : ViClient, IFunctionGenerator
    {
        public FunctionGenerator(string resourceName) : base(resourceName)
        {


        }

        ~FunctionGenerator()
        {
            Dispose();
        }

        public override void Open()
        {
            base.Open();
            FunctionGeneratorChannels[Channel1Name] = new FunctionGeneratorChannel(1, Channel1Name, this);
            FunctionGeneratorChannels[Channel2Name] = new FunctionGeneratorChannel(2, Channel2Name, this);
        }

        public Dictionary<string, FunctionGeneratorChannel> FunctionGeneratorChannels { get; } = new();

        public const string Channel1Name = "fgen1";
        public const string Channel2Name = "fgen2";

        public FunctionGeneratorChannel Channel1 => FunctionGeneratorChannels[Channel1Name];
        public FunctionGeneratorChannel Channel2 => FunctionGeneratorChannels[Channel2Name];

        public void FunctionGenerator_OFF(string channelName)
        {
            var ch = FunctionGeneratorChannels[channelName];
            Write("OUTP" + ch.ChannelNumber.ToString() + " OFF");
        }

        public void FunctionGenerator_ON(string channelName)
        {
            var ch = FunctionGeneratorChannels[channelName];
            Write("OUTP" + ch.ChannelNumber.ToString() + " ON");
        }

        public void FunctionGenerator_WriteSetting(string channelName)
        {
            var ch = FunctionGeneratorChannels[channelName];
            var config = ch.Config;
            Dictionary<string, string> param = new();

            if (config is FunctionGeneratorArbitraryConfig cfgArb)
            {



            }
            else if (config is FunctionGeneratorTriangleWaveConfig cfgTrian)
            {


            }
            else if (config is FunctionGeneratorSquareWaveConfig cfgSqure)
            {


            }
            else if (config is FunctionGeneratorSineWaveConfig cfgSine)
            {
                param["FUNC"] = "SIN";
                param["FREQ"] = cfgSine.Frequency.ToString("0.#####"); // " SIN";
                param["VOLT"] = cfgSine.Amplitude.ToString("0.#####");
                param["VOLT:OFFS"] = cfgSine.DcOffset.ToString("0.#####");

                if (cfgSine is FunctionGeneratorSineWavePhaseConfig cfgPh)
                    param["PHAS"] = cfgPh.Phase.ToString("0.#####");
            }
            else if (config is FunctionGeneratorDcConfig cfgDc)
            {


            }
            else
            {
                throw new Exception("Unsupported WaveFormType: " + config.GetType().FullName);
            };

            Write("SOUR" + ch.ChannelNumber.ToString(), param);
        }

        public void FunctionGenerator_ReadSetting(string channelName)
        {
            var ch = FunctionGeneratorChannels[channelName];

            string function = Query("SOUR" + ch.ChannelNumber.ToString() + ":FUNC?").Trim();
            Dictionary<string, string> param = new();
            switch (function)
            {
                case "SIN":
                    if (ch.Config is not FunctionGeneratorSineWavePhaseConfig)
                    {
                        ch.Config = new FunctionGeneratorSineWavePhaseConfig();
                    }

                    var cfgSine = ch.Config as FunctionGeneratorSineWavePhaseConfig;
                    cfgSine.Frequency = Query("SOUR" + ch.ChannelNumber.ToString() + ":FREQ?").ToDouble();
                    cfgSine.Amplitude = Query("SOUR" + ch.ChannelNumber.ToString() + ":VOLT?").ToDouble();
                    cfgSine.DcOffset = Query("SOUR" + ch.ChannelNumber.ToString() + ":VOLT:OFFS?").ToDouble();
                    cfgSine.Phase = Query("SOUR" + ch.ChannelNumber.ToString() + ":PHAS?").ToDouble();

                    break;

                case "SQU":
                    if (ch.Config is not FunctionGeneratorSquareWaveConfig)
                    {
                        ch.Config = new FunctionGeneratorSquareWaveConfig();
                    }
                    var config1 = ch.Config as FunctionGeneratorSquareWaveConfig;

                    break;

                case "RAMP":
                    if (ch.Config is not FunctionGeneratorTriangleWaveConfig)
                    {
                        ch.Config = new FunctionGeneratorTriangleWaveConfig();
                    }
                    var config2 = ch.Config as FunctionGeneratorTriangleWaveConfig;

                    break;

                case "DC":
                    if (ch.Config is not FunctionGeneratorDcConfig)
                    {
                        ch.Config = new FunctionGeneratorDcConfig();
                    }

                    var config3 = ch.Config as FunctionGeneratorDcConfig;

                    break;

                default: throw new Exception("Unknown Function: " + function);
            }

        }


    }
}
