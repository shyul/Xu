﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public abstract class PowerSupplyChannel : IOutputChannel
    {
        public string ChannelName { get; }

        public void WriteSetting() { }

        public IPowerSupply PowerSupply { get; }

        public void ON() => PowerSupply.PWR_ON(ChannelName);

        public void OFF() => PowerSupply.PWR_OFF(ChannelName);



        public double Voltage { get; }

        public double Current { get; }

        public Range<double> VoltageRange { get; }

        public Range<double> CurrentRange { get; }

    }
}