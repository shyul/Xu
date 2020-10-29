/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************


namespace Xu.WindowsNativeMethods
{
    public static class WindowsHook
    {
        public const int JOURNALRECORD = 0;
        public const int JOURNALPLAYBACK = 1;
        public const int KEYBOARD = 2;
        public const int GETMESSAGE = 3;
        public const int CALLWNDPROC = 4;
        public const int CBT = 5;
        public const int SYSMSGFILTER = 6;
        public const int MOUSE = 7;
        public const int HARDWARE = 8;
        public const int DEBUG = 9;
        public const int SHELL = 10;
        public const int FOREGROUNDIDLE = 11;
        public const int CALLWNDPROCRET = 12;
        public const int KEYBOARD_LL = 13;
        public const int MOUSE_LL = 14;
    }
}
