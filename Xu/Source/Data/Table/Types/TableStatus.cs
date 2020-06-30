/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

namespace Xu
{
    public enum TableStatus : int
    {
        Ready = 0,

        Loading = 1,

        Downloading = 2,

        Calculating = 3,

        Ticking = 4,

        Saving = 6,

        Maintaining = 7,

        LoadFinished = 10,

        TickingFinished = 11,

        CalculateFinished = 12,
    }
}
