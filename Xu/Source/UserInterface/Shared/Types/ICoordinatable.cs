/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;

namespace Xu
{
    public interface ICoordinatable
    {
        Rectangle Bounds { get; set; }

        Point Location { get; set; }

        Size Size { get; set; }

        int Top { get; }

        int Bottom { get; }

        int Left { get; }

        int Right { get; }

        Point Center { get; }
    }
}
