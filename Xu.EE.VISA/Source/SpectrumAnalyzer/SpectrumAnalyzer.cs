using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Xu.EE.Visa
{
    public class SpectrumAnalyzer : ViClient, ISpectrumAnalyzer
    {
        public SpectrumAnalyzer(string resourceName) : base(resourceName)
        {
            Reset();

            if (!Model.Contains("FSQ")) throw new Exception("Not an FSQ!");
        }

        public void SelectMode(FSQMode mode)
        {
            switch (mode)
            {
                case FSQMode.SpectrumAnalyzer: Write("INST:SEL SAN\n"); break;
                default: break;
            }
        }

        public void SelectScreen(FSQScreen sc)
        {
            switch (sc)
            {
                case FSQScreen.A: Write("DISP:WIND1:SEL\n"); break;
                case FSQScreen.B: Write("DISP:WIND2:SEL\n"); break;
                default: break;
            }
        }

        /// <summary>
        /// [:SENSe]:POWer[:RF]:ATTenuation:AUTO OFF|ON|0|1
        /// [:SENSe]:POWer[:RF]:ATTenuation:AUTO?
        /// </summary>
        public bool IsAutoAttenuation
        {
            get => Query("POW:RF:ATT:AUTO?\n").Trim() == "1";
            set
            {
                if (value)
                    Write("POW:RF:ATT:AUTO ON");
                else
                    Write("POW:RF:ATT:AUTO OFF");
            }
        }

        /// <summary>
        /// [:SENSe]:POWer[:RF]:ATTenuation <rel_ampl>
        /// [:SENSe]:POWer[:RF]:ATTenuation?
        /// </summary>
        public double InputAttenuation
        {
            get => GetNumber("POW:RF:ATT?");
            set => Write("POW:RF:ATT " + value.ToInt32() + "\n");
        }

        /// <summary>
        /// :DISPlay:WINDow[1]:TRACe:Y[:SCALe]:RLEVel <real>
        /// :DISPlay:WINDow[1]:TRACe:Y[:SCALe]:RLEVel?
        /// DISP:WIND1:TRAC:Y:RLEV?
        /// </summary>
        public double ReferenceLevel
        {
            get => GetNumber("DISP:WIND1:TRAC:Y:RLEV?");
            set => Write("DISP:WIND1:TRAC:Y:RLEV " + value.ToString("0.###") + "\n");
        }

        public double CenterFrequency
        {
            get => GetNumber("FREQ:CENT?\n");
            set => Write("FREQ:CENT " + value.ToString("0.#########") + "Hz\n");
        }

        public double OffsetFrequency
        {
            get => GetNumber("FREQ:OFFS?\n");
            set => Write("FREQ:OFFS " + value.ToString("0.#########") + "Hz\n");
        }

        public double StartFrequency
        {
            get => GetNumber("FREQ:STAR?\n");
            set => Write("FREQ:STAR " + value.ToString("0.#########") + "Hz\n");
        }

        public double StopFrequency
        {
            get => GetNumber("FREQ:STOP?\n");
            set => Write("FREQ:STOP " + value.ToString("0.#########") + "Hz\n");
        }

        public double SpanFrequency
        {
            get => GetNumber("FREQ:SPAN?\n");
            set => Write("FREQ:SPAN " + value.ToString("0.#########") + "Hz\n");
        }

        public void SetFullSpan() => Write("FREQ:SPAN:FULL");
        public void SetZeroSpan() => Write("FREQ:SPAN 0Hz");

        public bool IsAutoRBW
        {
            get => Query("BAND:AUTO?\n").Trim() == "1";
            set
            {
                if (value)
                    Write("BAND:AUTO ON");
                else
                    Write("BAND:AUTO OFF");
            }
        }

        /// <summary>
        /// BAND?
        /// BAND 10KHz
        /// </summary>
        public double RBW
        {
            get => GetNumber("BAND?\n");
            set => Write("BAND " + value.ToString("0.#########") + "Hz\n");
        }

        public bool IsAutoVBW
        {
            get => Query("BAND:VID:AUTO?\n").Trim() == "1";
            set
            {
                if (value)
                    Write("BAND:VID:AUTO ON");
                else
                    Write("BAND:VID:AUTO OFF");
            }
        }

        public double VBW
        {
            get => GetNumber("BAND:VID?\n");
            set => Write("BAND:VID " + value.ToString("0.#########") + "Hz\n");
        }

        public void SelectTrace(int num) => Write("DISP:WIND:TRAC" + num.ToString() + "\n");

        public IEnumerable<double> GetTraceData(int num = 1) => Query("TRAC? TRACE" + num.ToString() + "\n").Split(',').Select(n => n.ToDouble());

        public void GetTraceData(SpectrumTable st, int num = 1)
        {
            //SyncWait();
            while (!IsReady) { }

            double freq = StartFrequency;
            double stopFreq = StopFrequency;
            double delta = Math.Abs(stopFreq - freq);

            var list = GetTraceData(num).ToList();
            double space = delta / (list.Count - 2);

            //st.Status = TableStatus.Downloading;
            lock (st.DataLockObject)
            {
                st.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    SpectrumDatum sp = new(freq, list[i]);
                    st.Add(sp);

                    //Console.WriteLine(i + ": " + sp.Frequency + " | " + sp.Amplitude);

                    freq += space;
                }


            }
            st.Status = TableStatus.CalculateFinished;
        }

        public bool IsRefreshing
        {
            get => Query("INIT:CONT?\n").Trim() == "1";
            set
            {
                if (value)
                    Write("INIT:CONT ON");
                else
                    Write("INIT:CONT OFF");
            }
        }
    }
}
