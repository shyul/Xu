/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System.Drawing;

namespace Xu.Chart
{
    /// <summary>
    /// Advance and decline series
    /// </summary>
    public interface IAdvanceDeclineSeries : IOrdered
    {
        /// <summary>
        /// Gain data column
        /// </summary>
        public NumericColumn Gain_Column { get; }

        Color DownColor { get; }

        /// <summary>
        /// Theme for down trend
        /// </summary>
        ColorTheme DownTheme { get; }

        /// <summary>
        /// Theme for down trend text
        /// </summary>
        ColorTheme DownTextTheme { get; }
    }
}
