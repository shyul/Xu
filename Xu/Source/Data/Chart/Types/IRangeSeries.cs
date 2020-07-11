/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************


namespace Xu.Chart
{
    /// <summary>
    /// Band series with high and low bonds
    /// </summary>
    public interface IRangeSeries : IOrdered
    {
        /// <summary>
        /// higher bond data column
        /// </summary>
        NumericColumn High_Column { get; }

        /// <summary>
        /// lower bond data column
        /// </summary>
        NumericColumn Low_Column { get; }

        /// <summary>
        /// Theme for down trend
        /// </summary>
        ColorTheme LowTheme { get; }

        /// <summary>
        /// Theme for down trend text
        /// </summary>
        ColorTheme LowTextTheme { get; }
    }
}
