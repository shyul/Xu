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
                Write("SOUR" + ch.ChannelNumber.ToString() + ":DATA:VOL:CLE");

                List<double> list = new List<double>() { 0, 0, 0, 0.8, -0.5, 1.25, -1.0, 1.5, -1.8, 1.1, -2.6, 1.1, -1.8, 1.5, -1.0, 1.25, -0.5, 0.8, 0, 0, 0 };
                double peak = list.Select(n => Math.Abs(n)).Max();
                var newList = list.Select(n => n / peak);
                string s = string.Join(", ", newList.ToArray());
                param["DATA:ARB"] = "XuEE, " + s;

                param["FUNC"] = "ARB";
                param["FUNC:ARB:FILT"] = "OFF";
                param["FUNC:ARB"] = "XuEE";
                param["FUNC:ARB:SRAT"] = "1200000";
                param["VOLT:OFFS"] = cfgArb.DcOffset.ToString("0.#####");
                param["VOLT"] = "3";

                // Please also turn off the filter!!
            }
            else if (config is FunctionGeneratorTriangleWaveConfig cfgTrian)
            {
                param["FUNC"] = "RAMP";
                param["FREQ"] = cfgTrian.Frequency.ToString("0.#####");
                param["VOLT"] = cfgTrian.Amplitude.ToString("0.#####");
                param["VOLT:OFFS"] = cfgTrian.DcOffset.ToString("0.#####");
                param["PHAS"] = cfgTrian.Phase.ToString("0.#####");
                param["FUNC:RAMP:SYMM"] = cfgTrian.DutyCycle.ToString("0.#####");
            }
            else if (config is FunctionGeneratorSquareWaveConfig cfgSquare)
            {
                //[SOURce[1|2]:]FREQuency:MODE {CW|LIST|SWEep|FIXed}
                //[SOURce[1|2]:]FREQuency:MODE?
                param["FUNC"] = "SQU";
                param["FREQ"] = cfgSquare.Frequency.ToString("0.#####");
                param["VOLT"] = cfgSquare.Amplitude.ToString("0.#####");
                param["VOLT:OFFS"] = cfgSquare.DcOffset.ToString("0.#####");
                param["PHAS"] = cfgSquare.Phase.ToString("0.#####");
                param["FUNC:SQU:DCYC"] = cfgSquare.DutyCycle.ToString("0.#####");
            }
            else if (config is FunctionGeneratorSineWaveConfig cfgSine)
            {
                param["FUNC"] = "SIN";
                param["FREQ"] = cfgSine.Frequency.ToString("0.#####"); // " SIN";
                param["VOLT"] = cfgSine.Amplitude.ToString("0.#####");
                param["VOLT:OFFS"] = cfgSine.DcOffset.ToString("0.#####");
                param["PHAS"] = cfgSine.Phase.ToString("0.#####");
            }
            else if (config is FunctionGeneratorDcConfig cfgDc)
            {
                param["FUNC"] = "DC";
                param["VOLT:OFFS"] = cfgDc.DcOffset.ToString("0.#####");
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

            switch (function)
            {
                case "SIN":
                    if (ch.Config is not FunctionGeneratorSineWaveConfig)
                        ch.Config = new FunctionGeneratorSineWaveConfig();

                    var cfgSine = ch.Config as FunctionGeneratorSineWaveConfig;
                    cfgSine.Frequency = Query("SOUR" + ch.ChannelNumber.ToString() + ":FREQ?").ToDouble();
                    cfgSine.Amplitude = Query("SOUR" + ch.ChannelNumber.ToString() + ":VOLT?").ToDouble();
                    cfgSine.DcOffset = Query("SOUR" + ch.ChannelNumber.ToString() + ":VOLT:OFFS?").ToDouble();
                    cfgSine.Phase = Query("SOUR" + ch.ChannelNumber.ToString() + ":PHAS?").ToDouble();
                    break;

                case "SQU":
                    if (ch.Config is not FunctionGeneratorSquareWaveConfig)
                        ch.Config = new FunctionGeneratorSquareWaveConfig();

                    var cfgSquare = ch.Config as FunctionGeneratorSquareWaveConfig;
                    cfgSquare.Frequency = Query("SOUR" + ch.ChannelNumber.ToString() + ":FREQ?").ToDouble();
                    cfgSquare.Amplitude = Query("SOUR" + ch.ChannelNumber.ToString() + ":VOLT?").ToDouble();
                    cfgSquare.DcOffset = Query("SOUR" + ch.ChannelNumber.ToString() + ":VOLT:OFFS?").ToDouble();
                    cfgSquare.Phase = Query("SOUR" + ch.ChannelNumber.ToString() + ":PHAS?").ToDouble();
                    cfgSquare.DutyCycle = Query("SOUR" + ch.ChannelNumber.ToString() + ":FUNC:SQU:DCYC?").ToDouble();
                    break;

                case "RAMP":
                    if (ch.Config is not FunctionGeneratorTriangleWaveConfig)
                        ch.Config = new FunctionGeneratorTriangleWaveConfig();
                    
                    var cfgTrian = ch.Config as FunctionGeneratorTriangleWaveConfig;
                    cfgTrian.Frequency = Query("SOUR" + ch.ChannelNumber.ToString() + ":FREQ?").ToDouble();
                    cfgTrian.Amplitude = Query("SOUR" + ch.ChannelNumber.ToString() + ":VOLT?").ToDouble();
                    cfgTrian.DcOffset = Query("SOUR" + ch.ChannelNumber.ToString() + ":VOLT:OFFS?").ToDouble();
                    cfgTrian.Phase = Query("SOUR" + ch.ChannelNumber.ToString() + ":PHAS?").ToDouble();
                    cfgTrian.DutyCycle = Query("SOUR" + ch.ChannelNumber.ToString() + ":FUNC:RAMP:SYMM?").ToDouble();
                    break;

                case "DC":
                    if (ch.Config is not FunctionGeneratorDcConfig)
                        ch.Config = new FunctionGeneratorDcConfig();

                    var cfgDc = ch.Config as FunctionGeneratorDcConfig;
                    cfgDc.DcOffset = Query("SOUR" + ch.ChannelNumber.ToString() + ":VOLT:OFFS?").ToDouble();
                    break;

                case "ARB":
                    if (ch.Config is not FunctionGeneratorArbitraryConfig)
                        ch.Config = new FunctionGeneratorArbitraryConfig(ch);

                    var cfgArb = ch.Config as FunctionGeneratorArbitraryConfig;
                    cfgArb.DcOffset = Query("SOUR" + ch.ChannelNumber.ToString() + ":VOLT:OFFS?").ToDouble();
                    break;

                default: throw new Exception("Unknown Function: " + function);
            }

        }


    }
}
