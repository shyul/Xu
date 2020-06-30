/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Xu
{
    public interface IDataView
    {
        //bool ReadyToShow { get; set; }

        int StartPt { get; set; }

        int StopPt { get; set; }

        ITable Table { get; }

        void SetRefreshUI();
    }
}
