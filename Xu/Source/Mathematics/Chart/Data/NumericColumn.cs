﻿/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Xu
{
    public class NumericColumn : Column
    {
        public NumericColumn(string name) => Name = name;

        public NumericColumn(string name, string label)
        {
            Name = name;
            Label = label;
        }
    }
}
