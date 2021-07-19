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
    public interface ITrayActionEventTarget
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="button"></param>
        /// <param name="cursor"></param>
        bool OnMouse(Int64 id, MouseButton button, Point cursor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        bool OnRefresh(Int64 id);
    }
}
