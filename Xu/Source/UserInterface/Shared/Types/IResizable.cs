/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using System.Windows.Forms;

namespace Xu
{
    public interface IResizable
    {
        LayoutType LayoutType { get; }

        Rectangle SizeGrip { get; }

        Control Area { get; }

        Control Canvas { get; }

        void OnGetSize(int size);
    }
}
