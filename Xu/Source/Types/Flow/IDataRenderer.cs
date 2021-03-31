/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Xu
{
    public interface IDataRenderer : IDataConsumer
    {
        int StartPt { get; }

        int StopPt { get; }

        bool ReadyToShow { get; set; }

        int LastIndexMax { get; }

        /// <summary>
        /// Force Anchor the Data Renderer to show the end points 
        /// </summary>
        void PointerSnapToEnd();

        /// <summary>
        /// Only anchor the renderer pointer when it is near by the end.
        /// </summary>
        void PointerSnapToNextTick();
    }
}
