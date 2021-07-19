// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;

namespace SE.Hyperion.Desktop
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITrayEventTarget
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="icon"></param>
        void OnIconChanged(Icon icon);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        void OnTooltipChanged(string title);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visible"></param>
        void OnVisibleChanged(bool visible);
    }
}
