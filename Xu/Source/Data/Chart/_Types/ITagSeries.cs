﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System.Collections.Generic;

namespace Xu.Chart
{
    /// <summary>
    /// Band series with high and low bonds
    /// </summary>
    public interface ITagSeries
    {
        List<DatumColumn> TagColumns { get; }
    }
}
