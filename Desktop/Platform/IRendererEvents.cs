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
    internal interface IRendererEvents
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        void OnResize(Size size);

        /// <summary>
        /// 
        /// </summary>
        void OnFlushBuffer();
    }
}
