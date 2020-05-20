/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************


namespace Xu.WindowsNativeMethods
{
    public static class DeviceCap
    {
        /// <summary>
        /// Device driver version
        /// </summary>
        public const int DRIVERVERSION = 0;
        /// <summary>
        /// Device classification
        /// </summary>
        public const int TECHNOLOGY = 2;
        /// <summary>
        /// Horizontal size in millimeters
        /// </summary>
        public const int HORZSIZE = 4;
        /// <summary>
        /// Vertical size in millimeters
        /// </summary>
        public const int VERTSIZE = 6;
        /// <summary>
        /// Horizontal width in pixels
        /// </summary>
        public const int HORZRES = 8;
        /// <summary>
        /// Vertical height in pixels
        /// </summary>
        public const int VERTRES = 10;
        /// <summary>
        /// Number of bits per pixel
        /// </summary>
        public const int BITSPIXEL = 12;
        /// <summary>
        /// Number of planes
        /// </summary>
        public const int PLANES = 14;
        /// <summary>
        /// Number of brushes the device has
        /// </summary>
        public const int NUMBRUSHES = 16;
        /// <summary>
        /// Number of pens the device has
        /// </summary>
        public const int NUMPENS = 18;
        /// <summary>
        /// Number of markers the device has
        /// </summary>
        public const int NUMMARKERS = 20;
        /// <summary>
        /// Number of fonts the device has
        /// </summary>
        public const int NUMFONTS = 22;
        /// <summary>
        /// Number of colors the device supports
        /// </summary>
        public const int NUMCOLORS = 24;
        /// <summary>
        /// Size required for device descriptor
        /// </summary>
        public const int PDEVICESIZE = 26;
        /// <summary>
        /// Curve capabilities
        /// </summary>
        public const int CURVECAPS = 28;
        /// <summary>
        /// Line capabilities
        /// </summary>
        public const int LINECAPS = 30;
        /// <summary>
        /// Polygonal capabilities
        /// </summary>
        public const int POLYGONALCAPS = 32;
        /// <summary>
        /// Text capabilities
        /// </summary>
        public const int TEXTCAPS = 34;
        /// <summary>
        /// Clipping capabilities
        /// </summary>
        public const int CLIPCAPS = 36;
        /// <summary>
        /// Bitblt capabilities
        /// </summary>
        public const int RASTERCAPS = 38;
        /// <summary>
        /// Length of the X leg
        /// </summary>
        public const int ASPECTX = 40;
        /// <summary>
        /// Length of the Y leg
        /// </summary>
        public const int ASPECTY = 42;
        /// <summary>
        /// Length of the hypotenuse
        /// </summary>
        public const int ASPECTXY = 44;
        /// <summary>
        /// Shading and Blending caps
        /// </summary>
        public const int SHADEBLENDCAPS = 45;
        /// <summary>
        /// Logical pixels inch in X
        /// </summary>
        public const int LOGPIXELSX = 88;
        /// <summary>
        /// Logical pixels inch in Y
        /// </summary>
        public const int LOGPIXELSY = 90;
        /// <summary>
        /// Number of entries in physical palette
        /// </summary>
        public const int SIZEPALETTE = 104;
        /// <summary>
        /// Number of reserved entries in palette
        /// </summary>
        public const int NUMRESERVED = 106;
        /// <summary>
        /// Actual color resolution
        /// </summary>
        public const int COLORRES = 108;
        // Printing related DeviceCaps. These replace the appropriate Escapes
        /// <summary>
        /// Physical Width in device units
        /// </summary>
        public const int PHYSICALWIDTH = 110;
        /// <summary>
        /// Physical Height in device units
        /// </summary>
        public const int PHYSICALHEIGHT = 111;
        /// <summary>
        /// Physical Printable Area x margin
        /// </summary>
        public const int PHYSICALOFFSETX = 112;
        /// <summary>
        /// Physical Printable Area y margin
        /// </summary>
        public const int PHYSICALOFFSETY = 113;
        /// <summary>
        /// Scaling factor x
        /// </summary>
        public const int SCALINGFACTORX = 114;
        /// <summary>
        /// Scaling factor y
        /// </summary>
        public const int SCALINGFACTORY = 115;
        /// <summary>
        /// Current vertical refresh rate of the display device (for displays only) in Hz
        /// </summary>
        public const int VREFRESH = 116;
        /// <summary>
        /// Vertical height of entire desktop in pixels
        /// </summary>
        public const int DESKTOPVERTRES = 117;
        /// <summary>
        /// Horizontal width of entire desktop in pixels
        /// </summary>
        public const int DESKTOPHORZRES = 118;
        /// <summary>
        /// Preferred blt alignment
        /// </summary>
        public const int BLTALIGNMENT = 119;
    }
}
