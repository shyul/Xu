/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Xu
{
    public class SimpleDataProvider : IDataProvider
    {
        public DateTime UpdateTime { get; set; } = DateTime.MinValue;

        public List<IDataConsumer> DataConsumers { get; } = new List<IDataConsumer>();

        public bool AddDataConsumer(IDataConsumer idk)
        {
            return DataConsumers.CheckAdd(idk);
        }

        public bool RemoveDataConsumer(IDataConsumer idk)
        {
            return DataConsumers.CheckRemove(idk);
        }

        public void DataIsUpdated()
        {
            DataConsumers.ForEach(n => n.DataIsUpdated());
        }
    }
}
