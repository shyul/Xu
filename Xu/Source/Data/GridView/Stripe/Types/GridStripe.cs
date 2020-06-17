/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;

namespace Xu.GridView
{
    public abstract class GridStripe : IObject, IOrdered, IDependable, IEquatable<GridStripe>
    {
        public virtual string Name { get; set; } = string.Empty;

        public virtual string Label { get; set; } = string.Empty;

        public virtual string Description { get; set; } = string.Empty;

        public virtual bool Enabled { get; set; } = true;

        public virtual int Order { get; set; } = 0;

        public virtual Importance Importance { get; set; }


        protected virtual ColorTheme Theme { get; } = new ColorTheme();

        protected virtual ColorTheme TextTheme { get; } = new ColorTheme();

        #region Coordinate

        public virtual bool AutoWidth { get; set; } = false;

        public virtual int Width { get; set; }

        public virtual int MinimumCellHeight { get; set; } = 22;

        /// <summary>
        /// Set by GridWidget Coordinate
        /// </summary>
        public virtual bool Visible { get; set; } = true;

        /// <summary>
        /// Set by GridWidget Coordinate
        /// </summary>
        public virtual int ActualWidth { get; set; }

        public virtual int ActualHeight { get; set; }

        /// <summary>
        /// Set by GridWidget Coordinate
        /// </summary>
        public virtual int Actual_X { get; set; }

        #endregion Coordinate

        #region Dependable

        public void Remove(bool recursive) { }

        public virtual ICollection<IDependable> Children { get; } = new HashSet<IDependable>();

        public virtual ICollection<IDependable> Parents { get; } = new HashSet<IDependable>();

        #endregion Dependable

        #region Equality

        public override int GetHashCode() => Name.GetHashCode();

        public virtual bool Equals(GridStripe other) => Name == other.Name;

        public override bool Equals(object other)
        {
            if (other is GridStripe dc)
                return Equals(dc);
            else
                return false;
        }

        public static bool operator !=(GridStripe s1, GridStripe s2) => !s1.Equals(s2);
        public static bool operator ==(GridStripe s1, GridStripe s2) => s1.Equals(s2);

        #endregion Equality
    }
}
