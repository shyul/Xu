/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Runtime.Serialization;

namespace Xu
{
    public interface IOrdered
    {
        /// <summary>
        /// For Identification
        /// </summary>
        string Name { get; }

        /// <summary>
        /// For Sorting and Task
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// For Sorting and Task
        /// </summary>
        int Order { get; set; }

        /// <summary>
        /// For Sorting and Task
        /// </summary>
        Importance Importance { get; }
    }

    /// <summary>
    /// The order is important
    /// </summary>
    [Serializable, DataContract]
    public enum Importance : int
    {
        Huge = 64,
        Major = 32,
        Minor = 16,
        Tiny = 12,
        Micro = 8,
        None = 0,
    }
}
