/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

namespace Xu.Chart
{
    public interface IArea : IOrdered, ICoordinatable
    {
        ChartWidget Chart { get; }

        ColorTheme Theme { get; }

        int StopPt { get; }

        int StartPt { get; }

        int IndexToPixel(int index);

        DiscreteAxis AxisX { get; }

        ContinuousAxis AxisY(AlignType side);

        int RightCursorX { get; }

        int LeftCursorX { get; }
    }
}
