/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************


namespace Xu.Chart
{
    public interface IChartAnalysis
    {
        string AreaName { get; }

        bool ChartEnabled { get; set; }

        int DrawOrder { get; set; }
    }
}
