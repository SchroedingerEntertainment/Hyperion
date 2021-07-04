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
    public interface ISurfaceEventTarget
    {
        /// <summary>
        /// 
        /// </summary>
        void OnCreated();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        void OnMove(Point location);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        void OnResize(Size size);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visible"></param>
        void OnVisibleChanged(bool visible);

        /// <summary>
        /// 
        /// </summary>
        void OnClose();
    }
}
