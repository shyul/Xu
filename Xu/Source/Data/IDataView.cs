/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Xu
{
    public interface IDataView : IDataSink
    {
        int StartPt { get; set; }

        int StopPt { get; set; }

        bool ReadyToShow { get; set; }

        void PointerToEnd();

        void PointerToNextTick();
    }

    public interface IDataSink
    {
        //void AddDataSource(IDataSource ids);

        void RemoveDataSource();

        void DataIsUpdated(); // void DataIsUpdated(IDataSource ids);
    }

    public interface IDataSource
    {
        void AddDataSink(IDataSink idk);

        bool RemoveDataSink(IDataSink idk);

        void DataIsUpdated();

        DateTime UpdateTime { get; }
    }
}
