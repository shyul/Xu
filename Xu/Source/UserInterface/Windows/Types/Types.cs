/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Xu.WindowsNativeMethods
{
    public static class HWND
    {
        public static readonly IntPtr BROADCAST = new IntPtr(0xffff);
        public static readonly IntPtr TOP = new IntPtr(0);
        public static readonly IntPtr BOTTOM = new IntPtr(1);
        public static readonly IntPtr TOPMOST = new IntPtr(-1);
        public static readonly IntPtr NOTOPMOST = new IntPtr(-2);
        public static readonly IntPtr MESSAGE = new IntPtr(-3);
    }

    public static class HKEY
    {
        public static IntPtr CLASSES_ROOT = new IntPtr(0x80000000);
        public static IntPtr CURRENT_USER = new IntPtr(0x80000001);
        public static IntPtr LOCAL_MACHINE = new IntPtr(0x80000002);
        public static IntPtr USERS = new IntPtr(0x80000003);
    }

    public static class WVR
    {
        public static IntPtr ALIGNTOP = new IntPtr(0x0010);
        public static IntPtr ALIGNLEFT = new IntPtr(0x0020);
        public static IntPtr ALIGNBOTTOM = new IntPtr(0x0040);
        public static IntPtr ALIGNRIGHT = new IntPtr(0x0080);
        public static IntPtr HREDRAW = new IntPtr(0x0100);
        public static IntPtr VREDRAW = new IntPtr(0x0200);
        public static IntPtr REDRAW = new IntPtr(0x0100 | 0x0200);
        public static IntPtr VALIDRECTS = new IntPtr(0x400);
    }

    public static class SWP
    {
        public const uint NOSIZE = 0x0001;
        public const uint NOMOVE = 0x0002;
        public const uint NOZORDER = 0x0004;
        public const uint NOREDRAW = 0x0008;
        public const uint NOACTIVATE = 0x0010;
        public const uint FRAMECHANGED = 0x0020;
        public const uint SHOWWINDOW = 0x0040;
        public const uint HIDEWINDOW = 0x0080;
        public const uint NOCOPYBITS = 0x0100;
        public const uint NOOWNERZORDER = 0x0200;
        public const uint NOSENDCHANGING = 0x0400;
        public const uint DRAWFRAME = FRAMECHANGED;
        public const uint NOREPOSITION = NOOWNERZORDER;
        public const uint DEFERERASE = 0x2000;
        public const uint ASYNCWINDOWPOS = 0x4000;
    }

    public static class DCX
    {
        public const uint CACHE = 0x2;
        public const uint CLIPCHILDREN = 0x8;
        public const uint CLIPSIBLINGS = 0x10;
        public const uint EXCLUDERGN = 0x40;
        public const uint EXCLUDEUPDATE = 0x100;
        public const uint INTERSECTRGN = 0x80;
        public const uint INTERSECTUPDATE = 0x200;
        public const uint LOCKWINDOWUPDATE = 0x400;
        public const uint NORECOMPUTE = 0x100000;
        public const uint NORESETATTRS = 0x4;
        public const uint PARENTCLIP = 0x20;
        public const uint VALIDATE = 0x200000;
        public const uint WINDOW = 0x1;
    }
}
