/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Xu
{
    public interface IDataRenderer : IDataConsumer
    {
        int StartPt { get; set; }

        int StopPt { get; set; }

        bool ReadyToShow { get; set; }

        void PointerToEnd();

        void PointerToNextTick();
    }
}
