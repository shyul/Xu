/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Xu
{
    [Serializable, DataContract]
    public struct ShortcutKey
    {
        public ShortcutKey(Keys key, bool hasShift = false, bool hasCtrl = false, bool hasAlt = false)
        {
            Key = key;
            HasShift = hasShift;
            HasCtrl = hasCtrl;
            HasAlt = hasAlt;
        }

        [DataMember]
        public bool HasShift { get; set; }

        [DataMember]
        public bool HasCtrl { get; set; }

        [DataMember]
        public bool HasAlt { get; set; }

        [DataMember]
        public Keys Key { get; set; }

        public override string ToString()
        {
            string val = string.Empty;
            if (HasShift) val += "Shift + ";
            if (HasCtrl) val += "Ctrl + ";
            if (HasAlt) val += "Alt + ";
            val += Key.ToString();
            return val;
        }
    }
}
