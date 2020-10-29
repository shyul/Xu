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
    public sealed class GridColumnOrderAttribute : Attribute
    {
        public GridColumnOrderAttribute(int displayOrder, int displayPriority = int.MaxValue, int sortPriority = int.MaxValue)
        {
            DisplayOrder = displayOrder;
            DisplayPriority = displayPriority;
            SortPriority = sortPriority;
        }

        public int DisplayOrder { get; } = int.MaxValue;

        /// <summary>
        /// Display Priority
        /// </summary>
        public int DisplayPriority { get; } = int.MaxValue;

        /// <summary>
        /// Sort Priority
        /// </summary>
        public int SortPriority { get; } = int.MaxValue;
    }
}
