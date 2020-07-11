/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************


namespace Xu
{
    public interface IObject
    {
        /// <summary>
        /// For Identification
        /// </summary>
        string Name { get; }

        /// <summary>
        /// For Graphics Display
        /// </summary>
        string Label { get; } // a.k.a Comment

        /// <summary>
        /// For Documentation
        /// </summary>
        string Description { get; }
    }
}
