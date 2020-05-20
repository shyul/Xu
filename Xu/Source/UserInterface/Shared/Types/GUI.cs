/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Runtime.Serialization;

namespace Xu
{
    [Serializable, DataContract]
    public enum LayoutType : uint
    {
        None = 0x0,
        OverLay = 0x1,
        Vertical = 0x2,
        Horizontal = 0x4,
    }

    [Serializable, DataContract]
    public enum LayoutStatus : uint
    {
        Idle = 0x0,
        Drag = 0x1,
        Resizing = 0x2,
        Docking = 0x3,
    }

    [Serializable, DataContract]
    public enum MouseState : uint
    {
        Out = 0x0,
        Hover = 0x1,
        Down = 0x2,
        Drag = 0x3
    }

    [Serializable, DataContract]
    public enum IconType : int
    {
        Normal = 0,
        Hover = 1,
        Down = 2,
        Checked = 10,
        CheckedHover = 11,
        CheckedDown = 12,
    }

    [Serializable, DataContract]
    public enum AlignType : int
    {
        Right = -1,
        Center = 0,
        Left = 1,
    }
}
