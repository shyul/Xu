/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Xu.Chart
{
    /// <summary>
    /// Band series with high and low bonds
    /// </summary>
    public interface ITagSeries
    {
        List<TagColumn> TagColumns { get; }
    }
}
