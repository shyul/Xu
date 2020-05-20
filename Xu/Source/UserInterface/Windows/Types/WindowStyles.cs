/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************


namespace Xu.WindowsNativeMethods
{
    public static class WindowStyles
    {
        public const uint OVERLAPPED = 0x00000000;
        public const uint POPUP = 0x80000000;
        public const uint CHILD = 0x40000000;
        public const uint MINIMIZE = 0x20000000;
        public const uint VISIBLE = 0x10000000;
        public const uint DISABLED = 0x08000000;
        public const uint CLIPSIBLINGS = 0x04000000;
        public const uint CLIPCHILDREN = 0x02000000;
        public const uint MAXIMIZE = 0x01000000;
        public const uint BORDER = 0x00800000;
        public const uint DLGFRAME = 0x00400000;
        public const uint VSCROLL = 0x00200000;
        public const uint HSCROLL = 0x00100000;
        public const uint SYSMENU = 0x00080000;
        public const uint THICKFRAME = 0x00040000;
        public const uint GROUP = 0x00020000;
        public const uint TABSTOP = 0x00010000;

        public const uint MINIMIZEBOX = 0x00020000;
        public const uint MAXIMIZEBOX = 0x00010000;

        public const uint CAPTION = BORDER | DLGFRAME;
        public const uint TILED = OVERLAPPED;
        public const uint ICONIC = MINIMIZE;
        public const uint SIZEBOX = THICKFRAME;
        public const uint TILEDWINDOW = OVERLAPPEDWINDOW;

        public const uint OVERLAPPEDWINDOW = OVERLAPPED | CAPTION | SYSMENU | THICKFRAME | MINIMIZEBOX | MAXIMIZEBOX;
        public const uint POPUPWINDOW = POPUP | BORDER | SYSMENU;
        public const uint CHILDWINDOW = CHILD;

        //Extended Window Styles

        public const uint EX_DLGMODALFRAME = 0x00000001;
        public const uint EX_NOPARENTNOTIFY = 0x00000004;
        public const uint EX_TOPMOST = 0x00000008;
        public const uint EX_ACCEPTFILES = 0x00000010;
        public const uint EX_TRANSPARENT = 0x00000020;

        //#if(WINVER >= 0x0400)

        public const uint EX_MDICHILD = 0x00000040;
        public const uint EX_TOOLWINDOW = 0x00000080;
        public const uint EX_WINDOWEDGE = 0x00000100;
        public const uint EX_CLIENTEDGE = 0x00000200;
        public const uint EX_CONTEXTHELP = 0x00000400;

        public const uint EX_RIGHT = 0x00001000;
        public const uint EX_LEFT = 0x00000000;
        public const uint EX_RTLREADING = 0x00002000;
        public const uint EX_LTRREADING = 0x00000000;
        public const uint EX_LEFTSCROLLBAR = 0x00004000;
        public const uint EX_RIGHTSCROLLBAR = 0x00000000;

        public const uint EX_CONTROLPARENT = 0x00010000;
        public const uint EX_STATICEDGE = 0x00020000;
        public const uint EX_APPWINDOW = 0x00040000;

        public const uint EX_OVERLAPPEDWINDOW = (EX_WINDOWEDGE | EX_CLIENTEDGE);
        public const uint EX_PALETTEWINDOW = (EX_WINDOWEDGE | EX_TOOLWINDOW | EX_TOPMOST);

        public const uint EX_LAYERED = 0x00080000;

        public const uint EX_NOINHERITLAYOUT = 0x00100000; // Disable inheritence of mirroring by children
        public const uint EX_LAYOUTRTL = 0x00400000; // Right to left mirroring

        public const uint EX_COMPOSITED = 0x02000000;
        public const uint EX_NOACTIVATE = 0x08000000;
    }
}
