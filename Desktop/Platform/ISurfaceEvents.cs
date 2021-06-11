// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Mixin")]

namespace SE.Hyperion.Desktop
{
    /// <summary>
    /// 
    /// </summary>
    internal interface ISurfaceEvents
    {
        /// <summary>
        /// 
        /// </summary>
        void OnCreated();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        void OnMove(Point point);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visible"></param>
        void OnVisibleChanged(bool visible);
    }
}
