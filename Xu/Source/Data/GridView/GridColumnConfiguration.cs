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
    public sealed class GridColumnConfiguration
    {
        public GridColumnConfiguration(PropertyInfo pi)
        {
            PropertyInfo = pi;
            foreach (object obj in PropertyInfo.GetCustomAttributes(true))
            {
                switch (obj)
                {
                    case ReadOnlyAttribute roa: IsReadOnly = roa.IsReadOnly; break;
                    case DisplayNameAttribute dna: Name = dna.DisplayName; break;
                    case DescriptionAttribute dsa: Description = dsa.Description; break;
                    case GridColumnOrderAttribute gco:
                        DisplayOrder = gco.DisplayOrder;
                        DisplayPriority = gco.DisplayPriority;
                        SortPriority = gco.SortPriority;
                        break;

                    case CellRendererAttribute cra:
                        DataCellRenderer = cra.DataCellRenderer;
                        break;
                }
            }

            if (string.IsNullOrWhiteSpace(Name)) Name = PropertyInfo.Name;
            if (string.IsNullOrWhiteSpace(Description)) Description = PropertyInfo.Name;
            if (DataCellRenderer is null) DataCellRenderer = new TextCellRenderer();
        }

        public PropertyInfo PropertyInfo { get; }

        public string Name { get; set; }

        public string Label => PropertyInfo.Name;

        public string Description { get; set; }

        public bool IsReadOnly
        {
            get => m_IsReadOnly || (!PropertyInfo.CanWrite);
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
        public IDataCellRenderer DataCellRenderer { get; set; }

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
}
