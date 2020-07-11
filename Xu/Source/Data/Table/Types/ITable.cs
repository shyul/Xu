/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************


namespace Xu
{
    public interface ITable
    {
        int Count { get; }

        double this[int i, NumericColumn column] { get; }

        TableStatus Status { get; set; }

        object DataLockObject { get; }
    }
}
