/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Xu.WindowsNativeMethods
{
    public static class SysCommands
    {
        public const int SIZE = 0xF000;
        public const int MOVE = 0xF010;
        public const int MINIMIZE = 0xF020;
        public const int MAXIMIZE = 0xF030;
        public const int NEXTWINDOW = 0xF040;
        public const int PREVWINDOW = 0xF050;
        public const int CLOSE = 0xF060;
        public const int VSCROLL = 0xF070;
        public const int HSCROLL = 0xF080;
        public const int MOUSEMENU = 0xF090;
        public const int KEYMENU = 0xF100;
        public const int ARRANGE = 0xF110;
        public const int RESTORE = 0xF120;
        public const int TASKLIST = 0xF130;
        public const int SCREENSAVE = 0xF140;
        public const int HOTKEY = 0xF150;
        //#if(WINVER >= 0x0400) //Win95
        public const int DEFAULT = 0xF160;
        public const int MONITORPOWER = 0xF170;
        public const int CONTEXTHELP = 0xF180;
        public const int SEPARATOR = 0xF00F;
        //#endif /* WINVER >= 0x0400 */

        //#if(WINVER >= 0x0600) //Vista
        public const int F_ISSECURE = 0x00000001;
        //#endif /* WINVER >= 0x0600 */
        [Obsolete]
        public const int ICON = MINIMIZE;
        [Obsolete]
        public const int ZOOM = MAXIMIZE;
    }
}
