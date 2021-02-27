/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
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
    public sealed class GridRendererAttribute : Attribute
    {
        public GridRendererAttribute(Type rendererType, int width = 60, bool autoWidth = false, int minimumHeight = 22)
        {
            if (rendererType.GetInterfaces().Contains(typeof(IGridRenderer)))
            {
                Renderer = Activator.CreateInstance(rendererType) as IGridRenderer;
            }
            else
            {
                Renderer = new TextCellRenderer();
            }

            Renderer.Width = width;
            Renderer.MinimumHeight = minimumHeight;
            Renderer.AutoWidth = autoWidth;
        }

        public GridRendererAttribute(IGridRenderer renderer)
        {
            Renderer = renderer;
        }

        public IGridRenderer Renderer { get; }
    }
}
