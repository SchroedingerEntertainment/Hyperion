// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop
{
    /// <summary>
    /// 
    /// </summary>
    public enum TransparencyMask : short
    {
        /// <summary>
        /// The window background is opaque
        /// </summary>
        None = 0,

        /// <summary>
        /// The window background is transparent
        /// </summary>
        Transparent,

        /// <summary>
        /// The window background is a blur-behind
        /// </summary>
        Blur,

        /// <summary>
        /// The window background is a blur-behind with a high blur radius
        /// </summary>
        AcrylicBlur, 
    }
}
