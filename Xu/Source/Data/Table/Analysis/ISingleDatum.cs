/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

namespace Xu
{
    public interface ISingleDatum : IDependable
    {
        DatumColumn Column_Result { get; }
    }
}
