/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Xu
{
    public interface ITable
    {
        int Count { get; }

        double this[int i, NumericColumn column] { get; }

        object DataLockObject { get; }
    }
}
