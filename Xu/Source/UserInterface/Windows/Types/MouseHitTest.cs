/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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
        public static IntPtr ERROR = new IntPtr(-2);
        public static IntPtr TRANSPARENT = new IntPtr(-1);

        public static IntPtr NOWHERE = new IntPtr(0);
        public static IntPtr CLIENT = new IntPtr(1);

        public static IntPtr CAPTION = new IntPtr(2);
        public static IntPtr SYSMENU = new IntPtr(3);
        public static IntPtr GROWBOX = new IntPtr(4);
        public static IntPtr SIZE = new IntPtr(4);
        public static IntPtr MENU = new IntPtr(5);
        public static IntPtr HSCROLL = new IntPtr(6);
        public static IntPtr VSCROLL = new IntPtr(7);

        public static IntPtr REDUCE = new IntPtr(8);
        public static IntPtr MINBUTTON = new IntPtr(8);
        public static IntPtr ZOOM = new IntPtr(9);
        public static IntPtr MAXBUTTON = new IntPtr(9);

        public static IntPtr LEFT = new IntPtr(10);
        public static IntPtr RIGHT = new IntPtr(11);
        public static IntPtr TOP = new IntPtr(12);
        public static IntPtr BOTTOM = new IntPtr(15); // In the lower-horizontal border of a resizable window (the user can click the mouse to resize the window vertically).

        public static IntPtr TOPLEFT = new IntPtr(13);
        public static IntPtr TOPRIGHT = new IntPtr(14);
        public static IntPtr BOTTOMLEFT = new IntPtr(16); // In the lower-left corner of a border of a resizable window (the user can click the mouse to resize the window diagonally).
        public static IntPtr BOTTOMRIGHT = new IntPtr(17);

        public static IntPtr BORDER = new IntPtr(18); // In the border of a window that does not have a sizing border.
        public static IntPtr CLOSE = new IntPtr(20);
        public static IntPtr HELP = new IntPtr(21);
    }
}
