﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface IOutputChannel : IChannel
    {
        void ON();

        void OFF();
    }
}