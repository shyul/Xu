/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************


namespace Xu.WindowsNativeMethods
{
    /// <summary>
    /// Specifies a raster-operation code. These codes define how the color data for the
    /// source rectangle is to be combined with the color data for the destination
    /// rectangle to achieve the final color.
    /// http://pinvoke.net/default.aspx/Enums/TernaryRasterOperations.html
    /// </summary>
    public static class TernaryRasterOperations
    {
        /// <summary>dest = source</summary>
        public const int SRCCOPY = 0x00CC0020;
        /// <summary>dest = source OR dest</summary>
        public const int SRCPAINT = 0x00EE0086;
        /// <summary>dest = source AND dest</summary>
        public const int SRCAND = 0x008800C6;
        /// <summary>dest = source XOR dest</summary>
        public const int SRCINVERT = 0x00660046;
        /// <summary>dest = source AND (NOT dest)</summary>
        public const int SRCERASE = 0x00440328;
        /// <summary>dest = (NOT source)</summary>
        public const int NOTSRCCOPY = 0x00330008;
        /// <summary>dest = (NOT src) AND (NOT dest)</summary>
        public const int NOTSRCERASE = 0x001100A6;
        /// <summary>dest = (source AND pattern)</summary>
        public const int MERGECOPY = 0x00C000CA;
        /// <summary>dest = (NOT source) OR dest</summary>
        public const int MERGEPAINT = 0x00BB0226;
        /// <summary>dest = pattern</summary>
        public const int PATCOPY = 0x00F00021;
        /// <summary>dest = DPSnoo</summary>
        public const int PATPAINT = 0x00FB0A09;
        /// <summary>dest = pattern XOR dest</summary>
        public const int PATINVERT = 0x005A0049;
        /// <summary>dest = (NOT dest)</summary>
        public const int DSTINVERT = 0x00550009;
        /// <summary>dest = BLACK</summary>
        public const int BLACKNESS = 0x00000042;
        /// <summary>dest = WHITE</summary>
        public const int WHITENESS = 0x00FF0062;
        /// <summary>
        /// Capture window as seen on screen.  This includes layered windows 
        /// such as WPF windows with AllowsTransparency="true"
        /// </summary>
        public const int CAPTUREBLT = 0x40000000;
    }
}
