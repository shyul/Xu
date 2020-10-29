/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************


namespace Xu
{
    public interface IRow
    {
        /// <summary>
        /// TODO: Is this really useful?
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        object this[Column column] { get; }
    }
}
