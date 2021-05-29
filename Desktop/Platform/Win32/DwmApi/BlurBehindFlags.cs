// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    /// <summary>
    /// Flags used by the <see cref="BlurBehind"/> structure to indicate which of its members contain valid information.
    /// </summary>
    [Flags]
    public enum BlurBehindFlags : uint
    {
        /// <summary>
        /// A value for the fEnable member has been specified.
        /// </summary>
        DWM_BB_ENABLE = 0x1,

        /// <summary>
        /// A value for the hRgnBlur member has been specified.
        /// </summary>
        DWM_BB_BLURREGION = 0x2,

        /// <summary>
        /// A value for the fTransitionOnMaximized member has been specified.
        /// </summary>
        DWM_BB_TRANSITIONONMAXIMIZED = 0x4,
    }
}