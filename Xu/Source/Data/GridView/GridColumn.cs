/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using System.Drawing;

namespace Xu.GridView
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property), Serializable, DataContract]
    public sealed class GridColumnAttribute : Attribute
    {
        public GridColumnAttribute(string name, bool isReadOnly = true)
        {
            Name = name;
            IsReadOnly = isReadOnly;
        }

        public string Name { get; }

        public bool IsReadOnly { get; }
    }

    public sealed class GridColumnConfiguration
    {

        public GridColumnConfiguration(PropertyInfo pi, GridColumnAttribute gca)
        {
            PropertyInfo = pi;
            Attribute = gca;

            Description = string.Empty; // Get it from Description Attrib!!!
        }

        public PropertyInfo PropertyInfo { get; }

        public GridColumnAttribute Attribute { get; }

        public string Name => Attribute.Name;

        public string Label => PropertyInfo.Name;

        public string Description { get; }

        public bool IsReadOnly
        { 
            get => m_IsReadOnly || Attribute.IsReadOnly || (!PropertyInfo.CanWrite);
            set => m_IsReadOnly = value;
        }

        private bool m_IsReadOnly = false;

        public bool Enabled
        {
            get => m_Enabled && DataCellRenderer is IDataCellRenderer;
            set => m_Enabled = value;
        }

        private bool m_Enabled = true;

        

        /// <summary>
        /// Display
        /// </summary>
        public int DisplayOrder { get; set; } = int.MaxValue;

        /// <summary>
        /// Display Priority
        /// </summary>
        public int DisplayPriority { get; set; } = int.MaxValue;

        /// <summary>
        /// Sort Priority
        /// </summary>
        public int SortPriority { get; set; } = int.MaxValue;

        /// <summary>
        /// 
        /// </summary>
        public IDataCellRenderer DataCellRenderer { get; set; } = new TextCellRenderer();

        public bool Visible
        {
            get => m_Enabled && DataCellRenderer is IDataCellRenderer dcr && dcr.Visible;
            set
            {
                if (DataCellRenderer is IDataCellRenderer dcr)
                    dcr.Visible = value;
            }
        }

        public int ActualWidth { get; set; }

        public int Actual_X { get; set; }
    }


    public interface IDataCellRenderer
    {
        bool AutoWidth { get; set; }

        int Width { get; set; }

        int MinimumHeight { get; set; }

        bool Visible { get; set; }

        void Draw(Graphics g, Rectangle bound, object obj);
    }



    public class TextCellRenderer : IDataCellRenderer
    {

        public Font Font => Main.Theme.Font;

        public ColorTheme Theme { get; } = new ColorTheme();

        public bool AutoWidth { get; set; } = false;

        public int Width { get; set; } = 60;

        public int MinimumHeight { get; set; } = 22;

        public bool Visible { get; set; } = true;

        public void Draw(Graphics g, Rectangle bound, object obj)
        {
            g.DrawString(obj.ToString(), Main.Theme.Font, Theme.ForeBrush, bound.Location);
        }
    }

}
