/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Xu
{
    public interface IStringTable : ITable
    {
        string this[int i, StringColumn column] { get; }
    }
}
