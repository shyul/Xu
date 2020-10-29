/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Xu.WindowsNativeMethods
{
    public static class DWMAPI
    {
        // Default window procedure for Desktop Window Manager (DWM) hit-testing within the non-client area.
        [DllImport("dwmapi.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DwmDefWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref IntPtr result);

        // Obtains a value that indicates whether Desktop Window Manager (DWM) composition is enabled.
        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        // Gets if computer is glass capable and enabled
        public static bool IsGlassEnabled
        {
            get
            {
                //Check what DWM says about composition
                int enabled = 0;
                int response = DwmIsCompositionEnabled(ref enabled);
                return enabled > 0;
            }
        }

        // Extends the window frame behind the client area.
        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

        [StructLayout(LayoutKind.Sequential)]
        public struct DwmGetColorizationColors
        {
            public uint ColorizationColor,
                ColorizationAfterglow,
                ColorizationColorBalance,
                ColorizationAfterglowBalance,
                ColorizationBlurBalance,
                ColorizationGlassReflectionIntensity,
                ColorizationOpaqueBlend;
        }

        [DllImport("dwmapi.dll", EntryPoint = "#127")]
        private static extern void DwmGetColorizationParameters(ref DwmGetColorizationColors colors);

        public static Color GetWindowColorizationColor(bool opaque)
        {
            DwmGetColorizationColors c = new DwmGetColorizationColors();
            DwmGetColorizationParameters(ref c);
            return Color.FromArgb(
                (byte)(opaque ? 255 : c.ColorizationColor >> 24),
                (byte)(c.ColorizationColor >> 16),
                (byte)(c.ColorizationColor >> 8),
                (byte)c.ColorizationColor
                );
        }
    }
}
