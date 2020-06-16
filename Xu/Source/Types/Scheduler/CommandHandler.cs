/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Forms;

namespace Xu
{
    public delegate void CommandHandler(
        IOrdered sender = null, 
        string[] args = null, 
        Progress<Event> progress = null, 
        CancellationTokenSource cancellationToken = null
        );
}
