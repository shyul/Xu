/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Threading;

/// <summary>
/// 0. Click has to be overhauled
/// 1. Task Schedule, period of time, Frequency
/// 1. Format the title Bar, Text Widget and Search Bar
/// 2. Caption's Navgating button is for the GridDocks...
/// 1. Command List with Hierarchy information: embedded in the command or look up in the list? auto style vs manual style?? auto generate the list
/// 2. Event List, Event Type, Progress Type
/// 2. Status Type
/// 3. RibbonTab for each DockWindow. Add and Remove! with color line
/// 3. RibbonTab's corner button is customizable!! (Hide for permanent pane and show for customizable pane...)
/// 3. RibbonToggleButton, RibbonDropButton with List, RibbonTextWidget
/// 4. DockWindow, Dock to the SideBar when sidebar does not exist?? Currently easy when there is already a dock 
/// 5. Dock Freeze Control, Selective Dock
/// 5. Dock to a new Form!
/// 5. Dock Resize with scales showing on the side and magnets
/// 6. Context Menu to the Dock Tabs
/// 7. Command Parser -- Simple Script or Python?
/// 
/// 
/// 
/// 100. IStatus. IEvent has the potential for Chart descion making...
/// </summary>

namespace Xu
{
    [Serializable, DataContract]
    public static class Main
    {
        public static AppTheme Theme { get; set; } = new AppTheme();

        public static object Selected_Object { get; set; }

        // Source Container...

        // Target Container...

        public static Command Command_Nav_Back { get; } = new Command()
        {
            //Enabled = false,
            Label = "Backward",
            IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>()
                {
                    { IconType.Normal, Icons.Nav_Back },
                },

            Action = (TaskControl<float> control) =>
            {
                ObsoletedEvent.Debug("Backward is clicked");
            },
        };

        public static Command Command_Nav_Next { get; } = new Command()
        {
            //Enabled = false,
            Label = "Forward",
            IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>()
                {
                    { IconType.Normal, Icons.Nav_Next },
                },
            Action = (TaskControl<float> control) =>
            {
                ObsoletedEvent.Debug("Forward is clicked");
            },
            Enabled = false
        };

        public static Command Command_Undo { get; } = new Command()
        {
            //Enabled = false,
            Label = "Undo",
            IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>()
                {
                    { IconType.Normal, Icons.Undo },
                },
            Action = (TaskControl<float> control) =>
            {
                ObsoletedEvent.Debug("Undo is clicked");
            },
            Enabled = false
        };

        public static Command Command_Redo { get; } = new Command()
        {
            //Enabled = false,
            Label = "Redo",
            IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>()
                {
                    { IconType.Normal, Icons.Redo },
                },

            Action = (TaskControl<float> control) =>
            {
                ObsoletedEvent.Debug("Redo is clicked");
            },
        };

        public static Command Command_File_Open { get; } = new Command()
        {
            //Enabled = false,
            Label = "Open File",
            IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>()
                {
                    { IconType.Normal, Icons.File_Open },
                },
            Action = (TaskControl<float> control) =>
            {
                ObsoletedEvent.Debug("Open is clicked");
            },
        };

        public static Command Command_File_Save { get; } = new Command()
        {
            //Enabled = false,
            Label = "Save File",
            IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>()
                {
                    { IconType.Normal, Icons.File_Save },
                },
            Action = (TaskControl<float> control) =>
            {
                ObsoletedEvent.Debug("Save is clicked");
            },
        };

        public static Command Command_Clip_Copy = new Command()
        {
            //Enabled = false,
            Label = "Copy",
            IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>() { { IconType.Normal, Icons.Clip_Copy } },
            Action = (TaskControl<float> control) =>
            {
                ObsoletedEvent.Debug("Copy is clicked");
            },
        };

        public static Command Command_Clip_Cut = new Command()
        {
            //Enabled = false,
            Label = "Cut",
            IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>() { { IconType.Normal, Icons.Clip_Cut } },
            Action = (TaskControl<float> control) =>
            {
                ObsoletedEvent.Debug("Cut is clicked");
            },
        };

        public static Command Command_Clip_Paste = new Command()
        {
            //Enabled = false,
            Label = "Paste",
            IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>() { { IconType.Normal, Icons.Clip_Paste } },
            Action = (TaskControl<float> control) =>
            {
                ObsoletedEvent.Debug("Paste is clicked");
            },
        };

        public static Command Command_Clip_Delete = new Command()
        {
            //Enabled = false,
            Label = "Delete",
            IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>() { { IconType.Normal, Icons.Delete } },
            Action = (TaskControl<float> control) =>
            {
                ObsoletedEvent.Debug("Delete is clicked");
            },
        };


    }
}
