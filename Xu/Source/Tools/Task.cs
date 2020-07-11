/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Task Tools
/// 
/// ***************************************************************************

using System.Threading;

namespace Xu
{
    public static class TaskTool
    {


        public static bool Continue(this CancellationTokenSource cts)
        {
            if (cts is null)
                return true;
            else
                return !cts.IsCancellationRequested;
        }

        public static bool Cancelled(this CancellationTokenSource cts)
        {
            if (cts is null)
                return false;
            else
                return cts.IsCancellationRequested;
        }
    }
}
