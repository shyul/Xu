/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Drawing;

namespace Xu.GridView
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CellRendererAttribute : Attribute
    {
        public CellRendererAttribute(Type rendererType, int width, int minimumHeight, bool autoWidth)
        {
            if (rendererType.GetInterfaces().Contains(typeof(IDataCellRenderer)))
            {
                DataCellRenderer = Activator.CreateInstance(rendererType) as IDataCellRenderer;
                DataCellRenderer.Width = width;
                DataCellRenderer.MinimumHeight = minimumHeight;
                DataCellRenderer.AutoWidth = autoWidth;
            }
            else
            {
                DataCellRenderer = new TextCellRenderer();
            }
        }

        public CellRendererAttribute(IDataCellRenderer renderer)
        {
            DataCellRenderer = renderer;
        }

        public IDataCellRenderer DataCellRenderer { get; }
    }
}
