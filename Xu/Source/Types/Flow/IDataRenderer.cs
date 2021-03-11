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
        int StartPt { get; set; }

        int StopPt { get; set; }

        bool ReadyToShow { get; set; }

        /// <summary>
        /// Force Anchor the Data Renderer to show the end points 
        /// </summary>
        void PointerToEnd();

        /// <summary>
        /// Only anchor the renderer pointer when it is near by the end.
        /// </summary>
        void PointerToNextTick();
    }
}
