using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFSQ
{
    public class SigGen : ViClient
    {
        public SigGen(string resourceName) : base(resourceName)
        {
            Reset();
            RFOutputEnable = false;
            ModulationEnable = false;
        }

        public override void Dispose()
        {
            RFOutputEnable = false;
            ModulationEnable = false;
            Session?.Dispose();
        }

        public double Frequency
        {
            get => GetNumber("FREQ?\n");
            set => Write("FREQ " + value.ToString("0.#########") + "Hz\n");
        }

        public double Power
        {
            get => GetNumber("POWER?\n");
            set => Write("POWER " + value.ToString("0.#########") + "DBM\n");
        }

        public bool RFOutputEnable
        {
            get => Query("OUTP:STAT?\n").Trim() == "1";
            set
            {
                if (value)
                    Write("OUTP:STAT ON");
                else
                    Write("OUTP:STAT OFF");
            }
        }

        /// <summary>
        /// OUTP:MOD:STAT?
        /// OUTP:MOD:STAT OFF
        /// </summary>
        public bool ModulationEnable
        {
            get => Query("OUTP:MOD:STAT?\n").Trim() == "1";
            set
            {
                if (value)
                    Write("OUTP:MOD:STAT ON");
                else
                    Write("OUTP:MOD:STAT OFF");
            }
        }
    }
}
