/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Xu.WindowsNativeMethods
{
    /// <summary>
    /// Enumeration of HitTest values.
    /// See https://docs.microsoft.com/en-us/windows/desktop/inputdev/wm-nchittest
    /// for a description of each value.
    /// </summary>
    public static class MouseHitTest
    {
        public static IntPtr ERROR = new(-2);
        public static IntPtr TRANSPARENT = new(-1);

        public static IntPtr NOWHERE = new(0);
        public static IntPtr CLIENT = new(1);

        public static IntPtr CAPTION = new(2);
        public static IntPtr SYSMENU = new(3);
        public static IntPtr GROWBOX = new(4);
        public static IntPtr SIZE = new(4);
        public static IntPtr MENU = new(5);
        public static IntPtr HSCROLL = new(6);
        public static IntPtr VSCROLL = new(7);

        public static IntPtr REDUCE = new(8);
        public static IntPtr MINBUTTON = new(8);
        public static IntPtr ZOOM = new(9);
        public static IntPtr MAXBUTTON = new(9);

        public static IntPtr LEFT = new(10);
        public static IntPtr RIGHT = new(11);
        public static IntPtr TOP = new(12);
        public static IntPtr BOTTOM = new(15); // In the lower-horizontal border of a resizable window (the user can click the mouse to resize the window vertically).

        public static IntPtr TOPLEFT = new(13);
        public static IntPtr TOPRIGHT = new(14);
        public static IntPtr BOTTOMLEFT = new(16); // In the lower-left corner of a border of a resizable window (the user can click the mouse to resize the window diagonally).
        public static IntPtr BOTTOMRIGHT = new(17);

        public static IntPtr BORDER = new(18); // In the border of a window that does not have a sizing border.
        public static IntPtr CLOSE = new(20);
        public static IntPtr HELP = new(21);
    }
}
