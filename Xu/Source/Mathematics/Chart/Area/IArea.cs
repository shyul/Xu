/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

namespace Xu.Chart
{
    public interface IArea : ICoordinatable
    {
        string Name { get; set; }

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
