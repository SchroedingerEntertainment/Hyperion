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
    public interface IMouseEventTarget
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cursor"></param>
        void OnMouseEnter(Point cursor);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cursor"></param>
        void OnMouseMove(Point cursor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        void OnMouseDown(MouseButton button);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        void OnMouseUp(MouseButton button);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hDelta"></param>
        /// <param name="vDelat"></param>
        void OnMouseWheel(float hDelta, float vDelat);

        /// <summary>
        /// 
        /// </summary>
        void OnMouseLeave();
    }
}