/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Threading;

namespace Xu
{
    public delegate void CommandHandler(
        IObject sender = null,
        string[] args = null,
        Progress<Event> progress = null,
        CancellationTokenSource cancellationToken = null
        );
}
