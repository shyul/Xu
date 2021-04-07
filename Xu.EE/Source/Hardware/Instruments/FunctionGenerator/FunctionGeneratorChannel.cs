﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public class FunctionGeneratorChannel : IPort
    {
        public FunctionGeneratorChannel(string channelName, IFunctionGenerator fgen)
        {
            Name = channelName;
            Device = fgen;
        }

        public string Name { get; }

        public bool Enabled
        {
            get => m_Enabled;

            set
            {
                m_Enabled = value;

                if (m_Enabled)
                    Device.FunctionGenerator_ON(Name);
                else
                    Device.FunctionGenerator_OFF(Name);
            }
        }

        private bool m_Enabled = false;

        public IFunctionGenerator Device { get; }

        public FunctionGeneratorConfig Config { get; set; }

        public void WriteSetting() => Device.FunctionGenerator_WriteSetting(Name);

        public void ReadSetting() => Device.FunctionGenerator_ReadSetting(Name);
    }
}