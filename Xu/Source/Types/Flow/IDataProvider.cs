/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Xu
{
    public interface IDataProvider
    {
        bool AddDataConsumer(IDataConsumer idk);

        bool RemoveDataConsumer(IDataConsumer idk);

        void DataIsUpdated();

        DateTime UpdateTime { get; }
    }
}
