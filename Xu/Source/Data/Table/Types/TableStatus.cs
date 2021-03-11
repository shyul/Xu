/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

namespace Xu
{
    public enum TableStatus : int
    {
        Default = 0,

        Loading = 10,

        DataReady = 20,

        Ticking = 30,

        Calculating = 40,

        CalculateFinished = 50,

        TickingFinished = 60,

        Ready = 100,
    }
}
