/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;

namespace Xu
{
    [Serializable, DataContract]
    public static class Icons
    {
        public static Bitmap GetIcon(Dictionary<Size, Bitmap> icon, Size sz)
        {
            if (icon.ContainsKey(sz))
                return icon[sz];
            else
            {
                int pixel = sz.Width * sz.Height;
                if (icon.Count > 0)
                {
                    Bitmap bm = icon.ElementAt(0).Value;
                    for (int i = 0; i < icon.Count; i++)
                    {
                        Size s = icon.ElementAt(i).Key;
                        if (pixel < s.Width * s.Height)
                        {
                            bm = icon.ElementAt(i).Value;
                        }
                    }
                    return new Bitmap(bm, sz);
                }
                else
                    return new Bitmap(Xu.Properties.Resources.Blank_32, sz);
            }
        }

        public static readonly Dictionary<Size, Bitmap> Caption_Minimize = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.Caption_Minimize },
        };

        public static readonly Dictionary<Size, Bitmap> Caption_Maximize = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.Caption_Maximize },
        };

        public static readonly Dictionary<Size, Bitmap> Caption_Restore = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.Caption_RestoreNormal },
        };

        public static readonly Dictionary<Size, Bitmap> Caption_Close = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.Caption_Close },
        };


        #region File

        public static readonly Dictionary<Size, Bitmap> File_Open = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.Folder_Open_16 },
            { new Size(24, 24), Properties.Resources.Folder_Open_24 },
            { new Size(32, 32), Properties.Resources.Folder_Open_32 },
        };

        public static readonly Dictionary<Size, Bitmap> File_Save = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.File_Save_16 },
            { new Size(24, 24), Properties.Resources.File_Save_24 },
            { new Size(32, 32), Properties.Resources.File_Save_32 },
        };

        #endregion

        #region Clip Board

        public static readonly Dictionary<Size, Bitmap> Clip_Copy = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.Clip_Copy_16 },
            { new Size(24, 24), Properties.Resources.Clip_Copy_24 },
            { new Size(32, 32), Properties.Resources.Clip_Copy_32 }
        };

        public static readonly Dictionary<Size, Bitmap> Clip_Cut = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.Clip_Cut_16 },
            { new Size(24, 24), Properties.Resources.Clip_Cut_24 },
            { new Size(32, 32), Properties.Resources.Clip_Cut_32 }
        };

        public static readonly Dictionary<Size, Bitmap> Clip_Paste = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.Clip_Paste_16 },
            { new Size(24, 24), Properties.Resources.Clip_Paste_24 },
            { new Size(32, 32), Properties.Resources.Clip_Paste_32 }
        };

        #endregion

        #region Utility Symbols

        public static readonly Dictionary<Size, Bitmap> Undo = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.Undo_16 },
            { new Size(24, 24), Properties.Resources.Undo_24 },
        };

        public static readonly Dictionary<Size, Bitmap> Redo = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.Redo_16 },
            { new Size(24, 24), Properties.Resources.Redo_24 },
        };

        public static readonly Dictionary<Size, Bitmap> Nav_Back = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.Nav_Back_16 },
            { new Size(32, 32), Properties.Resources.Nav_Back_32 },
        };

        public static readonly Dictionary<Size, Bitmap> Nav_Next = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.Nav_Next_16 },
            { new Size(32, 32), Properties.Resources.Nav_Next_32 },
        };

        public static readonly Dictionary<Size, Bitmap> Delete = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.Delete_16 },
            { new Size(24, 24), Properties.Resources.Delete_24 },
            { new Size(32, 32), Properties.Resources.Delete_32 }
        };

        public static readonly Dictionary<Size, Bitmap> Erase = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.Eraser_16 },
            { new Size(24, 24), Properties.Resources.Eraser_24 }
        };

        public static readonly Dictionary<Size, Bitmap> Plus = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.Plus_16 },
            { new Size(24, 24), Properties.Resources.Plus_24 },
            { new Size(32, 32), Properties.Resources.Plus_32 }
        };

        public static readonly Dictionary<Size, Bitmap> Minus = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.Minus_16 },
            { new Size(24, 24), Properties.Resources.Minus_24 },
            { new Size(32, 32), Properties.Resources.Minus_32 }
        };

        public static readonly Dictionary<Size, Bitmap> Stop = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.Stop_16 },
            { new Size(24, 24), Properties.Resources.Stop_24 },
            { new Size(32, 32), Properties.Resources.Stop_32 }
        };

        #endregion

        #region Logging

        public static readonly Dictionary<Size, Bitmap> Log_Info = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.Log_Info_16 },
            { new Size(24, 24), Properties.Resources.Log_Info_24 },
            { new Size(32, 32), Properties.Resources.Log_Info_32 }
        };

        public static readonly Dictionary<Size, Bitmap> Log_Plus = new Dictionary<Size, Bitmap>() {
            { new Size(16, 16), Properties.Resources.Log_Plus_16 },
            { new Size(24, 24), Properties.Resources.Log_Plus_24 },
            { new Size(32, 32), Properties.Resources.Log_Plus_32 }
        };

        #endregion
    }
}
