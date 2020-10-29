/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Runtime.InteropServices;

namespace Xu.WindowsNativeMethods
{
    public static class Gdi32
    {
        // This function transfers pixels from a specified source rectangle to a specified destination rectangle, altering the pixels according to the selected raster operation (ROP) code.
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

        // This function creates a memory device context (DC) compatible with the specified device.
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        // This function selects an object into a specified device context. The new object replaces the previous object of the same type.
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        // The DeleteObject function deletes a logical pen, brush, font, bitmap, region, or palette, freeing all system resources associated with the object. After the object is deleted, the specified handle is no longer valid.
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(IntPtr hObject);

        // The DeleteDC function deletes the specified device context (DC).
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteDC(IntPtr hdc);

        // The SaveDC function saves the current state of the specified device context (DC) by copying data describing selected objects and graphic modes
        [DllImport("gdi32.dll")]
        public static extern IntPtr SaveDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern IntPtr GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("gdi32.dll")]
        public static extern IntPtr GetStockObject(int fnObject);
    }
}
