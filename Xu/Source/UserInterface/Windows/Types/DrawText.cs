/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************


namespace Xu.WindowsNativeMethods
{
    /// <summary>
    /// Create page DrawText (user32)
    /// http://pinvoke.net/default.aspx/user32/DrawText.html
    /// [DllImport("user32.dll")]
    /// static extern int DrawTextEx(IntPtr hdc, StringBuilder lpchText, int cchText,
    /// ref RECT lprc, uint dwDTFormat, ref DRAWTEXTPARAMS lpDTParams);
    /// </summary>
    public static class DrawText
    {
        public const int TOP = 0x00000000;
        public const int LEFT = 0x00000000;
        public const int CENTER = 0x00000001;
        public const int RIGHT = 0x00000002;
        public const int VCENTER = 0x00000004;
        public const int BOTTOM = 0x00000008;
        public const int WORDBREAK = 0x00000010;
        public const int SINGLELINE = 0x00000020;
        public const int EXPANDTABS = 0x00000040;
        public const int TABSTOP = 0x00000080;
        public const int NOCLIP = 0x00000100;
        public const int EXTERNALLEADING = 0x00000200;
        public const int CALCRECT = 0x00000400;
        public const int NOPREFIX = 0x00000800;
        public const int INTERNAL = 0x00001000;
        public const int EDITCONTROL = 0x00002000;
        public const int PATH_ELLIPSIS = 0x00004000;
        public const int END_ELLIPSIS = 0x00008000;
        public const int MODIFYSTRING = 0x00010000;
        public const int RTLREADING = 0x00020000;
        public const int WORD_ELLIPSIS = 0x00040000;
        public const int NOFULLWIDTHCHARBREAK = 0x00080000;
        public const int HIDEPREFIX = 0x00100000;
        public const int PREFIXONLY = 0x00200000;
    }
}
