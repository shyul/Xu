﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface IAnalogPin : IPort
    {
        Range<double> Range { get; }

        double Value { get; set; }
    }
}
